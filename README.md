# StREberryTag

## Name

reberry - Audio tag fixer/updater.

## Synopsis

`reberry <album-path> [-r|--released] [-t|--tracks] [-d|--discs] [-c|--cover]`

## Description

A console application that updates the tags on audio files to make sure it has values that the **Strawberry Music Player** expects.

## Arguments

**AlbumPath**

Path to the album folder on the file system, that contains the audio files to be updated.

## Options

**-m,--comment** \<string\>

An optional comment to add to all tracks in the album.

**-g,--genre** \<string\>

The genre of the album to add to each track.

**-p,--copyright** \<string\>

Copyright information for the album.

**-r,--released** \<number\>

The year (as an integer) that the album was released.

**-t,--tracks** \<number\>

The total track count of the album, as an integer.

**-d,--disks** \<number\>

The total disks of the album, as an integer.

**-c,--cover** \<image\>

The string path to the cover album art image.

## Example Usage

Simple Usage

```bash
/path/to/album/folder/ \
    --released 2001 \
    --tracks 6 \
    --discs 1 \
    --cover "/path/to/cover/art.jpeg"
```
