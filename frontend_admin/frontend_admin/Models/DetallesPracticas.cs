using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace frontend_admin.Models
{
    public class DetallesPracticas
    {
        [Key]
        public int? IdDetallePractica { get; set; }

        [Required]
        public int? IdPractica { get; set; }

        [ForeignKey("IdPractica")]
        public Practicas? Practica { get; set; }

        public int? DisparoNumero { get; set; }

        public float? Precision { get; set; }

        [StringLength(255)]
        public string? Ubicacion { get; set; }

        [StringLength(255)]
        public string? RutaImagen { get; set; }
    }
}
