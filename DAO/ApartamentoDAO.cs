using AquaSense.Models;
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

            };
            return null;
        }

        protected override ApartamentoViewModel MontaModel(DataRow registro)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetTabela()
        {
            throw new System.NotImplementedException();
        }
    }
}
