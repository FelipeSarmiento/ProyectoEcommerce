using System.Data.SqlClient;
using System.Data;

using ProyectoeCommerce.Models.Negocios;

namespace ProyectoeCommerce.Datos;

public class ProductoDatos
{
    public List<ProductoModel> ListarProductos()
    {
        List<ProductoModel> listaProductos = new List<ProductoModel>();
        try
        {
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("listarProductos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        do
                        {
                            listaProductos.Add(new ProductoModel()
                            {
                                idProducto = Convert.ToInt32(dr["idProducto"]),
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                Descripcion = dr["descripcion"].ToString(),
                                Precio = dr["precio"].ToString(),
                                idNegocio = Convert.ToInt32(dr["idNegocio"])
                            });
                        } while (dr.Read());
                    }
                }

            }

        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
        }

        return listaProductos;
    }

    public Tuple<bool, ProductoModel, string> ObtenerProducto(int idProducto, int idNegocio)
    {
        ProductoModel producto = new ProductoModel();
        try
        {
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerProducto", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("idProducto", idProducto);

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (Convert.ToInt32(dr["idNegocio"]) == idNegocio)
                        {
                            producto.idProducto = Convert.ToInt32(dr["idProducto"]);
                            producto.CodigoProducto = dr["CodigoProducto"].ToString();
                            producto.Nombre = dr["nombre"].ToString();
                            producto.Descripcion = dr["descripcion"].ToString();
                            producto.Precio = dr["precio"].ToString();
                            producto.idNegocio = Convert.ToInt32(dr["idNegocio"]);
                            return Tuple.Create(true, producto, "producto encontrado");
                        }
                        else
                        {
                            return Tuple.Create(false, producto, "No se encontro el producto para el negocio");
                        }

                    }
                    else
                    {
                        return Tuple.Create(false, producto, "producto no encontrado");
                    }
                }
            }
        }
        catch (Exception e)
        {
            return Tuple.Create(false, producto, e.Message);
        }
    }

    public Tuple<bool, ProductoModel, string> EditarProducto(ProductoModel oProducto)
    {
        try
        {
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("EditarProducto", conexion);
                cmd.Parameters.AddWithValue("idProducto", oProducto.idProducto);
                cmd.Parameters.AddWithValue("CodigoProducto", oProducto.CodigoProducto);
                cmd.Parameters.AddWithValue("Nombre", oProducto.Nombre);
                cmd.Parameters.AddWithValue("Descripcion", oProducto.Descripcion);
                cmd.Parameters.AddWithValue("Precio", oProducto.Precio);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                    {
                        return Tuple.Create(true, oProducto, "Se editó el producto");
                    }
                    else
                    {
                        return Tuple.Create(false, oProducto, "No se pudo editar el producto");
                    }
                }
            }
        }
        catch (Exception e)
        {
            return Tuple.Create(false, oProducto, e.Message);
        }
    }
}