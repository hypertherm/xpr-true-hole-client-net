# XPR True Hole Conversion API

Use this True Hole client for .NET in your CNC application to allow operators to quickly convert eligible holes in XPR parts into bolt-ready holes.

This client library uses the XPR True Hole Conversion API available at https://developer.hypertherm.com 

-------------

### [Get an API key](https://developer.hypertherm.com/get-api-key)
Before you can use this API, you need a Hypertherm API developer portal account and a key.


### Install the [NuGet package](https://www.nuget.org/packages/TrueHoleHttpClient/)
Using NuGet Package Manager Console:
```
PM> Install-Package TrueHoleHttpClient
```

_**OR**_ using the .NET Core tools:
```
> dotnet add package TrueHoleHttpClient
```

_**OR**_ search for *TrueHoleHttpClient* in your IDE's NuGet package manager.


### Basic use
```
using Hypertherm.TrueHoleHttpClient;

// Create an instance of TrueHoleClient
TrueHoleClient client = new TrueHoleClient("<My Subscription Key>");

// Convert a non-True Hole XPR part into a True Hole XPR part
String trueHolePart = client.Convert("<My Settings String>", "<My XPR Part String>");
```

See the [Hypertherm Developer Portal](https://developer.hypertherm.com) for more details.



