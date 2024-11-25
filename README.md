# README

## English Version

### Overview

This project is an IoT application using an ESP32 microcontroller to connect to a Wi-Fi network and communicate with an MQTT broker. The application is designed to control an onboard LED and send temperature readings to the broker. The code is written in C++ and utilizes the WiFi and PubSubClient libraries for network and MQTT functionalities.

### Components and Functionalities

1. **Libraries**: 
   - `WiFi.h`: Manages Wi-Fi connectivity.
   - `PubSubClient.h`: Handles MQTT protocol communication.

2. **Configuration Variables**:
   - `default_SSID` and `default_PASSWORD`: Define the Wi-Fi network credentials.
   - `default_BROKER_MQTT` and `default_BROKER_PORT`: Specify the MQTT broker's IP and port.
   - `default_TOPICO_SUBSCRIBE`, `default_TOPICO_PUBLISH_1`, `default_TOPICO_PUBLISH_2`: Define the MQTT topics for subscribing and publishing.
   - `default_ID_MQTT`: Sets the MQTT client ID.
   - `default_D4`: Specifies the pin for the onboard LED.
   - `topicPrefix`: Prefix used for MQTT topics.

3. **Wi-Fi and MQTT Initialization**:
   - `initSerial()`: Initializes serial communication for debugging.
   - `initWiFi()`: Connects to the specified Wi-Fi network.
   - `initMQTT()`: Sets up the MQTT client with the broker's details.

4. **Main Functions**:
   - `setup()`: Initializes the system, including the LED, serial communication, Wi-Fi, and MQTT connections.
   - `loop()`: Continuously checks connections, sends LED state, and handles temperature readings.

5. **Connection Management**:
   - `reconectWiFi()`: Reconnects to Wi-Fi if the connection is lost.
   - `reconnectMQTT()`: Reconnects to the MQTT broker if disconnected.

6. **MQTT Callback**:
   - `mqtt_callback()`: Processes incoming MQTT messages to control the LED.

7. **LED and Temperature Handling**:
   - `InitOutput()`: Initializes the LED and blinks it during startup.
   - `EnviaEstadoOutputMQTT()`: Sends the current LED state to the MQTT broker.
   - `handleTemperature()`: Reads temperature from a sensor and publishes it to the broker.

### Usage

To use this code, upload it to an ESP32 board. Ensure the Wi-Fi credentials and MQTT broker details are correctly set. The onboard LED will respond to MQTT messages, and temperature readings will be sent periodically.

---

### Eletric Diagram

![image](https://github.com/user-attachments/assets/aa40edaa-fc3d-43df-a7c1-34ec6426e57b)

### Basic Architecture for IoT Projects with ESP32 and Dashboard Integration
The application layer serves as the interface between users and IoT devices, enabling the visualization and interaction with system data via dashboards and data analysis tools. In this context, the ESP32 sends temperature data and device status to a central MQTT broker. This data can be extracted by a dashboard application, allowing users to monitor the system in real-time. The dashboard can also incorporate advanced features such as data analytics, machine learning models, and mobile applications to enhance user experience and decision-making.

The back-end layer operates as the communication and storage hub, managing data exchange between the application and IoT devices. The MQTT broker (e.g., Eclipse-Mosquitto) serves as the primary intermediary, enabling real-time communication. In addition, the back-end can include NoSQL databases such as MongoDB to store historical data (e.g., temperature readings) and provide integration with context management tools like Orion Context Broker for advanced querying and subscriptions.

The IoT layer represents the devices in the field, such as the ESP32, which collects temperature data and transmits it to the back-end for dashboard integration. Specifically:

The ESP32 reads data from a temperature sensor connected to an analog pin (e.g., GPIO34) and publishes this data to a specific MQTT topic.
The device also subscribes to a command topic, enabling interaction from the dashboard, such as toggling the built-in LED (simulating actuator control).
Dashboard Functionality
The dashboard application extracts and visualizes data sent by the ESP32, providing users with:

Real-Time Monitoring: Displaying the temperature values and LED status.
Historical Data Analysis: Using stored data from the back-end for trends, averages, or predictive analytics.
Device Control: Sending commands through the dashboard to control the ESP32, such as turning the LED on or off.
Alerts and Notifications: Triggering alerts if specific thresholds (e.g., temperature limits) are exceeded.
Security and Scalability
For production systems, incorporating security mechanisms is crucial, such as:

Secure protocols like MQTTs and HTTPs for encrypted data transmission.
Authentication and authorization using FIWARE components like Keyrock, Wilma PEP Proxy, and AuthZForce PDP.
Data integrity and access control mechanisms to ensure reliable and secure operations.
To ensure scalability and high availability, it is recommended to utilize container orchestration platforms such as Docker Swarm or Kubernetes, which can manage multiple IoT devices and scale the back-end and dashboard infrastructure as needed.

Code Operation and Dashboard Integration
Wi-Fi Connection: The ESP32 connects to the Wi-Fi network, establishing connectivity with the MQTT broker.
MQTT Data Transmission:
The ESP32 reads temperature data, processes it into a readable format, and publishes it to the broker for dashboard extraction.
The LED state (on/off) is also published, allowing the dashboard to display the current state of the actuator.
Command Handling: The ESP32 listens for commands from the broker, allowing the dashboard to control the LED via MQTT.
Dashboard Role:
Extracts data from the broker using the same topics.
Displays live updates and trends for users.
Sends commands to control the ESP32 in real-time.
This setup illustrates an efficient and flexible architecture for integrating IoT devices like the ESP32 into a dashboard-driven system, enabling monitoring, analysis, and control within a robust IoT ecosystem.

![esquema](https://github.com/user-attachments/assets/673ef074-8998-4d93-8874-3ffdcbd59f91)
