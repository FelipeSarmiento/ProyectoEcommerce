

namespace ProyectoeCommerce.Models.Autenticacion
{
    public class UsuarioModel
    {

        public int idUsuario { get; set; }
        public string? Usuario { get; set; }
        public string? PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? FNacimiento { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? TipoUsuario { get; set; }
        public string? Password { get; set; }

    }
}
