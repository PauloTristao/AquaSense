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

                retorno.ConjuntoHabitacional = conjuntoDao.ConsultaConjuntoHabitacionalPorUsuario(usuario.Id);

                ApartamentoDAO apartamentoDao = new ApartamentoDAO();

                retorno.Apartamentos = apartamentoDao.ConsultaApartamentosPorConjuntoHabitacional(retorno.ConjuntoHabitacional.Id);

                return View("Index", retorno);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

       
    }
}
