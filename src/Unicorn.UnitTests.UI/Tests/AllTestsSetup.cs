using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.UI.Tests
{
    [TestAssembly]
    internal class AllTestsSetup
    {
        [RunFinalize]
        public static void GlobalTeardown() =>
            DriverManager.Instance.Close();
    }
}
