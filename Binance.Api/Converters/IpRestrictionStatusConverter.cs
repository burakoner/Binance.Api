namespace Binance.Api.Converters;

internal class IpRestrictionStatusConverter : BaseConverter<IpRestrictionStatus>
{
    public IpRestrictionStatusConverter() : this(true) { }
    public IpRestrictionStatusConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<IpRestrictionStatus, string>> Mapping => new List<KeyValuePair<IpRestrictionStatus, string>>
    {
        new KeyValuePair<IpRestrictionStatus, string>(IpRestrictionStatus.Unrestricted, "1"),
        new KeyValuePair<IpRestrictionStatus, string>(IpRestrictionStatus.RestrictAccessToTrustedIPsOnly, "2"),
    };
}
