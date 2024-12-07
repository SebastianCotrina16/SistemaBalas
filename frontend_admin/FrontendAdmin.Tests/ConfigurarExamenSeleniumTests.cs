using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

public class ConfiguracionExamenSeleniumTests : IDisposable
{
    private readonly IWebDriver _driver;

    public ConfiguracionExamenSeleniumTests()
    {
        _driver = new ChromeDriver();
    }

    [Fact]
    public void ConfigurarExamen_AfterLogin_UpdatesNumeroDisparos()
    {

        _driver.Navigate().GoToUrl("http://localhost:5286/Usuarios/Login");

        _driver.FindElement(By.Name("correo")).SendKeys("prueba1@gmail.com");
        _driver.FindElement(By.Name("clave")).SendKeys("prueba123");
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        _driver.Navigate().GoToUrl("http://localhost:5286/Examen/Setup");

        Thread.Sleep(2000);

        var numeroDisparosInput = _driver.FindElement(By.Name("NumeroDisparos"));
        numeroDisparosInput.Clear();
        numeroDisparosInput.SendKeys("10");

        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        Thread.Sleep(2000);

        numeroDisparosInput = _driver.FindElement(By.Name("NumeroDisparos"));
        Assert.Equal("10", numeroDisparosInput.GetAttribute("value"));
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
