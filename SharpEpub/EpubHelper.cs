using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zip;

namespace SharpEpub
{
	public static class EpubHelper
	{
		public static string CombinePath(string path1, string path2)
		{
			return new Uri(string.Format("{0}/{1}", path1.Trim('/', '\\'), path2.Trim('/', '\\')), UriKind.Relative).ToString();
		}
		
		public static string GetRelativePath(string contentFolderPath, string filename)
		{
			if(filename.StartsWith(contentFolderPath, StringComparison.CurrentCultureIgnoreCase))
			{
				filename = filename.Substring(contentFolderPath.Length + 1);
			}
			return filename;
		}
		
		public static IEnumerable<ZipEntry> GetContentFiles(this ZipFile file, string contentDirectory)
		{
			string[] contentPatterns = Resources.ContentPatterns.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
			return contentPatterns.SelectMany(pattern=>file.SelectEntries(pattern, contentDirectory));
		}

		public static IEnumerable<ZipEntry> GetImageFiles(this ZipFile file, string imageDirectory)
		{
			string[] imagePatterns = Resources.ImagePatterns.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			return imagePatterns.SelectMany(pattern => file.SelectEntries(pattern, imageDirectory));
		}

		public static string GetImageMediaType(string filename)
		{
			string ext = Path.GetExtension(filename).TrimStart('.');
			return string.Concat("image/", string.Equals(ext, "jpg", StringComparison.CurrentCultureIgnoreCase) ? "jpeg" : ext);
		}
	}
}
