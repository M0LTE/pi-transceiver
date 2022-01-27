# pi-transceiver / software / flow-graphs

SSB_TX.grc is the most basic audio capture -> SSB transmitter I can conceive of in GNU Radio. It can be loaded into GNU Radio Companion 3.8 with gr-limesdr installed. Just set the lime_serial variable (block) value to the serial number of your own LimeSDR, found by running `LimeUtil --find` at the terminal. Click Run, and you'll find a signal at 144.400MHz USB +/- calibration. 

Default gain is 60 (who knows what units), frequency is set in the LimeSDR block.

Tested on Ubuntu 20.04 + GNU Radio installed by following [these (Installing via PPA) instructions](https://github.com/myriadrf/gr-limesdr#linux)

put config.conf in folder ~./.gnuradio if jittery RX audio is encountered, the values in the config file may need tweaking for hardware used.
