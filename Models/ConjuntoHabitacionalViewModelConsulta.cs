using System.Collections.Generic;

namespace AquaSense.Models
{
    public class ConjuntoHabitacionalViewModelConsulta
    {
        public ConjuntoHabitacionalViewModel ConjuntoHabitacional { get; set; }

        public List<ApartamentoViewModel> Apartamentos { get; set; }
    }
}
