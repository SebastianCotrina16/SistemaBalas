using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace frontend_admin.Models
{
    public class Reservas
    {
        [Key]
        public int? IdReserva { get; set; }

        [Required]
        public int? IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }

        [Required]
        public int? IdSala { get; set; }

        [ForeignKey("IdSala")]
        public SalaTiro? Sala { get; set; }

        [Required]
        public DateTime? FechaReserva { get; set; }

        [StringLength(20)]
        public string? Estado { get; set; } = "Pendiente";  

        public ICollection<Practicas>? Practicas { get; set; }
    }
}
