using AquaSense.DAO;
using AquaSense.Models;

namespace AquaSense.Controllers
{
    public class SensorController : PadraoController<SensorViewModel>
    {
        public SensorController()
        {
            DAO = new SensorDAO();
        }
    }
}
