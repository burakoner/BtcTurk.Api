namespace BtcTurk.Api;

public class BtcTurkRestClient : RestApiClient
{
    #region Endpoints
    // Api Version
    private const string v0 = "";
    private const string v1 = "1";
    private const string v2 = "2";
    private const string v3 = "3";

    // Public Endpoints
    private const string serverExchangeInfoEndpoint = "server/exchangeInfo";
    private const string serverTimeEndpoint = "server/time";
    private const string serverPingEndpoint = "server/ping";
    private const string tickerEndpoint = "ticker";
    private const string orderBookEndpoint = "orderbook";
    private const string tradesEndpoint = "trades";
    private const string ohlcsEndpoint = "ohlcs";
    private const string klinesHistoryEndpoint = "klines/history";

    // Private Endpoints
    private const string usersBalancesEndpoint = "users/balances";
    private const string usersTransactionsTradeEndpoint = "users/transactions/trade";
    private const string usersTransactionsFiatEndpoint = "users/transactions/fiat";
    private const string usersTransactionsCryptoEndpoint = "users/transactions/crypto";
    private const string openOrdersEndpoint = "openOrders";
    private const string allOrdersEndpoint = "allOrders";
    private const string orderIdEndpoint = "order/{id}";
    private const string orderEndpoint = "order";
    #endregion

    #region Constructors
    public BtcTurkRestClient() : this(new BtcTurkRestClientOptions())
    {
    }

    public BtcTurkRestClient(BtcTurkRestClientOptions options) : base("BtcTurk Rest Api", options)
    {
    }
    #endregion

    #region Override Methods
    internal static TimeSyncState TimeSyncState = new("BtcTurk Rest Api");

    protected override async Task<RestCallResult<DateTime>> GetServerTimestampAsync()
    {
        var result = await GetServerTimeAsync().ConfigureAwait(false);
        return result.As(result.Data.ServerTime);
    }

    protected override TimeSyncInfo GetTimeSyncInfo()
        => new(log, ((BtcTurkRestClientOptions)ClientOptions).AutoTimestamp, ((BtcTurkRestClientOptions)ClientOptions).TimestampRecalculationInterval, TimeSyncState);

    protected override TimeSpan GetTimeOffset()
        => TimeSyncState.TimeOffset;

    protected override Error ParseErrorResponse(JToken error)
    {
        if (error["code"] == null || error["message"] == null)
            return new ServerError(error.ToString());

        return new ServerError($"{(string)error["code"]}, {(string)error["message"]}");
    }

    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BtcTurkAuthenticationProvider(credentials);
    #endregion

    #region Internal Methods
    internal Uri GetUri(string version, string endpoint, string api = "")
    {
        var baseAddress = string.IsNullOrEmpty(api) ? ClientOptions.BaseAddress : api;
        
        return string.IsNullOrEmpty(version)
            ? new Uri($"{baseAddress.TrimEnd('/')}/{endpoint}")
            : new Uri($"{baseAddress.TrimEnd('/')}/v{version}/{endpoint}");
    }

    internal async Task<RestCallResult<T>> ExecuteAsync<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false, Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null, ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
    {
        var result = await SendRequestAsync<BtcTurkApiResponse<T>>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight).ConfigureAwait(false);
        if (!result) return result.AsError<T>(result.Error!);
        if (!string.IsNullOrWhiteSpace(result.Data.ErrorCode) && result.Data.ErrorCode != "SUCCESS" && result.Data.ErrorCode.ToInt32Safe() > 0) return result.AsError<T>(new BtcTurkApiError(result.Data.ErrorCode.ToInt32Safe(), result.Data.ErrorMessage, null));

