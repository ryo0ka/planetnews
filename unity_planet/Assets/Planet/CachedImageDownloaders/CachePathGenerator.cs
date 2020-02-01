using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Planet.CachedImageDownloaders
{
	public sealed class CachePathGenerator
	{
		readonly char[] _splitter = {'?'};

		public CachePathGenerator(string subdirName)
		{
			DirPath = Path.Combine(Application.temporaryCachePath, subdirName);
		}

		public string DirPath { get; }

		// https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
		string GenerateUrlHash(string url)
		{
			// Create a SHA256   
			using (var sha256Hash = SHA256.Create())
			{
				// ComputeHash - returns byte array  
				var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(url));

				// Convert byte array to a string   
				var builder = new StringBuilder();
				for (var i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}

				return builder.ToString().Substring(0, 10);
			}
		}

		public string GenerateCachePath(string url)
		{
			var hash = GenerateUrlHash(url);
			var ext = Path.GetExtension(url);

			if (ext.Contains("?"))
			{
				ext = ext.Split(_splitter)[0];
			}

			return Path.Combine(DirPath, $"{hash}{ext}");
		}
	}
}