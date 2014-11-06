using System;
using System.Security.Cryptography;
using System.Text;
using Common.Api.Exceptions;
using Common.Api.Types;
using Common.Api;

namespace Common.Api.Cryptography
{
    public class Hash
    {
        public const string MD5 = "MD5";
        public const string SHA1 = "SHA1";

        private static HashAlgorithm GetAlgorithm(string p_sName)
        {
            switch (p_sName)
            {
                case MD5:
                    return new MD5CryptoServiceProvider();
                case SHA1:
                    return new SHA1CryptoServiceProvider();

                default:
                    throw Trace.Log<ExceptionHolder>(
                        new ArgumentException("Invalid algorithm name", "p_sName"), Trace.Category.Code);
            }
        }

        public static string Compute(string p_sAlgorithmName, string p_sInput)
        {
            return Compute(p_sAlgorithmName, p_sInput, Encoding.Default);
        }

        public static string Compute(string p_sAlgorithmName, byte[] p_sInput)
        { 
            if(p_sInput.LongLength <= 0)
                return string.Empty;

            return TypeConverter.ByteArrayToString(ComputeInternal(p_sAlgorithmName, p_sInput),
                                                   Encoding.Default);
        }

        public static string Compute(string p_sAlgorithmName, string p_sInput, Encoding p_encoding)
        {
            if (string.IsNullOrEmpty(p_sInput))
                return string.Empty;

            return TypeConverter.ByteArrayToString(ComputeInternal(p_sAlgorithmName, p_sInput, p_encoding),
                                                   p_encoding);
        }

        private static byte[] ComputeInternal(string p_sAlgorithmName, byte[] p_sInput)
        {
            HashAlgorithm provider = GetAlgorithm(p_sAlgorithmName);
            return provider.ComputeHash(p_sInput);
        }

        private static byte[] ComputeInternal(string p_sAlgorithmName, string p_sInput, Encoding p_encoding)
        {
            if (string.IsNullOrEmpty(p_sInput))
                return new byte[0] {};

            return ComputeInternal(p_sAlgorithmName, p_encoding.GetBytes(p_sInput));
        }

        public static string ReadableHash(string p_sAlgorithmName, string p_sInput)
        {
            return ReadableHash(p_sAlgorithmName, p_sInput, Encoding.Default);
        }

        public static string ReadableHash(string p_sAlgorithmName, string p_sInput, Encoding p_encoding)
        {
            if (string.IsNullOrEmpty(p_sInput))
                return string.Empty;

            return HexEncoder.GetString(ComputeInternal(p_sAlgorithmName, p_sInput, p_encoding));
        }

		/// <summary>
		/// Calculates a MD-5 hash in lower case using UTF-8 encoding.
		/// </summary>
		/// <param name="input">Input string.</param>
		/// <returns></returns>
		public static string CalculateMd5Hash(string input)
		{
			using (var provider = GetAlgorithm(MD5))
			{
				return CalculateHashInternal(input, Encoding.UTF8, provider);
			}
		}

		/// <summary>
		/// Calculates a MD-5 hash in lower case using encoding specified in <see cref="encoding"/> param.
		/// </summary>
		/// <param name="input">Input string.</param>
		/// <param name="encoding">Encoding to calculate hash.</param>
		/// <returns></returns>
		public static string CalculateMd5Hash(string input, Encoding encoding)
		{
			using (var provider = GetAlgorithm(MD5))
			{
				return CalculateHashInternal(input, encoding, provider);
			}
		}

		/// <summary>
		/// Calculates a SHA-1 hash in lower case using UTF-8 encoding.
		/// </summary>
		/// <param name="input">Input string.</param>
		/// <returns></returns>
		public static string CalculateSha1Hash(string input)
		{
			using (var provider = GetAlgorithm(SHA1))
			{
				return CalculateHashInternal(input ?? string.Empty, Encoding.UTF8, provider);
			}
		}

		/// <summary>
		/// Calculates a SHA-1 hash in lower case using encoding specified in <see cref="encoding"/> param.
		/// </summary>
		/// <param name="input">Input string.</param>
		/// <param name="encoding">Encoding to calculate hash.</param>
		/// <returns></returns>
		public static string CalculateSha1Hash(string input, Encoding encoding)
		{
			using (var provider = GetAlgorithm(SHA1))
			{
				return CalculateHashInternal(input ?? string.Empty, encoding, provider);
			}
		}

	    private static string CalculateHashInternal(string input, Encoding encoding, HashAlgorithm hashProvider)
	    {
			// step 1, calculate hash from input.
			var inputBytes = encoding.GetBytes(input);
			var hashBytes = hashProvider.ComputeHash(inputBytes);

			// step 2, convert byte array to hex string.
			var builder = new StringBuilder();
			for (var i = 0; i < hashBytes.LongLength; i++)
			{
				builder.Append(hashBytes[i].ToString("x2"));
			}
			return builder.ToString();
	    }
    }
}