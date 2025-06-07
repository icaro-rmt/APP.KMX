using APP.KMX.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Xml;

namespace APP.KMX.Utils
{
    public static class FileConversion 
    {
        /// <summary>
        /// Validating if is Northern or Southern Hemisphere
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns>If is Southern: returns true</returns>
        public static async Task<string>ConvertXlsxToKML(string xlsxFilePath)
        {
            try
            {
                
                var data = ConversortUtm.ReadXlsx(xlsxFilePath);
              
                string kmlFilePath = Path.ChangeExtension(xlsxFilePath, ".kml");
                var coordenadas = ConversortUtm.TransformDataIntoLatitudeLongitude(data);

                if (data != null)
                {
                    string kmlContent = await GenerateKml(coordenadas);
                    File.WriteAllText(kmlFilePath, kmlContent);
                }
                return kmlFilePath;
            }
            catch (Exception)
            {

                throw new ApplicationException("An error ocurred during the conversion from XLSX to KML");
            }
        }

        public static string ConvertKMLToXlsx(string kmlFilePath)
        {
            try
            {
                var data = ConversorLatitudeLongitude.ReadKML(kmlFilePath);
                string xlsxFilePath = Path.ChangeExtension(kmlFilePath, ".xlsx");

                var coordenadas = ConversorLatitudeLongitude.LatLongToUTM(data);

                if (data != null)
                {
                    GenerateExcel(coordenadas, xlsxFilePath);
                }
                return xlsxFilePath;
            }
            catch (Exception)
            {

                throw new ApplicationException("An error ocurred during the conversion from KML to XLSX"); 
            }
        }

        private static void GenerateExcel(IEnumerable<UtmCoordinates> coordinatesData, string savePath )
        {
            using var workBook = new XLWorkbook();
            
            var worksheet = workBook.Worksheets.Add("Pagina1");
            int rowNumber = 1;

            //Add data to Cells
            worksheet.Cell(rowNumber, 1).Value = "Elemento";
            worksheet.Cell(rowNumber, 2).Value = "Fuso";
            worksheet.Cell(rowNumber, 3).Value = "Latitude";
            worksheet.Cell(rowNumber, 4).Value = "Longitude";

            foreach (UtmCoordinates ponto in coordinatesData)
            {
                rowNumber++;
                worksheet.Cell(rowNumber, 1).Value = ponto.PointName;
                worksheet.Cell(rowNumber, 2).Value = ponto.Zone.ToString() + ' ' + ponto.Letter.ToString();
                worksheet.Cell(rowNumber, 3).Value = ponto.Easting.ToString();
                worksheet.Cell(rowNumber, 4).Value = ponto.Northing.ToString();
            }

            workBook.SaveAs(savePath);

        }

        private static async Task<string> GenerateKml(IEnumerable<CoordinateOutput> data)
        {
            // Create XML document for KML
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            // Create KML root element
            XmlElement kmlElement = xmlDoc.CreateElement("kml", "http://www.opengis.net/kml/2.2");
            xmlDoc.AppendChild(kmlElement);

            // Create Document element
            XmlElement documentElement = xmlDoc.CreateElement("Document");
            kmlElement.AppendChild(documentElement);

            // Iterate through data and create Placemark elements
            foreach (var row in data)
            {
                // Create Placemark element
                XmlElement placemarkElement = xmlDoc.CreateElement("Placemark");
                documentElement.AppendChild(placemarkElement);

                // Create name element
                XmlElement nameElement = xmlDoc.CreateElement("name");
                nameElement.InnerText = row.Point; // Assuming the name is the first column
                placemarkElement.AppendChild(nameElement);

                // Create Point element
                XmlElement pointElement = xmlDoc.CreateElement("Point");
                placemarkElement.AppendChild(pointElement);

                // Create coordinates element
                var latLong = CoordinateFormatter(row.Coordinates);
                XmlElement coordinatesElement = xmlDoc.CreateElement("coordinates");
                coordinatesElement.InnerText = $"{latLong[1]},{latLong[0]}";
                pointElement.AppendChild(coordinatesElement);
            }

            return xmlDoc.OuterXml;
        }
        private static string[] CoordinateFormatter(double[] coordinates)
        {
            return new string[] 
            {
                coordinates[0].ToString().Replace(',', '.'),
                coordinates[1].ToString().Replace(',', '.'),
            };
        }
    }
}
