namespace Binance.Api.Spot.Converters;

internal class BinanceSymbolFilterConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return false;
    }
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
#pragma warning disable 8604, 8602
        var obj = JObject.Load(reader);
        var type = new SymbolFilterTypeConverter(false).ReadString(obj["filterType"].ToString());
        BinanceSymbolFilter result;
        switch (type)
        {
            case BinanceSymbolFilterType.LotSize:
                result = new BinanceSymbolLotSizeFilter
                {
                    MaxQuantity = (decimal)obj["maxQty"],
                    MinQuantity = (decimal)obj["minQty"],
                    StepSize = (decimal)obj["stepSize"]
                };
                break;
            case BinanceSymbolFilterType.MarketLotSize:
                result = new BinanceSymbolMarketLotSizeFilter
                {
                    MaxQuantity = (decimal)obj["maxQty"],
                    MinQuantity = (decimal)obj["minQty"],
                    StepSize = (decimal)obj["stepSize"]
                };
                break;
            case BinanceSymbolFilterType.MinNotional:
                result = new BinanceSymbolMinNotionalFilter
                {
                    MinNotional = (decimal)obj["minNotional"],
                    ApplyToMarketOrders = (bool)obj["applyToMarket"],
                    AveragePriceMinutes = (int)obj["avgPriceMins"]
                };
                break;
            case BinanceSymbolFilterType.Notional:
                result = new BinanceSymbolNotionalFilter
                {
                    MinNotional = (decimal)obj["minNotional"],
                    MaxNotional = (decimal)obj["maxNotional"],
                    ApplyMinToMarketOrders = (bool)obj["applyMinToMarket"],
                    ApplyMaxToMarketOrders = (bool)obj["applyMaxToMarket"],
                    AveragePriceMinutes = (int)obj["avgPriceMins"]
                };
                break;
            case BinanceSymbolFilterType.Price:
                result = new BinanceSymbolPriceFilter
                {
                    MaxPrice = (decimal)obj["maxPrice"],
                    MinPrice = (decimal)obj["minPrice"],
                    TickSize = (decimal)obj["tickSize"]
                };
                break;
            case BinanceSymbolFilterType.MaxNumberAlgorithmicOrders:
                result = new BinanceSymbolMaxAlgorithmicOrdersFilter
                {
                    MaxNumberAlgorithmicOrders = (int)obj["maxNumAlgoOrders"]
                };
                break;
            case BinanceSymbolFilterType.MaxNumberOrders:
                result = new BinanceSymbolMaxOrdersFilter
                {
                    MaxNumberOrders = (int)obj["maxNumOrders"]
                };
                break;

            case BinanceSymbolFilterType.IcebergParts:
                result = new BinanceSymbolIcebergPartsFilter
                {
                    Limit = (int)obj["limit"]
                };
                break;
            case BinanceSymbolFilterType.PricePercent:
                result = new BinanceSymbolPercentPriceFilter
                {
                    MultiplierUp = (decimal)obj["multiplierUp"],
                    MultiplierDown = (decimal)obj["multiplierDown"],
                    AveragePriceMinutes = (int)obj["avgPriceMins"]
                };
                break;
            case BinanceSymbolFilterType.MaxPosition:
                result = new BinanceSymbolMaxPositionFilter
                {
                    MaxPosition = obj.ContainsKey("maxPosition") ? (decimal)obj["maxPosition"] : 0
                };
                break;
            case BinanceSymbolFilterType.PercentagePriceBySide:
                result = new BinanceSymbolPercentPriceBySideFilter
                {
                    AskMultiplierUp = (decimal)obj["askMultiplierUp"],
                    AskMultiplierDown = (decimal)obj["askMultiplierDown"],
                    BidMultiplierUp = (decimal)obj["bidMultiplierUp"],
                    BidMultiplierDown = (decimal)obj["bidMultiplierDown"],
                    AveragePriceMinutes = (int)obj["avgPriceMins"]
                };
                break;
            case BinanceSymbolFilterType.TrailingDelta:
                result = new BinanceSymbolTrailingDeltaFilter
                {
                    MaxTrailingAboveDelta = (int)obj["maxTrailingAboveDelta"],
                    MaxTrailingBelowDelta = (int)obj["maxTrailingBelowDelta"],
                    MinTrailingAboveDelta = (int)obj["minTrailingAboveDelta"],
                    MinTrailingBelowDelta = (int)obj["minTrailingBelowDelta"],
                };
                break;
            case BinanceSymbolFilterType.IcebergOrders:
                result = new BinanceMaxNumberOfIcebergOrdersFilter
                {
                    MaxNumIcebergOrders = obj.ContainsKey("maxNumIcebergOrders") ? (int)obj["maxNumIcebergOrders"] : 0
                };
                break;
            default:
                Trace.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Can't parse symbol filter of type: " + obj["filterType"]);
                result = new BinanceSymbolFilter();
                break;
        }
