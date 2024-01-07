//All commands for 'master'
//COMMAND ARRAY ------------------------------------------------------------------------------
const commandList_t masterCommands[] = {
  
  {"hello",       helloHandler,     "hello"},
  {"get RGB",     getRGBHandler,    "get RGB LED colours"},
  {"set RGB",     setRGBHandler,    "set RGB colour"},
  {"get LED",     getLEDHandler,    "get LED state"},
  {"set LED",     setLEDHandler,    "set LED state ON/OFF"},
  {"get TX",      getTXHandler,     "get TX state"},
  {"set TX",      setTXHandler,     "set TX state ON/OFF"},
  {"get RX",      getRXHandler,     "get RX state"},
  {"set RX",      setRXHandler,     "set RX state ON/OFF"},
  {"get LNA",     getLNAHandler,    "get LNA state"},
  {"set LNA",     setLNAHandler,    "set LNA state ON/OFF"},
  {"get SINGLE",  getSINGLEHandler, "get TX state"},
  {"set SINGLE",  setSINGLEHandler, "set TX state ON/OFF"},
  {"get DRIVER",  getDRVHandler,    "get PA Driver state"},
  {"set DRIVER",  setDRVHandler,    "set PA Driver state ON/OFF"},
  {"get FAN",     getFANHandler,    "get FAN PWM"},
  {"set FAN",     setFANHandler,    "set FAN PWM"},
  {"get RPM",     getRPMHandler,    "get FAN RPM"},
  {"get TLIMIT",  getTINTHandler,   "get Temp limit state and setting"},
  {"set TLIMIT",  setTINTHandler,   "set Temp limit"},
  {"get VGG",     getVGGHandler,    "get VGG state and voltage"},
  {"set VGG",     setVGGHandler,    "set VGG state ON/OFF"},
  {"get DAC",     getDACHandler,    "get Vgg DAC value"},
  {"set DAC",     setDACHandler,    "set Vgg DAC value"},
  {"get volts",   getVOLTSHandler,  "get Vgg in volts"},
  {"set volts",   setVOLTSHandler,  "set Vgg in volts"},
  {"get temp",    getTEMPHandler,   "get Temperature"},
  {"get VDD",     getVDDHandler,    "get VDD voltage"},
  {"get IDD",     getIDDHandler,    "get IDD Current"},
  {"get FWD",     getFWDHandler,    "get Forward Power"},
  {"get REV",     getREVHandler,    "get Reverse Power"},
};

/* Command handler template
bool myFunc(Commander &Cmdr){
  //put your command handler code here
  return 0;
}
*/
void initialiseCommander(){
  cmd.begin(&Serial, masterCommands, sizeof(masterCommands));
  cmd.commandPrompt(ON); //enable the command prompt
  cmd.echo(true);     //Echo incoming characters to theoutput port
  cmd.errorMessages(ON); //error messages are enabled - it will tell us if we issue any unrecognised commands
  cmd.autoChain(OFF);
  cmd.setUserString(deviceInfo);
}

//These are the command handlers, there needs to be one for each command in the command array myCommands[]
//The command array can have multiple commands strings that all call the same function
bool helloHandler(Commander &Cmdr){
  Cmdr.print("Hello! this is ");
  Cmdr.println(Cmdr.commanderName);
  Cmdr.print("This is my buffer: ");
  Cmdr.print(Cmdr.bufferString);
  //Cmdr.printDiagnostics();
  return 0;
}


bool getRGBHandler(Commander &Cmdr){
  uint32_t pixelValue;

  pixelValue = RGB.getPixelColor(0);
  Cmdr.print("RGB 0 Colours Red = ");
  Cmdr.print((pixelValue >> 16) & 0xff );
  Cmdr.print(" Green = ");
  Cmdr.print((pixelValue >> 8) & 0xff);
  Cmdr.print(" Blue = ");
  Cmdr.println(pixelValue & 0xff);

  pixelValue = RGB.getPixelColor(1);
  Cmdr.print("RGB 1 Colours Red = ");
  Cmdr.print((pixelValue >> 8) & 0xff );
  Cmdr.print(" Green = ");
  Cmdr.print((pixelValue >>16) & 0xff);
  Cmdr.print(" Blue = ");
  Cmdr.println(pixelValue & 0xff);
  return 0;

}

