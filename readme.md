# .NET 8 SDK Installation & Build Guide

This guide walks you through installing the .NET 8 SDK and building a .NET application using the command line.

---

## 🛠️ Prerequisites

Make sure you have the .NET 8 SDK installed.

To check: run cmd and write

dotnet --version

## Instalation of .NET 8 SDK

1. Donload SDK from https://dotnet.microsoft.com/en-us/download/dotnet/8.0
2. Install running .exe file
3. Check after instalation writing in command line
<pre>
dotnet --version
</pre>

## Building application

1. Open Command Prompt and change to your project directory:
<pre>
cd path\to\your\project
</pre>

2. Restore dependencies (optional but recommended)
<pre>
dotnet restore
</pre>

3. Build and run project
<pre>
dotnet build
cd ReviewExtractor
dotnet run
</pre>
