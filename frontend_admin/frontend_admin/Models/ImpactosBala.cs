using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace frontend_admin.Models
{
    public class ImpactosBala
    {
        [Key]
        public int IdImpacto { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string? Ubicacion { get; set; }

        public float Precision { get; set; }

        [StringLength(255)]
        public string? RutaImagen { get; set; }
    }
}
