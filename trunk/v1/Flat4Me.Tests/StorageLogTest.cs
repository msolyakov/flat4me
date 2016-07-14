using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flat4Me.Data.Repository.Azure;

namespace Flat4Me.Tests
{
    [TestClass]
    public class StorageLogTest
    {
        [TestMethod]
        public void TestLogInfo()
        {
            StorageLogRepository logger = new StorageLogRepository();
            logger.Init();
            logger.LogInfo("TestLogInfo test was called.").Wait();            
        }

        [TestMethod]
        public void TestLogWarning()
        {
            StorageLogRepository logger = new StorageLogRepository();
            logger.Init();
            logger.LogWarning("TestLogWarning test was called.", new Exception("Test Warning")).Wait();
        }

        [TestMethod]
        public void TestLogException()
        {
            StorageLogRepository logger = new StorageLogRepository();
            logger.Init();
            logger.LogException("TestLogException test was called.", new Exception("Test Exception")).Wait();
        }
    }
}
