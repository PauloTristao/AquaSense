namespace AquaSense.Models
{
    public class ApartamentoViewModel : PadraoViewModel
    {
        public string NumeroApartamento { get; set; }

        public int IdConjuntoHabitacional { get; set; }

        public int IdSensor { get; set; }

        public int IdUsuario { get; set; }
    }
}
//SELECT TOP(1000) [id_apartamento] int
//      ,[numero_apartamento] varchar
//      ,[id_conjunto_habitacional] int FK
//      ,[id_sensor] int 
//      ,[id_usuario] int FK
//FROM[AquaSense].[dbo].[Apartamento]