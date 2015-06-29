// <copyright file="CryptoTest.cs" company="">Copyright ©  2008</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UtilityTest
{
    /// <summary>This class contains parameterized unit tests for Crypto</summary>
    [TestClass]
    [PexClass(typeof(Crypto))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public class CryptoTest
    {
        /// <summary>Test stub for Decrypt(String)</summary>
        [PexMethod]
        public string Decrypt(string cryptText)
        {
            // TODO: add assertions to method CryptoTest.Decrypt(String)
            string result = Crypto.Decrypt(cryptText);
            return result;
        }

        /// <summary>Test stub for Decrypt(String, Byte[], Byte[])</summary>
        [PexMethod]
        public string Decrypt01(
            string cryptText,
            byte[] key,
            byte[] iV
            )
        {
            // TODO: add assertions to method CryptoTest.Decrypt01(String, Byte[], Byte[])
            string result = Crypto.Decrypt(cryptText, key, iV);
            return result;
        }

        /// <summary>Test stub for Encrypt(String)</summary>
        [PexMethod]
        public string Encrypt(string plaintext)
        {
            // TODO: add assertions to method CryptoTest.Encrypt(String)
            string result = Crypto.Encrypt(plaintext);
            return result;
        }

        /// <summary>Test stub for Encrypt(String, Byte[], Byte[])</summary>
        [PexMethod]
        public string Encrypt01(
            string plaintext,
            byte[] key,
            byte[] iV
            )
        {
            // TODO: add assertions to method CryptoTest.Encrypt01(String, Byte[], Byte[])
            string result = Crypto.Encrypt(plaintext, key, iV);
            return result;
        }
    }
}