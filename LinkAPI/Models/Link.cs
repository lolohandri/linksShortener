using LinkAPI.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace LinkAPI.Models
{
    public class Link
    {
        public long Id { get; set; }
        public string OriginLink { get; set; }
        public string? ShortLink { get; set; }
        public DateTime Date { get; set; }
        public string? CreatedBy { get; set; }
    }
}
