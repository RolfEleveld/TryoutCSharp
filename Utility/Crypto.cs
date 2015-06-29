using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    /// <summary>
    /// Cryptographic support class
    /// </summary>
    /// <remarks>See <see cref="http://msdn.microsoft.com/en-us/library/ms998288.aspx">about asp.net security</see></remarks>
    public class Crypto
    {
        #region Constants
        private const int KeySize = 256;
        private const int BlockSize = 256;
        private const int FeedbackSize = 256;
        private const CipherMode Mode = CipherMode.ECB;
        private const PaddingMode Padding = PaddingMode.Zeros;
        private static readonly byte[] Key = new byte[] { 0xFF, 0xFE, 0xFD, 0xFC, 0xFB, 0xFA, 0xF9, 0xF8, 0xF7, 0xF6, 0xF5, 0xF4, 0xF3, 0xF2, 0xF1, 0xF0 };
        private static readonly byte[] Iv = new byte[] { 0xFF, 0xFE, 0xFD, 0xFC, 0xFB, 0xFA, 0xF9, 0xF8, 0xF7, 0xF6, 0xF5, 0xF4, 0xF3, 0xF2, 0xF1, 0xF0 };
        #endregion

        #region Encryption
        /// <summary>
        /// Encrypts the specified plaintext.
        /// </summary>
        /// <param name="plaintext">The plaintext to encrypt.</param>
        /// <returns>Crypted String</returns>
        public static string Encrypt( string plaintext )
        {
            return Encrypt( plaintext, Key, Iv );
        }

        public static byte[] GetPsuedoRandomByteArrayOfLength(int length)
        {
            Random random = new Random(length);
            byte[] returnvalue = new byte[length];
            random.NextBytes(returnvalue);
            return returnvalue;
        }

        public static byte[] GetEncryptionStrengthRandomByteArrayOfLength(int length)
        {
            RandomNumberGenerator random = RandomNumberGenerator.Create();
            byte[] returnvalue = new byte[length];
            random.GetBytes(returnvalue);
            return returnvalue;
        }
        
        /// <summary>
        /// Encrypts the specified plaintext.
        /// </summary>
        /// <param name="plaintext">The plaintext to encrypt.</param>
        /// <param name="key">The Key.</param>
        /// <param name="iV">The Initialization Vector.</param>
        /// <returns>Crypted String using key and Initialization Vector</returns>
        public static string Encrypt( string plaintext, byte[] key, byte[] iV )
        {
            byte[] plainTextBuffer = Encoding.Default.GetBytes( plaintext );
            SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create( "Rijndael" );

            // Settings
            symmetricAlgorithm.KeySize = KeySize;
            symmetricAlgorithm.BlockSize = BlockSize;
            symmetricAlgorithm.FeedbackSize = FeedbackSize;

            symmetricAlgorithm.Key = PaddArrayToKeySize( key );
            symmetricAlgorithm.IV = PaddArrayToKeySize( iV );

            symmetricAlgorithm.Mode = Mode;
            symmetricAlgorithm.Padding = Padding;

            // create an ICryptoTransform that can be used to encrypt data
            ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateEncryptor();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream( memoryStream, cryptoTransform, CryptoStreamMode.Write );
            cryptoStream.Write( plainTextBuffer, 0, plainTextBuffer.Length );
            cryptoStream.Close();

            // get the ciphertext from the MemoryStream
            byte[] cipherTextBuffer = memoryStream.ToArray();
            memoryStream.Close();
            string encryptedString = Convert.ToBase64String( cipherTextBuffer );
            cryptoTransform.Dispose();
            symmetricAlgorithm.Clear();

            return encryptedString;
        }
        #endregion

        #region Decryption
        /// <summary>
        /// Decrypts the specified cryptText.
        /// </summary>
        /// <param name="cryptText">The crypttext to decrypt.</param>
        /// <returns>Decrypted String</returns>
        public static string Decrypt( string cryptText )
        {
            return Decrypt( cryptText, Key, Iv );
        }

        /// <summary>
        /// Decrypts the specified cryptText.
        /// </summary>
        /// <param name="cryptText">The crypttext to decrypt.</param>
        /// <param name="key">The Key.</param>
        /// <param name="iV">The Initialization Vector.</param>
        /// <returns>Decrypted String using key and Initialization Vector</returns>
        public static string Decrypt( string cryptText, byte[] key, byte[] iV )
        {
            byte[] cryptoTextBuffer = Convert.FromBase64String( cryptText );
            SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create( "Rijndael" );

            // Settings
            symmetricAlgorithm.KeySize = KeySize;
            symmetricAlgorithm.BlockSize = BlockSize;
            symmetricAlgorithm.FeedbackSize = FeedbackSize;

            symmetricAlgorithm.Key = PaddArrayToKeySize( key );
            symmetricAlgorithm.IV = PaddArrayToKeySize( iV );

            symmetricAlgorithm.Mode = Mode;
            symmetricAlgorithm.Padding = Padding;

            // create an ICryptoTransform that can be used to encrypt data
            ICryptoTransform plainTextTransform = symmetricAlgorithm.CreateDecryptor();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream plainTextStream = new CryptoStream( memoryStream, plainTextTransform, CryptoStreamMode.Write );
            plainTextStream.Write( cryptoTextBuffer, 0, cryptoTextBuffer.Length );
            plainTextStream.Close();

            // get the ciphertext from the MemoryStream
            byte[] plainTextBuffer = memoryStream.ToArray();
            memoryStream.Close();
            string decryptedString = Encoding.Default.GetString( plainTextBuffer ).TrimEnd( new [] { '\0' } );
            plainTextTransform.Dispose();
            symmetricAlgorithm.Clear();

            return decryptedString;
        }
        #endregion

        /// <summary>
        /// Padds the size of the array to key length.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        private static byte[] PaddArrayToKeySize( ICollection<byte> array )
        {
            byte[] returnByteArray = new byte[KeySize / 8];

            for ( int i = 0; i < returnByteArray.Length; i++ )
            {
                returnByteArray[i] = 0;
            }
            if ( array != null )
            {
                array.CopyTo( returnByteArray, 0 );
            }
            return returnByteArray;
        }
    }
}