namespace SharpEpub
{
	public class EpubFiles
	{
		private string containerFolder;
		private string contentFolder;

		public string ContainerFile { get { return "container.xml"; } }
		public string ContainerFileFullPath { get { return EpubHelper.CombinePath(containerFolder, ContainerFile); } }

		public string TitleFile { get; set; }
		public string TitleFileFullPath { get { return EpubHelper.CombinePath(contentFolder, TitleFile); } }

		public string NcxFile { get; set; }
		public string NcxFileFullPath { get { return EpubHelper.CombinePath(contentFolder, NcxFile); } }

		public string OpfFile { get; set; }
		public string OpfFileFullPath { get { return EpubHelper.CombinePath(contentFolder, OpfFile); } }

		/// <summary>
		/// Initalizes default values 
		/// </summary>
		public EpubFiles(EpubDirectories epubDirectories)
		{
			UpdateBaseDirectories(epubDirectories);
			TitleFile = "title.xml";
			NcxFile = "book.ncx";
			OpfFile = "book.opf";
		}

		public void UpdateBaseDirectories(EpubDirectories epubDirectories)
		{
			containerFolder = epubDirectories.MetaInfFolder;
			contentFolder = epubDirectories.ContentFolder;
		}
	}
}
