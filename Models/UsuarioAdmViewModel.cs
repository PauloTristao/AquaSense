using Microsoft.AspNetCore.Http;
using System;

namespace AquaSense.Models
{
    public class UsuarioAdmViewModel : PadraoViewModel
    {
        public UsuarioAdmViewModel() 
        {
            Usuario = new UsuarioViewModel();
            ConjuntoHabitacional = new SensorViewModel();
        }

        public UsuarioViewModel Usuario { get; set; }

        public SensorViewModel ConjuntoHabitacional { get; set; }
    }
}
