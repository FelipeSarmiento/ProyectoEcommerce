using System.Data.SqlClient;
using System.Data;

using ProyectoeCommerce.Models.Negocios;

namespace ProyectoeCommerce.Datos
{
    public class CategoriaDatos
    {

        public Tuple<bool, List<CategoriaModel>> ListarCategorias()
        {
            var categorias = new List<CategoriaModel>();

            try
            {
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("listarCategorias", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            while (dr.Read())
                            {
                                var categoria = new CategoriaModel();
                                categoria.idCategoria = Convert.ToInt32(dr["idCategoria"]);
                                categoria.Nombre = dr["NombreCategoria"].ToString();
                                categoria.Descripcion = dr["Descripcion"].ToString();
                                categorias.Add(categoria);
                            }
                            return Tuple.Create(true, categorias);
                        }
                        else
                        {
                            return Tuple.Create(false, categorias);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Tuple.Create(false, new List<CategoriaModel>());
            }
        }
    }
}
