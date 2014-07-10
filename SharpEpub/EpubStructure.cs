namespace SharpEpub
{
	public class EpubStructure
	{
		private EpubFiles files;

		public EpubDirectories Directories { get; private set; }
		public EpubFiles Files
		{
			get
			{
				files.UpdateBaseDirectories(Directories);
				return files;
			}
			private set { files = value; }
		}

		/// <summary>
		/// Initalizes default settings for Epub file
		/// </summary>
		public EpubStructure()
		{
			Directories = new EpubDirectories();
			Files = new EpubFiles(Directories);
		}
	}
}
