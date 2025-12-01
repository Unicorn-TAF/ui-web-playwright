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
            return new DesktopWebDriver(BrowserType.Chrome, GetChromeOptions(), true);
        }

        private static BrowserTypeLaunchOptions GetChromeOptions()
        {
            BrowserTypeLaunchOptions browserOptions = new BrowserTypeLaunchOptions()
            {
                Headless = false,
                ChromiumSandbox = false,
                //SlowMo = 0f
            };

            //options.AddArguments(
            //    "allow-insecure-localhost",
            //    "ignore-certificate-errors",
            //    "ignore-ssl-errors=yes",
            //    "disable-extensions",
            //    "disable-infobars",
            //    "no-sandbox",
            //    "disable-impl-side-painting",
            //    "enable-gpu-rasterization",
            //    "force-gpu-rasterization",
            //    "headless",
            //    "--window-size=1920x1080");

            return browserOptions;
        }
    }
}
