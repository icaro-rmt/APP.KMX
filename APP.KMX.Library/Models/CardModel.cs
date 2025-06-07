
using System;

namespace APP.KMX.Models
{
    public class CardModel
    {
        public string ImageUrl { get; set; } = string.Empty;
        public string Title{ get; set; } = string.Empty;
        public string Text{ get; set; } = string.Empty;
        public string ActionName{ get; set; } = string.Empty;
        public string ControllerName{ get; set; } = string.Empty;
        public bool ShowForm { get; set; }        

    }
}
