#include <WiFi.h>
#include <PubSubClient.h>

// Configurations - Editable variables
const char* default_SSID = "S23 - Pierote"; // Wi-Fi network name
const char* default_PASSWORD = "234567891"; // Wi-Fi network password
const char* default_BROKER_MQTT = "191.235.241.244"; // MQTT Broker IP
const int default_BROKER_PORT = 1883; // MQTT Broker port
const char* default_TOPICO_SUBSCRIBE = "/TEF/lamp002/cmd"; // MQTT subscription topic
const char* default_TOPICO_PUBLISH_1 = "/TEF/lamp002/attrs"; // MQTT publishing topic 1
const char* default_TOPICO_PUBLISH_2 = "/TEF/lamp002/attrs/t"; // MQTT publishing topic 2
const char* default_TOPICO_PUBLISH_3 = "/TEF/lamp002/attrs/h"; // MQTT publishing topic 3
const char* default_ID_MQTT = "fiware_002"; // MQTT client ID
const int default_D4 = 2; // Onboard LED pin
const char* topicPrefix = "lamp002"; // Prefix for MQTT topics

// Variables for editable configurations
char* SSID = const_cast<char*>(default_SSID);
char* PASSWORD = const_cast<char*>(default_PASSWORD);
char* BROKER_MQTT = const_cast<char*>(default_BROKER_MQTT);
int BROKER_PORT = default_BROKER_PORT;
char* TOPICO_SUBSCRIBE = const_cast<char*>(default_TOPICO_SUBSCRIBE);
char* TOPICO_PUBLISH_1 = const_cast<char*>(default_TOPICO_PUBLISH_1);
char* TOPICO_PUBLISH_2 = const_cast<char*>(default_TOPICO_PUBLISH_2);
char* TOPICO_PUBLISH_3 = const_cast<char*>(default_TOPICO_PUBLISH_3);
char* ID_MQTT = const_cast<char*>(default_ID_MQTT);
int D4 = default_D4;

WiFiClient espClient;
PubSubClient MQTT(espClient);
char EstadoSaida = '0'; // State of the output

// Initialize serial communication
void initSerial() {
    Serial.begin(115200);
}

// Initialize Wi-Fi connection
void initWiFi() {
    delay(10);
    Serial.println("------Wi-Fi Connection------");
    Serial.print("Connecting to network: ");
    Serial.println(SSID);
    Serial.println("Please wait");
    reconectWiFi();
}

// Initialize MQTT connection
void initMQTT() {
    MQTT.setServer(BROKER_MQTT, BROKER_PORT);
    MQTT.setCallback(mqtt_callback);
}

// Setup function
void setup() {
    InitOutput(); // Initialize the LED output
    initSerial();
    initWiFi();
    initMQTT();
    delay(5000);
    MQTT.publish(TOPICO_PUBLISH_1, "s|on"); // Send initial state to the broker
}

// Variables for timing
unsigned long previousMillis = 0;
const long interval = 12000; // 12-second interval

// Main loop function
void loop() {
    unsigned long currentMillis = millis();
    while((currentMillis - previousMillis) < interval) {
        EnviaEstadoOutputMQTT(); // Send LED state to the broker
        VerificaConexoesWiFIEMQTT(); // Check Wi-Fi and MQTT connections
        MQTT.loop(); // Handle MQTT client
        currentMillis = millis();
    }
    previousMillis = currentMillis;

    // Call temperature handling function outside the loop
    handleTemperature();

    // Call MQTT.loop() once more after temperature handling
    MQTT.loop();
}

// Reconnect to Wi-Fi if not connected
void reconectWiFi() {
    if (WiFi.status() == WL_CONNECTED)
        return;
    WiFi.begin(SSID, PASSWORD);
    while (WiFi.status() != WL_CONNECTED) {
        delay(100);
        Serial.print(".");
    }
    Serial.println();
    Serial.println("Successfully connected to Wi-Fi network ");
    Serial.print(SSID);
    Serial.println("IP Address: ");
    Serial.println(WiFi.localIP());

    // Ensure the onboard LED starts off
    digitalWrite(D4, LOW);
}

// Callback function to handle MQTT messages
void mqtt_callback(char* topic, byte* payload, unsigned int length) {
    String msg;
    for (int i = 0; i < length; i++) {
        char c = (char)payload[i];
        msg += c;
    }
    Serial.print("- Message received: ");
    Serial.println(msg);

    // Format the topic for comparison
    String onTopic = String(topicPrefix) + "@on|";
    String offTopic = String(topicPrefix) + "@off|";

    // Compare received message with predefined topics
    if (msg.equals(onTopic)) {
        digitalWrite(D4, HIGH); // Turn LED on
        EstadoSaida = '1';
    }

    if (msg.equals(offTopic)) {
        digitalWrite(D4, LOW); // Turn LED off
        EstadoSaida = '0';
    }
}

// Check Wi-Fi and MQTT connections
void VerificaConexoesWiFIEMQTT() {
    if (!MQTT.connected())
        reconnectMQTT();
    reconectWiFi();
}

// Send LED state to the MQTT broker
void EnviaEstadoOutputMQTT() {
    if (EstadoSaida == '1') {
        MQTT.publish(TOPICO_PUBLISH_1, "s|on");
        Serial.println("- LED On");
    }

    if (EstadoSaida == '0') {
        MQTT.publish(TOPICO_PUBLISH_1, "s|off");
        Serial.println("- LED Off");
    }
    Serial.println("- LED state sent to broker!");
    delay(1000);
}

// Initialize LED output
void InitOutput() {
    pinMode(D4, OUTPUT);
    digitalWrite(D4, HIGH);
    boolean toggle = false;

    // Blink LED 10 times during initialization
    for (int i = 0; i <= 10; i++) {
        toggle = !toggle;
        digitalWrite(D4, toggle);
        delay(200);
    }
}

// Reconnect to MQTT broker
void reconnectMQTT() {
    while (!MQTT.connected()) {
        Serial.print("* Attempting to connect to MQTT Broker: ");
        Serial.println(BROKER_MQTT);
        if (MQTT.connect(ID_MQTT)) {
            Serial.println("Successfully connected to MQTT broker!");
            MQTT.subscribe(TOPICO_SUBSCRIBE); // Subscribe to topic
        } else {
            Serial.println("Failed to reconnect to the broker.");
            Serial.println("Retrying in 2 seconds");
            delay(2000);
        }
    }
}

// Handle temperature readings and send them to the broker
void handleTemperature() {
    const int potPin = 34; // Analog pin for temperature sensor
    int sensorValue = analogRead(potPin); // Read analog value

    float voltage = sensorValue * (3.3 / 4095.0);  // Read voltage

    float temp1 = sensorValue * (1 / 4095.0); // Convert to Temp1
    float tempCelsius = temp1 * 116.0; // Convert to Celsius

    float k = tempCelsius/(voltage*3);

    String mensagem = String(tempCelsius, 2); // Format with two decimal places
    String msg2 = String(k, 2);
    Serial.print("Voltage: ");
    Serial.println(voltage);

    Serial.print("Ganho °C/V: ");
    Serial.println(msg2.c_str());

    Serial.print("Temperature value: ");
    Serial.println(mensagem.c_str());
    MQTT.publish(TOPICO_PUBLISH_2, mensagem.c_str()); // Publish temperature to broker
    MQTT.publish(TOPICO_PUBLISH_3, msg2.c_str()); // Publish gain to broker
}
