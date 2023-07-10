using Spark.Library.Environment;

namespace BlazorSpark.Tests.Environment
{
    [TestClass]
    public class EnvironmentVariablesTest
    {
        [TestMethod]
        public void CanLoadConfig()
        {
            EnvManager.LoadConfig();
            Assert.IsTrue(Env.Get("APP_NAME") == "SparkTests");
		}

		[TestMethod]
		public void CanGetVariable()
		{
			EnvManager.LoadConfig();
			Assert.IsTrue(Env.Get("REG_VAR") == "test");
		}

		[TestMethod]
		public void CanGetQuoteVariable()
		{
			EnvManager.LoadConfig();
			var envVar = Env.Get("QUOTE_VAR");
			Assert.IsTrue(Env.Get("QUOTE_VAR") == "testing variables in quotes");
		}
	}
}