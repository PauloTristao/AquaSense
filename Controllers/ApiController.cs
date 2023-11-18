using Conexao_MongoDB;
using Microsoft.AspNetCore.Mvc;

namespace AquaSense.Controllers
{
    public class ApiController : Controller
    {
        public IActionResult RequestHistory()
        {
            connection_API conexaoApi = new connection_API();
            var teste = conexaoApi.RequestHistory("Flux:010", 10);

            return Json(teste);
        }
    }
}
