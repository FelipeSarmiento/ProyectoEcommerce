
namespace ProyectoeCommerce.Models.Negocios
{
    public class NegocioModel
    {

        public int? idNegocio { get; set; }

        public string? NITNegocio { get; set; }
        public string? Nombre { get; set; }
        public string? Telefono { get; set; }
        public string? Descripcion { get; set; }
        public string? Direccion { get; set; }
        public int? idCategoria { get; set; }
        public int? idUsuario { get; set; }
        
        public int? cantidadProductos { get; set; }

    }
}
