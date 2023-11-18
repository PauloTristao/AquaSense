using AquaSense.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AquaSense.DAO
{
    public class ApartamentoDAO : PadraoDAO<ApartamentoViewModel>
    {
        protected override SqlParameter[] CriaParametros(ApartamentoViewModel model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("id_apartamento", model.Id),
                new SqlParameter("numero_apartamento", model.NumeroApartamento),
                new SqlParameter("id_conjunto_habitacional", model.IdConjuntoHabitacional),
                new SqlParameter("id_sensor", model.IdSensor),
                new SqlParameter("id_usuario", model.IdUsuario)
            };
            return parameters;
        }

        protected override ApartamentoViewModel MontaModel(DataRow registro)
        {
            ApartamentoViewModel apartamento = new ApartamentoViewModel();
            apartamento.Id = Convert.ToInt32(registro["id_apartamento"]);
            apartamento.NumeroApartamento = registro["numero_apartamento"].ToString();
            apartamento.IdConjuntoHabitacional = Convert.ToInt32(registro["id_conjunto_habitacional"]);
            apartamento.IdSensor = Convert.ToInt32(registro["id_sensor"]);
            apartamento.IdUsuario = Convert.ToInt32(registro["id_usuario"]);

            return apartamento;
        }

        protected override void SetTabela()
        {
            Tabela = "Apartamento";
        }
    }
}
