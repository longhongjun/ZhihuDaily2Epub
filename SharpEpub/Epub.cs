using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace SharpEpub
{
	public class Epub : EpubBase
	{
		private string Directory { get; set; }
		private SearchOption directorySearchOption = SearchOption.TopDirectoryOnly;

		public SearchOption DirectorySearchOption
		{
			get { return directorySearchOption; }
			set { directorySearchOption = value; }
		}
		
		public Epub(string epubDirectory, TocOptions option)
			: this(epubDirectory)
		{
			tocOption = option;
		}

		public Epub(string epubDirectory)
		{
			Directory = epubDirectory;
			//ParseFilesByDefault();
		}

		private void ParseFilesByDefault()
		{
			ParseContentFilesByDefault();
			ParseImagesByDefault();
			ParseStylesheets();
		}

		private string[] GetPatterns(string patterns)
		{
			return patterns.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		}

		private void ParseContentFilesByDefault()
		{
			ParseContentFiles(tocOption, GetPatterns(Resources.ContentPatterns));
		}

		private void ParseContentFiles(TocOptions option, params string[] patterns)
		{
			if (patterns == null || patterns.Length == 0) return;
			foreach (string filePath in patterns.SelectMany(pattern => System.IO.Directory.GetFiles(Directory, pattern, directorySearchOption)))
			{
				if (file.ContainsEntry(Structure.Directories.ContentFolder + "/" + Path.GetFileName(filePath))) continue;
				ZipEntry entry = file.AddFile(filePath, Structure.Directories.ContentFolder);
				if (option != TocOptions.ByTitleTag) continue;
				string text = File.ReadAllText(filePath);
				string title = Regex.Match(text, "<title>\\s*.*\\s*</title>").Value;
				title = Regex.Match(title, ">\\s*.*\\s*<").Value.Trim('>', '<', ' ', '\t', '\n');
				entry.Comment = title;
			}
		}

		private void ParseImagesByDefault()
		{
			ParseImages(GetPatterns(Resources.ImagePatterns));
		}
		private void ParseImages(params string[] patterns)
		{
			if(patterns == null || patterns.Length == 0) return;
			foreach (string filePath in patterns.SelectMany(pattern => System.IO.Directory.GetFiles(Directory, pattern, directorySearchOption)))
			{
				if (!file.ContainsEntry(Structure.Directories.ImageFolderFullPath + "/" + Path.GetFileName(filePath)))
					file.AddFile(filePath, Structure.Directories.ImageFolderFullPath);
			}
		}

		private void ParseStylesheets()
		{
			foreach (string filePath in System.IO.Directory.GetFiles(Directory, "*.css", directorySearchOption))
			{
				if (!file.ContainsEntry(Structure.Directories.CssFolderFullPath + "/" + Path.GetFileName(filePath)))
					file.AddFile(filePath, Structure.Directories.CssFolderFullPath);
			}
		}

		protected override void DoMainActions()
		{
			BuildEpubStructure();
			GenerateContainerFile();
			//ParseContentFilesByDefault();
			ParseFilesByDefault();
			GenerateMetadataAndTocFiles();
			isDirty = true;
		}

	}
}
