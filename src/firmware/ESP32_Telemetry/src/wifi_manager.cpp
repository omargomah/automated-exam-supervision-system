#include "wifi_manager.h"
#include "config.h"
#include <esp_wifi.h>

static unsigned long lastReconnectAttempt = 0;
const unsigned long RECONNECT_INTERVAL = 5000; // Check every 5s if dropped

const char* allowedSSIDs[] = {
    WIFI_SSID,                  // Your primary telemetry network
    "Exam_Room_Router_Primary", // Add other official venue APs here
    "Panda"                     // Your home network/router name
};
const int allowedSSIDCount = sizeof(allowedSSIDs) / sizeof(allowedSSIDs[0]);

bool isWhitelisted(String ssid) {
    for (int i = 0; i < allowedSSIDCount; i++) {
        if (ssid.equals(allowedSSIDs[i])) {
            return true;
        }
    }
    return false;
}

// Callback function to sniff raw 802.11 management/probe packets
void wifiPromiscuousCallback(void* buf, wifi_promiscuous_pkt_type_t type) {
    wifi_promiscuous_pkt_t *pkt = (wifi_promiscuous_pkt_t*)buf;
    wifi_pkt_rx_ctrl_t ctrl = pkt->rx_ctrl;
    (void)ctrl; 
    
    if (type == WIFI_PKT_MGMT) {
        uint8_t *payload = pkt->payload;
        (void)payload;
    }
}

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

// Routine to scan active nearby access points and return the count of suspicious hotspots
int scanAndGetSuspiciousAPCount() {
    Serial.println("[Wi-Fi] Scanning environment for surrounding networks/hotspots...");
    int suspiciousCount = 0;
    int n = WiFi.scanNetworks(false, true);
    
    if (n > 0) {
        for (int i = 0; i < n; ++i) {
            String currentSSID = WiFi.SSID(i);
            long rssi = (long)WiFi.RSSI(i);
            long channel = (long)WiFi.channel(i);

            // Filter for strong signals in close proximity
            if (rssi > -75) {
                // Ignore empty or hidden SSIDs
                if (currentSSID.length() == 0) {
                    Serial.println("[INFO] Hidden/Blank AP detected (Ignored)");
                    continue;
                }

                // Check if it belongs to the whitelist
                if (isWhitelisted(currentSSID)) {
                    Serial.printf("[INFO] Whitelisted AP detected (Ignored) -> SSID: %s | RSSI: %ld dBm\n", 
                                  currentSSID.c_str(), rssi);
                } else {
                    suspiciousCount++;
                    Serial.printf("[ALERT] Suspicious Hotspot Detected -> SSID: %s | RSSI: %ld dBm | Ch: %ld\n", 
                                  currentSSID.c_str(), rssi, channel);
                }
            }
        }
    }
    WiFi.scanDelete();
    return suspiciousCount;
}