using System.ComponentModel.DataAnnotations;

namespace LinkAPI.Models
{
    public class Link
    {
        public long Id { get; set; }
        
        public string OriginLink { get; set; }
        
        [MinLength(1)]
        [MaxLength(50)]
        public string ShortLink { get; set; }
        
        public DateTime Date { get; set; }
        
        [MaxLength(30)]
        public string? CreatedBy { get; set; }
    }
}
