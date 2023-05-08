using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcCubosExamenEnriqueGPB.Filters;
using MvcCubosExamenEnriqueGPB.Models;
using MvcCubosExamenEnriqueGPB.Services;
using System.Configuration;

namespace MvcCubosExamenEnriqueGPB.Controllers
{
    public class CubosController : Controller
    {
        private ServiceCubos service;
        private ServiceBlobCubos serviceblobs;
        private string containerName;
        public CubosController(ServiceCubos service, ServiceBlobCubos serviceblobs, IConfiguration configuration)
        {
            this.service = service;
            this.serviceblobs = serviceblobs;
            this.containerName =
                 configuration.GetValue<string>("BlobContainers:CubosContainerName");
        }
        public async Task<IActionResult> Index()
        {
            List<Cubo> cubos = await this.service.GetCubosAsync();
            foreach (Cubo cubo in cubos)
            {
                string blobname = cubo.Imagen;
                cubo.Imagen = await this.serviceblobs.GetBlobUriAsync(this.containerName, blobname);
            }
            return View(cubos);
        }
        
        public IActionResult CubosMarca()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CubosMarca(string marca)
        {
            List<Cubo> cubos = await this.service.GetCubosMarcaAsync(marca);
            foreach(Cubo cubo in cubos)
            {
                string blobname = cubo.Imagen;
                cubo.Imagen = await this.serviceblobs.GetBlobUriAsync(this.containerName,blobname);
            }
            return View(cubos);
        }

        [AuthorizeUsuariosCubos]
        public async Task<IActionResult> Perfil()
        {
            string token =
                HttpContext.Session.GetString("token");
            UsuarioCubo usuario =
                await this.service.GetPerfilUsuarioAsync(token);
            return View(usuario);
        }
        
        [AuthorizeUsuariosCubos]
        public IActionResult NewCubo()
        {
            return View();
        }
        [AuthorizeUsuariosCubos]
        [HttpPost]
        public async Task<IActionResult> NewCubo(Cubo cubo, IFormFile file)
        {
            string token =
                HttpContext.Session.GetString("token");
            string blobName = file.FileName;
            cubo.Imagen = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceblobs.UploadBlobAsync(this.containerName, blobName, stream);
            }
            await this.service.NewCuboAsync(cubo, token);
            return View();
        }

        public async Task<IActionResult> PedidosUsuario()
        {
            string token =
               HttpContext.Session.GetString("token");
            List<CuboCompra> cubos = await this.service.GetPedidosAsync(token);
            foreach (CuboCompra cubo in cubos)
            {
                string blobname = cubo.Imagen;
                cubo.Imagen = await this.serviceblobs.GetBlobUriAsync(this.containerName, blobname);
            }
            return View(cubos);
        }

        //public IActionResult RealizarPedidos()
        //{
        //    return View();
        //}

        
    }
}
