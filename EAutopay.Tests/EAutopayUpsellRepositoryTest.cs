using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Upsells;
using EAutopay.Tests.Fakes;

namespace EAutopay.Tests
{
    [TestClass]
    public class EAutopayUpsellRepositoryTest
    {
        FakeConfig _config;
        FakeCrawler _crawler;
        FakeUpsellParser _parser;
        EAutopayUpsellRepository _repo;

        [TestMethod]
        public void Save_Upsell_Triggers_Crawler()
        {
            InitRepo();
            var u = new Upsell();

            _repo.Save(u, 1);
            Assert.IsTrue(_crawler.WasCalled);
        }

        [TestMethod]
        public void Existing_Upsell_Doesnt_Change_ID_On_Save()
        {
            InitRepo();
            var u = new Upsell();
            u.ID = 100;

            _repo.Save(u, 1);
            Assert.AreEqual(100, u.ID);
        }

        [TestMethod]
        public void New_Upsell_Changes_ID_On_Save()
        {
            InitRepo();
            var u = new Upsell();

            _repo.Save(u, 1);
            Assert.AreEqual(3, u.ID);
        }

        [TestMethod]
        public void Upsell_Changes_ParentID_On_Save()
        {
            InitRepo();
            var u = new Upsell();

            _repo.Save(u, 1);
            Assert.AreEqual(1, u.ParentID);
        }

        [TestMethod]
        public void Deleting_New_Upsell_Does_Nothing()
        {
            InitRepo();
            var u = new Upsell();

            _repo.Delete(u);
            Assert.IsFalse(_crawler.WasCalled);
        }

        [TestMethod]
        public void Deleting_Upsell_Resets_Values()
        {
            InitRepo();
            var u = new Upsell();
            u.ID = 1;
            u.Title = "title";
            u.Price = 123.00;
            u.OriginID = 10;
            u.ParentID = 20;
            u.SuccessUri = "success.com";
            u.ClientUri = "client_uri.com";

            _repo.Delete(u);

            Assert.AreEqual(0, u.ID);
            Assert.AreEqual("", u.Title);
            Assert.AreEqual(0, u.Price);
            Assert.AreEqual(0, u.OriginID);
            Assert.AreEqual(0, u.ParentID);
            Assert.AreEqual("", u.SuccessUri);
            Assert.AreEqual("", u.ClientUri);
        }

        [TestMethod]
        public void Get_Returns_Correct_Upsell()
        {
            InitRepo();
            var u = _repo.Get(2, 200);

            Assert.AreEqual(2, u.ID);
            Assert.AreEqual("title 2", u.Title);
            Assert.AreEqual(1234.00, u.Price);
            Assert.AreEqual(20, u.OriginID);
            Assert.AreEqual(200, u.ParentID);
            Assert.AreEqual("success2.com", u.SuccessUri);
            Assert.AreEqual("client2.com", u.ClientUri);
        }

        [TestMethod]
        public void Get_For_Not_Existing_Upsell_Returns_Null()
        {
            InitRepo();
            var u = _repo.Get(5, 200);
            Assert.IsNull(u);
        }

        [TestMethod]
        public void Upsell_Interval_Equals_20_If_Nothing_Specified()
        {
            InitRepo();
            _repo.EnableUpsells(1);

            Assert.AreEqual(_crawler.Paramz["time_for_add"], "20");
        }

        [TestMethod]
        public void Upsell_Interval_Equals_20_If_Bad_Value_Specified()
        {
            InitRepo();
            _config.SetUpsellInterval("asdf");
            _repo.EnableUpsells(1);

            Assert.AreEqual(_crawler.Paramz["time_for_add"], "20");
        }

        [TestMethod]
        public void Upsell_Interval_Has_Correct_Value()
        {
            InitRepo();
            _config.SetUpsellInterval("8");
            _repo.EnableUpsells(1);

            Assert.AreEqual(_crawler.Paramz["time_for_add"], "8");
        }

        private void InitRepo()
        {
            _config = new FakeConfig();
            _crawler = new FakeCrawler();
            _parser = new FakeUpsellParser();
            _repo = new EAutopayUpsellRepository(_config, _crawler, _parser);
        }
    }
}
