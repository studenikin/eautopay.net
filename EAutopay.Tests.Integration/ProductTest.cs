﻿using System.Web;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Products;

namespace EAutopay.Tests.Integration
{
    [TestClass]
    public class ProductTest
    {
        IProductRepository _repository = new EAutopayProductRepository();

        [TestInitialize]
        public void SetUp()
        {
            HttpContext.Current = Common.GetHttpContext();
            Common.Login();
        }

        [TestCleanup]
        public void TearDown()
        {
            Common.RemoveAllTestProducts();
            Common.Logout();
            HttpContext.Current = null;
        }

        [TestMethod]
        public void Product_Create()
        {
            var p1 = Common.CreateTestProduct();

            var p2 = _repository.Get(p1.ID);
            Assert.IsNotNull(p2);
            Assert.AreEqual(p1.Name, p2.Name);
            Assert.AreEqual(p1.Price, p2.Price);
        }

        [TestMethod]
        public void Product_Remove()
        {
            var p1 = Common.CreateTestProduct();

            // check product has been created in repository
            var p2 = _repository.Get(p1.ID);
            Assert.IsNotNull(p2);

            // check product has ID = 0 after removal
            _repository.Delete(p2);
            Assert.AreEqual(0, p2.ID);

            // check product has been removed from repository
            var p3 = _repository.Get(p1.ID);
            Assert.IsNull(p3);
        }
    }
}
