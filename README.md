# XPR True Hole Conversion API

Use this XPR True Hole client for .NET in your CNC application to allow operators to quickly convert eligible holes in XPR parts into bolt-ready holes.

This client library uses the XPR True Hole Conversion API at **https://developer.hypertherm.com** 

-------------

### [Get an API key](https://developer.hypertherm.com/get-api-key)
Before you can use this API, you need a Hypertherm Developer Portal account and a key.


### [Install the NuGet package](https://www.nuget.org/packages/XprTrueHoleHttpClient/)
Using NuGet Package Manager Console:
```
PM> Install-Package XprTrueHoleHttpClient
```

_**OR**_ using the .NET Core tools:
```
> dotnet add package XprTrueHoleHttpClient
```

_**OR**_ search for *XprTrueHoleHttpClient* in your IDE's NuGet package manager.


### Basic use
The following examples use C#.

#### Pass file content
```
using Hypertherm.XprTrueHoleHttpClient;

// Create an instance of XprTrueHoleClient
XprTrueHoleClient client = new XprTrueHoleClient("<My Subscription Key>");

// Convert a non-True Hole XPR part into a True Hole XPR part
String trueHolePart = client.Convert("<My Settings String>", "<My XPR Part String>");
```
_**OR**_ 
#### Pass file path
```
using Hypertherm.XprTrueHoleHttpClient;

// Create an instance of XprTrueHoleClient
XprTrueHoleClient client = new XprTrueHoleClient("<My Subscription Key>");

// Convert a non-True Hole XPR part into a True Hole XPR by passing part file paths
String trueHolePart = client.Convert(File.ReadAllText("<My Settings File Path>"), File.ReadAllText("<My XPR Part File Path>"));
```
  
See the **[Hypertherm Developer Portal](https://developer.hypertherm.com)** for more details.



