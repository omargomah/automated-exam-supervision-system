#ifndef CONFIG_H
#define CONFIG_H

// System Config
#define SERIAL_BAUD_RATE 115200

// Wi-Fi Credentials (Replace with exam hall AP details for field deployment)
#define WIFI_SSID "EXAM_HALL_WIFI"
#define WIFI_PASS "ExamSecurePass2026"

// Hardware Pin Definitions (Using ADC1 pins to avoid Wi-Fi conflicts)
#define AUDIO_PIN 35
#define RF_PIN 34

// Acoustic Telemetry Settings
#define AUDIO_SAMPLE_WINDOW_MS 50
#define AUDIO_ANOMALY_THRESHOLD_V 1.8 // Voltage spike threshold for whisper/talking flag

// RF & BLE Sniffer Settings
#define BLE_SCAN_DURATION_S 1
#define BLE_RSSI_PROXIMITY_THRESHOLD -80 // dBm cutoff for desk/room radius
#define RF_SPIKE_THRESHOLD_V 2.0         // Volts cutoff for suspicious RF energy

#endif // CONFIG_H
