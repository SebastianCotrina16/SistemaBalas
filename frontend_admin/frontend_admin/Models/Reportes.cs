using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace frontend_admin.Models
{
    public class Reportes
    {
        [Key]
        public int IdReporte { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }

        public DateTime FechaReporte { get; set; } = DateTime.Now;

        public int TotalImpactos { get; set; }

        public float PromedioPrecision { get; set; }

        public string? Detalles { get; set; }
    }
}
