using AquaSense.DAO;
using AquaSense.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AquaSense.Controllers
{
    public class ApartamentoController : PadraoController<ApartamentoViewModel>
    {
        public ApartamentoController()
        {
            DAO = new ApartamentoDAO();
        }

        public IActionResult CarregaApartamento(int idApartamento)
        {
            try
            {
                PreparaListaUsuariosCombo();
                PreparaListaSensoresCombo();
                ApartamentoViewModel model = new ApartamentoViewModel();
                model = DAO.Consulta(idApartamento);

                return View(NomeViewIndex, model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public override IActionResult Save(ApartamentoViewModel model, string Operacao)
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
                        return CarregaApartamento(DAO.Insert(model));
                    else
                    {
                        DAO.Update(model);
                        return CarregaApartamento(model.Id);
                    }
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        private void PreparaListaUsuariosCombo()
        {
            UsuarioDAO usuarioDao = new UsuarioDAO();
            var usuarios = usuarioDao.Listagem();
            List<SelectListItem> listaUsuarios = new List<SelectListItem>();
            listaUsuarios.Add(new SelectListItem("Selecione...", "0"));
            foreach (var usuario in usuarios)
            {
                SelectListItem item = new SelectListItem(usuario.LoginUsuario, usuario.Id.ToString());
                listaUsuarios.Add(item);
            }
            ViewBag.Usuarios = listaUsuarios;
        }

        private void PreparaListaSensoresCombo()
        {
            SensorDAO sensorDao = new SensorDAO();
            var sensores = sensorDao.Listagem();
            List<SelectListItem> listaSensores = new List<SelectListItem>();
            listaSensores.Add(new SelectListItem("Selecione...", "0"));
            foreach (var sensor in sensores)
            {
                SelectListItem item = new SelectListItem(sensor.Descricao, sensor.Id.ToString());
                listaSensores.Add(item);
            }
            ViewBag.Sensores = listaSensores;
        }

        public bool ConsultaApartamentoNome(ApartamentoViewModel model)
        {
            ApartamentoDAO apartamentoDao = new ApartamentoDAO();
            bool nomeJaCadastrado = apartamentoDao.ConsultaApartamentosPorConjuntoHabitacional(model.IdConjuntoHabitacional).Any(x => x.Descricao == model.Descricao);
            return nomeJaCadastrado;
        }
    }
}