bool setRGBHandler(Commander &Cmdr){
  
  int values[4] = {0, 0, 0, 0};
  int n;
  if(Cmdr.countItems() != 4){ // 4 values LED number plus R, G, B
  Cmdr.println("Argument error");
  return false;
  }
  
  for(n = 0; n < 4; n++){  // Valid integers?
    if(!Cmdr.getInt(values[n])){
      Cmdr.println("Argument error");
      return 0;
    }
  }

    if(values[0] <0 || values[0] > 1){ // LED 0 or 1?
      Cmdr.println("Value error");
      return 0;
    }
 
    for(n = 1; n < 4; n++){ // LED intensities in range
    if(values[n] < 0 || values[n] > 255){
      Cmdr.println("Value error");
      return 0;
    } 
  }
  RGB.setPixelColor(values[0], RGB.Color(values[2], values[1], values[3]));
  RGB.show();
  return 0;
}


bool getLEDHandler(Commander &Cmdr){
  if(LED_state) Cmdr.println("LED is ON");
  else Cmdr.println("LED is OFF"); 
  return 0;
}

bool setLEDHandler(Commander &Cmdr){
  if(Cmdr.containsOn()){
    LED_state = true;
    digitalWrite(LED_BUILTIN, HIGH);
    Cmdr.println("LED is ON");
  }
  else if(Cmdr.containsOff()){
    LED_state = false;
    digitalWrite(LED_BUILTIN, LOW);
    Cmdr.println("LED is OFF");
  }
  else Cmdr.println("Argument error");
  return 0;
}

bool getTXHandler(Commander &Cmdr){
  if(TX_state) Cmdr.println("TX is ON");
  else Cmdr.println("TX is OFF"); 
  return 0;
}

bool setTXHandler(Commander &Cmdr){
  if(Cmdr.containsOn()){
    TX_state = true;
    digitalWrite(PIN_TX_RELAY, HIGH);
    Cmdr.println("TX is ON"); 
  }
  else if(Cmdr.containsOff()){
    TX_state = false;
    digitalWrite(PIN_TX_RELAY, LOW);
    Cmdr.println("TX is OFF");
  }
  else Cmdr.println("Argument error");
  return 0;
}

bool getRXHandler(Commander &Cmdr){
  if(RX_state) Cmdr.println("RX is ON");
  else Cmdr.println("RX is OFF"); 
  return 0;
}

bool setRXHandler(Commander &Cmdr){
  if(Cmdr.containsOn()){
    RX_state = true;
    //digitalWrite(PIN_TX_RELAY, HIGH);
    Cmdr.println("RX is ON"); 
  }
  else if(Cmdr.containsOff()){
    RX_state = false;
    //digitalWrite(PIN_TX_RELAY, LOW);
    Cmdr.println("RX is OFF");
  }
  else Cmdr.println("Argument error");
  return 0;
}

bool getLNAHandler(Commander &Cmdr){
  if(LNA_state) Cmdr.println("LNA is ON");
  else Cmdr.println("LNA is OFF"); 
  return 0;
}

bool setLNAHandler(Commander &Cmdr){
  if(Cmdr.containsOn()){
    LNA_state = true;
    //digitalWrite(PIN_TX_RELAY, HIGH);
    Cmdr.println("LNA is ON"); 
  }
  else if(Cmdr.containsOff()){
    LNA_state = false;
    //digitalWrite(PIN_TX_RELAY, LOW);
    Cmdr.println("LNA is OFF");
  }
  else Cmdr.println("Argument error");
  return 0;
}