        return result.As(result.Data.Data);
    }

    internal async Task<RestCallResult<T>> RawExecuteAsync<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false, Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null, ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
    {
        var result = await SendRequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight).ConfigureAwait(false);
        if (!result) return result.AsError<T>(result.Error!);

        return result.As(result.Data);
    }
    #endregion

    #region Public Methods
    public void SetApiCredentials(string apikey, string secret)
    {
        base.SetApiCredentials(new ApiCredentials(apikey, secret));
    }
    #endregion

    #region Public Endpoints
    /// <summary>
    /// Ping
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<BtcTurkPing>> PingAsync(CancellationToken ct = default)
    {
        return await RawExecuteAsync<BtcTurkPing>(GetUri(v2, serverPingEndpoint), HttpMethod.Get, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns Server Time
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<BtcTurkTime>> GetServerTimeAsync(CancellationToken ct = default)
    {
        return await RawExecuteAsync<BtcTurkTime>(GetUri(v2, serverTimeEndpoint), HttpMethod.Get, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a list of all known currencies. 
    /// You can use exchangeinfo endpoint for all tradable pairs and their quantity or price scales.
    /// 
    /// 1. Trade Information
    ///     pairId, numeratorScale, denominatorScale, maxLimitOrderPrice and minLimitOrderPrice information can be displayed with exhangeinfo endpoint.
    ///     For more information about numeratorScale and denominatorScale, you can check our Pair Scale article.
    ///    
    /// What is maxLimitOrderPrice and minLimitOrderPrice ?
    ///     The maxLimitOrderPrice and minLimitOrderPrice values will be updated dynamically.
    ///     The values will be arranged as follows
    ///    
    /// 2. Currency Information
    ///     You can also view minWithdrawal, minDeposit, precision and address details with exchangeInfo endpoint.
    ///     You can check the current cryptocurrency deposit and withdrawal status with exchangeInfo endpoint.
    ///     currencySymbol, withdrawalDisabled, depositDisabled
    ///     Status changes astrue when deposits or withdrawals are turned off.
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<BtcTurkExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
    {
        return await ExecuteAsync<BtcTurkExchangeInfo>(GetUri(v2, serverExchangeInfoEndpoint), HttpMethod.Get, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets snapshot information about the last trade (tick), best bid/ask and 24h volume.
    /// </summary>
    /// <param name="symbol">Pair Symbol</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<IEnumerable<BtcTurkTicker>>> GetTickersAsync(string symbol = "", CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "pairSymbol", symbol },
        };

        return await ExecuteAsync<IEnumerable<BtcTurkTicker>>(GetUri(v2, tickerEndpoint), HttpMethod.Get, ct, queryParameters: parameters).ConfigureAwait(false);
    }

    /// <summary>
    /// Get a list of all open orders for a product.
    /// </summary>
    /// <param name="symbol">Pair Symbol</param>
    /// <param name="limit">limit the number of results (default 25)</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<BtcTurkOrderBook>> GetOrderBookAsync(string symbol, int limit = 25, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "pairSymbol", symbol },
            { "limit", limit },
        };

        return await ExecuteAsync<BtcTurkOrderBook>(GetUri(v2, orderBookEndpoint), HttpMethod.Get, ct, queryParameters: parameters).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a list the latest trades for a product.
    /// 2 parameters can be used for the trades enpoint: pairSymbol, last
    /// You can send pairSymbol parameter in this format BTCUSDT
    /// Max of 50 latest trades can be used for the trades parameter
    /// </summary>
    /// <param name="symbol">Pair Symbol</param>
    /// <param name="last">Indicates how many last trades you want. Max is 50.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<IEnumerable<BtcTurkTrade>>> GetTradesAsync(string symbol, int last = 50, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "pairSymbol", symbol },
            { "last", last },
        };

        return await ExecuteAsync<IEnumerable<BtcTurkTrade>>(GetUri(v2, tradesEndpoint), HttpMethod.Get, ct, queryParameters: parameters).ConfigureAwait(false);
    }

    /// <summary>
    /// This is the data that is shown in our charting interface.
    /// </summary>
    /// <param name="symbol">Pair Symbol</param>
    /// <param name="from">Unix time in seconds</param>
    /// <param name="to">Unix time in seconds</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<IEnumerable<BtcTurkOhlc>>> GetOHLCAsync(string symbol, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "pair", symbol },
        };
        parameters.AddOptionalParameter("from", from.ConvertToSeconds());
        parameters.AddOptionalParameter("to", to.ConvertToSeconds());

        return await RawExecuteAsync<IEnumerable<BtcTurkOhlc>>(GetUri(v1, ohlcsEndpoint, ((BtcTurkRestClientOptions)ClientOptions).GraphApiAddress), HttpMethod.Get, ct, queryParameters: parameters).ConfigureAwait(false);
    }

    /// <summary>
    /// Kline candlestick bars for a symbol.
    /// Kline history information can be viewed with kline endpoint.
    /// </summary>
    /// <param name="symbol">Pair Symbol</param>
    /// <param name="period">Resolution</param>
    /// <param name="from">Unix time in seconds</param>
    /// <param name="to">Unix time in seconds</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<IEnumerable<BtcTurkKline>>> GetKlinesAsync(string symbol, BtcTurkPeriod period, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol },
            { "resolution", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
        };
        parameters.AddOptionalParameter("from", from.ConvertToSeconds());
        parameters.AddOptionalParameter("to", to.ConvertToSeconds());

        var result = await RawExecuteAsync<BtcTurkKlineData>(GetUri(v1, klinesHistoryEndpoint, ((BtcTurkRestClientOptions)ClientOptions).GraphApiAddress), HttpMethod.Get, ct, queryParameters: parameters).ConfigureAwait(false);
        if (!result.Success 
            || result.Data == null
            || result.Data.Times == null
            || result.Data.Opens == null
            || result.Data.Highs == null
            || result.Data.Lows == null
            || result.Data.Closes == null
            || result.Data.Volumes == null)
            return result.AsError<IEnumerable<BtcTurkKline>>(new BtcTurkApiError(result.Error.Code, result.Error.Message, result.Error.Data));

        // Parse Result
        List<BtcTurkKline> response = new();
        var maxRows = Math.Min(result.Data.Times.Length, result.Data.Opens.Length);
        maxRows = Math.Min(maxRows, result.Data.Highs.Length);
        maxRows = Math.Min(maxRows, result.Data.Lows.Length);
        maxRows = Math.Min(maxRows, result.Data.Closes.Length);
        maxRows = Math.Min(maxRows, result.Data.Volumes.Length);

        for (int i = 0; i < maxRows; i++)
        {
            response.Add(new BtcTurkKline
            {
                OpenDateTime = result.Data.Times[i].FromUnixTimeSeconds(),
                Open = result.Data.Opens[i],
                High = result.Data.Highs[i],
                Low = result.Data.Lows[i],
                Close = result.Data.Closes[i],
                Volume = result.Data.Volumes[i],
            });
        }

        return result.As(response.AsEnumerable());
    }
    #endregion

    #region Private Endpoints
    /// <summary>
    /// Retrieve all cash balances.
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<IEnumerable<BtcTurkBalance>>> GetBalancesAsync(CancellationToken ct = default)
    {
        return await ExecuteAsync<IEnumerable<BtcTurkBalance>>(GetUri(v1, usersBalancesEndpoint), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieve all trade transactions.
    /// </summary>
    /// <param name="orderId">long, Optional you can not combine this parameter with other parameters. So you should send this parameter alone.</param>
    /// <param name="symbol">BTCTRY, ETHTRY etc.</param>
    /// <param name="assets">string array, {"btc", "try", ...etc.}</param>
    /// <param name="type">string array, {"buy", "sell"}</param>
    /// <param name="startTime">long, Optional timestamp if null will return last 30 days</param>
    /// <param name="endTime">long, Optional timestamp if null will return last 30 days</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<IEnumerable<BtcTurkOrderTransaction>>> GetTradeTransactionsAsync(long? orderId = null, string symbol = null, string[] assets = null, BtcTurkOrderSide[] type = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("orderId", orderId);
        parameters.AddOptionalParameter("symbol", assets);
        parameters.AddOptionalParameter("type", JsonConvert.DeserializeObject<string[]>(JsonConvert.SerializeObject(type, new OrderSideConverter(true))));
        parameters.AddOptionalParameter("startDate", startTime?.ToUnixTimeMilliseconds().ToString());
        parameters.AddOptionalParameter("endDate", endTime?.ToUnixTimeMilliseconds().ToString());
        parameters.AddOptionalParameter("pairSymbol", symbol);

        return await ExecuteAsync< IEnumerable<BtcTurkOrderTransaction>> (GetUri(v1, usersTransactionsTradeEndpoint), HttpMethod.Get, ct, queryParameters: parameters, signed: true).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieve all fiat transactions.
    /// </summary>
    /// <param name="assets">string  {"try"}</param>
    /// <param name="type">string array, {"deposit", "withdrawal"}</param>
    /// <param name="startTime">long, Optional timestamp if null will return last 30 days</param>
    /// <param name="endTime">long, Optional timestamp if null will return last 30 days</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<IEnumerable<BtcTurkWalletTransaction>>> GetFiatTransactionsAsync(string[] assets = null, BtcTurkOrderSide[] type = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", assets);
        parameters.AddOptionalParameter("type", JsonConvert.DeserializeObject<string[]>(JsonConvert.SerializeObject(type, new OrderSideConverter(true))));
        parameters.AddOptionalParameter("startDate", startTime?.ToUnixTimeMilliseconds().ToString());
        parameters.AddOptionalParameter("endDate", endTime?.ToUnixTimeMilliseconds().ToString());

        return await ExecuteAsync<IEnumerable<BtcTurkWalletTransaction>>(GetUri(v1, usersTransactionsFiatEndpoint), HttpMethod.Get, ct, queryParameters: parameters, signed: true).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieve all crypto transactions.
    /// </summary>
    /// <param name="assets">string  {"try"}</param>
    /// <param name="type">string array, {"deposit", "withdrawal"}</param>
    /// <param name="startTime">long, Optional timestamp if null will return last 30 days</param>
    /// <param name="endTime">long, Optional timestamp if null will return last 30 days</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<IEnumerable<BtcTurkWalletTransaction>>> GetCryptoTransactionsAsync(string[] assets = null, BtcTurkOrderSide[] type = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", assets);
        parameters.AddOptionalParameter("type", JsonConvert.DeserializeObject<string[]>(JsonConvert.SerializeObject(type, new OrderSideConverter(true))));
        parameters.AddOptionalParameter("startDate", startTime?.ToUnixTimeMilliseconds().ToString());
        parameters.AddOptionalParameter("endDate", endTime?.ToUnixTimeMilliseconds().ToString());

        return await ExecuteAsync<IEnumerable<BtcTurkWalletTransaction>>(GetUri(v1, usersTransactionsCryptoEndpoint), HttpMethod.Get, ct, queryParameters: parameters).ConfigureAwait(false);
    }

    /// <summary>
    /// List your current open orders. Only open or un-settled orders are returned by default. As soon as an order is no longer open and settled, it will no longer appear in the default request. 
    /// </summary>
    /// <param name="symbol">Pair Symbol</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<BtcTurkOpenOrders>> GetOpenOrdersAsync(string symbol = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("pairSymbol", symbol);

        return await ExecuteAsync<BtcTurkOpenOrders>(GetUri(v1, openOrdersEndpoint), HttpMethod.Get, ct, queryParameters: parameters, signed: true).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieve all orders of any status.
    /// </summary>
    /// <param name="symbol">Pair Symbol</param>
    /// <param name="startOrderId">If orderId set, it will return all orders greater than or equals to this order id</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">default 100, max 1000</param>
    /// <param name="page">page number</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<IEnumerable<BtcTurkOrder>>> GetAllOrdersAsync(string symbol = "", long? startOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = 100, int? page = 1, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("pairSymbol", symbol);
        parameters.AddOptionalParameter("orderId", startOrderId != null ? startOrderId : null);
        parameters.AddOptionalParameter("startTime", startTime?.ToUnixTimeMilliseconds().ToString());
        parameters.AddOptionalParameter("endTime", endTime?.ToUnixTimeMilliseconds().ToString());
        parameters.AddOptionalParameter("limit", limit != null ? limit : null);
        // parameters.AddOptionalParameter("page", page != null ? page - 1 : null);
        parameters.AddOptionalParameter("page", page != null ? page : null);

        return await ExecuteAsync<IEnumerable<BtcTurkOrder>>(GetUri(v1, allOrdersEndpoint), HttpMethod.Get, ct, queryParameters: parameters, signed: true).ConfigureAwait(false);
    }

    /// <summary>
    /// Get a single order by orderId.
    /// For all transactions related to the private endpoint, you must authorize before sending your request.
    /// For more information you can check our Authentication V1 article.
    /// This endpoint can be used for more detailed information about orders.
    /// 
    /// If your order matches in more than one trade, you can only get the price information for the first partial trade of your order.
    /// You can view the partial trades from users/transactions/trade endpoint.
    /// </summary>
    /// <param name="orderId">orderId of the order</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<BtcTurkOrder>> GetOrderAsync(long orderId, CancellationToken ct = default)
    {
        var endpoint = orderIdEndpoint.Replace("{id}", orderId.ToString());
        return await ExecuteAsync<BtcTurkOrder>(GetUri(v1, allOrdersEndpoint), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
    }

    /// <summary>
    /// Create an order. You can place 4 types of orders: limit, market, stoplimit and stopmarket. 
    /// Orders can only be placed if your account has sufficient funds.Once an order is placed, your account funds will be put on hold for the duration of the order.How much and which funds are put on hold depends on the order type and parameters specified.
    /// For all transactions related to the private endpoint, you must authorize before sending your request.
    /// For more information you can check our Authentication V1 article.
    /// 
    /// You need to send request with[POST] method.
    /// - quantity, price, stopPrice, newOrderClientId, orderMethod, orderType , pairSymbol parameters must be used for submit order.
    /// - price field will be ignored for market orders. Market orders get filled with different prices until your order is completely filled. There is a 5% limit on the difference between the first price and the last price. İ.e.you can't buy at a price more than 5% higher than the best sell at the time of order submission and you can't sell at a price less than 5% lower than the best buy at the time of order submission.
    /// - stopPrice parameter is valid only for stop orders
    /// - You can use symbol parameter in this format: BTCUSDT
    /// </summary>
    /// <param name="symbol"></param>
    /// <param name="quantity"></param>
    /// <param name="side"></param>
    /// <param name="method"></param>
    /// <param name="price"></param>
    /// <param name="stopPrice"></param>
    /// <param name="clientOrderId"></param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<BtcTurkPlacedOrder>> PlaceOrderAsync(string symbol, decimal quantity, BtcTurkOrderSide side, BtcTurkOrderMethod method, decimal? price = null, decimal? stopPrice = null, string clientOrderId = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "pairSymbol", symbol },
            { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
            { "orderType", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
            { "orderMethod", JsonConvert.SerializeObject(method, new OrderMethodConverter(false)) },
        };
        parameters.AddOptionalParameter("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("newClientOrderId", clientOrderId);
        return await ExecuteAsync<BtcTurkPlacedOrder>(GetUri(v1, orderEndpoint), HttpMethod.Post, ct, bodyParameters: parameters, signed: true).ConfigureAwait(false);
    }

    /// <summary>
    /// Cancel a single open order by {id}.
    /// </summary>
    /// <param name="orderId">orderId of the order</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<object>> CancelOrderAsync(long orderId, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "id", orderId.ToString() },
        };
        return await ExecuteAsync<object>(GetUri(v1, orderEndpoint), HttpMethod.Delete, ct, signed: true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

}