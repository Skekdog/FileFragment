# FileFragment

A CLI app that splits a file into multiple packets of a specified size, and can then reconstruct these packets.

## Usage

`FileFragment fragment --input [input file] --output [output directory] --packet-size [packet size]`: Split a file into packets of a specified size.

`FileFragment defragment --input [input directory] --output [output file]`: Defragment packets into a single file.