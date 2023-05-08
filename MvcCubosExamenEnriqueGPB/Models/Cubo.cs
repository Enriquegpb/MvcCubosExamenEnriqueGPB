using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcCubosExamenEnriqueGPB.Models
{
    public class Cubo
    {
        
        public int IdCubo { get; set; }
        
        public string Nonbre { get; set; }
        
        public string Marca { get; set; }
        
        public string Imagen { get; set; }
        
        public int Precio { get; set; }
    }
}
