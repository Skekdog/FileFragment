# FileFragment
A brief personal experiment into native Windows app development, using C#, .NET, and WinUI3. Available under the CC0 license. Code here is likely not of a high quality, do whatever you like with it.

A GUI / CLI app that splits a file into multiple packets of a specified size, and can then reconstruct these packets.

## CLI Usage

`FileFragment fragment --input [input file] --output [output directory] --packet-size [packet size]`: Split a file into packets of a specified size.

`FileFragment defragment --input [input directory] --output [output file]`: Defragment packets into a single file.

![FileFragment Icon](./FileFragment.png)