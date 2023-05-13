namespace LinkAPI.Interfaces
{
    public interface IUrlShortener
    {
        string GetShortUrl(HttpContext ctx);
    }
}
