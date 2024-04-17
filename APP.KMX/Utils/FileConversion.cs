using APP.KMX.Models;
using ClosedXML.Excel;
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

        public static string GerarKml(List<CoordinateOutput> data)
        {
            int item = 0;
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
                XmlElement coordinatesElement = xmlDoc.CreateElement("coordinates");
                coordinatesElement.InnerText = $"{row.Coordinates[1].ToString().Replace(',','.')},{row.Coordinates[0].ToString().Replace(',', '.')}";
                pointElement.AppendChild(coordinatesElement);
            }

            return xmlDoc.OuterXml;
        }
    }
}
