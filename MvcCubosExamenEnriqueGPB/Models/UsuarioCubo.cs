using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcCubosExamenEnriqueGPB.Models
{
    public class UsuarioCubo
    {
        
        public int IdUsuario { get; set; }
        
        public string Nonbre { get; set; }
        
        public string Email { get; set; }
        
        public string Pass { get; set; }
        
        public string Imagen { get; set; }
    }
}
