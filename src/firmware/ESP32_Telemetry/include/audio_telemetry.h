#ifndef AUDIO_TELEMETRY_H
#define AUDIO_TELEMETRY_H

#include <Arduino.h>

// Function Declarations
void initAudioSensor(int pin);
float readAudioPeakVoltage(int pin, unsigned long sampleWindowMs);
bool isAcousticAnomalyDetected(float soundVoltage, float thresholdVoltage);

#endif // AUDIO_TELEMETRY_H
