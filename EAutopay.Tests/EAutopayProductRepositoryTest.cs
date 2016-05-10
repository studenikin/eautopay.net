using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Products;
using EAutopay.Tests.Fakes;

namespace EAutopay.Tests
{
    [TestClass]
    public class EAutopayProductRepositoryTest
    {
        FakeConfig _config;
        FakeCrawler _crawler;
        FakeProductParser _parser;
        EAutopayProductRepository _repo;

        [TestMethod]
        public void Save_Product_Triggers_Crawler()
        {
            InitRepo();
            var p = new Product();

            _repo.Save(p, false);
            Assert.IsTrue(_crawler.WasCalled);
        }

        [TestMethod]
        public void Existing_Product_Doesnt_Change_ID_On_Save()
        {
            InitRepo();
            var p = new Product();
            p.ID = 100;

            _repo.Save(p, false);
            Assert.AreEqual(100, p.ID);
        }

        [TestMethod]
        public void New_Product_Changes_ID_On_Save()
        {
            InitRepo();
            var p = new Product();

            _repo.Save(p, false);
            Assert.AreEqual(10, p.ID);
        }

        [TestMethod]
        public void Deleting_New_Product_Does_Nothing()
        {
            InitRepo();
            var p = new Product();

            _repo.Delete(p);
            Assert.IsFalse(_crawler.WasCalled);
        }

        [TestMethod]
        public void Deleting_Product_Resets_Values()
        {
            InitRepo();
            var p = new Product();
            p.ID = 1;
            p.Name = "product";
            p.Price = 123.00;

            _repo.Delete(p);

            Assert.AreEqual(0, p.ID);
            Assert.AreEqual("", p.Name);
            Assert.AreEqual(0, p.Price);
        }

        [TestMethod]
        public void GetAll_Returns_Several_Products()
        {
            InitRepo();
            var products = _repo.GetAll();
            Assert.AreEqual(2, products.Count);
        }

        [TestMethod]
        public void Get_Returns_Correct_Product()
        {
            InitRepo();
            var p = _repo.Get(1);

            Assert.AreEqual(1, p.ID);
            Assert.AreEqual("product 1", p.Name);
            Assert.AreEqual(999.00, p.Price);
        }

        private void InitRepo()
        {
            _config = new FakeConfig();
            _crawler = new FakeCrawler();
            _parser = new FakeProductParser();
            _repo = new EAutopayProductRepository(_config, _crawler, _parser);
        }
    }
}
