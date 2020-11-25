# Byndyusoft.Net.Http.MessagePack
 Provides extension methods for System.Net.Http.HttpClient and System.Net.Http.HttpContent that perform automatic serialization and deserialization using MessagePack.

[![(License)](https://img.shields.io/github/license/Byndyusoft/Byndyusoft.Net.Http.MessagePack.svg)](LICENSE.txt)
[![Nuget](http://img.shields.io/nuget/v/Byndyusoft.Net.Http.MessagePack.svg?maxAge=10800)](https://www.nuget.org/packages/Byndyusoft.Net.Http.MessagePack/) [![NuGet downloads](https://img.shields.io/nuget/dt/Byndyusoft.Net.Http.MessagePack.svg)](https://www.nuget.org/packages/Byndyusoft.Net.Http.MessagePack/) 

[MessagePack](https://www.nuget.org/packages/MessagePack/) is an efficient binary serialization format. It lets you exchange data among multiple languages like JSON. But it's faster and smaller. 
Small integers are encoded into a single byte, and typical short strings require only one extra byte in addition to the strings themselves.

```Byndyusoft.Net.Http.MessagePack``` actually depends on ```Microsoft.Net.Http```, and extends the ```HttpClient``` with ```MessagePack```
features that you would likely need to talk to a RESTful service such as ASP.NET Web API.

Package operates in the ```System.Net.Http``` namespace and adds some handy extension methods to ```HttpClient``` and ```HttpContent```.

So for example:

```
using (var client = new HttpClient())
{
    var product = await client.GetFromMessagePackAsync<Product>("http://localhost/api/products/1");
}
```
or
```
using (var client = new HttpClient())
{
    var response = await _client.GetAsync("http://localhost/api/products/1");
    response.EnsureSuccessStatusCode();
    var product = await response.Content.ReadFromMessagePackAsync<Product>();
}
```
or
```
using (var client = new HttpClient())
{
    var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/products/1");
    request.Content = MessagePackContent.Create(new Product());
    var response = await _client.SendAsync(request);
    response.EnsureSuccessStatusCode();
}
```

If you tried to just use ```Microsoft.Net.Http```, the ```GetFromMessagePackAsync``` method wouldn't be available to you, and you'd only be able to read the content 
as raw data such as bytes or string, and have to do the serializing / de-serializing yourself.

You also get extension methods to PUT / POST back to the service in ```MessagePack``` format without having to do that yourself:

```
// Save the ProductInfo model back to the API service
await client.PutAsMessagePackAsync("http://localhost/api/products/1", product);
await client.PostAsMessagePackAsync("http://localhost/api/products/1", product);
```

## Installing

```shell
dotnet add package Byndyusoft.Net.Http.MessagePack
```

# Contributing

To contribute, you will need to setup your local environment, see [prerequisites](#prerequisites). For the contribution and workflow guide, see [package development lifecycle](#package-development-lifecycle).

A detailed overview on how to contribute can be found in the [contributing guide](CONTRIBUTING.md).

## Prerequisites

Make sure you have installed all of the following prerequisites on your development machine:

- Git - [Download & Install Git](https://git-scm.com/downloads). OSX and Linux machines typically have this already installed.
- .NET Core (version 3.1 or higher) - [Download & Install .NET Core](https://dotnet.microsoft.com/download/dotnet-core/3.1).

## General folders layout

### src
- source code

### tests

- unit-tests

### example

- example console application

## Package development lifecycle

- Implement package logic in `src`
- Add or addapt unit-tests (prefer before and simultaneously with coding) in `tests`
- Add or change the documentation as needed
- Open pull request in the correct branch. Target the project's `master` branch

# Maintainers

[github.maintain@byndyusoft.com](mailto:github.maintain@byndyusoft.com)