/* Test for RADARC Pi Transcever PA Vdd, Vgg and Idd
* !!THESE TESTS ASSUME that the MITSUBISHI POWER BLOCK IS NOT MOUNTED!!
* !!VALUES OF Vgg MAY CAUSE EXCESS Idd CURRENT
* Power must be applied to the PA board through the Battery connector
*/

#include <Wire.h>
#include <Adafruit_INA219.h>
#include <Adafruit_MCP4725.h>


#define VGG_MINIMUM 4095
#define VGG_MAXIMUM 0

Adafruit_MCP4725 dac;
Adafruit_INA219 ina219;

void setup(void) 
{
  pinMode(PIN_EN_VGG, OUTPUT);
  digitalWrite(PIN_EN_VGG, LOW);
  pinMode(PIN_DRIVER_EN, OUTPUT);
  digitalWrite(PIN_DRIVER_EN, HIGH);
  dac.begin(0x61);

  analogReadResolution(ADC_RESOLUTION);

  Serial.begin(115200);
  while (!Serial) {
      // will pause Zero, Leonardo, etc until serial console opens
      delay(1);
  }
    
  Serial.println("Power Tests");
  
  // Initialize the INA219.
 
  if (! ina219.begin()) {
    Serial.println("Failed to find INA219 chip");
    while (1) { delay(10); }
  }
  

  Serial.println("Measuring voltage and current with INA219 ...");
}

void loop(void) 
{
  float shuntvoltage = 0;
  float busvoltage = 0;
  float gatevoltage = 0;
  delay(1000);
  shuntvoltage = ina219.getShuntVoltage_mV();
  busvoltage = ina219.getBusVoltage_V();
  gatevoltage = analogRead(2) * ( (5.7 * 3.3) / 4095.0);
  
  Serial.println("Vgg disabled");
  Serial.print("Bus Voltage:   "); Serial.print(busvoltage); Serial.println(" V");
  Serial.print("Shunt Voltage: "); Serial.print(shuntvoltage); Serial.println(" mV");
  Serial.print("Gate Voltage:   "); Serial.print(gatevoltage); Serial.println(" V");
  Serial.println("");

  delay(2000);

  dac.setVoltage( VGG_MINIMUM, false);
  digitalWrite(PIN_EN_VGG, HIGH);
  delay(1000);
  shuntvoltage = ina219.getShuntVoltage_mV();
  busvoltage = ina219.getBusVoltage_V();
  gatevoltage =analogRead(2) * ( (5.7 * 3.3) / 4095.0);
  
  Serial.println("Vgg minimum");
  Serial.print("Bus Voltage:   "); Serial.print(busvoltage); Serial.println(" V");
  Serial.print("Shunt Voltage: "); Serial.print(shuntvoltage); Serial.println(" mV");
  Serial.print("Gate Voltage:   "); Serial.print(gatevoltage); Serial.println(" V");
  Serial.println("");

  delay(2000);

  dac.setVoltage( VGG_MAXIMUM, false);
  digitalWrite(PIN_EN_VGG, HIGH);
  delay(1000);
  shuntvoltage = ina219.getShuntVoltage_mV();
  busvoltage = ina219.getBusVoltage_V();
  gatevoltage =  analogRead(2) * ( (5.7 * 3.3) / 4095.0);
  
  Serial.println("Vgg maximum");
  Serial.print("Bus Voltage:   "); Serial.print(busvoltage); Serial.println(" V");
  Serial.print("Shunt Voltage: "); Serial.print(shuntvoltage); Serial.println(" mV");
  Serial.print("Gate Voltage:   "); Serial.print(gatevoltage); Serial.println(" V");
  Serial.println("");

  delay(2000);
 
  digitalWrite(PIN_EN_VGG, LOW);

}
