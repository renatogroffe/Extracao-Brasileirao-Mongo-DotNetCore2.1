namespace CargaDadosBrasileirao
{
    public class SeleniumConfigurations
    {
        public string CaminhoDriverFirefox { get; set; }
        public string UrlPaginaClassificacaoBrasileirao { get; set; }
        public int Timeout { get; set; }
    }

    public class MongoDBConfigurations
    {
        public string Connection { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
}