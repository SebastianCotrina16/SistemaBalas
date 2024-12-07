using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace frontend_admin.Models
{
    public class Practicas
    {
        [Key]
        public int? IdPractica { get; set; }

        [Required]
        public int? IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }

        [Required]
        public int? IdReserva { get; set; }

        [ForeignKey("IdReserva")]
        public Reservas? Reserva { get; set; }

        public DateTime? FechaPractica { get; set; } = DateTime.Now;

        public int? TotalDisparos { get; set; }

        public float? PrecisionPromedio { get; set; }

        public ICollection<DetallesPracticas>? DetallesPracticas { get; set; }
    }
}
