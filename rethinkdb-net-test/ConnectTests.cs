using System;
using NUnit.Framework;
using RethinkDb.Configuration;

namespace RethinkDb.Test
{
    [TestFixture]
    public class ConnectTests
    {
        [Test]
        public void Connect()
        {
            var connection = ConfigConnectionFactory.Instance.Get("testCluster");
            connection.Logger = new DefaultLogger(LoggingCategory.Debug, Console.Out);
            connection.Connect();
        }
    }
}

