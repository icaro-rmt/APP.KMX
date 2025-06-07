namespace APP.KMX.Models
{
    public class UTMFormat
    {
        public string PointName { get; set; }
        public double Easting { get; set; }
        public double Northing { get; set; }
        public int ZoneNumber { get; set; }
        public string ZoneLetter { get; set; }
        public bool IsNorth{ get; set; }

    }
}
