using System;
using System.Globalization;
using System.Text;

namespace PitneyBowes.Developer.ShippingApi.Rules
{
    class ShippingApiFormatProvider : IFormatProvider, ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (typeof(IShipment).IsAssignableFrom(arg.GetType()))
            {
                // Format string "{0:ToAddress} {1:FromAddress}"
                var shipment = arg as IShipment;
                switch (format.ToLower())
                {
                    // Binary formatting.
                    case "toaddress":
                        return FormatAddress(shipment.FromAddress.CountryCode, shipment.ToAddress.CountryCode, shipment.ToAddress);
                    case "fromaddress":
                        return FormatAddress(shipment.FromAddress.CountryCode, shipment.ToAddress.CountryCode, shipment.FromAddress);
                    // Handle unsupported format strings.
                    default:
                        try
                        {
                            return HandleOtherFormats(format, arg);
                        }
                        catch (FormatException e)
                        {
                            throw new FormatException(String.Format("The format of '{0}' is invalid.", format), e);
                        }
                }
            }
            else
            {
                try
                {
                    return HandleOtherFormats(format, arg);
                }
                catch (FormatException e)
                {
                    throw new FormatException(String.Format("The format of '{0}' is invalid.", format), e);
                }
            }
        }

        private string HandleOtherFormats(string format, object arg)
        {
            if (arg is IFormattable)
                return ((IFormattable)arg).ToString(format, CultureInfo.CurrentCulture);
            else if (arg != null)
                return arg.ToString();
            else
                return String.Empty;
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        private string FormatAddress( string originCountry, string destinationCountry, IAddress address )
        {
            var sb = new StringBuilder();
            //TODO: Implement other country formats look at this http://www.bitboost.com/ref/international-address-formats.html#Formats
            if (destinationCountry == "US")
            {
                if (address == null) return "";
                if (address.Name != null && address.Name != "") sb.AppendLine(address.Name);
                if (address.Company != null && address.Company != "") sb.AppendLine(address.Company);
                foreach (var line in address.AddressLines)
                {
                    if (line != null && line != "") sb.AppendLine(line);
                }
                sb.AppendFormat("{0} {1} {2}", address.CityTown, address.StateProvince, address.PostalCode);
                if (originCountry != destinationCountry)
                {
                    sb.AppendLine();
                    sb.Append(CountryRule.Rules[address.CountryCode]);
                }
            }
            return sb.ToString();
        }
    }
}
