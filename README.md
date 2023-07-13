# Pantry Pulse (Smart Scale Project)

![top](https://github.com/fbra-dev/PantryPulse/assets/9578470/c5d8b3db-56d9-4861-9bfc-34dfae0d6dcc)
![side](https://github.com/fbra-dev/PantryPulse/assets/9578470/a411ebe1-1bb6-4556-b6b3-d687cc90a880)

## Introduction

This project was created during a hackathon by a team of four passionate individuals. We have developed an IoT-enabled smart scale using Arduino and various other components. This scale is designed to help manage pantry inventory by providing real-time weight data, facilitating usage tracking and planning shopping. It could also integrate with recipe apps, enabling automatic actions based on certain weight thresholds.

## Features

Our smart scale offers the following capabilities:

- Precise weight measurements.
- WiFi and MQTT connectivity for IoT integration.
- Scale calibration.
- A captive portal for easy scale setup.
- An OLED screen for real-time weight display.

Our Application is capable of:

- User registration and sign-in features.
- Real-time weight display of all registered scales.
- Product creation with a default weight (at 100%).
- Assigning products to a scale.

## Prerequisites

Before you begin, ensure you have met the following requirements:

- You have Docker installed. If not, follow the installation guide from [here](https://docs.docker.com/engine/install/). The launch script will try to install Docker if it's not already installed.
- You have installed the latest version of Node.js and npm. You can install them from [here](https://nodejs.org/en/download/).
- You have installed the Angular CLI. If not, install it using npm by running the following command:

```bash
npm install -g @angular/cli
```

## Setup and Installation

### Hardware setup

To setup and use our smart scale, follow these steps:

1. Clone this repository.
2. Open the project in your favorite Arduino IDE.
3. Adjust the config.h file.
4. Upload the code to your microcontroller.
5. If your WiFi credentials are not saved, the microcontroller will host an access point. Connect to it and select your WiFi network and enter the password.
6. The microcontroller will now connect to your WiFi network and start publishing weight data over MQTT.

### Frontend setup

1. Clone this repository.
2. Adjust the environment.ts and proxy.config.json 

### Backend setup

1. Clone this repository.
2. Adjust the appsettings.json file.
3. To run the backend on your local machine you can use the SetupAndLaunchDocker.ps1 script

## Challenges and Learnings

During this project, we faced several challenges:

- **PWA Limitations**: We discovered that PWAs lack certain features necessary for a smooth IoT setup procedure. Specifically, automatic WiFi connection setup was not achievable using a PWA.

- **SSL Certificate for Intranet PWA Deployment**: SSL certificates, necessary for secure PWA deployment, typically require a fully qualified domain name which is publicly accessible on the internet. For offline or intranet applications, this posed a challenge as generating SSL certificates for such uses resulted in browser security warnings, affecting the user experience.

- **Scale Registration Process**: Due to the aforementioned PWA limitations and the need for a seamless user experience, we found that creating an optimal registration process for the scales was challenging.

From these challenges, we learned a lot:

- **Understanding PWA Capabilities**: This project deepened our understanding of PWAs, helping us recognize that for applications requiring low-level network control or offline capability, a PWA might not be the most suitable solution. Future technology choices for similar projects will be influenced by this insight.

- **Refining Device Registration**: Our experience underlined the importance of a streamlined registration process for IoT devices. This lesson will guide the evolution of our product, specifically in improving the user setup experience.

Our response to these challenges is to create a native Android / iOS app for the smart scale setup. We anticipate that this will provide us with more control over the WiFi connection setup and enhance the overall user experience.


## Future Improvements

While we are proud of what we achieved during the hackathon, we acknowledge that our project has room for improvements. We plan to enhance our smart scale in the following ways:

- `Create a native mobile app`.
- `Improve the frontend deployment process`.
- `Improve the scale-to-backend registration process`.
- `Add an option to tare the scale from the frontend`.

## Contributors

This project was made possible thanks to the efforts of:

- `Fabian Brandl`: `ESP32 (CaptivePortal), Docker Setup, Backend `
- `Sascha Lutterbach`: `Frontend (Angular)`
- `Alex Netsch`: `ESP32, Hardware`
- `Stefan Lang`: `Backend`

## License

The source code for the site is licensed under the MIT license, which you can find in
the MIT-LICENSE.txt file.
