using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

public class ReportesSeleniumTests : IDisposable
{
    private readonly IWebDriver _driver;

    public ReportesSeleniumTests()
    {
        _driver = new ChromeDriver();
    }

    [Fact]
    public void VisualizarReporteDeTiradores_AfterLogin_DisplaysTiradoresList()
    {
        _driver.Navigate().GoToUrl("http://localhost:5286/Usuarios/Login");

        _driver.FindElement(By.Name("correo")).SendKeys("prueba1@gmail.com");
        _driver.FindElement(By.Name("clave")).SendKeys("prueba123");
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        _driver.Navigate().GoToUrl("http://localhost:5286/Reportes/Index");

        System.Threading.Thread.Sleep(2000);

        var tiradoresTable = _driver.FindElement(By.CssSelector("table.table.table-bordered"));

        var headers = tiradoresTable.FindElements(By.TagName("th"));
        Assert.Contains(headers, header => header.Text.Contains("Usuario"));
        Assert.Contains(headers, header => header.Text.Contains("Total Impactos"));
        Assert.Contains(headers, header => header.Text.Contains("Promedio de Precisión"));
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
