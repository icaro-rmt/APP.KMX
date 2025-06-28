namespace APP.KMX.App.Configuration
{
    public class FileUploadOptions
    {
        public const string SectionName = "FileUpload";
        
        public long MaxFileSizeBytes { get; set; } = 10 * 1024 * 1024; // 10MB
        public string[] AllowedExtensions { get; set; } = { ".xls", ".xlsx", ".kml", ".kmz" };
        public string UploadFolder { get; set; } = "uploads";
        public string SampleFilesFolder { get; set; } = "fileModels";
        public bool DeleteTempFilesAfterConversion { get; set; } = true;
        public int TempFileCleanupIntervalMinutes { get; set; } = 60;
    }

    public class ConversionOptions
    {
        public const string SectionName = "Conversion";
        
        public int MaxConcurrentConversions { get; set; } = 5;
        public int ConversionTimeoutSeconds { get; set; } = 300; // 5 minutes
        public bool EnableLogging { get; set; } = true;
    }
}
