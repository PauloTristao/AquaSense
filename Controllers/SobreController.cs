using AquaSense.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AquaSense.Controllers
{
    public class SobreController : Controller
    {
        // Ação para a página inicial
        public ActionResult Index()
        {

            return View("Index");
        }

        // Ação para a página de contato
        public ActionResult Contact()
        {
            return View();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                context.Result = RedirectToAction("Index", "Login");
            else
            {
                UsuarioViewModel usuario = HttpContext.Session.GetObject<UsuarioViewModel>("Usuario");
                ViewBag.LoginUsuario = usuario.LoginUsuario.ToUpper();
                ViewBag.Adm = usuario.Adm;
                ViewBag.NomePessoa = usuario.NomePessoa.ToUpper();
                ViewBag.ImagemBase64 = usuario.ImagemEmBase64;
                base.OnActionExecuting(context);
            }
        }

    }
}

