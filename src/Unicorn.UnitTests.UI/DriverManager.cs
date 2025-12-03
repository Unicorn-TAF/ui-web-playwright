using Microsoft.Playwright;
using BrowserType = Unicorn.UI.Web.BrowserType;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UnitTests.UI
{
    internal static class DriverManager
    {
        public static DesktopWebDriver Instance { get; } = GetDriverInstance();

        private static DesktopWebDriver GetDriverInstance()
        {
            return new DesktopWebDriver(BrowserType.Chrome, GetBrowserOptions(), true);
        }

        private static BrowserTypeLaunchOptions GetBrowserOptions()
        {
            BrowserTypeLaunchOptions browserOptions = new BrowserTypeLaunchOptions()
            {
                Headless = false,
                ChromiumSandbox = false
            };

            return browserOptions;
        }
    }
}
