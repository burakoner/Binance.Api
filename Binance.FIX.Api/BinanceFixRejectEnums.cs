namespace Binance.FIX.Api;

/// <summary>
/// Published Binance Spot FIX SessionRejectReason values.
/// </summary>
public enum BinanceFixSessionRejectReason
{
    /// <summary>The rejected field tag number is invalid.</summary>
    InvalidTagNumber = 0,

    /// <summary>A required field is missing.</summary>
    RequiredTagMissing = 1,

    /// <summary>The field is not defined for this message type.</summary>
    TagNotDefinedForMessageType = 2,

    /// <summary>The field tag is undefined.</summary>
    UndefinedTag = 3,

    /// <summary>The field value is incorrect.</summary>
    IncorrectValue = 5,

    /// <summary>The field value uses an incorrect data format.</summary>
    IncorrectDataFormat = 6,

    /// <summary>The message signature is invalid.</summary>
    SignatureProblem = 8,

    /// <summary>The SendingTime is outside the accepted accuracy window.</summary>
    SendingTimeAccuracyProblem = 10,

    /// <summary>The message failed XML validation.</summary>
    XmlValidationError = 12,

    /// <summary>A field tag appears more than once.</summary>
    TagAppearsMoreThanOnce = 13,

    /// <summary>A field tag is outside the required order.</summary>
    TagOutOfRequiredOrder = 14,

    /// <summary>Repeating-group fields are outside the required order.</summary>
    RepeatingGroupFieldsOutOfOrder = 15,

    /// <summary>A repeating-group count does not match the number of entries.</summary>
    IncorrectRepeatingGroupCount = 16,

    /// <summary>Another reason; inspect ErrorCode and ErrorText.</summary>
    Other = 99
}
