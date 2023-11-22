using Microsoft.AspNetCore.Http;
using System;

namespace AquaSense.Models
{
    public class UsuarioAdmViewModel : PadraoViewModel
    {
        public UsuarioAdmViewModel()
        {
            Usuario = new UsuarioViewModel();
            ConjuntoHabitacional = new ConjuntoHabitacionalViewModel();
        }

        public UsuarioViewModel Usuario { get; set; }

        public ConjuntoHabitacionalViewModel ConjuntoHabitacional { get; set; }
    }
}
