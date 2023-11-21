using AquaSense.DAO;
using AquaSense.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;

namespace AquaSense.Controllers
{
    public class UsuarioController : PadraoController<UsuarioViewModel>
    {
        public UsuarioController()
        {
            DAO = new UsuarioDAO();
        }

        public IActionResult ExibeUsuario()
        {
            try
            {
                UsuarioViewModel usuario = HttpContext.Session.GetObject<UsuarioViewModel>("Usuario");
                PreparaComboAdm();
                ViewBag.AdmCombo.Insert(0, new SelectListItem("TODAS", "0"));
                return View("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.Message));
            }
        }

        private void PreparaComboAdm()
        {
            List<SelectListItem> listaRetorno = new List<SelectListItem>();
            listaRetorno.Add(new SelectListItem("True", "1"));
            listaRetorno.Add(new SelectListItem("False", "2"));

            ViewBag.AdmCombo = listaRetorno;
        }

        public override IActionResult Save(UsuarioViewModel model, string Operacao)
        {
            try
            {
                ValidaDados(model, Operacao);

                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    PreencheDadosParaView(Operacao, model);
                    return View(NomeViewForm, model);
                }
                else
                {
                    if (Operacao == "I")
                    {
                        DAO.Insert(model);
                    }
                    else
                    {
                        DAO.Update(model);
                    }

                    return RedirectToAction(NomeViewIndex);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
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

        protected override void ValidaDados(UsuarioViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);
            if (string.IsNullOrEmpty(model.NomePessoa))
                ModelState.AddModelError("NomePessoa", "Preencha o nome.");
            if (string.IsNullOrEmpty(model.Senha))
                ModelState.AddModelError("senha", "Preencha a senha.");
            if (model != null && !string.IsNullOrEmpty(model.Senha) && model.Senha.Length < 4)
                ModelState.AddModelError("senha", "A senha tem que conter pelo menos 4 caracteres");
            if (model.Imagem == null && operacao == "I")
                ModelState.AddModelError("Imagem", "Escolha uma imagem.");
            if (model.Imagem != null && model.Imagem.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Imagem", "Imagem limitada a 2 mb.");
            if (ModelState.IsValid)
            {
                //na alteração, se não foi informada a imagem, iremos manter a que já estava salva.
                if (operacao == "A" && model.Imagem == null)
                {
                    UsuarioViewModel cid = DAO.Consulta(model.Id);
                    model.ImagemEmByte = cid.ImagemEmByte;
                }
                else
                {
                    model.ImagemEmByte = ConvertImageToByte(model.Imagem);
                }

            }

        }
        public override IActionResult Edit(int id)
        {
            try
            {
                ViewBag.Operacao = "A";
                var model = DAO.Consulta(id);
                var portifolioDAO = new ConjuntoHabitacionalDAO();
                if (model == null)
                    return RedirectToAction(NomeViewIndex);
                else
                {
                    PreencheDadosParaView("A", model);
                    return View(NomeViewForm, model);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public IActionResult ConsultaUsuariosCombo()
        {
            UsuarioDAO usuarioDao = new UsuarioDAO();
            var lista = usuarioDao.Listagem();

            return Json(lista.Select(usuario => new { Id = usuario.Id, Descricao = usuario.LoginUsuario }));
        }

        public IActionResult ObtemDadosConsultaAvancada(string login, string nomePessoa, int adm)
        {
            try
            {
                if (adm == 2)
                    adm = 0;
                else if (adm == 0)
                    adm = 2;
                if (login == null)
                    login = "";
                if (nomePessoa == null)
                    nomePessoa = "";

                UsuarioDAO dao = new UsuarioDAO();
                var lista = dao.ConsultaAvancadaUsuario(login, nomePessoa, adm);
                return PartialView("pvGridUsuario", lista);
            }
            catch (Exception erro)
            {
                return Json(new { erro = true, msg = erro.Message });
            }
        }
    }

}
