/* Test example for RADARC Pi Transceiver PA Board DOTSTAR RGB LED
and Watchdog LED
*/
#include <Adafruit_DotStar.h>

Adafruit_DotStar strip = Adafruit_DotStar(DOTSTAR_NUM, PIN_DOTSTAR_DATA, PIN_DOTSTAR_CLK, DOTSTAR_BGR);

void setup() {
  strip.begin();
  pinMode(LED_BUILTIN, OUTPUT); 
  digitalWrite(LED_BUILTIN, LOW);
}

void loop() {
  digitalWrite(LED_BUILTIN, HIGH);
  strip.setPixelColor(0, 64, 0, 0); strip.show(); delay(1000); //red
  strip.setPixelColor(0, 0, 64, 0); strip.show(); delay(1000); //green
  digitalWrite(LED_BUILTIN, LOW);
  strip.setPixelColor(0, 0, 0, 64); strip.show(); delay(1000); //blue
}