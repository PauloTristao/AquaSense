using AquaSense.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AquaSense.DAO
{
    public class SensorDAO : PadraoDAO<SensorViewModel>
    {
        protected override SqlParameter[] CriaParametros(SensorViewModel model)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("id", model.Id),
                new SqlParameter("descricao", model.Descricao),
                new SqlParameter("codigo_fiware", model.CodigoFiware),
            };

            return parameters;
        }

        protected override SensorViewModel MontaModel(DataRow registro)
        {
            SensorViewModel SensorViewModel = new SensorViewModel();
            SensorViewModel.Id = Convert.ToInt32(registro["id_sensor"]);
            SensorViewModel.Descricao = registro["descricao"].ToString();
            SensorViewModel.CodigoFiware = registro["codigo_fiware"].ToString();
            return SensorViewModel;
        }

        protected override void SetTabela()
        {
            Tabela = "Sensor";
            ChaveIdentity = true;
        }
    }
}
