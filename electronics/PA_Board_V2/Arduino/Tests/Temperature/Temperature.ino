/*
Test for RADARC Pi Transceiver PA Board LM75B temperature sensor, and display it
in Celcius every 250ms. 
*/

#include <Temperature_LM75_Derived.h>

// The Generic_LM75 class will provide 12-bit temperature

Generic_LM75_12Bit temperature(&Wire2);

void setup() {
  while(!Serial) {}
  
  Serial.begin(9600);

  Wire2.begin(); // Internal I2C Bus
}

void loop() {
  Serial.print("Temperature = ");
  Serial.print(temperature.readTemperatureC(),1);
  Serial.println(" C");

  delay(250);
}
