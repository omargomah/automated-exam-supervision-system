#include "wifi_manager.h"

static unsigned long lastReconnectAttempt = 0;
const unsigned long RECONNECT_INTERVAL = 5000; // Retry every 5 seconds non-blocking

void initWiFi(const char* ssid, const char* password) {
    WiFi.mode(WIFI_STA);
    WiFi.begin(ssid, password);
    Serial.printf("[Wi-Fi] Connecting to %s ", ssid);

    int attempts = 0;
    while (WiFi.status() != WL_CONNECTED && attempts < 20) {
        delay(500);
        Serial.print(".");
        attempts++;
    }

    if (WiFi.status() == WL_CONNECTED) {
        Serial.println("\n[Wi-Fi] Connected successfully!");
        Serial.print("[Wi-Fi] IP Address: ");
        Serial.println(WiFi.localIP());
    } else {
        Serial.println("\n[Wi-Fi] Initial connection timeout. Auto-reconnect active in background.");
    }
}

void handleWiFiReconnect(const char* ssid, const char* password) {
    unsigned long currentMillis = millis();

    if (WiFi.status() != WL_CONNECTED) {
        if (currentMillis - lastReconnectAttempt >= RECONNECT_INTERVAL) {
            lastReconnectAttempt = currentMillis;
            Serial.println("[Wi-Fi] Connection lost! Attempting background reconnect...");
            WiFi.disconnect();
            WiFi.begin(ssid, password);
        }
    }
}

bool isWiFiConnected() {
    return WiFi.status() == WL_CONNECTED;
}
