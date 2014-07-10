using System.Collections.Generic;
using System.IO;
using System.Xml;
using Ionic.Zip;
using SharpEpub.XmlBuilder;

namespace SharpEpub
{
	using Ionic.Zlib;

	public abstract class EpubBase
	{
		public TocOptions tocOption = TocOptions.ByFilename;
		protected ZipFile file;
		public EpubStructure Structure { get; set; }
		private EpubMetadata metadata;
		private EpubTableOfContents tableOfContents;
		protected bool isDirty = false;
		
		public EpubMetadata Metadata
		{
			get
			{
				if(metadata == null) 
					return (metadata = new EpubMetadata(Structure, file, TableOfContents.GetBookId));
				metadata.UpdatStructure(Structure, file);
				return metadata;
			}
			set { metadata = value; }
		}

		public EpubTableOfContents TableOfContents
		{
			get
			{
				if (tableOfContents == null) 
					return (tableOfContents = new EpubTableOfContents(Structure, file, tocOption));
				tableOfContents.UpdatStructure(Structure, file);
				return tableOfContents;
			}
			set { tableOfContents = value; }
		}

		protected EpubBase()
		{
			Structure = new EpubStructure();
			file = new ZipFile();
		}

		public void Clear()
		{
			file.RemoveSelectedEntries("*.*");
		}

		protected void GenerateContainerFile()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlBuilder.XmlBuilder document = xmlDocument.CreateXmlBuilder("container", new Dictionary<string, string>
			                                          	{
			                                          		{"xmlns", Resources.ContainerNs},
			                                          		{"version", "1.0"}
			                                          	});

			document
				.AppendElement(xmlDocument.CreateXmlElement("rootfiles")
				               	.AppendElement(xmlDocument.CreateXmlElement("rootfile",
				               	                                            new Dictionary<string, string>
				               	                                            	{
				               	                                            		{"full-path", Structure.Files.OpfFileFullPath},
				               	                                            		{"media-type", Resources.ContainerMediaType}
				               	                                            	})));
			file.AddEntry(Structure.Files.ContainerFileFullPath, document.GetDocument().OuterXml);
		}

		protected void GenerateMetadataAndTocFiles()
		{
			file.AddEntry(Structure.Files.NcxFileFullPath, TableOfContents.BuildAndGetContent());
			file.AddEntry(Structure.Files.OpfFileFullPath, Metadata.BuildAndGetContent());
		}

		protected void BuildEpubStructure()
		{
			file = new ZipFile();
			file.AddEntry("mimetype", Resources.Mimetype).CompressionLevel = CompressionLevel.None;
			if(!string.IsNullOrEmpty(Structure.Directories.ContentFolder)) file.AddDirectoryByName(Structure.Directories.ContentFolder);
			file.AddDirectoryByName(Structure.Directories.MetaInfFolder);
			if (!string.IsNullOrEmpty(Structure.Directories.ImageFolder)) file.AddDirectoryByName(Structure.Directories.ImageFolderFullPath);
			if (!string.IsNullOrEmpty(Structure.Directories.CssFolder)) file.AddDirectoryByName(Structure.Directories.CssFolderFullPath);
		}

		public void BuildToFile(string filename)
		{
			if (!isDirty) DoMainActions();
			file.Save(filename);
		}

		public Stream BuildToStream()
		{
			if (!isDirty) DoMainActions();
			MemoryStream stream = new MemoryStream();
			file.Save(stream);
			stream.Seek(0, SeekOrigin.Begin);
			return stream;
		}

		public byte[] BuildToBytes()
		{
			if (!isDirty) DoMainActions();
			MemoryStream stream = new MemoryStream();
			file.Save(stream);
			return stream.ToArray();
		}

		protected abstract void DoMainActions();
	}
}
