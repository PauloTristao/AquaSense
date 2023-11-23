using AquaSense.DAO;
using AquaSense.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AquaSense.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                LoginViewModel model = new LoginViewModel();
                return View(model);
            }
            catch (Exception erro)
            {
                return View("ErrorLogin", new ErrorViewModel(erro.ToString()));
            }
}
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                ValidaDados(model);
                if (ModelState.IsValid == false)
                {
                    return View("Index", model);
                }
                else
                {
                    UsuarioDAO usuarioDAO = new UsuarioDAO();
                    UsuarioViewModel user = usuarioDAO.ConsultaAcesso(model.Login, model.Senha);

                    if (user != null)
                    {
                        var json = JsonConvert.SerializeObject(user);
                        HttpContext.Session.SetString("Usuario", json);
                        HttpContext.Session.SetString("UsuarioName", json);
                        HttpContext.Session.SetString("UsuarioFoto", json);
                        HttpContext.Session.SetString("Logado", "true");
                        return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Senha", "Usuário ou senha inválidos!");
                        return View("Index", model);
                    }
                }
            }catch(Exception erro)
            {
                return View("ErrorLogin", new ErrorViewModel(erro.ToString()));
            }
        }
        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        protected void ValidaDados(LoginViewModel model)
        {
            ModelState.Clear();
            if (model.Login == null)
                ModelState.AddModelError("Login", "Preencha esse campo");
            if (model.Senha == null)
                ModelState.AddModelError("Senha", "Preencha esse campo");

        }

        public virtual IActionResult Create()
        {
            try
            {
                ViewBag.Operacao = "I";
                //UsuarioAdmViewModel model = Activator.CreateInstance<UsuarioAdmViewModel>();
                UsuarioAdmViewModel model = new UsuarioAdmViewModel();
                return View("Form", model);
            }
            catch (Exception erro)
            {
                return View("ErrorLogin", new ErrorViewModel(erro.ToString()));
            }
        }

        public virtual IActionResult Save(UsuarioAdmViewModel model, string Operacao)
        {
            try
            {
                UsuarioDAO usuarioDao = new UsuarioDAO();
                ValidaDados(model, Operacao);
                if (ModelState.IsValid == false)
                {
                    return View("Form", model);
                }
                else
                {
                    usuarioDao.InsertUsuarioAdm(model);

                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception erro)
            {
                return View("ErrorLogin", new ErrorViewModel(erro.ToString()));
            }
        }

        public byte[] ConvertImageToByte(IFormFile file)
        {
            if (file != null)
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            else
                return null;
        }

        protected void ValidaDados(UsuarioAdmViewModel model, string operacao)
        {
            if (string.IsNullOrEmpty(model.Usuario.NomePessoa))
                ModelState.AddModelError("Usuario.NomePessoa", "Preencha o nome.");
            if (string.IsNullOrEmpty(model.Usuario.Senha))
                ModelState.AddModelError("Usuario.Senha", "Preencha a senha.");
            if (model != null && !string.IsNullOrEmpty(model.Usuario.Senha) && model.Usuario.Senha.Length < 4)
                ModelState.AddModelError("Usuario.Senha", "A senha tem que conter pelo menos 4 caracteres");
            if (model.Usuario.Imagem == null && operacao == "I")
                ModelState.AddModelError("Usuario.Imagem", "Escolha uma imagem.");
            if (model.Usuario.Imagem != null && model.Usuario.Imagem.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Usuario.Imagem", "Imagem limitada a 2 mb.");

            if (string.IsNullOrEmpty(model.ConjuntoHabitacional.Nome))
                ModelState.AddModelError("ConjuntoHabitacional.Nome", "Preencha o nome.");
            if (string.IsNullOrEmpty(model.ConjuntoHabitacional.Endereco))
                ModelState.AddModelError("ConjuntoHabitacional.Endereco", "Preencha o endereço.");
            if (string.IsNullOrEmpty(model.ConjuntoHabitacional.Cnpj))
                ModelState.AddModelError("ConjuntoHabitacional.Cnpj", "Preencha o CNPJ.");


            model.Usuario.ImagemEmByte = ConvertImageToByte(model.Usuario.Imagem);
        }
    }
}
