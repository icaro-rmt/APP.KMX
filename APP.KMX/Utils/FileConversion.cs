using APP.KMX.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Xml;

namespace APP.KMX.Utils
{
    public class FileConversion 
    {
        /// <summary>
        /// Validating if is Northern or Southern Hemisphere
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns>If is Southern: returns true</returns>
        public static string ConvertXlsxToKML(string xlsxFilePath)
        {
            var conversor = new ConversortUtm();
            // Load XLSX data and process it
            var data = conversor.ReadXlsx(xlsxFilePath);
            string kmlFilePath = Path.ChangeExtension(xlsxFilePath, ".kml");
            var coordenadas = conversor.TransformDataIntoLatitudeLongitude(data);

            if (data != null)
            {
                // Generate KML content
                string kmlContent = GerarKml(coordenadas);

                // Write KML content to file
                File.WriteAllText(kmlFilePath, kmlContent);
            }
            return kmlFilePath;
        }

        public static string ConvertKMLToXlsx(string kmlFilePath)
        {
            var conversor = new ConversorLatitudeLongitude();
            // Load XLSX data and process it
            var data = conversor.ReadKML(kmlFilePath);
            string xlsxFilePath = Path.ChangeExtension(kmlFilePath, ".xlsx");

            var coordenadas = conversor.LatLongToUTM(data);



            if (data != null)
            {
                // Generate KML content
                GerarExcel(coordenadas, xlsxFilePath);

                //// Write KML content to file 
                //File.WriteAllText(kmlFilePath, kmlContent);
            }
            return xlsxFilePath;
        }

        public static string GerarExcel(List<UtmCoordinates> coordinatesData, string savePath )
        {
            var workBook = new XLWorkbook();

            var worksheet = workBook.Worksheets.Add("Pagina1");
            int rowNumber = 1;
            worksheet.Cell(rowNumber, 1).Value = "Elemento";
            worksheet.Cell(rowNumber, 2).Value = "Fuso";
            worksheet.Cell(rowNumber, 3).Value = "Latitude";
            worksheet.Cell(rowNumber, 4).Value = "Longitude";

            foreach(UtmCoordinates ponto in coordinatesData)
            {
                rowNumber++;
                worksheet.Cell(rowNumber, 1).Value = ponto.PointName;
                worksheet.Cell(rowNumber, 2).Value = ponto.Zone.ToString() +  ' '+ ponto.Letter.ToString();
                worksheet.Cell(rowNumber, 3).Value = ponto.Easting.ToString();
                worksheet.Cell(rowNumber, 4).Value = ponto.Northing.ToString();
            }

            workBook.SaveAs(savePath);

            return savePath;

        }

        public static string GerarKml(List<CoordinateOutput> data)
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
            return new string[] {
            coordinates[0].ToString().Replace(',', '.'),
            coordinates[1].ToString().Replace(',', '.'),
            };
        }
    }
}
