using Microsoft.VisualStudio.TestTools.UnitTesting;
using StartConanServers;
using System;

namespace StartConanServer.Tests
{
    [TestClass]
    public class TestConfig
    {
        [TestMethod]
        public void TestLoadConfig()
        {
            JsonConfig config = new JsonConfig();

            config.Load();
        }
    }
}
