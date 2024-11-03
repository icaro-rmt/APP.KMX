using APP.KMX.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace APP.KMX.App.Controllers
{
    public class PlanosController : Controller
    {
        private readonly PlanModel _freeplan;
        public PlanosController()
        {
            PlanModel freePlan = new PlanModel()
            {
                NomePlano = "Plano Gratuito",
                QuantidadeConversoes= "1",
                Formatos= "Formatos permitidos apenas tabelas .xls em mapas .kmz",
                QuantidadeArquivos = "1 arquivo por vez",
                QuantidadeUsuarios ="1",
            };

            _freeplan = freePlan;
        }
        public IActionResult Index()
        {

            return View(_freeplan);
        }
    }
}