#pragma warning restore 8604
        result.FilterType = type;
        return result;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var filter = (BinanceSymbolFilter)value!;
        writer.WriteStartObject();

        writer.WritePropertyName("filterType");
        writer.WriteValue(JsonConvert.SerializeObject(filter.FilterType, new SymbolFilterTypeConverter(false)));

        switch (filter.FilterType)
        {
            case BinanceSymbolFilterType.LotSize:
                var lotSizeFilter = (BinanceSymbolLotSizeFilter)filter;
                writer.WritePropertyName("maxQty");
                writer.WriteValue(lotSizeFilter.MaxQuantity);
                writer.WritePropertyName("minQty");
                writer.WriteValue(lotSizeFilter.MinQuantity);
                writer.WritePropertyName("stepSize");
                writer.WriteValue(lotSizeFilter.StepSize);
                break;
            case BinanceSymbolFilterType.MarketLotSize:
                var marketLotSizeFilter = (BinanceSymbolMarketLotSizeFilter)filter;
                writer.WritePropertyName("maxQty");
                writer.WriteValue(marketLotSizeFilter.MaxQuantity);
                writer.WritePropertyName("minQty");
                writer.WriteValue(marketLotSizeFilter.MinQuantity);
                writer.WritePropertyName("stepSize");
                writer.WriteValue(marketLotSizeFilter.StepSize);
                break;
            case BinanceSymbolFilterType.MinNotional:
                var minNotionalFilter = (BinanceSymbolMinNotionalFilter)filter;
                writer.WritePropertyName("minNotional");
                writer.WriteValue(minNotionalFilter.MinNotional);
                writer.WritePropertyName("applyToMarket");
                writer.WriteValue(minNotionalFilter.ApplyToMarketOrders);
                writer.WritePropertyName("avgPriceMins");
                writer.WriteValue(minNotionalFilter.AveragePriceMinutes);
                break;
            case BinanceSymbolFilterType.Price:
                var priceFilter = (BinanceSymbolPriceFilter)filter;
                writer.WritePropertyName("maxPrice");
                writer.WriteValue(priceFilter.MaxPrice);
                writer.WritePropertyName("minPrice");
                writer.WriteValue(priceFilter.MinPrice);
                writer.WritePropertyName("tickSize");
                writer.WriteValue(priceFilter.TickSize);
                break;
            case BinanceSymbolFilterType.MaxNumberAlgorithmicOrders:
                var algoFilter = (BinanceSymbolMaxAlgorithmicOrdersFilter)filter;
                writer.WritePropertyName("maxNumAlgoOrders");
                writer.WriteValue(algoFilter.MaxNumberAlgorithmicOrders);
                break;
            case BinanceSymbolFilterType.MaxPosition:
                var maxPositionFilter = (BinanceSymbolMaxPositionFilter)filter;
                writer.WritePropertyName("maxPosition");
                writer.WriteValue(maxPositionFilter.MaxPosition);
                break;
            case BinanceSymbolFilterType.MaxNumberOrders:
                var orderFilter = (BinanceSymbolMaxOrdersFilter)filter;
                writer.WritePropertyName("maxNumOrders");
                writer.WriteValue(orderFilter.MaxNumberOrders);
                break;
            case BinanceSymbolFilterType.IcebergParts:
                var icebergPartsFilter = (BinanceSymbolIcebergPartsFilter)filter;
                writer.WritePropertyName("limit");
                writer.WriteValue(icebergPartsFilter.Limit);
                break;
            case BinanceSymbolFilterType.PricePercent:
                var pricePercentFilter = (BinanceSymbolPercentPriceFilter)filter;
                writer.WritePropertyName("multiplierUp");
                writer.WriteValue(pricePercentFilter.MultiplierUp);
                writer.WritePropertyName("multiplierDown");
                writer.WriteValue(pricePercentFilter.MultiplierDown);
                writer.WritePropertyName("avgPriceMins");
                writer.WriteValue(pricePercentFilter.AveragePriceMinutes);
                break;
            case BinanceSymbolFilterType.TrailingDelta:
                var TrailingDelta = (BinanceSymbolTrailingDeltaFilter)filter;
                writer.WritePropertyName("maxTrailingAboveDelta");
                writer.WriteValue(TrailingDelta.MaxTrailingAboveDelta);
                writer.WritePropertyName("maxTrailingBelowDelta");
                writer.WriteValue(TrailingDelta.MaxTrailingBelowDelta);
                writer.WritePropertyName("minTrailingAboveDelta");
                writer.WriteValue(TrailingDelta.MinTrailingAboveDelta);
                writer.WritePropertyName("minTrailingBelowDelta");
                writer.WriteValue(TrailingDelta.MinTrailingBelowDelta);
                break;
            case BinanceSymbolFilterType.IcebergOrders:
                var MaxNumIcebergOrders = (BinanceMaxNumberOfIcebergOrdersFilter)filter;
                writer.WritePropertyName("maxNumIcebergOrders");
                writer.WriteValue(MaxNumIcebergOrders.MaxNumIcebergOrders);
                break;
            case BinanceSymbolFilterType.PercentagePriceBySide:
                var pricePercentSideBySideFilter = (BinanceSymbolPercentPriceBySideFilter)filter;
                writer.WritePropertyName("askMultiplierUp");
                writer.WriteValue(pricePercentSideBySideFilter.AskMultiplierUp);
                writer.WritePropertyName("askMultiplierDown");
                writer.WriteValue(pricePercentSideBySideFilter.AskMultiplierDown);
                writer.WritePropertyName("bidMultiplierUp");
                writer.WriteValue(pricePercentSideBySideFilter.BidMultiplierUp);
                writer.WritePropertyName("bidMultiplierDown");
                writer.WriteValue(pricePercentSideBySideFilter.BidMultiplierDown);
                writer.WritePropertyName("avgPriceMins");
                writer.WriteValue(pricePercentSideBySideFilter.AveragePriceMinutes);
                break;
            case BinanceSymbolFilterType.Notional:
                var notionalFilter = (BinanceSymbolNotionalFilter)filter;
                writer.WritePropertyName("minNotional");
                writer.WriteValue(notionalFilter.MinNotional);
                writer.WritePropertyName("maxNotional");
                writer.WriteValue(notionalFilter.MaxNotional);
                writer.WritePropertyName("applyMinToMarketOrders");
                writer.WriteValue(notionalFilter.ApplyMinToMarketOrders);
                writer.WritePropertyName("applyMaxToMarketOrders");
                writer.WriteValue(notionalFilter.ApplyMaxToMarketOrders);
                writer.WritePropertyName("avgPriceMins");
                writer.WriteValue(notionalFilter.AveragePriceMinutes);
                break;
            default:
                Trace.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Can't write symbol filter of type: " + filter.FilterType);
                break;
        }

        writer.WriteEndObject();
    }
}
