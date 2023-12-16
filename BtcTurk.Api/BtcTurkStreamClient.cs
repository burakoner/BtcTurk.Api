namespace BtcTurk.Api;

public class BtcTurkStreamClient : WebSocketApiClient
{
    #region Constructor/Destructor
    public BtcTurkStreamClient() : this(BtcTurkStreamClientOptions.Default)
    {
    }

    public BtcTurkStreamClient(BtcTurkStreamClientOptions options) : base(null, options)
    {
    }

    public BtcTurkStreamClient(ILogger logger, BtcTurkStreamClientOptions options) : base(logger, options)
    {
    }
    #endregion

    #region Overrided Methods

    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BtcTurkAuthenticationProvider(credentials);

    protected override bool HandleQueryResponse<T>(WebSocketConnection connection, object request, JToken data, out CallResult<T> callResult)
    {
        // out params
        callResult = new CallResult<T>(null);

        if (data is JArray jarr)
        {
            var isV2Response = jarr.Count == 2 && jarr.LastOrDefault()["type"] != null;
            if (isV2Response)
            {
                var desResult = Deserialize<T>(data);
                if (!desResult.Success)
                {
                    _logger.Log(LogLevel.Warning, $"Failed to deserialize data: {desResult.Error}. Data: {data}");
                    return false;
                }

                callResult = new CallResult<T>(desResult.Data);
                return true;
            }
        }

        return true;
    }

