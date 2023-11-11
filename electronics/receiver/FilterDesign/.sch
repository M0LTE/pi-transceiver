<Qucs Schematic 0.0.19>
<Properties>
  <View=46,-79,1713,923,0.95081,0,0>
  <Grid=10,10,1>
  <DataSet=filter.dat>
  <DataDisplay=filter.dpl>
  <OpenDisplay=1>
  <Script=filter.m>
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
  <R R1 1 260 330 15 -26 0 1 "25.9671026827673kOhm" 1 "26.85" 0 "0.0" 0 "0.0" 0 "26.85" 0 "european" 0>
  <GND * 1 300 250 0 0 0 2>
  <C C4 1 290 510 17 -26 0 1 "15.9600044206872pF" 1 "" 0 "neutral" 0>
  <C C7 1 510 400 -30 -50 0 0 "0.75pF" 1 "" 0 "neutral" 0>
  <C C5 1 330 310 17 -26 0 1 "8.2pF" 1 "" 0 "neutral" 0>
  <C C3 1 290 440 17 -26 0 1 "18pF" 1 "" 0 "neutral" 0>
  <C C6 1 430 400 -30 -50 0 0 "3.9pF" 1 "" 0 "neutral" 0>
  <L L1 1 400 460 17 -26 0 1 "0.0544675278273342uH" 1 "" 0>
  <C C1 1 110 400 -30 -50 0 0 "15.0pF" 1 "" 0 "neutral" 0>
  <Pac P1 1 80 430 18 -26 0 1 "1" 1 "50 Ohm" 1 "0 dBm" 0 "1 GHz" 0 "26.85" 0>
  <C C2 1 180 400 -30 -50 0 0 "6.8pF" 1 "" 0 "neutral" 0>
  <GND * 1 540 540 0 0 0 0>
  <R R2 1 580 330 15 -26 0 1 "25.9671026827673kOhm" 1 "26.85" 0 "0.0" 0 "0.0" 0 "26.85" 0 "european" 0>
  <GND * 1 620 250 0 0 0 2>
  <C C10 1 650 310 17 -26 0 1 "12pF" 1 "" 0 "neutral" 0>
  <L L2 1 720 460 17 -26 0 1 "0.0544675278273342uH" 1 "" 0>
  <GND * 1 860 540 0 0 0 0>
  <R R3 1 900 330 15 -26 0 1 "25.9671026827673kOhm" 1 "26.85" 0 "0.0" 0 "0.0" 0 "26.85" 0 "european" 0>
  <GND * 1 940 250 0 0 0 2>
  <C C15 1 970 310 17 -26 0 1 "12pF" 1 "" 0 "neutral" 0>
  <L L3 1 1040 460 17 -26 0 1 "0.0544675278273342uH" 1 "" 0>
  <GND * 1 1180 540 0 0 0 0>
  <R R4 1 1220 330 15 -26 0 1 "25.9671026827673kOhm" 1 "26.85" 0 "0.0" 0 "0.0" 0 "26.85" 0 "european" 0>
  <GND * 1 1260 250 0 0 0 2>
  <C C20 1 1290 310 17 -26 0 1 "12pF" 1 "" 0 "neutral" 0>
  <L L4 1 1360 460 17 -26 0 1 "0.0544675278273342uH" 1 "" 0>
  <C C11 1 750 400 -30 -50 0 0 "0.9pF" 1 "" 0 "neutral" 0>
  <C C12 1 830 400 -30 -50 0 0 "0.5pF" 1 "" 0 "neutral" 0>
  <C C9 1 610 510 17 -26 0 1 "15.2790476353989pF" 1 "" 0 "neutral" 0>
  <C C8 1 610 440 17 -26 0 1 "22pF" 1 "" 0 "neutral" 0>
  <C C13 1 930 440 17 -26 0 1 "22pF" 1 "" 0 "neutral" 0>
  <C C16 1 1070 400 -30 -50 0 0 "0.8pF" 1 "" 0 "neutral" 0>
  <C C17 1 1150 400 -30 -50 0 0 "0.75pF" 1 "" 0 "neutral" 0>
  <C C21 1 1390 400 -30 -50 0 0 "6.8pF" 1 "" 0 "neutral" 0>
  <C C22 1 1470 400 -30 -50 0 0 "3.9pF" 1 "" 0 "neutral" 0>
  <C C14 1 930 510 17 -26 0 1 "15.9869169128157pF" 1 "" 0 "neutral" 0>
  <C C18 1 1250 440 17 -26 0 1 "15pF" 1 "" 0 "neutral" 0>
  <C C19 1 1250 510 17 -26 0 1 "13.4871251048767pF" 1 "" 0 "neutral" 0>
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
  <460 400 480 400 "" 0 0 0 "">
  <260 400 280 400 "" 0 0 0 "">
  <260 360 260 400 "" 0 0 0 "">
  <260 250 260 300 "" 0 0 0 "">
  <330 250 330 280 "" 0 0 0 "">
  <300 250 330 250 "" 0 0 0 "">
  <260 250 300 250 "" 0 0 0 "">
  <290 540 400 540 "" 0 0 0 "">
  <400 400 420 400 "" 0 0 0 "">
  <330 400 400 400 "" 0 0 0 "">
  <400 400 400 430 "" 0 0 0 "">
  <400 540 540 540 "" 0 0 0 "">
  <400 490 400 540 "" 0 0 0 "">
  <80 460 100 460 "" 0 0 0 "">
  <100 460 100 540 "" 0 0 0 "">
  <210 400 260 400 "" 0 0 0 "">
  <140 400 150 400 "" 0 0 0 "">
  <650 340 650 400 "" 0 0 0 "">
  <610 400 650 400 "" 0 0 0 "">
  <610 400 610 410 "" 0 0 0 "">
  <610 470 610 480 "" 0 0 0 "">
  <540 540 610 540 "" 0 0 0 "">
  <780 400 800 400 "" 0 0 0 "">
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
  <540 400 580 400 "" 0 0 0 "">
  <530 400 540 400 "" 0 0 0 "">
  <970 340 970 400 "" 0 0 0 "">
  <930 400 970 400 "" 0 0 0 "">
  <930 400 930 410 "" 0 0 0 "">
  <930 470 930 480 "" 0 0 0 "">
  <860 540 930 540 "" 0 0 0 "">
  <1100 400 1120 400 "" 0 0 0 "">
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
  <1420 400 1440 400 "" 0 0 0 "">
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
  <1500 390 1500 400 "" 0 0 0 "">
  <1500 390 1580 390 "" 0 0 0 "">
  <860 400 900 400 "" 0 0 0 "">
  <610 540 720 540 "" 0 0 0 "">
  <1180 400 1220 400 "" 0 0 0 "">
  <930 540 1040 540 "" 0 0 0 "">
  <1250 540 1360 540 "" 0 0 0 "">
</Wires>
<Diagrams>
</Diagrams>
<Paintings>
</Paintings>
