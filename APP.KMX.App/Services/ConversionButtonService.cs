using APP.KMX.App.Models;
using APP.KMX.App.Services.Interfaces;

namespace APP.KMX.App.Services
{
    public class ConversionButtonService : IConversionButtonService
    {
        public List<ConversionButtonModel> GetAvailableConversions()
        {
            return new List<ConversionButtonModel>
            {
                new ConversionButtonModel 
                { 
                    Title = "Ponto de coordenadas .xls para .kmz", 
                    IconClass = "xls-green.svg", 
                    Link = "/Home/FileConversion", 
                    ConvertFrom = ".xls", 
                    ConvertTo = ".kmz" 
                },
                new ConversionButtonModel 
                { 
                    Title = "Pontos de coordenadas .kmz ou .kml para .xls", 
                    IconClass = "kmz-purple.svg", 
                    Link = "/Home/FileConversion", 
                    ConvertFrom = ".kmz", 
                    ConvertTo = ".xls" 
                },
                new ConversionButtonModel 
                { 
                    Title = "Calcular distância entre vários pontos .xls para .kmz", 
                    IconClass = "xls-yellow.svg", 
                    Link = "/Home/FileConversion", 
                    ConvertFrom = ".xls", 
                    ConvertTo = ".kmz" 
                },
                new ConversionButtonModel 
                { 
                    Title = "Calcular distância entre vários pontos .kmz para .xls", 
                    IconClass = "xls-blue.svg", 
                    Link = "/Home/FileConversion",
                    ConvertFrom = ".kmz",
                    ConvertTo = ".xls" 
                },
                new ConversionButtonModel 
                { 
                    Title = "Criar rotas em endereço .xls para .kmz", 
                    IconClass = "xls-purple.svg", 
                    Link = "/Home/FileConversion", 
                    ConvertFrom = ".xls", 
                    ConvertTo = ".kmz" 
                },
                new ConversionButtonModel 
                { 
                    Title = "Criar rotas em endereço .kmz para .kml", 
                    IconClass = "kml-purple.svg", 
                    Link = "/Home/FileConversion", 
                    ConvertFrom = ".kmz", 
                    ConvertTo = ".kml" 
                }
            };
        }
    }
}
