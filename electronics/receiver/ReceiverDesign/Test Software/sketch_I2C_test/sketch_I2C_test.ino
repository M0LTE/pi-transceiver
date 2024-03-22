//Sketch to control Pi-Transceiver Receiver Module via I2C using Serial Monitor


#include <Wire.h>
int clockFrequency= 100000;  //Standard Rate 100kHz


void setup() {
// setup code, runs once:

//Control strings.  Only one character followed by a line feed will set the variable.  The currect overall state will be displayed in a standard order

//Preamp/Attenuator:  P, A
//Tx/Rx:  R, T
//Duplex/Simplex SDR:  D, S
//Default setting is first in each list


Serial.begin(9600); // opens serial port, sets data rate to 9600 bps

		}//end setup


///String incomingString; obsolete
//char state;
char  Atten ='P';
char Mode = 'R';
char Type = 'D'; 


byte incomingByte = 0; // for each byte (char) incoming serial data from Serial Monitor




//Await character input from Serial Monitor


void loop() {


 

 // send data only when you receive data:
  if (Serial.available() > 0) {
 // read the incoming byte:
    incomingByte = Serial.read();

 // say what you got:
    Serial.print("I received: ");
	Serial.write(incomingByte);
    Serial.println();
  

// Set state from incoming data

switch (incomingByte) {
	case 'P':
	case 'p':
	Atten= 'P';
	break;
	case 'A':
	case 'a':
	Atten= 'A';
	break;
	}

switch (incomingByte) {
	case 'R':
	case 'r':
	Mode= 'R';
	break;
	case 'T':
	case 't':
	Mode= 'T';
	break;
	}

switch (incomingByte) {
	case 'D':
	case 'd':
	Type= 'D';
	break;
	case 'S':
	case 's':
	Type= 'S';
	break;
	}

//End switch

// Print state

String state = String(Atten) + String(Mode) + String(Type);
Serial.print("State = ");
Serial.println(state);
	}// end receive data

   

//Calculate the I2C data to send to receiver module

// See specification

byte val,val1;

if (Atten== 'P') {val1 = 0b00000001;} else {val1 = 0b00000010;}
if (Type== 'D') {
	if (Mode == 'T') {val = 0b00010100;} else {val= 0b00101000;}
		}
else           {
	if (Mode == 'T') {val = 0b00010100;} else {val= 0b00011000;};
	};

val=val+val1;
Serial.print("I2C bus data binary representation: ");
Serial.println(String(val, BIN));


// I2C code:

delay(1000);

// Define I2C address
const byte address = 0b0111000;  // 7-bit I2C address with Write bit

// Set clock frequency
Wire.setClock(clockFrequency);

// Join I2C bus
Wire.begin();

// Begin I2C transmission
Wire.beginTransmission(address);

// Send value byte
Wire.write(val);

// End I2C transmission and check for errors

byte result = Wire.endTransmission();
if (result != 0) {
    Serial.println("Error in I2C transmission");
              }
  } // end void loop 