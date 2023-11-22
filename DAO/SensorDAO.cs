using AquaSense.Models;
using System;
using System.Collections.Generic;
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

        public List<SensorViewModel> ListaSensoresDisponiveis()
        {
            var p = new SqlParameter[]{};
            
            DataTable tabela = HelperDAO.ExecutaProcSelect("spConsulta_SensoresDisponiveis", p);
            List<SensorViewModel> retorno = new List<SensorViewModel>();

            foreach (DataRow registro in tabela.Rows)
            {
                retorno.Add(MontaModel(registro));
            }

            return retorno;

        }

        protected override void SetTabela()
        {
            Tabela = "Sensor";
            ChaveIdentity = true;
        }
    }
}
