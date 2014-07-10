using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Ionic.Zip;
using SharpEpub.XmlBuilder;
using XmlElement = SharpEpub.XmlBuilder.XmlElement;

namespace SharpEpub
{
	public class EpubMetadata : AbstractFile
	{
		#region Public members

		public string Title { get; set; }
		public string Creator { get; set; }
		public string Publisher { get; set; }
		public string date;
		public string Date
		{
			get { return date; }
			set
			{
				if(string.IsNullOrEmpty(value)) return;
				DateTime dateTime;
				if(DateTime.TryParse(value, out dateTime))
				{
					date = dateTime.ToString("yyyy-MM-dd");
				}
			}
		}
		public string Subject { get; set; }
		public string Source { get; set; }
		public string Rights { get; set; }
		public string Language { get; set; }
		public string Description { get; set; } 

		#endregion

		#region Constructors

		public EpubMetadata(EpubStructure structure, ZipFile file, string bookId) : base(structure, file)
		{
			bookGuid = bookId;
		}
		
		#endregion

		#region Overrides of AbstractFile

		protected override void Build()
		{
			contentItems = new List<string>();
			xmlDocument = new XmlDocument();
			XmlBuilder.XmlBuilder document = xmlDocument.CreateXmlBuilder("package", new Dictionary<string, string>
			                                                          	{
			                                                          		{"xmlns", Resources.OpfPackageNs},
			                                                          		{"version", "2.0"},
			                                                          		{"unique-identifier", Resources.OpfIdentifierField}
			                                                          	});
			document.AppendElement(BuildMetadataSection());
			document.AppendElement(BuildManifestSection());
			document.AppendElement(BuildSpineSection());
			xmlDocument = document.GetDocument();
		}

		#endregion
		
		#region HelperFunctions

		private XmlElement BuildMetadataSection()
		{
			XmlElement metadataSection = xmlDocument.CreateXmlElement("metadata", new Dictionary<string, string>
			                                                                  	{
			                                                                  		{"xmlns:opf", Resources.MetadataXmlnsOpf},
			                                                                  		{"xmlns:dc", Resources.MetadataXmlnsDc}
			                                                                  	});
			metadataSection.NamespaceUri = Resources.MetadataXmlnsDc;
			metadataSection.AttributesNamespaceUri = Resources.MetadataXmlnsOpf;
			metadataSection.AppendTextElement("dc:title", Title);
			metadataSection.AppendTextElement("dc:creator", Creator, new Dictionary<string, string>
			                                                         	{
			                                                         		{"opf:role", Resources.OpfRole},
			                                                         		{"opf:file-as", Creator}
			                                                         	});
			metadataSection.AppendTextElement("dc:publisher", Publisher);
			metadataSection.AppendTextElement("dc:date", Date, new Dictionary<string, string>
			                                                   	{
			                                                   		{"opf:event", Resources.OpfEvent}
			                                                   	});
			metadataSection.AppendTextElement("dc:subject", Subject);
			metadataSection.AppendTextElement("dc:source", Source);
			metadataSection.AppendTextElement("dc:rights", Rights);
			metadataSection.AttributesNamespaceUri = null;
			metadataSection.AppendTextElement("dc:identifier", string.Concat(Resources.IdentifierPrefix, bookGuid),
											  new Dictionary<string, string>
			                                  	{
			                                  		{"id", Resources.OpfIdentifierField}
			                                  	});
			metadataSection.AppendTextElement("dc:language", Language);
			metadataSection.AppendTextElement("dc:description", Description);
			return metadataSection;
		}
		private XmlElement BuildManifestSection()
		{
			XmlElement manifestSection = xmlDocument.CreateXmlElement("manifest");

			//adding ncx file link
			Dictionary<string, string> attributes = new Dictionary<string, string>();
			attributes["id"] = "ncx";
			attributes["href"] = structure.Files.NcxFile;
			attributes["media-type"] = Resources.NcxMediaType;
			manifestSection.AppendElement("item", attributes);

			//adding content files links
			attributes["media-type"] = Resources.ContentMediaType;
			foreach (ZipEntry entry in file.GetContentFiles(structure.Directories.ContentFolder))
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(entry.FileName);
				contentItems.Add(fileNameWithoutExtension);
				attributes["id"] = "cnt_" + fileNameWithoutExtension;
				attributes["href"] = EpubHelper.GetRelativePath(structure.Directories.ContentFolder, entry.FileName);
				manifestSection.AppendElement("item", attributes);
			}

			//adding css files links
			attributes["media-type"] = Resources.CssMediaType;
			foreach (ZipEntry entry in file.SelectEntries("*.css", structure.Directories.CssFolderFullPath))
			{
				attributes["id"] = "style_" + Path.GetFileNameWithoutExtension(entry.FileName);
				attributes["href"] = EpubHelper.GetRelativePath(structure.Directories.ContentFolder, entry.FileName);
				manifestSection.AppendElement("item", attributes);
			}

			//adding images files links
			foreach (ZipEntry entry in file.GetImageFiles(structure.Directories.ImageFolderFullPath))
			{
				attributes["id"] = "img_" + Path.GetFileNameWithoutExtension(entry.FileName);
				attributes["href"] = EpubHelper.GetRelativePath(structure.Directories.ContentFolder, entry.FileName);
				attributes["media-type"] = EpubHelper.GetImageMediaType(entry.FileName);
				manifestSection.AppendElement("item", attributes);
			}
			return manifestSection;
		}

		private XmlElement BuildSpineSection()
		{
			Dictionary<string, string> attributes = new Dictionary<string, string>();
			attributes["toc"] = "ncx";
			XmlElement spineSection = xmlDocument.CreateXmlElement("spine", attributes);
			attributes.Clear();
			if (contentItems.Count > 0)
			{
				attributes["linear"] = "yes";
				foreach (string item in contentItems)
				{
					attributes["idref"] = "cnt_"+item;
					spineSection.AppendElement("itemref", attributes);
				}
			}
			return spineSection;
		} 

		#endregion
	}
}
