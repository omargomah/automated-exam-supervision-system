#include "wifi_manager.h"

static unsigned long lastReconnectAttempt = 0;
const unsigned long RECONNECT_INTERVAL = 5000; // Check every 5s if dropped

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
        Serial.println("\n[Wi-Fi] Initial connection timeout. Auto-reconnect active.");
    }
}

// Automatic background reconnection if signal drops
void handleWiFiReconnect(const char* ssid, const char* password) {
    if (WiFi.status() != WL_CONNECTED) {
        unsigned long currentMillis = millis();
        if (currentMillis - lastReconnectAttempt >= RECONNECT_INTERVAL) {
            lastReconnectAttempt = currentMillis;
            Serial.println("[Wi-Fi] Signal lost! Attempting reconnect...");
            WiFi.disconnect();
            WiFi.begin(ssid, password);
        }
    }
}
