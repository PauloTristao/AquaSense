using AquaSense.DAO;
using AquaSense.Models;
using Conexao_MongoDB;
using Microsoft.AspNetCore.Mvc;

namespace AquaSense.Controllers
{
    public class ApiController : Controller
    {
        public IActionResult RequestHistoryApartamento(int idSensor)
        {
            SensorDAO sensorDAO = new SensorDAO();
            SensorViewModel sensor = sensorDAO.Consulta(idSensor);

            connection_API conexaoApi = new connection_API();
            var dados = conexaoApi.RequestHistory("Flux:" + sensor.CodigoFiware, 50);

            return Json(dados);
        }

        public IActionResult RequestHistory()
        {
            connection_API conexaoApi = new connection_API();
            var dados = conexaoApi.RequestHistory("Flux:010", 50);

            return Json(dados);
        }
    }
}
