using APP.KMX.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace APP.KMX.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var cards = GenerateCards();
            return View(cards);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult FileConversion(ConversionButtonModel model)
        {
            model = new ConversionButtonModel { Title = "Ponto de coordenadas .xls para .kmz", IconClass = "xls-green.svg", Link = "/Home/FileConversion", ConvertFrom = ".xls", ConvertTo = ".kmz" };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        protected private List<ConversionButtonModel> GenerateCards()
        {
            List<ConversionButtonModel> cards = new List<ConversionButtonModel>
            {
                new ConversionButtonModel { Title = "Ponto de coordenadas .xls para .kmz", IconClass = "xls-green.svg", Link = "/Home/FileConversion", ConvertFrom=".xls", ConvertTo = ".kmz" },
                new ConversionButtonModel { Title = "Pontos de coordenadas .kmz ou .kml para .xls", IconClass = "kmz-purple.svg", Link = "/Home/FileConversion", ConvertFrom = ".kmz ", ConvertTo = ".xls" },
                new ConversionButtonModel { Title = "Calcular distância entre vários pontos .xls para .kmz", IconClass = "xls-yellow.svg", Link = "/Home/FileConversion",ConvertFrom=".xls", ConvertTo = ".kmz" },
                new ConversionButtonModel { Title = "Calcular distância entre vários pontos .kmz para .xls", IconClass = "xls-blue.svg", Link = "/Home/FileConversion" },
                new ConversionButtonModel { Title = "Criar rotas em endereço .xls para .kmz", IconClass = "xls-purple.svg", Link = "/Home/FileConversion", ConvertFrom=".xls", ConvertTo = ".kmz" },
                new ConversionButtonModel { Title = "Criar rotas em endereço .kmz para .kml", IconClass = "kml-purple.svg", Link = "/Home/FileConversion", ConvertFrom=".xls", ConvertTo = ".kml" },

            };

            return cards;
               

        }
    }
}
