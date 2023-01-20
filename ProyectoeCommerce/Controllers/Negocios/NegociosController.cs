using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoeCommerce.Datos;
using ProyectoeCommerce.Models.Autenticacion;
using ProyectoeCommerce.Models.Negocios;

namespace ProyectoeCommerce.Controllers.Negocios
{
    public class NegociosController : Controller
    {
        NegocioDatos _NegocioDatos = new NegocioDatos();
        CategoriaDatos _CategoriaDatos = new CategoriaDatos();
        ProductoDatos _ProductoDatos = new ProductoDatos();

        [Authorize(Roles = "negocio")]
        public IActionResult Index()
        {
            var idUsuario = User.Claims.Where(c => c.Type == "idUsuario").Select(c => c.Value).FirstOrDefault();
            ViewBag.Negocios = _NegocioDatos.ListarNegocioUsuario(Convert.ToInt32(idUsuario));

            return View();
        }

        public IActionResult Negocio(int idNegocio)
        {
            ViewBag.InformacionNegocio = _NegocioDatos.ObtenerNegocio(idNegocio);

            return View();
        }

        [Authorize(Roles = "negocio")]
        public IActionResult NuevoNegocio()
        {
            var listaCategorias = _CategoriaDatos.ListarCategorias();

            ViewBag.Categorias = listaCategorias;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "negocio")]
        public IActionResult NuevoNegocio(NegocioModel oNegocio)
        {
            var idUsuario = User.Claims.Where(c => c.Type == "idUsuario").Select(c => c.Value).FirstOrDefault();

            var respuesta = _NegocioDatos.RegistrarNegocio(oNegocio, int.Parse(idUsuario));

            if (respuesta.Item1)
            {
                TempData["Message"] = respuesta.Item2;
                return RedirectToAction("NuevoNegocio", "Negocios");
            }
            else
            {
                TempData["Message"] = respuesta.Item2;
                return RedirectToAction("NuevoNegocio", "Negocios");
            }
        }

        [Authorize(Roles = "negocio")]
        public IActionResult AdministrarNegocio(NegocioModel oNegocio)
        {
            var idUsuario = User.Claims.Where(c => c.Type == "idUsuario").Select(c => c.Value).FirstOrDefault();
            var respuesta = _NegocioDatos.ObtenerNegocio(Convert.ToInt32(oNegocio.idNegocio), int.Parse(idUsuario));

            ViewBag.InformacionNegocio = respuesta;

            ViewBag.Categorias = _CategoriaDatos.ListarCategorias();
            ;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "negocio")]
        public IActionResult AdministrarNegocioForm(NegocioModel oNegocio)
        {
            var respuesta = _NegocioDatos.EditarNegocio(oNegocio);
            var idUsuario = User.Claims.Where(c => c.Type == "idUsuario").Select(c => c.Value).FirstOrDefault();

            TempData["Message"] = respuesta.Item3;
            var respuestaNegocio =
                _NegocioDatos.ObtenerNegocio(Convert.ToInt32(oNegocio.idNegocio), Convert.ToInt32(idUsuario));

            ViewBag.InformacionNegocio = respuestaNegocio;

            ViewBag.Categorias = _CategoriaDatos.ListarCategorias();
            ;

            return View("AdministrarNegocio", respuesta.Item2);
        }

        [HttpPost]
        [Authorize(Roles = "negocio")]
        public IActionResult NuevoProducto(NegocioModel oNegocio)
        {
            ProductoModel productoModel = new ProductoModel();
            productoModel.idNegocio = oNegocio.idNegocio;
            return View(productoModel);
        }

        [HttpPost]
        [Authorize(Roles = "negocio")]
        public IActionResult NuevoProductoForm(ProductoModel oProducto)
        {
            var respuesta = _NegocioDatos.RegistrarProducto(oProducto, Convert.ToInt32(oProducto.idNegocio));
            TempData["Message"] = respuesta.Item2;
            return View("NuevoProducto", new ProductoModel());
        }

        [HttpPost]
        [Authorize(Roles = "negocio")]
        public IActionResult AdministrarProducto(int idNegocio, int idProducto)
        {
            var respuesta = _ProductoDatos.ObtenerProducto(idProducto, idNegocio);
            ViewBag.DatosProducto = respuesta;
            return View(respuesta.Item2);
        }

        [HttpPost]
        [Authorize(Roles = "negocio")]
        public IActionResult AdministrarProductoForm(ProductoModel oProducto)
        {
            var respuesta = _ProductoDatos.EditarProducto(oProducto);
            TempData["Message"] = respuesta.Item3;
            ViewBag.DatosProducto = respuesta;
            return View("AdministrarProducto", oProducto);
        }
    }
}