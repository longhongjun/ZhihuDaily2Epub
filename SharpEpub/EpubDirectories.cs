namespace SharpEpub
{
	public class EpubDirectories
	{
		public string MetaInfFolder { get { return "META-INF"; } }
		public string ContentFolder { get; set; }

		public string ImageFolder { get; set; }
		public string ImageFolderFullPath { get { return EpubHelper.CombinePath(ContentFolder, ImageFolder); } }

		public string CssFolder { get; set; }
		public string CssFolderFullPath { get { return EpubHelper.CombinePath(ContentFolder, CssFolder); } }

		/// <summary>
		/// Initalizes default values 
		/// </summary>
		public EpubDirectories()
		{
			ContentFolder = "OPS";
			ImageFolder = "Images";
			CssFolder = "Css";
		}
	}
}
