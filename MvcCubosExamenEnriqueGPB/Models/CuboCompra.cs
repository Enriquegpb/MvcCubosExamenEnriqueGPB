using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcCubosExamenEnriqueGPB.Models
{
    public class CuboCompra
    {
        
        public int IdPedido { get; set; }
        
        public int IdCubo { get; set; }
        
        public int IdUsuario { get; set; }
        
        public string Nonbre { get; set; }
        
        public string Marca { get; set; }
        
        public string Imagen { get; set; }
        
        public int Precio { get; set; }
    }
}
