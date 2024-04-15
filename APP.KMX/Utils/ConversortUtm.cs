using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using ClosedXML.Excel;
using APP.KMX.Models;
using DocumentFormat.OpenXml.InkML;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using CoordinateSharp;

namespace APP.KMX.Utils
{
    public class ConversortUtm
    {
        private readonly CoordinateTransformationFactory _transformFactory;
        private static readonly List<string> SouthKeys = new List<string> { "C", "D", "E", "F", "G", "H", "J", "K", "L", "M" };

        public ConversortUtm()
        {
            _transformFactory = new CoordinateTransformationFactory();
        }

        public double[] UTMToLatLon(double easting, double northing, int zoneNumber, string letraZona)
        {
            UniversalTransverseMercator utm = new UniversalTransverseMercator(letraZona.ToUpper(), zoneNumber, easting, northing);

            Coordinate c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
            c.FormatOptions.Format = CoordinateFormatType.Decimal;
            c.FormatOptions.Round = 7;
            double latitude = c.Latitude.ToDouble();
            double longitude = c.Longitude.ToDouble();

            double[] testes = new double[] { latitude, longitude };
            return testes;
        }


        public List<double[]> TransformDataIntoLatitudeLongitude (List<UTMFormat> data)
        {
            List<double[]> coordenadas = new List<double[]>();
            foreach(UTMFormat info in data)
            {
                Console.WriteLine(info.ToString());
                coordenadas.Add(UTMToLatLon(info.Easting, info.Northing, info.ZoneNumber,info.ZoneLetter));
            }
            return coordenadas;            

        }

        public List<UTMFormat> ReadXlsx(string filePath)
        {
            List<UTMFormat> data = new List<UTMFormat>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // Assuming first worksheet

                // Iterate over the rows
                foreach (var row in worksheet.RowsUsed())
                {
                    UTMFormat line = GetDataFromRows(row);
                    data.Add(line);
                }
            }

            return data;
        }

        private UTMFormat GetDataFromRows(IXLRow row)
        {
            var data = new UTMFormat();
            var linha = new List<string>();
            foreach (var cell in row.Cells())
            {
                linha.Add(cell.Value.ToString());
            }
            
            data.Northing = Convert.ToDouble(linha[2].Split(' ')[0].Replace('.',','));
            data.Easting = Convert.ToDouble(linha[1].Split(' ')[0].Replace('.', ','));
            data.ZoneNumber = Convert.ToInt32(linha[0].Split(' ')[0]);
            data.ZoneLetter = (linha[0].Split(' ')[1]);
            data.IsNorth = ValidateNorthHemisphere(data.ZoneLetter);

            return data;
        }

        private bool ValidateNorthHemisphere(string parametro)
        {
            if (SouthKeys.Contains(parametro.ToUpper()))
            {
                return false;
            }
            return true;
        }
    }

}

