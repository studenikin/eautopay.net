using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Tests.Fakes;

namespace EAutopay.Tests
{
    [TestClass]
    public class AppConfigTest
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationMissingException))]
        public void Ctor_Throws_If_Login_Is_Null()
        {
            var fake = new FakeConfig();
            var config = new AppConfig(fake.Settings);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationMissingException))]
        public void Ctor_Throws_If_Login_Is_Empty()
        {
            var fake = new FakeConfig();
            fake.SetLogin("");
            var config = new AppConfig(fake.Settings);
        }

        [TestMethod]
        public void Login_Returns_Correct_Value()
        {
            var fake = new FakeConfig();
            fake.SetLogin("test_login");
            var config = new AppConfig(fake.Settings);

            Assert.AreEqual("test_login", config.Login);
        }

        [TestMethod]
        public void UpsellLandingPage_Returns_Empty_String_If_Not_Assigned()
        {
            var fake = new FakeConfig();
            fake.SetLogin("test_login");
            var config = new AppConfig(fake.Settings);

            Assert.AreEqual("", config.UpsellLandingPage);
        }

        [TestMethod]
        public void UpsellLandingPage_Returns_Correct_Value()
        {
            var fake = new FakeConfig();
            fake.SetLogin("test_login");
            fake.SetUpsellLandingPage("http://eautopay.com");
            var config = new AppConfig(fake.Settings);

            Assert.AreEqual("http://eautopay.com", config.UpsellLandingPage);
        }

        [TestMethod]
        public void UpsellInterval_Returns_0_If_Not_Assigned()
        {
            var fake = new FakeConfig();
            fake.SetLogin("test_login");
            var config = new AppConfig(fake.Settings);

            Assert.AreEqual(0, config.UpsellInterval);
        }

        [TestMethod]
        public void UpsellInterval_Returns_Correct_Value()
        {
            var fake = new FakeConfig();
            fake.SetLogin("test_login");
            fake.SetUpsellInterval("25");
            var config = new AppConfig(fake.Settings);

            Assert.AreEqual(25, config.UpsellInterval);
        }

        [TestMethod]
        public void UpsellInterval_Returns_0_If_Incorrect_Int()
        {
            var fake = new FakeConfig();
            fake.SetLogin("test_login");
            fake.SetUpsellInterval("bad_integer_value");
            var config = new AppConfig(fake.Settings);

            Assert.AreEqual(0, config.UpsellInterval);
        }
    }
}
