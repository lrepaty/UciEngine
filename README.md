# Introduction

C# dotnet 8.0 Communication via Process class in C# with UCI chess engine
UCI chess engine using UCI protocol https://www.wbec-ridderkerk.nl/html/UCIProtocol.html

# Getting started
## Prerequisites
dotnet 8.0

## Installation
You can install it with the Package Manager in your IDE or alternatively using the command line:

```bash
dotnet add package UciEngine
```
## Usage

```csharp
Uci.UciEngine engine = new Uci.UciEngine(path);
engine.OutputDataReceived += OutputDataReceived;
engine.SendCommand(UciCommand.IsReady);
engine.SetOption("Threads", "4");
engine.SetOption("MultiPV", "4");
engine.SendCommand(UciCommand.UciNewGame);
foreach (KeyValuePair<string, string> kv in engine.Options) 
{
   Console.WriteLine($"option name: {kv.Key} value: {kv.Value}");
}
engine.Dispose();
```

public void OutputDataReceived(object sender, DataReceivedEventArgs e)
{
   response = e.Data;
}
