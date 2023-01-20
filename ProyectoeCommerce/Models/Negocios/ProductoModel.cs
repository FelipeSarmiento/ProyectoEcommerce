namespace ProyectoeCommerce.Models.Negocios
{
    public class ProductoModel
    {

        public int idProducto { get; set; }
        public string? CodigoProducto { get; set; }
        public string? Nombre{ get; set; }
        public string? Precio{ get; set; }
        public string? Descripcion{ get; set; }
        public int? idNegocio{ get; set; }

    }
}
