This folder contains the design of the filter within the Receiver PCB, including documents used to design and simulate the band pass flter.

The filter is a 4 pole band pass filter covering 144 to 147 MHz and is used on both transmit and receive

It is designed using the book by Zverev entitled Handbook of Filter Synthesis.  An Excel spreadsheet is used to calculate the values and it provides output to QUCS and Kicad.

Note for QUCS the output worksheet is save in .prn format and this is then renamed to .sch.  It is necessary for filter.dpl to be put in gitignore if using git.
When siluation is completed export the schematic and simualtion results.

Currently the values are input to Kicad manually.