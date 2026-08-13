using System.Globalization;
using QuickFix;
using QuickFix.Fields;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class RejectTests
{
    [Fact]
    public void ParsesCompleteCurrentRejectSurface()
    {
        var message = CreateReject();
        Set(message, Tags.RefSeqNum, long.MaxValue.ToString(CultureInfo.InvariantCulture));
        Set(message, Tags.RefTagID, long.MinValue.ToString(CultureInfo.InvariantCulture));
        Set(message, Tags.RefMsgType, "XAK");
        Set(message, Tags.SessionRejectReason, "12");
        Set(message, BinanceFixRejectParser.ErrorCodeTag, "-1102");
        Set(message, Tags.Text, "Mandatory parameter was not sent.");

        var reject = BinanceFixRejectParser.Parse(message);

        Assert.Equal(long.MaxValue, reject.ReferencedSequenceNumber);
        Assert.Equal(long.MinValue, reject.ReferencedTagId);
        Assert.Equal("XAK", reject.ReferencedMessageType);
        Assert.Equal(BinanceFixSessionRejectReason.XmlValidationError, reject.SessionRejectReason);
        Assert.Equal(-1102, reject.ErrorCode);
        Assert.Equal("Mandatory parameter was not sent.", reject.ErrorText);
    }

    [Fact]
    public void AcceptsRejectWithEveryOptionalFieldOmitted()
    {
        var reject = BinanceFixRejectParser.Parse(CreateReject());

        Assert.Null(reject.ReferencedSequenceNumber);
        Assert.Null(reject.ReferencedTagId);
        Assert.Null(reject.ReferencedMessageType);
        Assert.Null(reject.SessionRejectReason);
        Assert.Null(reject.ErrorCode);
        Assert.Null(reject.ErrorText);
    }

    [Theory]
    [InlineData("0", BinanceFixSessionRejectReason.InvalidTagNumber)]
    [InlineData("1", BinanceFixSessionRejectReason.RequiredTagMissing)]
    [InlineData("2", BinanceFixSessionRejectReason.TagNotDefinedForMessageType)]
    [InlineData("3", BinanceFixSessionRejectReason.UndefinedTag)]
    [InlineData("5", BinanceFixSessionRejectReason.IncorrectValue)]
    [InlineData("6", BinanceFixSessionRejectReason.IncorrectDataFormat)]
    [InlineData("8", BinanceFixSessionRejectReason.SignatureProblem)]
    [InlineData("10", BinanceFixSessionRejectReason.SendingTimeAccuracyProblem)]
    [InlineData("12", BinanceFixSessionRejectReason.XmlValidationError)]
    [InlineData("13", BinanceFixSessionRejectReason.TagAppearsMoreThanOnce)]
    [InlineData("14", BinanceFixSessionRejectReason.TagOutOfRequiredOrder)]
    [InlineData("15", BinanceFixSessionRejectReason.RepeatingGroupFieldsOutOfOrder)]
    [InlineData("16", BinanceFixSessionRejectReason.IncorrectRepeatingGroupCount)]
    [InlineData("99", BinanceFixSessionRejectReason.Other)]
    public void ParsesEveryPublishedSessionRejectReason(
        string value,
        BinanceFixSessionRejectReason expected)
    {
        var message = CreateReject();
        Set(message, Tags.SessionRejectReason, value);

        Assert.Equal(expected, BinanceFixRejectParser.Parse(message).SessionRejectReason);
    }

    [Theory]
    [InlineData(45, "1.5")]
    [InlineData(371, "9223372036854775808")]
    [InlineData(373, "4")]
    [InlineData(372, "")]
    [InlineData(372, "D\n")]
    [InlineData(25016, "-9223372036854775809")]
    [InlineData(58, "bad\u001ftext")]
    public void RejectsInvalidPublishedFieldDomain(int tag, string value)
    {
        var message = CreateReject();
        Set(message, tag, value);

        Assert.Throws<FormatException>(() => BinanceFixRejectParser.Parse(message));
    }

    [Fact]
    public void AcceptsEmptyOptionalErrorText()
    {
        var message = CreateReject();
        Set(message, Tags.Text, string.Empty);

        Assert.Equal(string.Empty, BinanceFixRejectParser.Parse(message).ErrorText);
    }

    [Fact]
    public void RejectsWrongMessageTypeAndDictionarylessDuplicates()
    {
        var wrongType = CreateReject();
        wrongType.Header.SetField(new MsgType(MsgType.EXECUTION_REPORT));

        Assert.Throws<FormatException>(() => BinanceFixRejectParser.Parse(wrongType));

        var duplicate = CreateReject();
        duplicate.RepeatedTags.Add(new StringField(Tags.Text, "duplicate"));

        Assert.Throws<FormatException>(() => BinanceFixRejectParser.Parse(duplicate));
    }

    [Fact]
    public void ParserRejectsNullMessageAndMissingMessageType()
    {
        Assert.Throws<ArgumentNullException>(() => BinanceFixRejectParser.Parse(null!));
        Assert.Throws<FormatException>(() => BinanceFixRejectParser.Parse(new Message()));
    }

    private static Message CreateReject()
    {
        var message = new Message();
        message.Header.SetField(new MsgType(MsgType.REJECT));
        return message;
    }

    private static void Set(FieldMap fields, int tag, string value)
        => fields.SetField(new StringField(tag, value));
}
