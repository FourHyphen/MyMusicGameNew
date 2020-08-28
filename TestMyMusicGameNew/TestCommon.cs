using System;
using System.Collections.Generic;
using System.Windows.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMyMusicGameNew
{
    [TestClass]
    public class TestCommon
    {
        private string BeforeEnvironment { get; set; }
        private readonly string TestReadJsonFilePath = "/Resource/TestJsonRead.json";

        [TestInitialize]
        public void TestInit()
        {
            BeforeEnvironment = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Common.GetEnvironmentDirPath();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Environment.CurrentDirectory = BeforeEnvironment;
        }

        [TestMethod]
        public void TestGetFilePathOfDependentEnvironment()
        {
            string test = MyMusicGameNew.Common.GetFilePathOfDependentEnvironment(TestReadJsonFilePath);
            Assert.IsTrue(test.Contains(Environment.CurrentDirectory));
            Assert.IsTrue(test.Contains(TestReadJsonFilePath));
        }

        [TestMethod]
        public void TestGetStringListInJson()
        {
            string filePath = MyMusicGameNew.Common.GetFilePathOfDependentEnvironment(TestReadJsonFilePath);
            List<string> test = MyMusicGameNew.Common.GetStringListInJson(filePath);
            Assert.IsTrue(test.Contains("test1"));
            Assert.IsTrue(test.Contains("test2"));
            Assert.IsFalse(test.Contains("test3"));
        }
    }
}
