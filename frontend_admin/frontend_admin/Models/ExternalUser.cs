using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace frontend_admin.Models
{
    [Table("users")]
    public class ExternalUser
    {
        [Key]
        [StringLength(20)]
        public string? dni { get; set; } 

        public string? name { get; set; }

        public string? face_descriptor { get; set; }

        public string? image_path { get; set; }
    }
}
