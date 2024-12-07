using System.ComponentModel.DataAnnotations;

namespace frontend_admin.Models
{
    public class SalaTiro
    {
        [Key]
        public int? IdSala { get; set; }

        [Required]
        [StringLength(50)]
        public string? Nombre { get; set; }

        public bool? Disponible { get; set; } = true;


        public ICollection<Reservas>? Reservas { get; set; }
    }
}
