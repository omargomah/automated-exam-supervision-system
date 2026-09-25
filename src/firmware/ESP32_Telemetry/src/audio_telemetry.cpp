#include "audio_telemetry.h"

void initAudioSensor(int pin) {
    pinMode(pin, INPUT);
    Serial.printf("[Audio] Initialized microphone sensor on ADC pin %d\n", pin);
}

// Rapidly samples the analog audio pin over sampleWindowMs (e.g., 50ms) to find max peak-to-peak amplitude
float readAudioPeakVoltage(int pin, unsigned long sampleWindowMs) {
    unsigned long startMillis = millis();
    int signalMax = 0;
    int signalMin = 4095;

    while (millis() - startMillis < sampleWindowMs) {
        int sample = analogRead(pin);
        
        if (sample < 4095) { // Filter out spurious ADC pin reads
            if (sample > signalMax) {
                signalMax = sample;
            }
            if (sample < signalMin) {
                signalMin = sample;
            }
        }
    }

    int peakToPeak = signalMax - signalMin;
    float soundVoltage = (peakToPeak / 4095.0) * 3.3; // Convert 12-bit ADC range (0-4095) to 0V-3.3V
    return soundVoltage;
}

bool isAcousticAnomalyDetected(float soundVoltage, float thresholdVoltage) {
    return soundVoltage > thresholdVoltage;
}
