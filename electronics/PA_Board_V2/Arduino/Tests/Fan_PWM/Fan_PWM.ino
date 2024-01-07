/* Fan PWM control for RADARC Pi Transceiver PA board
 * Version 2.0 December 2023
 * Richard Ibbotson
 */

void Fan_Init(void)
{
   REG_GCLK_GENDIV = GCLK_GENDIV_DIV(1) |          // Divide the 48MHz clock source by divisor N=1: 48MHz/1=48MHz
                    GCLK_GENDIV_ID(4);            // Select Generic Clock (GCLK) 4
  while (GCLK->STATUS.bit.SYNCBUSY);              // Wait for synchronization

  REG_GCLK_GENCTRL = GCLK_GENCTRL_IDC |           // Set the duty cycle to 50/50 HIGH/LOW
                     GCLK_GENCTRL_GENEN |         // Enable GCLK4
                     GCLK_GENCTRL_SRC_DFLL48M |   // Set the 48MHz clock source
                     GCLK_GENCTRL_ID(4);          // Select GCLK4
  while (GCLK->STATUS.bit.SYNCBUSY);              // Wait for synchronization

  // Enable the port multiplexer for the digital pin D14 **** g_APinDescription() converts Arduino Pin to SAMD21 pin
  PORT->Group[g_APinDescription[PIN_FAN_PWM].ulPort].PINCFG[g_APinDescription[PIN_FAN_PWM].ulPin].bit.PMUXEN = 1;
 
  
  // Connect the TCC0 timer to digital output D14 - port pins are paired odd PMUO and even PMUXE
 

  PORT->Group[g_APinDescription[PIN_FAN_PWM].ulPort].PMUX[g_APinDescription[PIN_FAN_PWM].ulPin >> 1].reg = PORT_PMUX_PMUXE_F; // D1 is on PA18 = even, use device F on TCC0/WO[2]

  // Feed GCLK4 to TCC0 and TCC1
  REG_GCLK_CLKCTRL = GCLK_CLKCTRL_CLKEN |         // Enable GCLK4 to TCC0 and TCC1
                     GCLK_CLKCTRL_GEN_GCLK4 |     // Select GCLK4
                     GCLK_CLKCTRL_ID_TCC0_TCC1;   // Feed GCLK4 to TCC0 and TCC1
  while (GCLK->STATUS.bit.SYNCBUSY);              // Wait for synchronization

  // Dual slope PWM mode
  REG_TCC0_WAVE |= TCC_WAVE_POL(0xF) |           // Reverse the output polarity on all TCC0 outputs
                    TCC_WAVE_WAVEGEN_DSBOTTOM;   // Setup dual slope PWM on TCC0
  while (TCC0->SYNCBUSY.bit.WAVE);               // Wait for synchronization

  // Timer counts up to a maximum or TOP value set by the PER register,
  // this determines the frequency of the PWM operation: Freq = 48Mhz/(PER * 2)
    REG_TCC0_PER = 960;                           // Set the FreqTcc of the PWM on TCC1 to 24Khz
  while (TCC0->SYNCBUSY.bit.PER);                 // Wait for synchronization
 
  // Set the PWM signal to output , PWM ds = 2*N(TOP-CCx)/Freqtcc => PWM=0 => CCx=PER, PWM=50% => CCx = PER/2
  REG_TCC0_CCB2 = 80;                             // TCC0 CCB0 - on D14  50%
  while (TCC0->SYNCBUSY.bit.CCB2);                // Wait for synchronization
  
 
  // Divide the GCLOCK signal by 1 giving  in this case 48MHz (20.83ns) TCC1 timer tick and enable the outputs
  REG_TCC0_CTRLA |= TCC_CTRLA_PRESCALER_DIV1 |    // Divide GCLK4 by 1
                    TCC_CTRLA_ENABLE;             // Enable the TCC0 output
  while (TCC0->SYNCBUSY.bit.ENABLE);              // Wait for synchronization

}
void Fan_PWM(uint8_t percent)
{
  REG_TCC0_CCB2 = percent * 10;                   // TCC0 CCB2 - on PIN_FAN_PWM- PWM signalling
  while (TCC0->SYNCBUSY.bit.CCB2);                // Wait for synchronization
}


void setup() {

  Fan_Init(); //initialise PWM for Fan to 25kHz
  //Fan_PWM(50);
}

void loop() {
  // put your main code here, to run repeatedly:

}
