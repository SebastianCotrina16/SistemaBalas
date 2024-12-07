using System.ComponentModel.DataAnnotations;

namespace frontend_admin.Models
{
    public class ExamenConfiguracion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NumeroDisparos { get; set; }
    }
}
