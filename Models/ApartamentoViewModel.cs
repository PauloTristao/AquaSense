namespace AquaSense.Models
{
    public class ApartamentoViewModel : PadraoViewModel
    {
        public string Descricao { get; set; }

        public int IdConjuntoHabitacional { get; set; }

        public int IdSensor { get; set; }

        public int IdUsuario { get; set; }
    }
}
