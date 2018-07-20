using MongoDB.Driver;

namespace CargaDadosBrasileirao
{
    public class ClassificacaoRepository
    {
        private MongoClient _client;
        private IMongoDatabase _db;

        public ClassificacaoRepository(
            MongoDBConfigurations configurations)
        {
            _client = new MongoClient(
                configurations.Connection);
            _db = _client.GetDatabase(configurations.Database);
        }

        public void Incluir(Classificacao classificacao)
        {
            var collection =
                _db.GetCollection<Classificacao>("Classificacao");
            collection.InsertOne(classificacao);
        }
    }
}