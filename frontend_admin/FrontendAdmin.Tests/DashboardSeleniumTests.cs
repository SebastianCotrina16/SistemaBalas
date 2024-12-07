using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

public class DashboardSeleniumTests : IDisposable
{
    private readonly IWebDriver _driver;

    public DashboardSeleniumTests()
    {
        _driver = new ChromeDriver();
    }

    [Fact]
    public void Dashboard_VisibleToOperator_AfterLogin()
    {
        _driver.Navigate().GoToUrl("http://localhost:5286/Usuarios/Login");

        _driver.FindElement(By.Name("correo")).SendKeys("prueba1@gmail.com");
        _driver.FindElement(By.Name("clave")).SendKeys("prueba123");
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        Thread.Sleep(2000);
        Assert.Contains("Dashboard", _driver.Url);

        WaitForElementToBeVisible(By.LinkText("Inicio"));
        Assert.True(_driver.FindElement(By.LinkText("Inicio")).Displayed);

        WaitForElementToBeVisible(By.LinkText("Configurar Examen"));
        Assert.True(_driver.FindElement(By.LinkText("Configurar Examen")).Displayed);

        WaitForElementToBeVisible(By.LinkText("Tiradores"));
        Assert.True(_driver.FindElement(By.LinkText("Tiradores")).Displayed);

        WaitForElementToBeVisible(By.LinkText("Reporte de Tiradores"));
        Assert.True(_driver.FindElement(By.LinkText("Reporte de Tiradores")).Displayed);

        WaitForElementToBeVisible(By.LinkText("Registrar Tirador"));
        Assert.True(_driver.FindElement(By.LinkText("Registrar Tirador")).Displayed);

    }

    private void WaitForElementToBeVisible(By by, int timeoutInSeconds = 10)
    {
        var end = DateTime.Now + TimeSpan.FromSeconds(timeoutInSeconds);
        while (DateTime.Now < end)
        {
            try
            {
                var element = _driver.FindElement(by);
                if (element.Displayed)
                    return;
            }
            catch (NoSuchElementException)
            {
            }
            Thread.Sleep(500);
        }
        throw new NoSuchElementException($"Element {by} not found within {timeoutInSeconds} seconds");
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
