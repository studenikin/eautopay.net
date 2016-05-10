using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Forms;
using EAutopay.Tests.Fakes;

namespace EAutopay.Tests
{
    [TestClass]
    public class EAutopayFormRepositoryTest
    {
        FakeConfig _config;
        FakeCrawler _crawler;
        FakeFormParser _parser;
        EAutopayFormRepository _repo;

        [TestMethod]
        public void Save_Form_Triggers_Crawler()
        {
            InitRepo();
            var f = new Form();

            _repo.Save(f);
            Assert.IsTrue(_crawler.WasCalled);
        }

        [TestMethod]
        public void Deleting_New_Form_Does_Nothing()
        {
            InitRepo();
            var f = new Form();

            _repo.Delete(f);
            Assert.IsFalse(_crawler.WasCalled);
        }

        [TestMethod]
        public void Deleting_Form_Resets_Values()
        {
            InitRepo();
            var f = new Form();
            f.ID = 1;
            f.Name = "form";

            _repo.Delete(f);

            Assert.AreEqual(0, f.ID);
            Assert.AreEqual("", f.Name);
        }

        [TestMethod]
        public void Existing_Form_Doesnt_Change_ID_On_Save()
        {
            InitRepo();
            var f = new Form();
            f.ID = 10;

            _repo.Save(f);
            Assert.AreEqual(10, f.ID);
        }

        [TestMethod]
        public void New_Form_Changes_ID_On_Save()
        {
            InitRepo();
            var f = new Form();

            _repo.Save(f);
            Assert.AreEqual(2, f.ID);
        }

        [TestMethod]
        public void GetAll_Returns_Several_Forms()
        {
            InitRepo();
            var forms = _repo.GetAll();
            Assert.AreEqual(2, forms.Count);
        }

        [TestMethod]
        public void Get_Returns_Correct_Form()
        {
            InitRepo();
            var form = _repo.Get(1);

            Assert.AreEqual(1, form.ID);
            Assert.AreEqual("form 1", form.Name);
        }

        [TestMethod]
        public void GetNewest_Returns_Correct_Form()
        {
            InitRepo();
            var form = _repo.GetNewest();

            Assert.AreEqual(2, form.ID);
            Assert.AreEqual("form 2", form.Name);
        }

        private void InitRepo()
        {
            _config = new FakeConfig();
            _crawler = new FakeCrawler();
            _parser = new FakeFormParser();
            _repo = new EAutopayFormRepository(_config, _crawler, _parser);
        }
    }
}
