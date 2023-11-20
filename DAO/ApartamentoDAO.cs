using AquaSense.Models;
using System;
using System.Collections.Generic;
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
                new SqlParameter("id", model.Id),
                new SqlParameter("descricao", model.Descricao),
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
            apartamento.Descricao = registro["descricao"].ToString();
            apartamento.IdConjuntoHabitacional = Convert.ToInt32(registro["id_conjunto_habitacional"]);
            apartamento.IdSensor = Convert.ToInt32(registro["id_sensor"]);
            apartamento.IdUsuario = Convert.ToInt32(registro["id_usuario"]);

            return apartamento;
        }
        public List<ApartamentoViewModel> ConsultaApartamentosPorConjuntoHabitacional(int conjuntoHabitacional)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id_conjunto_habitacional", conjuntoHabitacional),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_ApartamentoPorConjuntoHabitacional", p);
            List<ApartamentoViewModel> lista = new List<ApartamentoViewModel>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;
        }

        protected override void SetTabela()
        {
            Tabela = "Apartamento";
            ChaveIdentity = true;
        }
    }
}
