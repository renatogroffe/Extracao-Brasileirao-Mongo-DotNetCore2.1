using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CargaDadosBrasileirao
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Carregando configurações...");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");
            var configuration = builder.Build();

            var seleniumConfigurations = new SeleniumConfigurations();
            new ConfigureFromConfigurationOptions<SeleniumConfigurations>(
                configuration.GetSection("SeleniumConfigurations"))
                    .Configure(seleniumConfigurations);

            var mongoDBConfigurations = new MongoDBConfigurations();
            new ConfigureFromConfigurationOptions<MongoDBConfigurations>(
                configuration.GetSection("MongoDBConfigurations"))
                    .Configure(mongoDBConfigurations);

            Console.WriteLine(
                "Carregando driver do Selenium para Firefox em modo headless...");
            var paginaClassificacao = new PaginaClassificacao(
                seleniumConfigurations);

            Console.WriteLine(
                "Carregando página com classificações do Brasileirão...");
            paginaClassificacao.CarregarPagina();

            Console.WriteLine(
                "Extraindo dados...");
            var classificacao = paginaClassificacao.ObterClassificacao();
            paginaClassificacao.Fechar();

            Console.WriteLine("Gravando dados extraídos...");
            new ClassificacaoRepository(mongoDBConfigurations)
                .Incluir(classificacao);
            Console.WriteLine(
                "Carga de dados concluída com sucesso!");

            Console.ReadKey();
        }
    }
}