bool getSINGLEHandler(Commander &Cmdr){
  if(SINGLE_state) Cmdr.println("Single SDR Port");
  else Cmdr.println("Dual SDR Port"); 
  return 0;
}

bool setSINGLEHandler(Commander &Cmdr){
  if(Cmdr.containsOn()){
    SINGLE_state = true;
    //digitalWrite(PIN_TX_RELAY, HIGH);
    Cmdr.println("Single SDR Port"); 
  }
  else if(Cmdr.containsOff()){
    SINGLE_state = false;
    //digitalWrite(PIN_TX_RELAY, LOW);
    Cmdr.println("Dual SDR Port");
  }
  else Cmdr.println("Argument error");
  return 0;
}


bool getDRVHandler(Commander &Cmdr){
  if(DRIVER_state) Cmdr.println("PA Driver is ON");
  else Cmdr.println("PA Driver is OFF"); 
  return 0;
}

bool setDRVHandler(Commander &Cmdr){
  if(Cmdr.containsOn()){
    DRIVER_state = true;
    digitalWrite(PIN_DRIVER_EN, HIGH);
    Cmdr.println("PA Driver is ON"); 
  }
  else if(Cmdr.containsOff()){
    TX_state = false;
    digitalWrite(PIN_DRIVER_EN, LOW);
    Cmdr.println("PA Driver is OFF");
  }
  else Cmdr.println("Argument error");
  return 0;
}

bool getFANHandler(Commander &Cmdr){
  Cmdr.print("Fan speed = ");
  Cmdr.print(FAN_speed);
  Cmdr.println("%");
  
  return 0;
}


bool setFANHandler(Commander &Cmdr){
  if(Cmdr.getInt(myFloat)){
    if(myInt >= 0 && myInt <= 100){
      FAN_speed = myFloat;
      Fan_PWM(FAN_speed);
      Cmdr.print("Fan speed set to ");
      Cmdr.print(FAN_speed,1);
      Cmdr.println("%");
    }
    else Cmdr.println("Value error");
  }
  else Cmdr.println("Argument error");
  return 0;
}

bool getRPMHandler(Commander &Cmdr){
  Cmdr.print("Fan RPM = ");
  Cmdr.println(FAN_rpm);
 
  return 0;
}

bool getTINTHandler(Commander &Cmdr){
  
  Cmdr.print("Current Temperature = ");
  Cmdr.print(currentTemperature, 1);
  Cmdr.println(" deg C");
  Cmdr.print("Temperature Limit = ");
  Cmdr.print(TEMP_limit, 1);
  Cmdr.println(" deg C");
  if(TEMP_alarm) Cmdr.println("Temperature Alarm is ON");
  else Cmdr.println("Temperature Alarm is OFF"); 

  
  return 0;
}


bool setTINTHandler(Commander &Cmdr){
  if(Cmdr.getFloat(myFloat)){
    if(myFloat >= 0.00 && myFloat < 120.00){
      TEMP_limit = myFloat;
      Cmdr.print("Temperature Limit set to ");
      Cmdr.println(TEMP_limit, 1);
    }
    else Cmdr.println("Value error");
  }
  else Cmdr.println("Argument error");
  return 0;
}

bool getVGGHandler(Commander &Cmdr){
  if(VGG_state) Cmdr.println("VGG is ON");
  else Cmdr.println("VGG is OFF"); 
  float gatevoltage =  analogRead(2) * ( (5.7 * 3.3) / 4095.0);
  Cmdr.print("Gate Voltage:   "); Serial.print(gatevoltage); Serial.println(" V");
  return 0;
}

bool setVGGHandler(Commander &Cmdr){
  if(Cmdr.containsOn()){
    VGG_state = true;
    digitalWrite(PIN_EN_VGG, HIGH);
    Cmdr.println("VGG is ON");
  }
  if(Cmdr.containsOff()){
    VGG_state = false;
    digitalWrite(PIN_EN_VGG, LOW);
    Cmdr.println("VGG is OFF");
  }
  return 0;
}


