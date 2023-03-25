# BtcTurk.Api 

A .Net wrapper for the BtcTurk API as described on [BtcTurk](https://docs.btcturk.com/), including all features the API provides using clear and readable objects.

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/burakoner/BtcTurk.Api/issues)**

## Donations
Donations are greatly appreciated and a motivation to keep improving.

**BTC**:  33WbRKqt7wXARVdAJSu1G1x3QnbyPtZ2bH  
**ETH**:  0x65b02db9b67b73f5f1e983ae10796f91ded57b64  

## Installation
![Nuget version](https://img.shields.io/nuget/v/BtcTurk.Api.svg)  ![Nuget downloads](https://img.shields.io/nuget/dt/BtcTurk.Api.svg)
Available on [Nuget](https://www.nuget.org/packages/BtcTurk.Api).
```
PM> Install-Package BtcTurk.Api
```
To get started with BtcTurk.Api first you will need to get the library itself. The easiest way to do this is to install the package into your project using  [NuGet](https://www.nuget.org/packages/BtcTurk.Api). Using Visual Studio this can be done in two ways.

### Using the package manager
In Visual Studio right click on your solution and select 'Manage NuGet Packages for solution...'. A screen will appear which initially shows the currently installed packages. In the top bit select 'Browse'. This will let you download net package from the NuGet server. In the search box type 'BtcTurk.Api' and hit enter. The BtcTurk.Api package should come up in the results. After selecting the package you can then on the right hand side select in which projects in your solution the package should install. After you've selected all project you wish to install and use BtcTurk.Api in hit 'Install' and the package will be downloaded and added to you projects.

### Using the package manager console
In Visual Studio in the top menu select 'Tools' -> 'NuGet Package Manager' -> 'Package Manager Console'. This should open up a command line interface. On top of the interface there is a dropdown menu where you can select the Default Project. This is the project that BtcTurk.Api will be installed in. After selecting the correct project type  `Install-Package BtcTurk.Api`  in the command line interface. This should install the latest version of the package in your project.

After doing either of above steps you should now be ready to actually start using BtcTurk.Api.

## Getting started
After installing it's time to actually use it. To get started we have to add the BtcTurk.Api namespace:  `using BtcTurk.Api;`.

BtcTurk.Api provides two clients to interact with the BtcTurk API. The  `BtcTurkRestClient`  provides all rest API calls. The  `BtcTurkStreamClient` provides functions to interact with the websocket provided by the BtcTurk API. Both clients are disposable and as such can be used in a  `using`statement.

## Examples
**Public Api Endpoints:**
```C#
var api = new BtcTurkRestClient();
var btcTurk_01 = await api.PingAsync();
var btcTurk_02 = await api.GetServerTimeAsync();
var btcTurk_03 = await api.GetExchangeInfoAsync();
var btcTurk_04 = await api.GetTickersAsync();
var btcTurk_05 = await api.GetOrderBookAsync("BTCTRY");
var btcTurk_06 = await api.GetTradesAsync("BTCTRY");
var btcTurk_07 = await api.GetOHLCAsync("BTCTRY");
var btcTurk_08 = await api.GetKlinesAsync("BTCTRY", Api.Enums.BtcTurkPeriod.OneHour, DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
... etc
```

**Private Api Endpoints:**
```C#
var api = new BtcTurkRestClient();
api.SetApiCredentials("XXXXXXXX-API-KEY-XXXXXXXX", "XXXXXXXX-API-SECRET-XXXXXXXX");
var btcTurk_11 = await api.GetBalancesAsync();
var btcTurk_12 = await api.GetTradeTransactionsAsync();
var btcTurk_13 = await api.GetFiatTransactionsAsync();
var btcTurk_14 = await api.GetCryptoTransactionsAsync();
var btcTurk_15 = await api.GetOpenOrdersAsync();
var btcTurk_16 = await api.GetAllOrdersAsync();
var btcTurk_17 = await api.GetOrderAsync(100001);
var btcTurk_18 = await api.PlaceOrderAsync("BTCTRY", 1.0m, BtcTurkOrderSide.Buy, BtcTurkOrderMethod.Market);
var btcTurk_19 = await api.CancelOrderAsync(100001);
... etc
```

## Websockets
The BtcTurk.Api socket client provides several socket endpoint to which can be subscribed.

**Public Socket Endpoints:**
```C#
var pairs = new List<string> {
    "ATOMTRY","BTCTRY","DASHTRY","EOSTRY","ETHTRY","LINKTRY","LTCTRY","NEOTRY","USDTTRY","XLMTRY","XRPTRY","XTZTRY",
    "ATOMUSDT","BTCUSDT","DASHUSDT","EOSUSDT","ETHUSDT","LINKUSDT","LTCUSDT","NEOUSDT","XLMUSDT","XRPUSDT","XTZUSDT",
    "ATOMBTC","DASHBTC","EOSBTC","ETHBTC","LINKBTC","LTCBTC","NEOBTC","XLMBTC","XRPBTC","XTZBTC"
};

// BtcTurk Websocket Api Client
var ws = new BtcTurkStreamClient();

// Public Socket Endpoints
var subs = new List<UpdateSubscription>();

// Single Ticker
{
    var subscription = await ws.SubscribeToTickerAsync("BTCTRY", (ticker) =>
    {
        if (ticker != null)
        {
            Console.WriteLine($"Channel: {ticker.Channel} Event:{ticker.Event} Type:{ticker.Type} >> " +
            $"B:{ticker.Bid} A:{ticker.Ask} PS:{ticker.PairSymbol} NS:{ticker.NumeratorSymbol} DS:{ticker.DenominatorSymbol} " +
            $"O:{ticker.Open} H:{ticker.High} L:{ticker.Low} LA:{ticker.Close} V:{ticker.Volume} " +
            $"AV:{ticker.AveragePrice} D:{ticker.DailyChange} DP:{ticker.DailyChangePercent} PId:{ticker.PairId} Ord:{ticker.OrderNum} ");
        }
    });
    subs.Add(subscription.Data);
}

// All Tickers
{
    var subscription = await ws.SubscribeToTickersAsync((data) =>
    {
        if (data != null)
        {
            foreach (var ticker in data.Items)
            {
                Console.WriteLine($"Channel: {data.Channel} Event:{data.Event} Type:{data.Type} >> " +
                    $"B:{ticker.Bid} A:{ticker.Ask} PS:{ticker.PairSymbol} NS:{ticker.NumeratorSymbol} DS:{ticker.DenominatorSymbol} " +
                    $"O:{ticker.Open} H:{ticker.High} L:{ticker.Low} LA:{ticker.Close} V:{ticker.Volume} " +
                    $"AV:{ticker.AveragePrice} D:{ticker.DailyChange} DP:{ticker.DailyChangePercent} PId:{ticker.PairId} Ord:{ticker.OrderNum} ");
            }
        }
    });
    subs.Add(subscription.Data);
}

// Klines
foreach (var pair in pairs)
{
    var subscription = await ws.SubscribeToKlinesAsync(pair, BtcTurkPeriod.OneMinute, (data) =>
    {
        if (data != null)
        {
            Console.WriteLine($"Channel: {data.Channel} Event:{data.Event} Type:{data.Type} >> D:{data.Date} S:{data.Pair} P:{data.Period} O:{data.Open} H:{data.High} L:{data.Low} V:{data.Close} V:{data.Volume}");
        }
    });
    subs.Add(subscription.Data);
}

// Single Trade
{
    var subscription = await ws.SubscribeToTradesAsync("BTCTRY", (trades) =>
    {
        if (trades != null)
        {
            foreach (var trade in trades.Items)
            {
                Console.WriteLine($"Channel: {trades.Channel} [List] Event:{trades.Event} Type:{trades.Type} PairSymbol:{trades.PairSymbol} >> " +
                    $"D:{trade.Time} I:{trade.TradeId} A:{trade.Amount} A:{trade.Price} PS:{trade.PairSymbol} S:{trade.S} ");
            }
        }
    }, (trade) =>
    {
        if (trade != null)
        {
            Console.WriteLine($"Channel: {trade.Channel} [Row] Event:{trade.Event} Type:{trade.Type} PairSymbol:{trade.PairSymbol} >> " +
            $"D:{trade.Time} I:{trade.TradeId} A:{trade.Amount} A:{trade.Price} PS:{trade.PairSymbol} S:{trade.S} ");
        }
    });
    subs.Add(subscription.Data);
}

// All Pairs Trades
foreach (var pair in pairs)
{
    var subscription = await ws.SubscribeToTradesAsync(pair, (trades) =>
    {
        if (trades != null)
        {
            foreach (var trade in trades.Items)
            {
                Console.WriteLine($"Channel: {trades.Channel} [List] Event:{trades.Event} Type:{trades.Type} PairSymbol:{trades.PairSymbol} >> " +
                    $"D:{trade.Time} I:{trade.TradeId} A:{trade.Amount} A:{trade.Price} PS:{trade.PairSymbol} S:{trade.S} ");
            }
        }
    }, (trade) =>
    {
        if (trade != null)
        {
            Console.WriteLine($"Channel: {trade.Channel} [Row] Event:{trade.Event} Type:{trade.Type} PairSymbol:{trade.PairSymbol} >> " +
                $"D:{trade.Time} I:{trade.TradeId} A:{trade.Amount} A:{trade.Price} PS:{trade.PairSymbol} S:{trade.S} ");
        }
    });
    subs.Add(subscription.Data);
}

// Order Book Full
foreach (var pair in pairs)
{
    var subscription = await ws.SubscribeToOrderBookFullAsync(pair, (data) =>
    {
        if (data != null)
        {
            Console.WriteLine($"Channel: {data.Channel} [Full] Event:{data.Event} Type:{data.Type} PairSymbol:{data.PairSymbol} >> ");
            for (var i = 0; i < Math.Min(data.Asks.Count(), 5); i++)
            {
                Console.WriteLine($"Ask {i + 1} -> Price: {data.Asks[i].Price} Amount:{data.Asks[i].Amount}");
            }
            for (var i = 0; i < Math.Min(data.Bids.Count(), 5); i++)
            {
                Console.WriteLine($"Bid {i + 1} -> Price: {data.Bids[i].Price} Amount:{data.Bids[i].Amount}");
            }
        }
    });
    subs.Add(subscription.Data);
}

// Order Book Difference
foreach (var pair in pairs)
{
    var subscription = await ws.SubscribeToOrderBookDiffAsync("BTCTRY", (data) =>
    {
        if (data != null)
        {
            Console.WriteLine($"Channel: {data.Channel} [Full] Event:{data.Event} Type:{data.Type} PairSymbol:{data.PairSymbol} >> ");
            for (var i = 0; i < Math.Min(data.Asks.Count(), 5); i++)
            {
                Console.WriteLine($"Ask {i + 1} -> Price: {data.Asks[i].Price} Amount:{data.Asks[i].Amount}");
            }
            for (var i = 0; i < Math.Min(data.Bids.Count(), 5); i++)
            {
                Console.WriteLine($"Bid {i + 1} -> Price: {data.Bids[i].Price} Amount:{data.Bids[i].Amount}");
            }
        }
    }, (data) =>
    {
        if (data != null)
        {
            Console.WriteLine($"Channel: {data.Channel} [Diff] Event:{data.Event} Type:{data.Type} PairSymbol:{data.PairSymbol} >> ");
            for (var i = 0; i < Math.Min(data.Asks.Count(), 5); i++)
            {
                Console.WriteLine($"Ask {i + 1} -> Price: {data.Asks[i].Price} Amount:{data.Asks[i].Amount}");
            }
            for (var i = 0; i < Math.Min(data.Bids.Count(), 5); i++)
            {
                Console.WriteLine($"Bid {i + 1} -> Price: {data.Bids[i].Price} Amount:{data.Bids[i].Amount}");
            }
        }
    });
    subs.Add(subscription.Data);
}
```

**Private Socket Endpoints:**
```C#
/* Not supported yet because there isnt any guide how to create login token */
```

## Release Notes
* Version 1.0.0 - 25 Mar 2023
    * First Release