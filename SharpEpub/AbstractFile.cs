using System;
using System.Collections.Generic;
using System.Xml;
using Ionic.Zip;

namespace SharpEpub
{
	public abstract class AbstractFile
	{
		protected string bookGuid = Guid.NewGuid().ToString();
		protected List<string> contentItems;
		protected XmlDocument xmlDocument;
		protected EpubStructure structure;
		protected ZipFile file;
		
		public string GetBookId { get { return bookGuid; } }

		public void UpdatStructure(EpubStructure structure, ZipFile file)
		{
			this.file = file;
			this.structure = structure;
		}

		protected AbstractFile(EpubStructure structure, ZipFile file)
		{
			UpdatStructure(structure, file);
		}

		protected abstract void Build();

		public string BuildAndGetContent()
		{
			Build();
			return xmlDocument.OuterXml;
		}
		public XmlDocument BuildAndGetXmlDocument()
		{
			Build();
			return xmlDocument;
		}
	}
}