bool getDACHandler(Commander &Cmdr){
  float gatevoltage;
  Cmdr.print("DAC value = ");
  Cmdr.println(DAC_value);
  gatevoltage = VGG_MIN_VOLTS + (((4095.0 - DAC_value) / 4095.0) * VGG_SPAN_VOLTS);
  Cmdr.print("Calculated Gate Voltage:   "); Serial.print(gatevoltage); Serial.println(" V");
  gatevoltage =  analogRead(2) * ( (5.7 * 3.3) / 4095.0);
  Cmdr.print("Measured Gate Voltage:   "); Serial.print(gatevoltage); Serial.println(" V");
  return 0;
}


bool setDACHandler(Commander &Cmdr){
  if(Cmdr.getInt(myInt)){
    if(myInt >= 0 && myInt < 4096){
      DAC_value = myInt;
      dac.setVoltage( DAC_value, false);
      Cmdr.print("DAC value set to ");
      Cmdr.println(DAC_value);
    }
    else Cmdr.println("Value error");
  }
  else Cmdr.println("Argument error");
  return 0;
}

bool getVOLTSHandler(Commander &Cmdr){
  float gatevoltage;
  Cmdr.print("DAC Value = ");
  Cmdr.println(DAC_value);
  gatevoltage = VGG_MIN_VOLTS + (((4095.0 - DAC_value) / 4095.0) * VGG_SPAN_VOLTS);
  Cmdr.print("Calculated Gate Voltage:   "); Serial.print(gatevoltage); Serial.println(" V");
  gatevoltage =  analogRead(2) * ( (5.7 * 3.3) / 4095.0);
  Cmdr.print("Measured Gate Voltage:   "); Serial.print(gatevoltage); Serial.println(" V");
  return 0;
}


bool setVOLTSHandler(Commander &Cmdr){
  float working;
  if(Cmdr.getFloat(myFloat)){
    if(myFloat >= 2.00 && myFloat < 5.85){
      working =((myFloat - VGG_MIN_VOLTS) / VGG_SPAN_VOLTS) * 4095;
      working = 4095- working;
      DAC_value = (int16_t) working;
      dac.setVoltage( DAC_value, false);
      Cmdr.print("DAC value set to ");
      Cmdr.println(DAC_value);
    }
    else Cmdr.println("Value error");
  }
  else Cmdr.println("Argument error");
  return 0;
}

bool getTEMPHandler(Commander &Cmdr){
 
  Cmdr.print("Temperature:   "); Cmdr.print(temperature.readTemperatureC(),1); Cmdr.println(" deg C");
  return 0;
}

bool getVDDHandler(Commander &Cmdr){
  float busvoltage = ina219.getBusVoltage_V();
  Cmdr.print("Vdd Voltage =  "); Serial.print(busvoltage); Serial.println(" V");
  return 0;
}

bool getIDDHandler(Commander &Cmdr){
  float shuntvoltage = ina219.getShuntVoltage_mV();
  Cmdr.print("Idd Shunt Voltage =  "); Serial.print(shuntvoltage); Serial.println(" mV");
  Cmdr.print("Idd Current =  "); Serial.print(shuntvoltage /10.0); Serial.println(" A");
  return 0;
}

bool getFWDHandler(Commander &Cmdr){
  float fwdvoltage = analogRead(0) * ( 3.3 / 4095.0);
  Cmdr.print("FWD Voltage =  "); Serial.print(fwdvoltage); Serial.println(" V");
  return 0;
}

bool getREVHandler(Commander &Cmdr){
  float revvoltage = analogRead(1) * ( 3.3 / 4095.0);
  Cmdr.print("REV Voltage =  "); Serial.print(revvoltage); Serial.println(" V");
  return 0;
}
