#!/usr/bin/env python3
# -*- coding: utf-8 -*-

#
# SPDX-License-Identifier: GPL-3.0
#
# GNU Radio Python Flow Graph
# Title: FM receiver
# Author: Lime Microsystems
# GNU Radio version: 3.8.1.0

from gnuradio import analog
from gnuradio import audio
from gnuradio import blocks
from gnuradio import filter
from gnuradio.filter import firdes
from gnuradio import gr
import sys
import signal
from argparse import ArgumentParser
from gnuradio.eng_arg import eng_float, intx
from gnuradio import eng_notation
import limesdr

class WBFM_Test(gr.top_block):

    def __init__(self):
        gr.top_block.__init__(self, "FM receiver")

        ##################################################
        # Variables
        ##################################################
        self.volume = volume = 1
        self.trans_width = trans_width = 210000
        self.samp_rate = samp_rate = 2e6
        self.cut_freq = cut_freq = 28000

        ##################################################
        # Blocks
        ##################################################
        self.rational_resampler_xxx_1_0_1 = filter.rational_resampler_ccc(
                interpolation=48,
                decimation=200,
                taps=None,
                fractional_bw=None)
        self.low_pass_filter_0_1 = filter.fir_filter_ccf(
            1,
            firdes.low_pass(
                1,
                2e6,
                cut_freq,
                trans_width,
                firdes.WIN_HAMMING,
                6.76))
        self.limesdr_source_0_0 = limesdr.source('1D588FD736569E', 0, '', False)


        self.limesdr_source_0_0.set_sample_rate(samp_rate)


        self.limesdr_source_0_0.set_center_freq(94.2e6, 0)

        self.limesdr_source_0_0.set_bandwidth(1.5e6, 0)


        self.limesdr_source_0_0.set_digital_filter(samp_rate, 0)


        self.limesdr_source_0_0.set_gain(60, 0)


        self.limesdr_source_0_0.set_antenna(255, 0)
        self.blocks_multiply_const_vxx_0_1 = blocks.multiply_const_ff(volume)
        self.audio_sink_0_0 = audio.sink(44100, '', True)
        self.analog_wfm_rcv_0_1 = analog.wfm_rcv(
        	quad_rate=480e3,
        	audio_decimation=10,
        )



        ##################################################
        # Connections
        ##################################################
        self.connect((self.analog_wfm_rcv_0_1, 0), (self.blocks_multiply_const_vxx_0_1, 0))
        self.connect((self.blocks_multiply_const_vxx_0_1, 0), (self.audio_sink_0_0, 0))
        self.connect((self.limesdr_source_0_0, 0), (self.low_pass_filter_0_1, 0))
        self.connect((self.low_pass_filter_0_1, 0), (self.rational_resampler_xxx_1_0_1, 0))
        self.connect((self.rational_resampler_xxx_1_0_1, 0), (self.analog_wfm_rcv_0_1, 0))

    def get_volume(self):
        return self.volume

    def set_volume(self, volume):
        self.volume = volume
        self.blocks_multiply_const_vxx_0_1.set_k(self.volume)

    def get_trans_width(self):
        return self.trans_width

    def set_trans_width(self, trans_width):
        self.trans_width = trans_width
        self.low_pass_filter_0_1.set_taps(firdes.low_pass(1, 2e6, self.cut_freq, self.trans_width, firdes.WIN_HAMMING, 6.76))

    def get_samp_rate(self):
        return self.samp_rate

    def set_samp_rate(self, samp_rate):
        self.samp_rate = samp_rate
        self.limesdr_source_0_0.set_digital_filter(self.samp_rate, 0)
        self.limesdr_source_0_0.set_digital_filter(self.samp_rate, 1)

    def get_cut_freq(self):
        return self.cut_freq

    def set_cut_freq(self, cut_freq):
        self.cut_freq = cut_freq
        self.low_pass_filter_0_1.set_taps(firdes.low_pass(1, 2e6, self.cut_freq, self.trans_width, firdes.WIN_HAMMING, 6.76))



def main(top_block_cls=WBFM_Test, options=None):
    tb = top_block_cls()

    def sig_handler(sig=None, frame=None):
        tb.stop()
        tb.wait()
        sys.exit(0)

    signal.signal(signal.SIGINT, sig_handler)
    signal.signal(signal.SIGTERM, sig_handler)

    tb.start()
    try:
        input('Press Enter to quit: ')
    except EOFError:
        pass
    tb.stop()
    tb.wait()


if __name__ == '__main__':
    main()
