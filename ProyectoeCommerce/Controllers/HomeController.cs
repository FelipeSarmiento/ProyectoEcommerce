using System.Diagnostics;
using ProyectoeCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProyectoeCommerce.Models.Negocios;
using ProyectoeCommerce.Datos;

namespace ProyectoeCommerce.Controllers
{
    public class HomeController : Controller
    {
        private NegocioDatos _Negocios = new NegocioDatos();
        private CategoriaDatos _Categorias = new CategoriaDatos();
        private ProductoDatos _Productos = new ProductoDatos();
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        public IActionResult Index()
        {
            ViewBag.ListaNegocios = _Negocios.ListarNegocio();;
            ViewBag.ListaCategorias = _Categorias.ListarCategorias();;
            ViewBag.ListaProductos = _Productos.ListarProductos();
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}