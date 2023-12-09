<Qucs Schematic 0.0.19>
<Properties>
  <View=46,-19,1713,919,0.95081,0,0>
  <Grid=10,10,1>
  <DataSet=145MHz BPF DesignV4.dat>
  <DataDisplay=145MHz BPF DesignV4.dpl>
  <OpenDisplay=1>
  <Script=145MHz BPF DesignV4.m>
  <RunScript=0>
  <showFrame=0>
  <FrameText0=Title>
  <FrameText1=Drawn By:>
  <FrameText2=Date:>
  <FrameText3=Revision:>
</Properties>
<Symbol>
</Symbol>
<Components>
  <GND * 1 100 540 0 0 0 0>
  <Pac P2 1 1580 460 18 -26 0 1 "2" 1 "50 Ohm" 1 "0 dBm" 0 "1 GHz" 0 "26.85" 0>
  <GND * 1 1580 530 0 0 0 0>
  <.SP SP1 1 600 10 0 71 0 0 "log" 1 "106.8MHz" 1 "198.2MHz" 1 "1001" 1 "no" 0 "1" 0 "2" 0 "no" 0 "no" 0>
  <Eqn Eqn1 1 850 80 -28 15 0 0 "dBS21=dB(S[2,1])" 1 "dBS11=dB(S[1,1])" 1 "group_delay=-diff(unwrap(angle(S[2,1])),2*pi*frequency)" 1 "yes" 0>
  <GND * 1 220 540 0 0 0 0>
  <R R1 1 260 330 15 -26 0 1 "26.0202051422413kOhm" 1 "26.85" 0 "0.0" 0 "0.0" 0 "26.85" 0 "european" 0>
  <GND * 1 300 250 0 0 0 2>
  <C C4 1 290 510 17 -26 0 1 "14.7126572057788pF" 1 "" 0 "neutral" 0>
  <C C5 1 330 310 17 -26 0 1 "8.2pF" 1 "" 0 "neutral" 0>
  <C C3 1 290 440 17 -26 0 1 "18pF" 1 "" 0 "neutral" 0>
  <L L1 1 400 460 17 -26 0 1 "0.0545789133648135uH" 1 "" 0>
  <Pac P1 1 80 430 18 -26 0 1 "1" 1 "50 Ohm" 1 "0 dBm" 0 "1 GHz" 0 "26.85" 0>
  <GND * 1 540 540 0 0 0 0>
  <R R2 1 580 330 15 -26 0 1 "26.0202051422413kOhm" 1 "26.85" 0 "0.0" 0 "0.0" 0 "26.85" 0 "european" 0>
  <GND * 1 620 250 0 0 0 2>
  <C C10 1 650 310 17 -26 0 1 "12pF" 1 "" 0 "neutral" 0>
  <L L2 1 720 460 17 -26 0 1 "0.0545789133648135uH" 1 "" 0>
  <GND * 1 860 540 0 0 0 0>
  <R R3 1 900 330 15 -26 0 1 "26.0202051422413kOhm" 1 "26.85" 0 "0.0" 0 "0.0" 0 "26.85" 0 "european" 0>
  <GND * 1 940 250 0 0 0 2>
  <C C15 1 970 310 17 -26 0 1 "12pF" 1 "" 0 "neutral" 0>
  <L L3 1 1040 460 17 -26 0 1 "0.0545789133648135uH" 1 "" 0>
  <GND * 1 1180 540 0 0 0 0>
  <R R4 1 1220 330 15 -26 0 1 "26.0202051422413kOhm" 1 "26.85" 0 "0.0" 0 "0.0" 0 "26.85" 0 "european" 0>
  <GND * 1 1260 250 0 0 0 2>
  <C C20 1 1290 310 17 -26 0 1 "12pF" 1 "" 0 "neutral" 0>
  <L L4 1 1360 460 17 -26 0 1 "0.0545789133648135uH" 1 "" 0>
  <C C11 1 750 400 -30 -50 0 0 "1.0pF" 1 "" 0 "neutral" 0>
  <C C9 1 610 510 17 -26 0 1 "15.0663741548219pF" 1 "" 0 "neutral" 0>
  <C C8 1 610 440 17 -26 0 1 "22pF" 1 "" 0 "neutral" 0>
  <C C13 1 930 440 17 -26 0 1 "22pF" 1 "" 0 "neutral" 0>
  <C C14 1 930 510 17 -26 0 1 "15.7585895307135pF" 1 "" 0 "neutral" 0>
  <C C18 1 1250 440 17 -26 0 1 "15pF" 1 "" 0 "neutral" 0>
  <C C19 1 1250 510 17 -26 0 1 "12.6684671509507pF" 1 "" 0 "neutral" 0>
  <C C23 1 690 600 17 -26 0 1 "1 pF" 1 "" 0 "neutral" 0>
  <C C2 1 180 400 -30 -50 0 0 "5 pF" 1 "" 0 "neutral" 0>
  <C C22 1 1470 400 -30 -50 0 0 "470 pF" 1 "" 0 "neutral" 0>
  <C C21 1 1390 400 -30 -50 0 0 "2.7 pF" 1 "" 0 "neutral" 0>
  <C C6 1 430 400 -30 -50 0 0 "2pF" 1 "" 0 "neutral" 0>
  <C C7 1 510 400 -30 -50 0 0 "2 pF" 1 "" 0 "neutral" 0>
  <C C12 1 830 400 -30 -50 0 0 "0.75pF" 1 "" 0 "neutral" 0>
  <C C16 1 1070 400 -30 -50 0 0 "1.5 pF" 1 "" 0 "neutral" 0>
  <C C17 1 1150 400 -30 -50 0 0 "1.5pF" 1 "" 0 "neutral" 0>
  <C C1 1 100 390 -30 -50 0 0 "470 pF" 1 "" 0 "neutral" 0>
  <MLIN MS1 1 470 250 -26 15 0 0 "Subst1" 1 "0.4 mm" 1 "14 mm" 1 "Hammerstad" 0 "Kirschning" 0 "26.85" 0>
  <MLIN MS2 1 770 250 -26 15 0 0 "Subst1" 1 "0.4 mm" 1 "3.3 mm" 1 "Hammerstad" 0 "Kirschning" 0 "26.85" 0>
  <MLIN MS3 1 1090 250 -26 15 0 0 "Subst1" 1 "0.4 mm" 1 "16 mm" 1 "Hammerstad" 0 "Kirschning" 0 "26.85" 0>
  <MLIN MS4 1 1440 230 -26 15 0 0 "Subst1" 1 "0.4 mm" 1 "14 mm" 1 "Hammerstad" 0 "Kirschning" 0 "26.85" 0>
  <MLIN MS5 1 160 250 -26 15 0 0 "Subst1" 1 "0.4 mm" 1 "14 mm" 1 "Hammerstad" 0 "Kirschning" 0 "26.85" 0>
  <MLIN MS6 1 390 160 -26 15 0 0 "Subst1" 1 "0.4 mm" 1 "14 mm" 1 "Hammerstad" 0 "Kirschning" 0 "26.85" 0>
  <SUBST Subst1 1 1100 750 -30 24 0 0 "4.6" 1 "0.21 mm" 1 "35 um" 1 "0.02" 1 "0.022e-8" 1 "0.15e-8" 1>
