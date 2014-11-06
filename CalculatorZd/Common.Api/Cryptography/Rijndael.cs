using Common.Api;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Api.Cryptography
{
    public class Rijndael
    {

        public static byte[] StringToByteArray(string input)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetBytes(input);
        }

        public static string ByteArrayToString(byte[] input)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetString( input );
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            byte[] ret = new byte[hex.Length / 2];
            for (int j = 0; j < hex.Length; j += 2)
            {
                ret[j / 2] = byte.Parse(hex.Substring(j, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return ret;
        }

        public static string ByteArrayToHexString(byte[] input)
        {
            StringBuilder hex = new StringBuilder(input.Length * 2);
            foreach (byte b in input)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }


        public static byte[] EncryptStringFromBytes_AES(string data, byte[] Key, byte[] IV)
        {
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            System.Security.Cryptography.Rijndael algorithm = System.Security.Cryptography.Rijndael.Create();

            algorithm.Mode = CipherMode.ECB;
            MemoryStream ms = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(ms, algorithm.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
            StreamWriter myWriter = new StreamWriter(cryptoStream);
            try
            {
                myWriter.Write(data);
            }
            catch (Exception e)
            {
                Trace.Log<ExceptionHolder>(e, Trace.Category.Code);
            }
            finally
            {
                myWriter.Close();
                cryptoStream.Close();
            }
            byte[] res = ms.ToArray();
            ms.Close();
            return res;
        }

        public static string DecryptStringFromBytes_AES(byte[] data, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("data");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            System.Security.Cryptography.Rijndael algorithm = System.Security.Cryptography.Rijndael.Create();
            algorithm.Mode = CipherMode.ECB;
            MemoryStream ms = new MemoryStream(data);
            CryptoStream cryptoStream = new CryptoStream(ms, algorithm.CreateDecryptor(Key, IV), CryptoStreamMode.Read);
            StreamReader myReader = new StreamReader(cryptoStream);
            string str = null;
            try
            {
                str = myReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Trace.Log<ExceptionHolder>(e, Trace.Category.Code);
            }
            finally
            {
                myReader.Close();
                cryptoStream.Close();
                ms.Close();
            }
            return str;
        }
    }
}