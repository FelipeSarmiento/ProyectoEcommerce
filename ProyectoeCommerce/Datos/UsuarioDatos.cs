using ProyectoeCommerce.Models.Autenticacion;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using ProyectoeCommerce.Models.Negocios;

namespace ProyectoeCommerce.Datos
{
    public class UsuarioDatos
    {
        [HttpPost]
        public Tuple<bool, string, UsuarioModel> Validar(UsuarioModel oUsuario)
        {

            bool existenciaUsuario = ValidarExistencia(oUsuario.Usuario);

            if (existenciaUsuario)
            {
                var usuarios = new UsuarioModel();

                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("validarUsuario", conexion);
                    cmd.Parameters.AddWithValue("Usuario", oUsuario.Usuario);
                    cmd.Parameters.AddWithValue("Password", oUsuario.Password);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuarios.idUsuario = Convert.ToInt32(dr["idUsuario"]);
                            usuarios.Usuario = dr["Usuario"].ToString();
                            usuarios.PrimerNombre = dr["PrimerNombre"].ToString();
                            usuarios.SegundoNombre = dr["SegundoNombre"].ToString();
                            usuarios.PrimerApellido = dr["PrimerApellido"].ToString();
                            usuarios.SegundoApellido = dr["SegundoApellido"].ToString();
                            usuarios.FNacimiento = dr["FNacimiento"].ToString();
                            usuarios.Email = dr["Email"].ToString();
                            usuarios.Telefono = dr["Telefono"].ToString();
                            usuarios.Direccion = dr["Direccion"].ToString();
                            usuarios.TipoUsuario = dr["TipoUsuario"].ToString();
                            
                            return Tuple.Create(true, "Datos Correctos", usuarios);
                        }
                        else
                        {
                            return Tuple.Create(false, "El usuario o contraseña son incorrectos", usuarios);
                        }
                    }
                }
}
            else
            {
                return Tuple.Create(false, "El usuario no existe", new UsuarioModel());
            }
        }
        [HttpPost]
        public Tuple<bool, string> Registrar(UsuarioModel oUsuario)
        {

            bool exitenciaUsuario = ValidarExistencia(oUsuario.Usuario);

            if (exitenciaUsuario == false)
            {
                try
                {
                    var cn = new Conexion();

                    using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                    {
                        conexion.Open();
                        SqlCommand cmd = new SqlCommand("registrarUsuario", conexion);
                        cmd.Parameters.AddWithValue("Usuario", oUsuario.Usuario);
                        cmd.Parameters.AddWithValue("PrimerNombre", oUsuario.PrimerNombre);
                        if (oUsuario.SegundoNombre != "")
                        {
                            cmd.Parameters.AddWithValue("SegundoNombre", oUsuario.SegundoNombre);  
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("SegundoNombre", "");  
                        }
                        cmd.Parameters.AddWithValue("PrimerApellido", oUsuario.PrimerApellido);
                        cmd.Parameters.AddWithValue("SegundoApellido", oUsuario.SegundoApellido);
                        cmd.Parameters.AddWithValue("FNacimiento", oUsuario.FNacimiento);
                        cmd.Parameters.AddWithValue("Email", oUsuario.Email);
                        cmd.Parameters.AddWithValue("Telefono", oUsuario.Telefono);
                        cmd.Parameters.AddWithValue("Direccion", oUsuario.Direccion);
                        cmd.Parameters.AddWithValue("TipoUsuario", oUsuario.TipoUsuario);
                        cmd.Parameters.AddWithValue("Password", oUsuario.Password);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();

                    }
                    return Tuple.Create(true, "Usuario registrado correctamente");
                }
                catch(Exception e)
                {
                    return Tuple.Create(false, e.Message);
                }
            }
            else
            {
                return Tuple.Create(false, "El usuario ya existe");
            }

        }
        
        public bool ValidarExistencia(string usuario)
        {
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("validarExistenciaUsuario", conexion);
                cmd.Parameters.AddWithValue("Usuario", usuario);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
