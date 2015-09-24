using System.ComponentModel.DataAnnotations;

namespace AuthorizeServer.api.Models
{
    public class AudienceModel
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}