# Board Files for Pi Transciever PA Board

The Arduino environment uses board files and variants to configure for different processors and board pinouts.
We use the board files for the Adafruit SAMD boards and modify to add Pi transceiver board. (This is a bit messy and could be improved with proper custom board files when stable)

## Installation

- Install the latest arduino IDE: [Install IDE 2](https://docs.arduino.cc/software/ide-v2/tutorials/getting-started/ide-v2-downloading-and-installing)

- Install the Adafruit SAMD board support into Arduino board manager [Adafruit SAMD](https://learn.adafruit.com/adafruit-trinket-m0-circuitpython-arduino/arduino-ide-setup)

- Install the Adafruit SAMD boards inside Arduino Board Manager [Board Manager](https://learn.adafruit.com/adafruit-trinket-m0-circuitpython-arduino/using-with-arduino-ide) Follow this tutorial until the point where you have reboted the PC then stop.

- Find where the board files have been installed. There will be a directory like this "C:\Users\'your_user_name'\AppData\Local\Arduino15\packages\adafruit\hardware\samd\1.7.10" which contains a file "boards.txt" and "variants" directory.

- Replace the "boards.txt" file in the directory found above with the one in this Github directory.

- Add the "Pi_Tx_PA" and the "Pi_Tx_PA_V2" directories from the "Board/variants" of this Github repository. into the variants directory found above.

- Restart PC and open Arduino IDE

- Plug in Pi Transceiver PA board to PC using USB connection. PC should enumerate the board.

- Select Board type as RADARC Pi_Tx_PA under "tools/board/Adafruit SAMD"

- Select the Comport which shows the RADARC Pi_Tx_PA connected

- Add the additional Arduino Libraries listed in the READ_ME if not already installed.

-  Compile and Upload code or tests from this Github repository


## Issues

1.  If you subsequently update the Adafruit Boards in board manager then the "boards.txt" will most likely be changed by the update and the steps above to replace the boards.txt with our version will have to done again. Best to not update the Adafruit SAMD boards when offered.

2. The Pi_Tx_PA has the same USB VID and PID as the Adafruit Trinket M0 and the Adafruit PiRkey (SAMD) which means the serial port often needs to be reset to the correct COM Port. This may be fixed by Arduino/Adafruit or may been a custom bootlodare with unique VID PID
    