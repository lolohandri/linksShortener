using LinkAPI.Interfaces;

namespace LinkAPI.Utils
{
    public class UrlShortener : IUrlShortener
    {
        public string GetShortUrl(HttpContext ctx)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@az";
            var randomStr = new string(Enumerable.Repeat(chars, 8)
                .Select(str => str[random.Next(str.Length)]).ToArray());

            var resultStr = $"{ctx.Request.Scheme}://{ctx.Request.Host}/{randomStr}";

            return resultStr;
        }
    }
}
