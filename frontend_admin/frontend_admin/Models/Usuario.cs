using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace frontend_admin.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public string? Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string? Correo { get; set; }

        [Required]
        [StringLength(200)]
        public string? Clave { get; set; }

        public bool Restablecer { get; set; } = false;
        public bool? Confirmado { get; set; } = false;

        [StringLength(200)]
        public string Token { get; set; } = " ";

        [Required]
        [StringLength(20)]
        public string? DNI { get; set; }

        // Optional collections
        public virtual ICollection<ImpactosBala> ImpactosBala { get; set; } = new HashSet<ImpactosBala>();
        public virtual ICollection<Reportes> Reportes { get; set; } = new HashSet<Reportes>();
        public virtual ICollection<Reservas> Reservas { get; set; } = new HashSet<Reservas>();
        public virtual ICollection<Practicas> Practicas { get; set; } = new HashSet<Practicas>();
    }
}
