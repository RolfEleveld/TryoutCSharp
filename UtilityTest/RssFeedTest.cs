using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel.Syndication;
using Utility;

namespace UtilityTest
{
    
    
    /// <summary>
    ///This is a test class for RssFeedTest and is intended
    ///to contain all RssFeedTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RssFeedTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Feed
        ///</summary>
        [TestMethod]
        public void AtomFeedTest()
        {
            RssFeed target = new RssFeed();
            string format = "Atom"; 
            SyndicationFeedFormatter expected = null; 
            SyndicationFeedFormatter actual;
            actual = target.Feed( format );
            Assert.AreEqual( expected, actual );
            Assert.Inconclusive( "Verify the correctness of this test method." );
        }
    }
}
