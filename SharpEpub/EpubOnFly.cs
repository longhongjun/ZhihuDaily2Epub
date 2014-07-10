using System.IO;

namespace SharpEpub
{
	public class EpubOnFly : EpubBase
	{
		public EpubOnFly()
		{
			BuildEpubStructure();
		}

		public void AddContent(string filename, string content)
		{
			string entryName = EpubHelper.CombinePath(Structure.Directories.ContentFolder, filename);
			if(!file.ContainsEntry(entryName)) file.AddEntry(EpubHelper.CombinePath(Structure.Directories.ContentFolder, filename), content);
		}

		public void AddContent(string filename, Stream stream)
		{
			string entryName = EpubHelper.CombinePath(Structure.Directories.ContentFolder, filename);
			if(!file.ContainsEntry(entryName)) file.AddEntry(EpubHelper.CombinePath(Structure.Directories.ContentFolder, filename), stream);
		}

		public void AddImage(string filename, byte[] bytes)
		{
			string entryName = EpubHelper.CombinePath(Structure.Directories.ImageFolderFullPath, filename);
			if(!file.ContainsEntry(entryName)) file.AddEntry(EpubHelper.CombinePath(Structure.Directories.ImageFolderFullPath, filename), bytes);
		}

		public void AddImage(string filename, Stream stream)
		{
			string entryName = EpubHelper.CombinePath(Structure.Directories.ImageFolderFullPath, filename);
			if(!file.ContainsEntry(entryName)) file.AddEntry(EpubHelper.CombinePath(Structure.Directories.ImageFolderFullPath, filename), stream);
		}

		public void AddCss(string filename, string content)
		{
			string entryName = EpubHelper.CombinePath(Structure.Directories.CssFolderFullPath, filename);
			if(!file.ContainsEntry(entryName)) file.AddEntry(EpubHelper.CombinePath(Structure.Directories.CssFolderFullPath, filename), content);
		}

		public void AddCss(string filename, Stream stream)
		{
			string entryName = EpubHelper.CombinePath(Structure.Directories.CssFolderFullPath, filename);
			if(!file.ContainsEntry(entryName)) file.AddEntry(EpubHelper.CombinePath(Structure.Directories.CssFolderFullPath, filename), stream);
		}

		public void AddResource(string path, string filename, string content)
		{
			string entryName = EpubHelper.CombinePath(path, filename);
			if(!file.ContainsEntry(entryName)) file.AddEntry(EpubHelper.CombinePath(path, filename), content);
		}

		public void AddResource(string path, string filename, byte[] bytes)
		{
			string entryName = EpubHelper.CombinePath(path, filename);
			if(!file.ContainsEntry(entryName)) file.AddEntry(EpubHelper.CombinePath(path, filename), bytes);
		}

		public void AddResource(string path, string filename, Stream stream)
		{
			string entryName = EpubHelper.CombinePath(path, filename);
			if(!file.ContainsEntry(entryName)) file.AddEntry(EpubHelper.CombinePath(path, filename), stream);
		}

		#region Overrides of EpubBase

		protected override void DoMainActions()
		{
			GenerateContainerFile();
			GenerateMetadataAndTocFiles();
			isDirty = true;
		}

		#endregion
	}
}
