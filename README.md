# CollisionBenchmark

A .NET 10 project to benchmark different collision detection algorithms
using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). 
This code was used for my blog post https://shendriks.dev/devlog/2d-shmup/0003-collisions/.

## Overview

This project compares the performance of two common collision detection approaches:

1. **Brute Force**: Every object is checked against every other object.
2. **Spatial Hash**: The environment is divided into a grid of cells. Objects are registered in the cells they occupy,
   and collision checks are only performed between objects in the same or neighboring cells.

## Project Structure

- **CollisionDetection**: A library containing the core logic.
    - `Common`: Shared types like `GameObject` and collision results.
    - `BruteForce`: Implementation of the brute-force detection algorithm.
    - `SpatialHash`: Implementation of the grid-based detection algorithm.
- **CollisionBenchmark**: A console application that runs the benchmarks.
    - Uses `BenchmarkDotNet` to measure execution time and memory usage across different object counts.

## How to Run

### Prerequisites

- .NET 10 SDK

### Running Benchmarks

To run the benchmarks in Release mode (required by BenchmarkDotNet):

```bash
dotnet run -c Release --project CollisionBenchmark/CollisionBenchmark.csproj
```

The results will be displayed in the console and saved to the `BenchmarkDotNet.Artifacts` folder.

## Configuration

The benchmarks are configured in `CollisionBenchmark.cs`. You can adjust:

- `ObjectCount`: The number of game objects to simulate (tested with 10 to 100 in the default configuration).
- `ScreenX` / `ScreenY`: The dimensions of the simulation area.
- `SpatialHashCellSize`: The size of each grid cell in the Spatial Hash algorithm.
