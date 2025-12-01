using Microsoft.Playwright;
using System;
using System.IO;
using System.Reflection;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Verification;
using Unicorn.UI.Web.PageObject;

namespace Unicorn.UnitTests.UI.Tests
{
    public class WebTestsBase : TestSuite
    {
        protected static T NavigateToPage<T>(bool forceNavigation) where T : WebPage
        {
            IPage driver = DriverManager.Instance.CurrentPage;
            T page = (T)Activator.CreateInstance(typeof(T), new object[] { DriverManager.Instance });

            string fullUrl = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestPages",
                page.Url);

            if (forceNavigation || driver.Url != fullUrl)
            {
                driver.GotoAsync(fullUrl).Wait();
            }

            return page;
        }

        protected static T NavigateToPage<T>() where T : WebPage =>
            NavigateToPage<T>(false);

        protected static void Refresh() =>
            DriverManager.Instance.CurrentPage.ReloadAsync().Wait();

        protected static void AssertThrows<T>(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex is T)
                {
                    return;
                }
                else
                {
                    Assert.Fail($"Expected fail with {typeof(T)}, but there was exception of type {ex.GetType()}");
                }
            }

            Assert.Fail($"Expected fail with {typeof(T)}, but there was no any exception");
        }
    }
}
