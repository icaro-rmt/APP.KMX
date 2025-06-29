using APP.KMX.App.Models;

namespace APP.KMX.App.Services.Interfaces
{
    public interface IConversionButtonService
    {
        List<ConversionButtonModel> GetAvailableConversions();
    }
}
