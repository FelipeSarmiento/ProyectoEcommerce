using ProyectoeCommerce.Models.Negocios;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices.ComTypes;
using ProyectoeCommerce.Models.Negocios;

namespace ProyectoeCommerce.Datos
{
    public class NegocioDatos
    {
        public Tuple<bool, List<NegocioModel>> ListarNegocio()
        {
            var oNegocio = new List<NegocioModel>();

            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("listarNegocios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;


                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            do
                            {
                                oNegocio.Add(new NegocioModel()
                                {
                                    idNegocio = Convert.ToInt32(dr["idNegocio"]),
                                    NITNegocio = dr["NITNegocio"].ToString(),
                                    Nombre = dr["Nombre"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Direccion = dr["Direccion"].ToString(),
                                    idCategoria = Convert.ToInt32(dr["idCategoria"]),
                                    idUsuario = Convert.ToInt32(dr["idUsuario"])
                                });
                            } while (dr.Read());

                            return Tuple.Create(true, oNegocio);
                        }
                        else
                        {
                            return Tuple.Create(false, oNegocio);
                        }                    
                    }
                }
            }
            catch (Exception e)
            {
                return Tuple.Create(false, new List<NegocioModel>());
            }
        }

        public Tuple<bool, List<NegocioModel>> ListarNegocioUsuario(int idUsuario)
        {
            var oNegocio = new List<NegocioModel>();

            try
            {
                var cn = new Conexion();


                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("ListarNegociosUsuario", conexion);
                    cmd.Parameters.AddWithValue("idUsuario", idUsuario);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();


                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            do
                            {
                                oNegocio.Add(new NegocioModel()
                                {
                                    idNegocio = Convert.ToInt32(dr["idNegocio"]),
                                    NITNegocio = dr["NITNegocio"].ToString(),
                                    Nombre = dr["Nombre"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Direccion = dr["Direccion"].ToString(),
                                    idCategoria = Convert.ToInt32(dr["idCategoria"]),
                                    idUsuario = Convert.ToInt32(dr["idUsuario"]),
                                    cantidadProductos = ListarProductosNegocio(Convert.ToInt32(dr["idNegocio"])).Item2
                                        .Count
                                });
                            } while (dr.Read());

                            return Tuple.Create(true, oNegocio);
                        }
                        else
                        {
                            return Tuple.Create(false, oNegocio);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Tuple.Create(false, oNegocio);
            }
        }

        public Tuple<bool, string> RegistrarNegocio(NegocioModel oNegocio, int idUsuario)
        {
            bool rpta = false;

            try
            {
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("validarNegocio", conexion);
                    cmd.Parameters.AddWithValue("NITNegocio", oNegocio.NITNegocio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            rpta = true;
                            return Tuple.Create(false, "El NIT ya existe");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Tuple.Create(rpta, e.Message);
            }

            if (rpta == false)
            {
                try
                {
                    var cn = new Conexion();

                    using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                    {
                        conexion.Open();
                        SqlCommand cmd = new SqlCommand("registrarNegocio", conexion);
                        cmd.Parameters.AddWithValue("NITNegocio", oNegocio.NITNegocio);
                        cmd.Parameters.AddWithValue("Nombre", oNegocio.Nombre);
                        cmd.Parameters.AddWithValue("Telefono", oNegocio.Telefono);
                        cmd.Parameters.AddWithValue("Descripcion", oNegocio.Descripcion);
                        cmd.Parameters.AddWithValue("Direccion", oNegocio.Direccion);
                        cmd.Parameters.AddWithValue("idCategoria", oNegocio.idCategoria);
                        cmd.Parameters.AddWithValue("idUsuario", idUsuario);

                        cmd.CommandType = CommandType.StoredProcedure;

                        //cmd.ExecuteNonQuery();

                        using (var dr = cmd.ExecuteReader())
                        {
                            if (!dr.Read())
                            {
                                return new Tuple<bool, string>(true, "Se registró correctamente");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    return new Tuple<bool, string>(false, e.Message);
                }
            }

            return new Tuple<bool, string>(false, "Se produjo un error al registrar el negocio");
        }

        public Tuple<bool, List<ProductoModel>, string> ListarProductosNegocio(int idNegocio)
        {
            List<ProductoModel> oProducto = new List<ProductoModel>();
            try
            {
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("ListarProductosNegocio", conexion);
                    cmd.Parameters.AddWithValue("idNegocio", idNegocio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            do
                            {
                                oProducto.Add(new ProductoModel()
                                {
                                    idProducto = Convert.ToInt32(dr["idProducto"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Precio = dr["Precio"].ToString(),
                                    idNegocio = Convert.ToInt32(dr["idNegocio"]),
                                    CodigoProducto = dr["CodigoProducto"].ToString()
                                });
                            } while (dr.Read());

                            return Tuple.Create(true, oProducto, "Productos cargados");
                        }
                        else
                        {
                            return Tuple.Create(false, oProducto, "No hay productos registrados");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Tuple.Create(false, oProducto, "Ha ocurrido un problema");
            }
        }

        public Tuple<bool, NegocioModel, List<ProductoModel>, string> ObtenerNegocio(int idNegocio)
        {
            bool estado = false;
            NegocioModel oNegocio = new NegocioModel();
            List<ProductoModel> oProducto = new List<ProductoModel>();
            try
            {
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("ObtenerNegocio", conexion);
                    cmd.Parameters.AddWithValue("idNegocio", idNegocio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            oNegocio.Nombre = dr["Nombre"].ToString();
                            oNegocio.NITNegocio = dr["NITNegocio"].ToString();
                            oNegocio.Telefono = dr["Telefono"].ToString();
                            oNegocio.Descripcion = dr["Descripcion"].ToString();
                            oNegocio.Direccion = dr["Direccion"].ToString();
                            oNegocio.idCategoria = Convert.ToInt32(dr["idCategoria"]);
                            oNegocio.idNegocio = Convert.ToInt32(dr["idNegocio"]);
                            estado = true;
                        }
                        else
                        {
                            return Tuple.Create(false, oNegocio, oProducto, "No hay negocio registrado");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Tuple.Create(false, oNegocio, oProducto, e.Message);
            }
            
            oProducto = ListarProductosNegocio(idNegocio).Item2;
            
            return Tuple.Create(estado, oNegocio, oProducto, "Negocio cargado");
        }
        public Tuple<bool, NegocioModel, List<ProductoModel>, string> ObtenerNegocio(int idNegocio, int idUsuario)
        {
            bool estado = false;
            NegocioModel negocio = new NegocioModel();

            List<ProductoModel> productos = new List<ProductoModel>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerNegocio", conexion);
                cmd.Parameters.AddWithValue("idNegocio", idNegocio);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        negocio.idNegocio = Convert.ToInt32(dr["idNegocio"]);
                        negocio.NITNegocio = dr["NITNegocio"].ToString();
                        negocio.Nombre = dr["Nombre"].ToString();
                        negocio.Descripcion = dr["Descripcion"].ToString();
                        negocio.Telefono = dr["Telefono"].ToString();
                        negocio.Direccion = dr["Direccion"].ToString();
                        negocio.idCategoria = Convert.ToInt32(dr["idCategoria"]);
                        negocio.idUsuario = Convert.ToInt32(dr["idUsuario"]);
                    }
                }
            }

            if (negocio.idUsuario == idUsuario)
            {
                try
                {
                    var cnPro = new Conexion();

                    using (var conexion = new SqlConnection(cnPro.getCadenaSQL()))
                    {
                        conexion.Open();
                        SqlCommand cmd = new SqlCommand("ListarProductosNegocio", conexion);
                        cmd.Parameters.AddWithValue("idNegocio", idNegocio);
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                do
                                {
                                    productos.Add(new ProductoModel()
                                    {
                                        idProducto = Convert.ToInt32(dr["idProducto"]),
                                        Nombre = dr["Nombre"].ToString(),
                                        Descripcion = dr["Descripcion"].ToString(),
                                        Precio = dr["Precio"].ToString(),
                                        idNegocio = Convert.ToInt32(dr["idNegocio"]),
                                        CodigoProducto = dr["CodigoProducto"].ToString()
                                    });
                                } 
                                while (dr.Read());
                            }
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    return Tuple.Create(false, negocio, productos, "Ha ocurrido un problema");
                }
                return Tuple.Create(true, negocio, productos, "Negocio cargado");
            }
            else
            {
                return Tuple.Create(false, new NegocioModel(), new List<ProductoModel>(), "No tiene permisos para ver este negocio");
            }
        }

        public Tuple<bool, NegocioModel, string> EditarNegocio(NegocioModel oNegocio)
        {

            try
            {
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    
                    SqlCommand cmd = new SqlCommand("EditarNegocio", conexion);
                    cmd.Parameters.AddWithValue("idNegocio", oNegocio.idNegocio);
                    cmd.Parameters.AddWithValue("NITNegocio", oNegocio.NITNegocio);
                    cmd.Parameters.AddWithValue("Nombre", oNegocio.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", oNegocio.Descripcion);
                    cmd.Parameters.AddWithValue("Telefono", oNegocio.Telefono);
                    cmd.Parameters.AddWithValue("Direccion", oNegocio.Direccion);
                    cmd.Parameters.AddWithValue("idCategoria", oNegocio.idCategoria);
                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            return Tuple.Create(true, oNegocio, "Negocio editado");
                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
            return Tuple.Create(false, oNegocio, "No se pudo editar el negocio");
        }

        public Tuple<bool, string> RegistrarProducto(ProductoModel oProducto, int idNegocio)
        {

            NegocioModel negocio;
            try
            {
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("registrarProducto", conexion);
                    cmd.Parameters.AddWithValue("Nombre", oProducto.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", oProducto.Descripcion);
                    cmd.Parameters.AddWithValue("Precio", oProducto.Precio);
                    cmd.Parameters.AddWithValue("idNegocio", idNegocio);
                    cmd.Parameters.AddWithValue("CodigoProducto", oProducto.CodigoProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            return Tuple.Create(true, "Producto registrado");
                        }
                        else
                        {
                            return Tuple.Create(false, "El producto no se pudo registrar");
                        }
                    }
                }
            }
            catch (Exception e)
            {
               return Tuple.Create(false, "El producto no se pudo registrar");
            }
        }
    }
}
