#include <Arduino.h>
#include <WiFi.h>
#include <PubSubClient.h>
#include <WebServer.h>
#include <EEPROM.h>
#include "HX711.h"
#include "./config.h"
#include <DNSServer.h>
#include <SSD1306Ascii.h>
#include <SSD1306AsciiWire.h>
#include "soc/soc.h"
#include "soc/rtc_cntl_reg.h"


// put function declarations here:
int calibrate_scale();
void init_scale();
void init_oled();
void connect_wifi();
void connect_mqtt();
bool is_credentials_saved();
void host_credential_AP();
void clear_eeprom();
void mqtt_callback(char* topic, byte* message, unsigned int length);
void save_credentials(const String& ssid, const String& password);

float current_weight = 0;

String saved_ssid;
String saved_password;


HX711 scale;
WiFiClient espClient;
PubSubClient client(espClient);
SSD1306AsciiWire oled;
WebServer server(80);
DNSServer dnsServer;
const byte DNS_PORT = 53;



void setup()
{
  WRITE_PERI_REG(RTC_CNTL_BROWN_OUT_REG, 0); //disable brownout detector
  Serial.begin(115200);

  delay(1000);

#ifdef CALIBRATE
  scale.begin(LOADCELL_DOUT_PIN, LOADCELL_SCK_PIN);
#else
  // clear_eeprom();
  init_scale();
  init_oled();
  if (is_credentials_saved())
  // if credentials are saved, try to connect to wifi
  {
    Serial.println("Credentials saved, using them");
    Serial.println(saved_ssid);
    Serial.println(saved_password);
    WiFi.begin(saved_ssid.c_str(), saved_password.c_str());
  }
  // open AP if no credentials are saved
  else
  {
    Serial.println("Credentials not saved, creating AP");
    host_credential_AP();
  }
  connect_wifi();
  connect_mqtt();

#endif
}

void clear_eeprom()
// clears eeprom, for example to reset credentials
{
  EEPROM.begin(eepromSize);
  for (int i = 0; i < eepromSize; ++i)
  {
    EEPROM.write(i, 0);
  }
  EEPROM.commit();
  EEPROM.end();
}

bool is_credentials_saved()
// checks if credentials are saved in eeprom
{
  EEPROM.begin(eepromSize);
  for (int i = ssidAddress; i < passwordAddress; ++i)
  {
    saved_ssid+= char(EEPROM.read(i));
  }
  for (int i = passwordAddress; i < eepromSize; ++i)
  {
    saved_password+= char(EEPROM.read(i));
  }
  EEPROM.end();
  return saved_ssid != "" && saved_password!= "";
}

void save_credentials(const String& ssid, const String& password) {
  EEPROM.begin(eepromSize);
  for (int i = 0; i < ssid.length(); i++) {
    EEPROM.write(ssidAddress + i, ssid[i]);
  }
  for (int i = 0; i < password.length(); i++) {
    EEPROM.write(passwordAddress + i, password[i]);
  }
  EEPROM.commit();
  EEPROM.end();
  Serial.println("Credentials saved: ");
  Serial.println(ssid);
  Serial.println(password);
}


void host_credential_AP() {
  // Scan networks before setting up access point
  int n = WiFi.scanNetworks();
  String* ssids = new String[n];
  for (int i = 0; i < n; ++i) {
    ssids[i] = WiFi.SSID(i);
  }

  // Create access point
  WiFi.softAP(wifi_ssid, password);

  // Setup DNS to capture all domains
  dnsServer.start(DNS_PORT, "*", WiFi.softAPIP());

  // Start web server
  server.on("/", HTTP_GET, [ssids, n](){
    String form = R"=====(
    <!DOCTYPE html>
    <html>
    <head>
      <style>
        body {
          font-family: Arial, sans-serif;
          margin: 0;
          padding: 0;
          background-color: #f0f0f0;
          display: flex;
          justify-content: center;
          align-items: center;
          height: 100vh;
        }

        form {
          background-color: #ffffff;
          padding: 20px;
          border-radius: 8px;
          box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
        }

        input[type='submit'] {
          background-color: #007BFF;
          color: #ffffff;
          border: none;
          padding: 10px 20px;
          border-radius: 5px;
          cursor: pointer;
        }

        input[type='submit']:hover {
          background-color: #0056b3;
        }

        select, input[type='password'] {
          margin-top: 10px;
          padding: 10px;
          width: 100%;
          border-radius: 5px;
          border: 1px solid #ddd;
        }
      </style>
    </head>
    <body>
    <form method='POST' action='/save' onsubmit='return validateForm()'>
      <label for='ssid'>SSID:</label>
      <select name='ssid' id='ssid'>
    )=====";

    for (int i = 0; i < n; ++i) {
      form += "<option>";
      form += ssids[i];
      form += "</option>";
    }

    form += R"=====(
      </select>
      <label for='password'>Password:</label>
      <input type='password' name='password' id='password'>
      <input type='submit' value='Save'>
    </form>
    <script>
      function validateForm() {
        var password = document.getElementById('password').value;
        if (password == '') {
          alert('Please enter the password.');
          return false;
        }
        return true;
      }
    </script>
    </body>
    </html>
    )=====";

    server.send(200, "text/html", form);
  });

  // Save form POST action
  server.on("/save", HTTP_POST, [](){
    String ssid = server.arg("ssid");
    String password = server.arg("password");
    Serial.println(ssid);
    Serial.println(password);
    
    // Save credentials to EEPROM
    save_credentials(ssid, password);

    server.send(200, "text/html", R"=====(
    <!DOCTYPE html>
    <html>
    <head>
      <style>
        body {
          font-family: Arial, sans-serif;
          margin: 0;
          padding: 0;
          background-color: #f0f0f0;
          display: flex;
          justify-content: center;
          align-items: center;
          height: 100vh;
        }
        p {
          background-color: #ffffff;
          padding: 20px;
          border-radius: 8px;
          box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
        }
      </style>
    </head>
    <body>
      <p>Credentials saved. Restarting...</p>
    </body>
    </html>
    )=====");

    // Restart ESP32
    ESP.restart();
  });

  server.begin();

  while(WiFi.status() != WL_CONNECTED) {
    //captive portal
    dnsServer.processNextRequest();
    server.handleClient();
  }

  delete[] ssids;  // remember to free the memory
}


