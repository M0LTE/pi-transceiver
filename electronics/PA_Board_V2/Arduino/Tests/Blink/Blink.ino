

/* Test example for RADARC Pi Transceiver PA Board SK6812 RGBW LED
and Watchdog LED
*/
#include <Adafruit_NeoPixel.h>

Adafruit_NeoPixel RGB(SK6812_NUM, PIN_SK6812_DATA, NEO_RGB + NEO_KHZ800);

void setup() {
  RGB.begin();
  RGB.setBrightness(10);
  RGB.show();
  pinMode(LED_BUILTIN, OUTPUT); 
  digitalWrite(LED_BUILTIN, LOW);
  
}

void loop() {
  digitalWrite(LED_BUILTIN, HIGH);
  
  delay(200); //red
  RGB.setPixelColor (0, RGB.Color(255,0,0));
  RGB.setPixelColor (1, RGB.Color(255,0,0));
  RGB.show();
   digitalWrite(LED_BUILTIN, LOW);
  delay(200); //green
  RGB.setPixelColor (0, RGB.Color(0,255,0));
  RGB.setPixelColor (1, RGB.Color(0,255,0));
  RGB.show();
  delay(200); //blue
  RGB.setPixelColor (0, RGB.Color(0,0,255));
  RGB.setPixelColor (1, RGB.Color(0,0,255));
  RGB.show();
  
}