    protected override bool HandleSubscriptionResponse(WebSocketConnection connection, WebSocketSubscription subscription, object request, JToken data, out CallResult<object> callResult)
    {
        // out params
        callResult = new CallResult<object>(null);

        // Check Point
        if (data == null || data[0] == null || data.Count() != 2)
            return false;

        int modelNumber = 0;
        var parseResult = int.TryParse(data[0].ToString(), out modelNumber);

        // Check Point
        if (parseResult == false || modelNumber == 0)
            return false;

        // 100: Result for Request
        if (modelNumber == 100)
        {
            try
            {
                if (request is object[] req)
                {
                    if (req.Length == 2)
                    {
                        if (req[0] is int && req[1] is BtcTurkStreamRequest)
                        {
                            BtcTurkStreamRequest r = (BtcTurkStreamRequest)req[1];
                            var subResponse = Deserialize<BtcTurkStreamAnswer>(data[1]);

                            if (subResponse.Data.Message == $"join|{r.Channel}:{r.Event}")
                            {
                                if (subResponse.Data.OK)
                                {
                                    _logger.Log(LogLevel.Debug, "Subscription completed");
                                    callResult = new CallResult<object>(subResponse.Data);
                                    return true;
                                }
                                else
                                {
                                    _logger.Log(LogLevel.Warning, "Subscription failed: " + subResponse.Error);
                                    callResult = new CallResult<object>(subResponse.Error);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // 991: Welcome Message
        else if (modelNumber == 991)
            return false;

        return false;
    }

    protected override bool MessageMatchesHandler(WebSocketConnection connection, JToken message, object request)
    {
        try
        {
            // Check Point
            if (message == null || message[0] == null || message.Count() != 2)
                return false;

            int modelNumber = 0;
            var parseResult = int.TryParse(message[0].ToString(), out modelNumber);

            // Check Point
            if (parseResult == false || modelNumber == 0)
                return false;

            // Result
            if (modelNumber == 100)
                return false;

            // Request
            else if (modelNumber == 101)
                return false;

            // UserLogin
            else if (modelNumber == 111)
                return false;

            // UserLoginResult
            else if (modelNumber == 112)
                return false;

            // UserLogout
            else if (modelNumber == 113)
                return false;

            // Subscription Request
            else if (modelNumber == 151)
                return false;

            // BalanceUpdated
            else if (modelNumber == 201)
                return false;

            // 401: TickerAll
            // 402: TickerPair
            // 421: TradeList
            // 422: TradeSingle
            // 428: TradingView
            // 431: OrderBookFull
            // 432: OrderBookDifference
            else if (modelNumber.IsOneOf(401, 402, 421, 422, 428, 431, 432))
            {
                var ch = string.Empty;
                if (modelNumber == 401) ch = "ticker";
                else if (modelNumber == 402) ch = "ticker";
                else if (modelNumber == 421) ch = "trade";
                else if (modelNumber == 422) ch = "trade";
                else if (modelNumber == 428) ch = "tradeview";
                else if (modelNumber == 431) ch = "orderbook";
                else if (modelNumber == 432) ch = "obdiff";
                if (request is object[] req)
                {
                    if (req.Length == 2)
                    {
                        if (req[0] is int && req[1] is BtcTurkStreamRequest)
                        {
                            BtcTurkStreamRequest r = (BtcTurkStreamRequest)req[1];
                            if (r.Type == 151
                                && (modelNumber.IsOneOf(431, 432) ? r.Channel.IsOneOf("orderbook", "obdiff") : r.Channel == ch)
                                && r.Event == message[1]["event"].ToString()
                                && r.Join)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            // UserOrderMatch
            else if (modelNumber == 441)
                return false;

            // OrderInsert
            else if (modelNumber == 451)
                return false;

            // OrderDelete
            else if (modelNumber == 452)
                return false;

            // OrderUpdate
            else if (modelNumber == 453)
                return false;

            // Welcome Message
            else if (modelNumber == 991)
                return false;
        }
        catch { }

        return false;
    }

    protected override bool MessageMatchesHandler(WebSocketConnection connection, JToken message, string identifier)
    {
        if (message.Type != JTokenType.Object)
            return false;

        return false;
    }

    protected override Task<CallResult<bool>> AuthenticateAsync(WebSocketConnection s)
    {
        throw new NotImplementedException();

        /*
        if (authProvider == null)
            return new CallResult<bool>(false, new NoApiCredentialsError());

        var authParams = authProvider.AddAuthenticationToParameters(""/*baseAddressAuthenticated* /, HttpMethod.Get, new Dictionary<string, object>(), true, HttpMethodParameterPosition.InBody, ArrayParametersSerialization.Array);
        var authObjects = new BtcTurkAuthenticationRequest
        {
            AccessKeyId = authProvider.Credentials.Key.GetString(),
            Operation = "auth",
            SignatureMethod = (string)authParams["SignatureMethod"],
            SignatureVersion = authParams["SignatureVersion"].ToString(),
            Timestamp = (string)authParams["Timestamp"],
            Signature = (string)authParams["Signature"]
        };

        CallResult<bool> result = new CallResult<bool>(false, new ServerError("No response from server"));
        await s.SendAndWaitAsync(authObjects, ResponseTimeout, data =>
        {
            if ((string)data["op"] != "auth")
                return false;

            /*
            var authResponse = Deserialize<BtcTurkSocketAuthDataResponse<object>>(data, false);
            if (!authResponse.Success)
            {
                log.Write(LogVerbosity.Warning, "Authorization failed: " + authResponse.Error);
                result = new CallResult<bool>(false, authResponse.Error);
                return true;
            }
            if (!authResponse.Data.IsSuccessful)
            {
                log.Write(LogVerbosity.Warning, "Authorization failed: " + authResponse.Data.ErrorMessage);
                result = new CallResult<bool>(false, new ServerError(authResponse.Data.ErrorCode, authResponse.Data.ErrorMessage));
                return true;
            }
            * /

            log.Write(LogLevel.Debug, "Authorization completed");
            result = new CallResult<bool>(true, null);
            return true;
        });

        return result;
        */
    }

    protected override async Task<bool> UnsubscribeAsync(WebSocketConnection connection, WebSocketSubscription s)
    {
        // string topic = "";
        object unsub = null;
        string unsubId = null;
        string idField = "id";

        /*
        if (s.Request is BtcTurkSubscribeRequest hRequest)
        {
            topic = hRequest.Topic;
            unsubId = NextId().ToString();
            unsub = new BtcTurkUnsubscribeRequest(unsubId, topic);
        }

        if (s.Request is BtcTurkAuthenticatedRequest haRequest)
        {
            topic = haRequest.Topic;
            unsubId = NextId().ToString();
            unsub = new BtcTurkAuthUnsubscribeRequest(unsubId, topic);
            idField = "cid";
        }
        */

        var result = false;
        await connection.SendAndWaitAsync(unsub, TimeSpan.FromSeconds(10), data =>
        {
            if (data.Type != JTokenType.Object)
                return false;

            var id = (string)data[idField];
            if (id == unsubId)
            {
                result = (string)data["status"] == "ok";
                return true;
            }

            return false;
        });

        return result;
    }
    #endregion

    #region Internal Methods
    #endregion

    #region Common Methods
    public virtual void SetApiCredentials(string apikey, string secret)
    {
        base.SetApiCredentials(new ApiCredentials(apikey, secret));
    }
    #endregion

    #region Subscriptions
    public virtual async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerAsync(string symbol, Action<BtcTurkStreamTickerSingle> onData, CancellationToken ct = default)
    {
        var request = new BtcTurkStreamRequest(151, "ticker", symbol, true);
        var internalHandler = new Action<WebSocketDataEvent<BtcTurkStreamResponse>>(data => onData(JsonConvert.DeserializeObject<BtcTurkStreamTickerSingle>(data.Data.Data)));
        return await SubscribeAsync(request.RequestObject(), null, false, internalHandler, ct).ConfigureAwait(false);
    }

    public virtual async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(Action<BtcTurkStreamTickerAll> onData, CancellationToken ct = default)
    {
        var request = new BtcTurkStreamRequest(151, "ticker", "all", true);
        var internalHandler = new Action<WebSocketDataEvent<BtcTurkStreamResponse>>(data => onData(JsonConvert.DeserializeObject<BtcTurkStreamTickerAll>(data.Data.Data)));
        return await SubscribeAsync(request.RequestObject(), null, false, internalHandler, ct).ConfigureAwait(false);
    }

    public virtual async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, BtcTurkPeriod period, Action<BtcTurkStreamKline> onData, CancellationToken ct = default)
    {
        var request = new BtcTurkStreamRequest(151, "tradeview", $"{symbol}_{JsonConvert.SerializeObject(period, new PeriodConverter(false))}", true);
        var internalHandler = new Action<WebSocketDataEvent<BtcTurkStreamResponse>>(data => onData(JsonConvert.DeserializeObject<BtcTurkStreamKline>(data.Data.Data)));
        return await SubscribeAsync(request.RequestObject(), null, false, internalHandler, ct).ConfigureAwait(false);
    }

    public virtual async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(string symbol, Action<BtcTurkStreamTradeList> onListData, Action<BtcTurkStreamTradeSingle> onRowData, CancellationToken ct = default)
    {
        var request = new BtcTurkStreamRequest(151, "trade", symbol, true);
        var internalHandler = new Action<WebSocketDataEvent<BtcTurkStreamResponse>>(data =>
          {
              if (data.Data.Model == 421) onListData(JsonConvert.DeserializeObject<BtcTurkStreamTradeList>(data.Data.Data));
              if (data.Data.Model == 422) onRowData(JsonConvert.DeserializeObject<BtcTurkStreamTradeSingle>(data.Data.Data));
          }
        );
        return await SubscribeAsync(request.RequestObject(), null, false, internalHandler, ct).ConfigureAwait(false);
    }

    public virtual async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookFullAsync(string symbol, Action<BtcTurkStreamOrderBookFull> onData, CancellationToken ct = default)
    {
        var request = new BtcTurkStreamRequest(151, "orderbook", symbol, true);
        var internalHandler = new Action<WebSocketDataEvent<BtcTurkStreamResponse>>(data => onData(JsonConvert.DeserializeObject<BtcTurkStreamOrderBookFull>(data.Data.Data)));
        return await SubscribeAsync(request.RequestObject(), null, false, internalHandler, ct).ConfigureAwait(false);
    }

    public virtual async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookDiffAsync(string symbol, Action<BtcTurkStreamOrderBookFull> onFullData, Action<BtcTurkStreamOrderBookDifference> onDiffData, CancellationToken ct = default)
    {
        var request = new BtcTurkStreamRequest(151, "obdiff", symbol, true);
        var internalHandler = new Action<WebSocketDataEvent<BtcTurkStreamResponse>>(data =>
        {
            if (data.Data.Model == 431) onFullData(JsonConvert.DeserializeObject<BtcTurkStreamOrderBookFull>(data.Data.Data));
            if (data.Data.Model == 432) onDiffData(JsonConvert.DeserializeObject<BtcTurkStreamOrderBookDifference>(data.Data.Data));
        }
        );
        return await SubscribeAsync(request.RequestObject(), null, false, internalHandler, ct).ConfigureAwait(false);
    }
    #endregion

    /*
    public virtual async Task<CallResult<bool>> Login(string username, string token)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token))
            return new CallResult<bool>(false);

        // Try to Login
        var request = new BtcTurkSocketLoginRequest(111, NextId(), username, token);
        CallResult<bool> result = new CallResult<bool>(false);
        var response = await QueryAsync<BtcTurkSocketLoginResponse>(request, false);

        /*
        GetSocketConnection()

        /*
        , data =>
        {
            if ((string)data["op"] != "auth")
                return false;

            /*
            var authResponse = Deserialize<BtcTurkSocketAuthDataResponse<object>>(data, false);
            if (!authResponse.Success)
            {
                log.Write(LogVerbosity.Warning, "Authorization failed: " + authResponse.Error);
                result = new CallResult<bool>(false, authResponse.Error);
                return true;
            }
            if (!authResponse.Data.IsSuccessful)
            {
                log.Write(LogVerbosity.Warning, "Authorization failed: " + authResponse.Data.ErrorMessage);
                result = new CallResult<bool>(false, new ServerError(authResponse.Data.ErrorCode, authResponse.Data.ErrorMessage));
                return true;
            }
            * /

            log.Write(LogLevel.Debug, "Authorization completed");
            result = new CallResult<bool>(true, null);
            return true;
        });
        * /

        return result;
    }
    */


}