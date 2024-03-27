using ClosedXML.Excel;
using System.Xml;

namespace APP.KMX.Utils
{
    public class FileConversion
    {
        //TODO: Create a call for the ConvertXlsxToKML method, passing the filePath;
        public static void ConvertXlsxToKML(string xlsxFilePath)
        {
            // Load XLSX data and process it
            List<List<string>> data = ReadXlsx(xlsxFilePath);

            if (data != null)
            {
                // Generate KML content
                string kmlContent = GenerateKML(data);

                // Write KML content to file
                string kmlFilePath = Path.ChangeExtension(xlsxFilePath, ".kml");
                File.WriteAllText(kmlFilePath, kmlContent);
            }
        }

        static List<List<string>> ReadXlsx(string filePath)
        {
            List<List<string>> data = new List<List<string>>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // Assuming first worksheet

                // Iterate over the rows
                foreach (var row in worksheet.RowsUsed())
                {
                    List<string> rowData = new List<string>();

                    // Iterate over the cells in the row
                    foreach (var cell in row.Cells())
                    {
                        rowData.Add(cell.Value.ToString());
                        Console.WriteLine(cell.Value.ToString());
                    }

                    data.Add(rowData);
                }
            }

            return data;
        }

        static string GenerateKML(List<List<string>> data)
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
                // Assuming the first row contains headers
                if (row == data[0])
                    continue;

                // Create Placemark element
                XmlElement placemarkElement = xmlDoc.CreateElement("Placemark");
                documentElement.AppendChild(placemarkElement);

                // Create name element
                XmlElement nameElement = xmlDoc.CreateElement("name");
                nameElement.InnerText = row[0]; // Assuming the name is the first column
                placemarkElement.AppendChild(nameElement);

                // Create Point element
                XmlElement pointElement = xmlDoc.CreateElement("Point");
                placemarkElement.AppendChild(pointElement);

                // Create coordinates element
                XmlElement coordinatesElement = xmlDoc.CreateElement("coordinates");
                coordinatesElement.InnerText = $"{row[2].Replace(',', '.')},{row[1].Replace(',', '.')},0"; // Assuming Latitude and Longitude are the second and third columns respectively
                pointElement.AppendChild(coordinatesElement);
            }

            return xmlDoc.OuterXml;
        }
    }
}
