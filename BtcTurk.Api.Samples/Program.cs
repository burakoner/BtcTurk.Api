using ApiSharp.Stream;
using BtcTurk.Api;
using BtcTurk.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtcTurk.Samples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // BtcTurk Rest Api Client
            var api = new BtcTurkRestClient();
            api.SetApiCredentials("XXXXXXXX-API-KEY-XXXXXXXX", "XXXXXXXX-API-SECRET-XXXXXXXX");

            // Public Endpoints
            var btcTurk_01 = await api.PingAsync();
            var btcTurk_02 = await api.GetServerTimeAsync();
            var btcTurk_03 = await api.GetExchangeInfoAsync();
            var btcTurk_04 = await api.GetTickersAsync();
            var btcTurk_05 = await api.GetOrderBookAsync("BTCTRY");
            var btcTurk_06 = await api.GetTradesAsync("BTCTRY");
            var btcTurk_07 = await api.GetOHLCAsync("BTCTRY");
            var btcTurk_08 = await api.GetKlinesAsync("BTCTRY", Api.Enums.BtcTurkPeriod.OneHour, DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);

            // Private Endpoints
            var btcTurk_11 = await api.GetBalancesAsync();
            var btcTurk_12 = await api.GetTradeTransactionsAsync();
            var btcTurk_13 = await api.GetFiatTransactionsAsync();
            var btcTurk_14 = await api.GetCryptoTransactionsAsync();
            var btcTurk_15 = await api.GetOpenOrdersAsync();
            var btcTurk_16 = await api.GetAllOrdersAsync();
            var btcTurk_17 = await api.GetOrderAsync(100001);
            var btcTurk_18 = await api.PlaceOrderAsync("BTCTRY", 1.0m, BtcTurkOrderSide.Buy, BtcTurkOrderMethod.Market);
            var btcTurk_19 = await api.CancelOrderAsync(100001);

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

            Console.ReadLine();
        }
    }
}