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
                new SqlParameter("decricao", model.descricao),
                new SqlParameter("id_apartamento", model.Id_Apartamento),

            };

            return parameters;
        }

        protected override SensorViewModel MontaModel(DataRow registro)
        {
            SensorViewModel SensorViewModel = new SensorViewModel();
            SensorViewModel.Id = Convert.ToInt32(registro["id_sensor"]);
            SensorViewModel.descricao = registro["descricao"].ToString();
            SensorViewModel.Id_Apartamento = Convert.ToInt32(registro["id_apartamento"]);
            return SensorViewModel;
        }

        protected override void SetTabela()
        {
            Tabela = "Sensor";
            ChaveIdentity = true;
        }
    }
}
