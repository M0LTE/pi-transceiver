
#include <Wire.h>
volatile uint8_t address;
volatile uint8_t memory_map [256]; // array is initialize all low values

#define SLAVE_ID 0x0A

void setup()
{
  Wire.begin(SLAVE_ID);         // join i2c bus with slave_id SLAVE_ID
  Wire.onReceive(receiveEvent); // register write to slave
  Wire.onRequest(requestEvent); // register read from slave
}

void loop()
{
  delay(100);
}

// function that executes when the master writes data to this slave
void receiveEvent(int bytes)
{
  address = Wire.read(); // read first byte to determine address
  while (Wire.available())
  {
    memory_map[address++] = Wire.read();
  }

}

// function that executes when the master reads from this slave
void requestEvent()
{
  address = Wire.read(); // read first byte to determine address
  Wire.write(memory_map[address++]);
  for (int i = 0; i < 32; i++) // this is needed for multibyte reads, up to 32 bytes
  {
    Wire.write(memory_map[address++]);
  }
}
