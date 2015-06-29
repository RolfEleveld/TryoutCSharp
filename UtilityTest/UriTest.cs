using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UtilityTest
{
    /// <summary>
    /// Summary description for UriTest
    /// </summary>
    [TestClass]
    public class UriTest
    {
        public UriTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestSimpleJoininUri()
        {
            Uri source = new Uri("http://www.localhost.name/path/page?query=value&param=values");
            string pathAndQuery = "/otherpath/otherpage?newquery=newvalue&param=othervalue";
            Uri result = new Uri( source, pathAndQuery );
            Debug.WriteLine( "Simple Join " + result );
        }
        [TestMethod]
        public void TestComplexKeepOldJoininUri()
        {
            Uri source = new Uri( "http://www.localhost.name/path/page?query=value&param=values" );
            string pathAndQuery = "/otherpath/otherpage?newquery=newvalue&param=othervalue";
            
            Uri result = new Uri( source, pathAndQuery );
            StringDictionary queryParameters = new StringDictionary();
            // add the new parameters
            foreach (string parameter in result.Query.Trim("?&".ToCharArray()).Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                string name = parameter.Split("=".ToCharArray())[0];
                string value = parameter.Substring(name.Length + 1);
                if (queryParameters.ContainsKey(name)) // do not duplicate a parameter, keep last value
                {
                    queryParameters[name] = value;
                }
                else
                {
                    queryParameters.Add(name, value);
                }
            }
            // keep the original set
            foreach ( string parameter in source.Query.Trim( "?&".ToCharArray() ).Split( "&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries ) )
            {
                string name = parameter.Split( "=".ToCharArray() )[0];
                string value = parameter.Substring( name.Length + 1 );
                if ( queryParameters.ContainsKey( name ) ) // do not duplicate a parameter, keep last original value
                {
                    queryParameters[name] = value;
                }
                else
                {
                    queryParameters.Add( name, value );
                }
            }
            // recreate the uri
            result = new Uri(result.GetLeftPart(UriPartial.Path));
            StringBuilder query = new StringBuilder();
            foreach (string key in queryParameters.Keys)
            {
                if (query.Length > 0)
                {
                    query.AppendFormat("&{0}={1}", key, queryParameters[key]);
                }
                else
                {
                    query.AppendFormat( "?{0}={1}", key, queryParameters[key] );
                }
            }
            result = new Uri(result.ToString() + query.ToString());
            Debug.WriteLine( "Complex Join Maintain Original " + result );
        }
    }
}
