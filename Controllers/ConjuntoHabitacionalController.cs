using AquaSense.DAO;
using AquaSense.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AquaSense.Controllers
{
    public class ConjuntoHabitacionalController : PadraoController<ConjuntoHabitacionalViewModel>
    {
        public ConjuntoHabitacionalController()
        {
            DAO = new ConjuntoHabitacionalDAO();
        }
        public override IActionResult Index()
        {
            try
            {
                ConjuntoHabitacionalViewModelConsulta retorno = new ConjuntoHabitacionalViewModelConsulta();
                UsuarioViewModel usuario = HttpContext.Session.GetObject<UsuarioViewModel>("Usuario");
                ConjuntoHabitacionalDAO conjuntoDao = new ConjuntoHabitacionalDAO();
                UsuarioDAO usuarioDAO = new UsuarioDAO();
                SensorDAO sensorDAO = new SensorDAO();

                retorno.ConjuntoHabitacional = conjuntoDao.ConsultaConjuntoHabitacionalPorUsuario(usuario.Id);

                ApartamentoDAO apartamentoDao = new ApartamentoDAO();

                retorno.Apartamentos = apartamentoDao.ConsultaApartamentosPorConjuntoHabitacional(retorno.ConjuntoHabitacional.Id);

                foreach(var ap in retorno.Apartamentos)
                {
                    if (ap.IdUsuario > 0)
                        ap.LoginUsuario = usuarioDAO.Consulta(ap.IdUsuario).LoginUsuario;
                    if (ap.IdSensor > 0)
                        ap.DescricaoSensor = sensorDAO.Consulta(ap.IdSensor).Descricao;

                }

                retorno.ConjuntoHabitacional.IdUsuarioAdm = usuario.Id;

                return View("Index", retorno);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
    }
}
