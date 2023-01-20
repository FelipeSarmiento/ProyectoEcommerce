using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProyectoeCommerce.Datos;
using ProyectoeCommerce.Models.Autenticacion;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ProyectoeCommerce.Controllers.Autenticacion
{
    public class AutenticacionController : Controller
    {

        UsuarioDatos _UsuarioDatos = new UsuarioDatos();

        public IActionResult Login()
        {
            var nombre = User.Claims.Where(c => c.Type == "nombre").Select(c => c.Value).FirstOrDefault();

			if (!String.IsNullOrEmpty(nombre))
            {
                ViewBag.nombre = nombre;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        }


        [HttpPost]

        public async Task<IActionResult> Login(UsuarioModel oUsuario)
        {
            var respuesta = _UsuarioDatos.Validar(oUsuario);

            if (respuesta.Item1)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", oUsuario.Usuario));
                claims.Add(new Claim("idUsuario", respuesta.Item3.idUsuario.ToString()));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, oUsuario.Usuario)); 
                claims.Add(new Claim(ClaimTypes.Name, oUsuario.Usuario)); 
                claims.Add(new Claim("nombre", respuesta.Item3.PrimerNombre)); 
                claims.Add(new Claim(ClaimTypes.Role, respuesta.Item3.TipoUsuario)); 
                 
                
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Message"] = respuesta.Item2;
                return View();
            }

        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UsuarioModel oCliente)
        {
            var respuesta = _UsuarioDatos.Registrar(oCliente);
            TempData["Message"] = respuesta.Item2;
            return View();
        }

    }
}