void loop()
{
  client.loop();
#ifdef CALIBRATE
  calibrate_scale();
#else
  float reading = scale.get_units(1);
  if (reading < current_weight - weight_treshold || reading > current_weight + weight_treshold)
  {
    current_weight = reading;
    Serial.println(current_weight);
    oled.clear();
    oled.println(current_weight);
    if (client.connected())
    {
      char msg[10];
      dtostrf(current_weight, 6, 2, msg);
      client.publish(MQTT_TOPIC, msg);
    }
    else
    {
      Serial.println("MQTT not connected");
      if (WiFi.status() != WL_CONNECTED)
      {
        connect_wifi();
      }
      connect_mqtt();
      current_weight = 0; // send reading during next loop run
    }
  }
  delay(500);
#endif
}

int calibrate_scale()
{
  long reading;

  if (scale.is_ready())
  {
    scale.set_scale();
    Serial.println("Tare... remove any weights from the scale.");
    delay(5000);
    scale.tare();
    Serial.println("Tare done...");
    Serial.print("Place a known weight on the scale...");
    delay(5000);
    reading = scale.get_units(10);
    Serial.print("Result: ");
    Serial.println(reading);
  }
  else
  {
    Serial.println("HX711 not found.");
  }
  delay(1000);
  return reading;
}

void init_scale()
{
  scale.begin(LOADCELL_DOUT_PIN, LOADCELL_SCK_PIN);
  scale.set_scale(calibration_factor);
  scale.tare();
}

void init_oled()
{
  Wire.begin();
  Wire.setClock(400000L);
  oled.begin(&Adafruit128x32, I2C_ADDRESS);
  oled.displayRemap(true);
  oled.setFont(System5x7);
  oled.println("Select WIFI using AP");
  oled.println("SSID: PantryPoint");
  oled.println("PW: password");
  oled.setFont(Cooper26);
}

void connect_wifi()
{
  WiFi.begin(saved_ssid, saved_password);
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.println("Connecting to WiFi..");
  }
  Serial.println("Connected to the WiFi network");
}

void connect_mqtt() {
    client.setServer(MQTT_SERVER, MQTT_PORT);
    client.setCallback(mqtt_callback);  // Set callback function
  
    while (!client.connected() && WiFi.status() == WL_CONNECTED) {
        Serial.println("Connecting to MQTT...");
        
        if (client.connect("ESP32_Scale_1")) {
            Serial.println("connected");
            client.subscribe("your_tare_topic");  // Subscribe to topic
        } else {
            Serial.print("failed with state ");
            Serial.print(client.state());
            delay(2000);
        }
    }
}

void mqtt_callback(char* topic, byte* message, unsigned int length) {
    String messageTemp;
  
    for (int i = 0; i < length; i++) {
        messageTemp += (char)message[i];
    }

    if (messageTemp == "tare") {
        scale.tare();
        Serial.println("Tared the scale due to MQTT message");
    }
    if (messageTemp == "reset") {
        Serial.println("Reset due to MQTT message");
        delay(100); // delays to make sure the battery manages it
        clear_eeprom();
        delay(100);
        ESP.restart();
    }
}
