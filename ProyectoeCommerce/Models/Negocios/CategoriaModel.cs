using System.ComponentModel.DataAnnotations;

namespace ProyectoeCommerce.Models.Negocios
{
    public class CategoriaModel
    {

        public int idCategoria { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion{ get; set; }

    }
}
