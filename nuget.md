# Pitney Bowes Shipping API C# Client

This package provides an easy to use, developer centric .net interface to the Pitney Bowes Shipping API.
The package uses the [.NET Standard 2.0 framework](https://docs.microsoft.com/en-us/dotnet/standard/net-standard), meaning 
it is compatible with:
* .NET core 2.0 and above
* .NET framework 4.6.1 and above

Mono and Xamarin are also supported.

## Features:
* **full wrapping of the API**. Hides (encapsulates) all protocol details, including authentication and report pagination.
* **strong typing** objects for all entities and enums for all options with intellisense support.
* **Contract via interfaces** as well as DTOs to reduce the need to copy data
* **Linq provider** for reports
* **Fluent interface** - less typing and really good way to extend with extension methods
* **Support for the metadata** provided by the carrier rules method. Use this for local validation/UI options/rate shop.
* **Mocking and recording** of live messages to disk for capture or later playback in mock mode. Mocking for unit and regression testing.
* **Example** console app.
* **Plug in your own configuration and log providers**
* **Keeps the API secret out of cleartext**

## Prerequisites

- To generate shipping labels, you will need a Pitney Bowes Shipping API sandbox account. The account is free and gives access to a fully functional sandbox environment. Sign up for the account here: [Shipping API Signup](https://signup.pitneybowes.com/signup/shipping).

  You will need the following information from your Shipping API account. To get the information, see [Getting Started](https://shipping.pitneybowes.com/getting-started.html).