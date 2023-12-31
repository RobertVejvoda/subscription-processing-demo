using System.Text.Encodings.Web;

namespace CustomerService.Helpers;

public static class Converter
{
    public static string ConvertToShortString(string input, int maxLength)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var base64 = Convert.ToBase64String(bytes);
        return UrlEncoder.Default.Encode(base64[..maxLength]);
    }
}