</Components>
<Wires>
  <1580 390 1580 430 "" 0 0 0 "">
  <1580 490 1580 530 "" 0 0 0 "">
  <330 340 330 400 "" 0 0 0 "">
  <280 400 290 400 "" 0 0 0 "">
  <290 400 330 400 "" 0 0 0 "">
  <290 400 290 410 "" 0 0 0 "">
  <290 470 290 480 "" 0 0 0 "">
  <220 540 290 540 "" 0 0 0 "">
  <260 400 280 400 "" 0 0 0 "">
  <260 360 260 400 "" 0 0 0 "">
  <260 250 260 300 "" 0 0 0 "">
  <330 250 330 280 "" 0 0 0 "">
  <300 250 330 250 "" 0 0 0 "">
  <260 250 300 250 "" 0 0 0 "">
  <290 540 400 540 "" 0 0 0 "">
  <400 400 420 400 "" 0 0 0 "">
  <330 400 360 400 "" 0 0 0 "">
  <400 540 540 540 "" 0 0 0 "">
  <400 490 400 540 "" 0 0 0 "">
  <80 460 100 460 "" 0 0 0 "">
  <100 460 100 540 "" 0 0 0 "">
  <650 340 650 400 "" 0 0 0 "">
  <610 400 650 400 "" 0 0 0 "">
  <610 400 610 410 "" 0 0 0 "">
  <610 470 610 480 "" 0 0 0 "">
  <540 540 610 540 "" 0 0 0 "">
  <580 400 610 400 "" 0 0 0 "">
  <580 360 580 400 "" 0 0 0 "">
  <580 250 580 300 "" 0 0 0 "">
  <650 250 650 280 "" 0 0 0 "">
  <620 250 650 250 "" 0 0 0 "">
  <580 250 620 250 "" 0 0 0 "">
  <720 400 740 400 "" 0 0 0 "">
  <650 400 720 400 "" 0 0 0 "">
  <720 400 720 430 "" 0 0 0 "">
  <720 540 860 540 "" 0 0 0 "">
  <720 490 720 540 "" 0 0 0 "">
  <530 400 540 400 "" 0 0 0 "">
  <970 340 970 400 "" 0 0 0 "">
  <930 400 970 400 "" 0 0 0 "">
  <930 400 930 410 "" 0 0 0 "">
  <930 470 930 480 "" 0 0 0 "">
  <860 540 930 540 "" 0 0 0 "">
  <900 400 930 400 "" 0 0 0 "">
  <900 360 900 400 "" 0 0 0 "">
  <900 250 900 300 "" 0 0 0 "">
  <970 250 970 280 "" 0 0 0 "">
  <940 250 970 250 "" 0 0 0 "">
  <900 250 940 250 "" 0 0 0 "">
  <1040 400 1060 400 "" 0 0 0 "">
  <970 400 1040 400 "" 0 0 0 "">
  <1040 400 1040 430 "" 0 0 0 "">
  <1040 540 1180 540 "" 0 0 0 "">
  <1040 490 1040 540 "" 0 0 0 "">
  <850 400 860 400 "" 0 0 0 "">
  <1290 340 1290 400 "" 0 0 0 "">
  <1250 400 1290 400 "" 0 0 0 "">
  <1250 400 1250 410 "" 0 0 0 "">
  <1250 470 1250 480 "" 0 0 0 "">
  <1180 540 1250 540 "" 0 0 0 "">
  <1220 400 1250 400 "" 0 0 0 "">
  <1220 360 1220 400 "" 0 0 0 "">
  <1220 250 1220 300 "" 0 0 0 "">
  <1290 250 1290 280 "" 0 0 0 "">
  <1260 250 1290 250 "" 0 0 0 "">
  <1220 250 1260 250 "" 0 0 0 "">
  <1360 400 1380 400 "" 0 0 0 "">
  <1290 400 1360 400 "" 0 0 0 "">
  <1360 400 1360 430 "" 0 0 0 "">
  <1360 540 1500 540 "" 0 0 0 "">
  <1360 490 1360 540 "" 0 0 0 "">
  <1170 400 1180 400 "" 0 0 0 "">
  <610 540 690 540 "" 0 0 0 "">
  <930 540 1040 540 "" 0 0 0 "">
  <1250 540 1360 540 "" 0 0 0 "">
  <690 540 720 540 "" 0 0 0 "">
  <690 540 690 570 "" 0 0 0 "">
  <690 630 690 660 "" 0 0 0 "">
  <690 660 800 660 "" 0 0 0 "">
  <540 400 580 400 "" 0 0 0 "">
  <860 400 900 400 "" 0 0 0 "">
  <1180 400 1220 400 "" 0 0 0 "">
  <1500 390 1580 390 "" 0 0 0 "">
  <1500 390 1500 400 "" 0 0 0 "">
  <210 400 260 400 "" 0 0 0 "">
  <1440 400 1450 400 "" 0 0 0 "">
  <70 390 70 400 "" 0 0 0 "">
  <70 400 80 400 "" 0 0 0 "">
  <150 280 150 400 "" 0 0 0 "">
  <150 280 180 280 "" 0 0 0 "">
  <440 250 440 360 "" 0 0 0 "">
  <440 360 460 360 "" 0 0 0 "">
  <460 360 460 400 "" 0 0 0 "">
  <480 340 480 400 "" 0 0 0 "">
  <480 340 500 340 "" 0 0 0 "">
  <500 250 500 340 "" 0 0 0 "">
  <780 350 780 400 "" 0 0 0 "">
  <800 340 800 400 "" 0 0 0 "">
  <800 340 810 340 "" 0 0 0 "">
  <1060 250 1060 350 "" 0 0 0 "">
  <1060 350 1100 350 "" 0 0 0 "">
  <1100 350 1100 400 "" 0 0 0 "">
  <1120 250 1120 400 "" 0 0 0 "">
  <1410 230 1410 350 "" 0 0 0 "">
  <1410 350 1420 350 "" 0 0 0 "">
  <1420 350 1420 400 "" 0 0 0 "">
  <1450 320 1450 400 "" 0 0 0 "">
  <1450 320 1480 320 "" 0 0 0 "">
  <1480 230 1480 320 "" 0 0 0 "">
  <1470 230 1480 230 "" 0 0 0 "">
  <810 250 810 340 "" 0 0 0 "">
  <800 250 810 250 "" 0 0 0 "">
  <740 350 780 350 "" 0 0 0 "">
  <740 250 740 350 "" 0 0 0 "">
  <180 250 180 280 "" 0 0 0 "">
  <180 250 190 250 "" 0 0 0 "">
  <130 250 130 390 "" 0 0 0 "">
  <360 400 400 400 "" 0 0 0 "">
  <360 160 360 400 "" 0 0 0 "">
  <420 160 420 360 "" 0 0 0 "">
  <380 360 420 360 "" 0 0 0 "">
  <380 360 380 430 "" 0 0 0 "">
  <380 430 400 430 "" 0 0 0 "">
  <400 400 400 430 "" 0 0 0 "">
</Wires>
<Diagrams>
</Diagrams>
<Paintings>
</Paintings>
