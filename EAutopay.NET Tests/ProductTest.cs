﻿using System;
using System.Web;
using System.Linq;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.NET;

namespace EAutopay.NET_Tests
{
    [TestClass]
    public class ProductTest
    {

        [TestInitialize]
        public void SetUp()
        {
            HttpContext.Current = Common.GetHttpContext();
            Common.Login();
        }

        [TestCleanup]
        public void TearDown()
        {
            RemoveAllTestProducts();
            Common.Logout();
            HttpContext.Current = null;
        }

        [TestMethod]
        public void Product_Create()
        {
            var p1 = Common.CreateTestProduct();

            var p2 = ProductFactory.Get(p1.ID);
            Assert.IsNotNull(p2);
            Assert.AreEqual(p1.Name, p2.Name);
            Assert.AreEqual(p1.Price, p2.Price);
        }

        [TestMethod]
        public void Product_Remove()
        {
            var p1 = Common.CreateTestProduct();

            // check product has been created in repository
            var p2 = ProductFactory.Get(p1.ID);
            Assert.IsNotNull(p2);

            // check product has ID=0 after removal
            p2.Delete();
            Assert.AreEqual(0, p2.ID);

            // check product has been removed from repository
            var p3 = ProductFactory.Get(p1.ID);
            Assert.IsNull(p3);
        }

        private void RemoveAllTestProducts()
        {
            var products = ProductFactory.GetAll();
            foreach (var product in products.Where(p => p.Name == Common.TEST_PRODUCT_NAME))
            {
                product.Delete();
            }
        }
    }
}