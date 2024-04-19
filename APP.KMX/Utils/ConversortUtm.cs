using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using ClosedXML.Excel;
using APP.KMX.Models;
using CoordinateSharp;

namespace APP.KMX.Utils
{
    public class ConversortUtm
    {
        private static readonly List<string> SouthKeys = new List<string> { "C", "D", "E", "F", "G", "H", "J", "K", "L", "M" };

        public double[] UTMToLatLon(double easting, double northing, int zoneNumber, string letraZona)
        {
            UniversalTransverseMercator utm = new UniversalTransverseMercator(letraZona.ToUpper(), zoneNumber, easting, northing);

            Coordinate c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
            c.FormatOptions.Format = CoordinateFormatType.Decimal;
            c.FormatOptions.Round = 7;
            double latitude = c.Latitude.ToDouble();
            double longitude = c.Longitude.ToDouble();

            double[] coordenadas = new double[] { latitude, longitude };
            return coordenadas;
        }
        
        public List<CoordinateOutput> TransformDataIntoLatitudeLongitude (List<UTMFormat> data)
        {
            List<CoordinateOutput> listaCoordenadas = new List<CoordinateOutput>();
            foreach(UTMFormat info in data)
            {
                CoordinateOutput coordenadas = new CoordinateOutput();
                coordenadas.Coordinates = UTMToLatLon(info.Easting, info.Northing, info.ZoneNumber, info.ZoneLetter);
                coordenadas.Point = info.PointName;

                listaCoordenadas.Add(coordenadas);
            }
            return listaCoordenadas;            

        }

        public List<UTMFormat> ReadXlsx(string filePath)
        {
            List<UTMFormat> data = new List<UTMFormat>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);

                // Iterate over the rows
                foreach (var row in worksheet.RowsUsed())
                {
                    if (row.FirstCell().Value.ToString() == "Elemento")
                    {
                        continue;
                    }
                    
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

            try
            {
                var valorZona = linha[1][linha[1].Length - 1].ToString();

                data.PointName = linha[0].ToString();
                data.Northing = Convert.ToDouble(linha[3].Split(' ')[0].Replace('.', ','));
                data.Easting = Convert.ToDouble(linha[2].Split(' ')[0].Replace('.', ','));
                data.ZoneNumber = Convert.ToInt32(linha[1].Split(' ')[0]);
                data.ZoneLetter = valorZona;
                data.IsNorth = ValidateNorthHemisphere(data.ZoneLetter);

                return data;
            }
            catch (Exception)
            {

                return data;
            }
            
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

