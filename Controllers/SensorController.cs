using AquaSense.DAO;
using AquaSense.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AquaSense.Controllers
{
    public class SensorController : PadraoController<SensorViewModel>
    {
        public SensorController()
        {
            DAO = new SensorDAO();
        }

        public IActionResult ConsultaSensoresDisponiveis()
        {
            SensorDAO sensorDAO = new SensorDAO();  
            var lista = sensorDAO.ListaSensoresDisponiveis();

            return Json(lista.Select(sensor => new { Id = sensor.Id, Descricao = sensor.Descricao }));
        }

        public IActionResult Consulta(int idSensor)
        {
            SensorDAO sensorDAO = new SensorDAO();
            var sensor = sensorDAO.Consulta(idSensor);

            return Json( new { Id = sensor.Id, Descricao = sensor.Descricao });
        }

    }
}
