#define MQTT_SERVER "192.168.183.211" // "172.22.120.62"
#define MQTT_PORT 1883
#define MQTT_TOPIC "esp32/weight"
// mögliche Adressen I2C: 0X3C+SA0  0x3C or 0x3D
#define I2C_ADDRESS 0x3C

const char* wifi_ssid = "PantryPoint";
const char* password = "password";
const int eepromSize = 512;
const int ssidAddress = 0;
const int passwordAddress = 32;
// HX711 circuit wiring
const int LOADCELL_DOUT_PIN = 19;
const int LOADCELL_SCK_PIN = 18;

const float calibration_factor = 482.2f;
const float weight_treshold = 1.0f;
// #define CALIBRATE