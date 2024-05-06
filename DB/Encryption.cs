using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace HexStrategyInRazor.Map.DB
{
	public static class Encryption
	{
		private static string encryptionKey = "abc123";
		private static byte[] encryptionKeyBytes = "Parol krutoi"u8.ToArray();


		public static string Encrypt(string clearText)
		{
			byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new(encryptionKey, encryptionKeyBytes, 4, HashAlgorithmName.SHA512);

				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new())
				{
					using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					clearText = Convert.ToBase64String(ms.ToArray());
				}
			}
			return clearText;
		}

		public static string Decrypt(string? cipherText)
		{
			if (cipherText.IsNullOrEmpty() || cipherText == null)
			{
				return string.Empty;
			}
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			try
			{
				using (Aes encryptor = Aes.Create())
				{
					Rfc2898DeriveBytes pdb = new(encryptionKey, encryptionKeyBytes, 4, HashAlgorithmName.SHA512);
					encryptor.Key = pdb.GetBytes(32);
					encryptor.IV = pdb.GetBytes(16);
					using (MemoryStream ms = new())
					{
						using (CryptoStream cs = new(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
						{
							cs.Write(cipherBytes, 0, cipherBytes.Length);
							cs.Close();
						}
						cipherText = Encoding.Unicode.GetString(ms.ToArray());
					}
				}
			}
			catch
			{
				return string.Empty;
			}
			return cipherText;
		}
	}
}
