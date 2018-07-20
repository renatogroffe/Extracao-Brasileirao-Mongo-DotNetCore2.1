using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace CargaDadosBrasileirao
{
    public class PaginaClassificacao
    {
        private SeleniumConfigurations _configurations;
        private IWebDriver _driver;

        public PaginaClassificacao(SeleniumConfigurations configurations)
        {
            _configurations = configurations;

            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--headless");

            _driver = new FirefoxDriver(
                _configurations.CaminhoDriverFirefox,
                options);
        }

        public void CarregarPagina()
        {
            _driver.Manage().Timeouts().PageLoad =
                TimeSpan.FromSeconds(_configurations.Timeout);
            _driver.Navigate().GoToUrl(
                _configurations.UrlPaginaClassificacaoBrasileirao);
        }

        public Classificacao ObterClassificacao()
        {
            DateTime dataCarga = DateTime.Now;
            List<Equipe> equipes = new List<Equipe>();

            Classificacao classificacao = new Classificacao();
            classificacao.CodigoExtracao = $"Brasileirao {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
            classificacao.NomeCampeonato = "Brasileirão";
            classificacao.Temporada = _driver
                .FindElement(By.ClassName("automated-header"))
                .FindElement(By.TagName("h1"))
                .Text.Split(new char[] { ' ' }).Last();
            classificacao.Esporte = "Futebol";
            classificacao.Pais = "Brasil";
            classificacao.DataExtracao = DateTime.Now;
            classificacao.Equipes = equipes;

            var dadosEquipes = _driver
                .FindElement(By.ClassName("responsive-table-wrap"))
                .FindElement(By.TagName("table"))
                .FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("tr"));
            int posicao = 0;
            foreach (var dadosEquipe in dadosEquipes)
            {
                var estatisticasEquipe =
                    dadosEquipe.FindElements(By.TagName("td"));

                posicao++;
                Equipe equipe = new Equipe();
                equipe.Posicao = posicao;
                equipe.Nome =
                    estatisticasEquipe[0].FindElement(
                        By.ClassName("team-names")).GetAttribute("innerHTML");
                equipe.Jogos = Convert.ToInt32(
                    estatisticasEquipe[1].Text);
                equipe.Vitorias = Convert.ToInt32(
                    estatisticasEquipe[2].Text);
                equipe.Empates = Convert.ToInt32(
                    estatisticasEquipe[3].Text);
                equipe.Derrotas = Convert.ToInt32(
                    estatisticasEquipe[4].Text);
                equipe.GolsPro = Convert.ToInt32(
                    estatisticasEquipe[5].Text);
                equipe.GolsContra = Convert.ToInt32(
                    estatisticasEquipe[6].Text);
                equipe.SaldoGols = Convert.ToInt32(
                    estatisticasEquipe[7].Text);
                equipe.Pontos = Convert.ToInt32(
                    estatisticasEquipe[8].Text);

                equipes.Add(equipe);
            }

            return classificacao;
        }

        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}