using APP.KMX.Models;
using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems;
using System.Xml;

namespace APP.KMX.Utils
{
    public static class ConversorLatitudeLongitude
    { 
        public static IEnumerable<CoordinateOutput> ReadKML(string filePath)
        {
            List<CoordinateOutput> coordenadas = new List<CoordinateOutput>();
            using (FileStream streamedFile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(streamedFile);

                XmlElement root = xmlDocument.DocumentElement;

                // Get a list of all Placemark nodes
                XmlNodeList placemarks = root.SelectNodes("//Placemark");

                foreach (XmlNode placemark in placemarks)
                {
                    // Get the name value
                    string name = placemark.SelectSingleNode("name").InnerText;

                    // Get the coordinates value
                    string coordinates = placemark.SelectSingleNode("Point/coordinates").InnerText;

                    double[] points = GetCoordinatesFromString(coordinates);

                    coordenadas.Add(new CoordinateOutput
                    {
                        Coordinates = points,
                        Point = name,
                    });
                }
            }

            return coordenadas;

        }

        public static IEnumerable<UtmCoordinates> LatLongToUTM(IEnumerable<CoordinateOutput> coordinates)
        {
            try
            {
                List<UtmCoordinates> coordenadasConvertidas = new List<UtmCoordinates>();
                foreach (CoordinateOutput coord in coordinates)
                {
                    var longitude = coord.Coordinates[0];
                    var latitude = coord.Coordinates[1];

                    // Extract UTM zone and zone letter
                    int utmZone = (int)Math.Floor((longitude + 180.0) / 6) + 1;
                    char utmZoneLetter = GetUtmZoneLetter(latitude);

                    ICoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;

                    // Define the UTM coordinate system
                    IProjectedCoordinateSystem utm = ProjectedCoordinateSystem.WGS84_UTM(utmZone, latitude >= 0);

                    // Create the transformation
                    CoordinateTransformationFactory ctFactory = new CoordinateTransformationFactory();
                    ICoordinateTransformation transformation = ctFactory.CreateFromCoordinateSystems(wgs84, utm);

                    // Perform the conversion
                    double[] input = new double[] { longitude, latitude };
                    double[] output = transformation.MathTransform.Transform(input);

                    var coordenadasUTM = SetDadosCoordenadas(utmZoneLetter,utmZone,output,coord);

                    coordenadasConvertidas.Add(coordenadasUTM);
                }
                return coordenadasConvertidas;

            }
            catch (Exception)
            {

                throw new ApplicationException("Error during the conversion of LAT/ LONG to UTM");
            }

        }
        private static UtmCoordinates SetDadosCoordenadas(char zoneLetter, int zoneNumber, double[] numerosConvertidos, CoordinateOutput coordenada)
        {
            UtmCoordinates coordenadasUTM = new UtmCoordinates{
                Letter = zoneLetter,
                Zone = zoneNumber,
                Easting = Math.Round(numerosConvertidos[0],2),
                Northing = Math.Round(numerosConvertidos[1],2),
                PointName = coordenada.Point
            };           

            return coordenadasUTM;

        }

        private static double[] GetCoordinatesFromString(string linha)
        {
            string[] coordParts = linha.Split(',');
            double longitude = Convert.ToDouble(coordParts[0].Replace('.', ','));
            double latitude = Convert.ToDouble(coordParts[1].Replace('.', ','));

            double[] coordinates = new double[] { longitude, latitude };

            return coordinates;
        }

        private static char GetUtmZoneLetter(double latitude)
        {
            char[] letters = "CDEFGHJKLMNPQRSTUVWXX".ToCharArray();
            int index = (int)Math.Floor((latitude + 80) / 8);
            return letters[index];
        }
    }
}
