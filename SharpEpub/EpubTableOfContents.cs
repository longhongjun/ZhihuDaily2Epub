using System.Collections.Generic;
using System.IO;
using System.Xml;
using Ionic.Zip;
using SharpEpub.XmlBuilder;
using XmlElement = SharpEpub.XmlBuilder.XmlElement;

namespace SharpEpub
{
	public class EpubTableOfContents : AbstractFile
	{
		public string BookTitle { get; set; }
		private readonly TocOptions option = TocOptions.ByFilename;
		
		public EpubTableOfContents(EpubStructure structure, ZipFile file, TocOptions option, string bookId) : base(structure, file)
		{
			bookGuid = bookId;
			this.option = option;
		}
		public EpubTableOfContents(EpubStructure structure, ZipFile file, TocOptions option) : base(structure, file)
		{
			this.option = option;
		}

		#region Overrides of AbstractFile

		protected override void Build()
		{
			xmlDocument = new XmlDocument();
			XmlBuilder.XmlBuilder document = xmlDocument.CreateXmlBuilder("ncx", new Dictionary<string, string>
			                                                          	{
			                                                          		{"xmlns", Resources.NcxNs},
			                                                          		{"version", Resources.NcxVersion}
			                                                          	});
			document.AppendElement(BuildHeadSection());
			document.AppendElement(BuildDocTitleSection());
			document.AppendElement(BuildNavMapSection());
			xmlDocument = document.GetDocument();
		}

		#endregion
		
		private XmlElement BuildHeadSection()
		{
			XmlElement headSection = xmlDocument.CreateXmlElement("head");
			var items = new List<Dictionary<string, string>>
			                                        	{
			                                        		new Dictionary<string, string>{ {"name", "dtb:depth"}, {"content", "1"}},
															new Dictionary<string, string>{ {"name", "dtb:totalPageCount"}, {"content", "1"}},
															new Dictionary<string, string>{ {"name", "dtb:uid"}, {"content", GetBookId}},
															new Dictionary<string, string>{ {"name", "dtb:maxPageNumber"}, {"content", "0"}}
			                                        	};
			foreach (var valuePair in items)
			{
				headSection.AppendElement("meta", valuePair);
			}
			return headSection;
		}

		private XmlElement BuildDocTitleSection()
		{
			XmlElement docTitleSection = xmlDocument.CreateXmlElement("docTitle");
			docTitleSection.AppendTextElement("text", BookTitle);
			return docTitleSection;
		}

		private XmlElement BuildNavMapSection()
		{
			XmlElement navMapSection = xmlDocument.CreateXmlElement("navMap");
			Dictionary<string ,string> attributes = new Dictionary<string, string>();
			int id = 0;
			foreach (ZipEntry entry in file.GetContentFiles(structure.Directories.ContentFolder))
			{
				id++;
				attributes["playOrder"] = id.ToString();
				attributes["id"] = "id" + id;
				navMapSection
					.AppendElement(xmlDocument.CreateXmlElement("navPoint", attributes)
					               	.AppendElement(xmlDocument.CreateXmlElement("navLabel")
					               	               	.AppendTextElement("text",
					               	               	                   option == TocOptions.ByTitleTag
					               	               	                   	? entry.Comment
					               	               	                   	: Path.GetFileNameWithoutExtension(entry.FileName)))
					               	.AppendElement("content", new KeyValuePair<string, string>("src", Path.GetFileName(entry.FileName))));
			}
			return navMapSection;
		}

	}
}
