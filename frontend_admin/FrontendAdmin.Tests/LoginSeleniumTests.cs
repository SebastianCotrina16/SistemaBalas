using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

public class LoginSeleniumTests : IDisposable
{
    private readonly IWebDriver _driver;

    public LoginSeleniumTests()
    {
        _driver = new ChromeDriver();
    }

    [Fact]
    public void LoginTestWithSelenium()
    {
        // Navegar a la página de inicio de sesión
        _driver.Navigate().GoToUrl("http://localhost:5286/Usuarios/Login");
        Console.WriteLine("Página de inicio de sesión cargada.");

        // Confirmar si la URL actual es la esperada
        Assert.Contains("Usuarios/Login", _driver.Url);

        // Ingresar credenciales de prueba
        _driver.FindElement(By.Name("correo")).SendKeys("prueba1@gmail.com");
        _driver.FindElement(By.Name("clave")).SendKeys("prueba123");
        Console.WriteLine("Credenciales ingresadas.");

        // Enviar el formulario
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        Console.WriteLine("Formulario enviado.");

        // Espera unos segundos para permitir el redireccionamiento
        Thread.Sleep(2000);

        // Verificar si el usuario ha sido redirigido correctamente
        string currentUrl = _driver.Url;
        Console.WriteLine("URL actual después del login: " + currentUrl);

        if (currentUrl.Contains("Dashboard"))
        {
            Console.WriteLine("Redirigido correctamente al Dashboard.");
        }
        else if (currentUrl.Contains("Usuarios/Login"))
        {
            Console.WriteLine("Redirigido de nuevo a Login, probablemente debido a credenciales incorrectas.");
            Assert.True(false, "Redirigido a la página de Login en lugar de Dashboard. Verifica las credenciales.");
        }
        else
        {
            Assert.True(false, "La redirección no fue a Dashboard ni a Login, revisar flujo de autenticación.");
        }
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
