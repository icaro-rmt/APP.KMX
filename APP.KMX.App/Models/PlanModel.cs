namespace APP.KMX.App.Models
{
    public class PlanModel
    {
        public string NomePlano { get; set; } = string.Empty;
        public string QuantidadeConversoes { get; set; } = string.Empty;
        public string Formatos { get; set; } = string.Empty;
        public string QuantidadeArquivos { get; set; } = string.Empty;
        public string QuantidadeUsuarios { get; set; } = string.Empty;
        public decimal? DePreco { get; set; }
        public decimal? PorPreco { get; set; }
        public decimal? Desconto { get; set; }
    }
}
