This is the proposed design for the Receiver PCB including Low Noise Amplifier, Band Pass Filter and PIN diode switching.
The band pass filter is 4 pole butterworth with 1dB bandwidth of 144.0 to 147.0 MHz.  It is switched between receive and transmit paths. 
The low noise amplifier has a 2dB noise figure.  It has protection against antenna relay leakage and close by transmitters.  It has a gain of 25dB.  Provision is made to attenuate the Receiver gain by a fixed amount of 20dB to optimse strong signal performance.
RFswitching is provided for Receive/Transmit switching.  A monitor channel is provided to allow the LimeSDR Mini receiver to monitor the attenuated transmit signal.
The switching is capable of allowing use of a half duplex SDR in place of the LimeSDR Mini.
The inductors in the 4 pole bandpas filter are screened using 22mm pipe caps (copper).  With care and sutable shielding using kapton tape or PTFE thermal insulation it should be possible to solder these after the board has been fitted with all smd devices and tested.  I am not sure but it may be necessary to fit the trimmer capacitors last.

