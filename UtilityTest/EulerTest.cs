using System;
using Utility;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UtilityTest
{
    ///This is a test class for EulerTest and is intended
    ///to contain all EulerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EulerTest
    {


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
        ///A test for GetSum
        ///</summary>
        [TestMethod]
        public void GetSumTest()
        {
            Euler target = new Euler(); 
            int fromValue = 1; 
            int toValue = 999; 
            int divisor = 3; 
            long expected = 166833; 
            long actual;
            actual = target.GetSum( fromValue, toValue, divisor );
            Assert.AreEqual( expected, actual );
        }
        /// <summary>
        ///A test for GetSum
        ///</summary>
        [TestMethod()]
        public void GetSumAssociativepPart1Test()
        {
            Euler target = new Euler();
            int fromValue = 1;
            int toValue = 500;            
            int divisor = 3;
            long expected = 41583;
            long actual;
            actual = target.GetSum( fromValue, toValue, divisor );
            Assert.AreEqual( expected, actual );
        }
        /// <summary>
        ///A test for GetSum
        ///</summary>
        [TestMethod()]
        public void GetSumAssociativepPart2Test()
        {
            Euler target = new Euler();
            int fromValue = 501;
            int toValue = 999;
            int divisor = 3;
            long expected = 166833 - 41583;
            long actual;
            actual = target.GetSum( fromValue, toValue, divisor );
            Assert.AreEqual( expected, actual );
        }


        /// <summary>
        ///A test for GetFibonacciSum
        ///</summary>
        [TestMethod()]
        public void GetFibonacciSum1Test()
        {
            Euler target = new Euler(); 
            int toValue = 1; 
            int divisor = 1; 
            long expected = 1; 
            long actual;
            actual = target.GetFibonacciSum( toValue, divisor );
            Assert.AreEqual( expected, actual );
        }
        /// <summary>
        ///A test for GetFibonacciSum
        ///</summary>
        [TestMethod()]
        public void GetFibonacciSum3Test()
        {
            Euler target = new Euler();
            int toValue = 2;
            int divisor = 1;
            long expected = 3;
            long actual;
            actual = target.GetFibonacciSum( toValue, divisor );
            Assert.AreEqual( expected, actual );
        }
        /// <summary>
        ///A test for GetFibonacciSum
        ///</summary>
        [TestMethod()]
        public void GetFibonacciSum2Test()
        {
            Euler target = new Euler();
            int toValue = 2;
            int divisor = 2;
            long expected = 2;
            long actual;
            actual = target.GetFibonacciSum( toValue, divisor );
            Assert.AreEqual( expected, actual );
            Assert.Inconclusive( "Verify the correctness of this test method." );
        }
        /// <summary>
        ///A test for GetFibonacciSum
        ///</summary>
        [TestMethod()]
        public void GetFibonacci3SumTest()
        {
            Euler target = new Euler();
            int toValue = 3;
            int divisor = 1;
            long expected = 6;
            long actual;
            actual = target.GetFibonacciSum( toValue, divisor );
            Assert.AreEqual( expected, actual );
        }
        /// <summary>
        ///A test for GetFibonacciSum
        ///</summary>
        [TestMethod()]
        public void GetFibonacci4SumTest()
        {
            Euler target = new Euler();
            int toValue = 4;
            int divisor = 1;
            long expected = 6;
            long actual;
            actual = target.GetFibonacciSum( toValue, divisor );
            Assert.AreEqual( expected, actual );
        }
        /// <summary>
        ///A test for GetFibonacciSum
        ///</summary>
        [TestMethod()]
        public void GetFibonacci5SumTest()
        {
            Euler target = new Euler();
            int toValue = 5;
            int divisor = 1;
            long expected = 11;
            long actual;
            actual = target.GetFibonacciSum( toValue, divisor );
            Assert.AreEqual( expected, actual );
        }

        /// <summary>
        ///A test for PrimeFactors
        ///</summary>
        [TestMethod()]
        public void PrimeFactors2Test()
        {
            Euler target = new Euler(); 
            ulong number = 2;
            IEnumerable<ulong> expected = new HashSet<ulong>();
            ( (HashSet<ulong>) expected ).Add( 2 );
            IEnumerable<ulong> actual;
            actual = target.PrimeFactors( number );
            System.Diagnostics.Debug.Write( "Actual: " );
            foreach ( ulong value in actual )
            {
                System.Diagnostics.Debug.Write(value + ", ");
            }
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.Write( "Expected: " );
            foreach ( ulong value in expected )
            {
                System.Diagnostics.Debug.Write( value + ", " );
            }
            System.Diagnostics.Debug.WriteLine("");
            //Assert.AreEqual( expected, actual );
        }

        /// <summary>
        ///A test for PrimeFactors
        ///</summary>
        [TestMethod()]
        public void PrimeFactors45Test()
        {
            Euler target = new Euler();
            ulong number = 45;
            IEnumerable<ulong> expected = new HashSet<ulong>();
            ( (HashSet<ulong>) expected ).Add( 3 );
            ( (HashSet<ulong>) expected ).Add( 5 );
            IEnumerable<ulong> actual;
            actual = target.PrimeFactors( number );
            System.Diagnostics.Debug.Write( "Actual: " );
            foreach ( ulong value in actual )
            {
                System.Diagnostics.Debug.Write( value + ", " );
            }
            System.Diagnostics.Debug.WriteLine( "" );
            System.Diagnostics.Debug.Write( "Expected: " );
            foreach ( ulong value in expected )
            {
                System.Diagnostics.Debug.Write( value + ", " );
            }
            System.Diagnostics.Debug.WriteLine( "" );
            //Assert.AreEqual( expected, actual );
        }
    }
}
