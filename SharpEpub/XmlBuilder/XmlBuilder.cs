using System;
using System.Collections.Generic;
using System.Xml;

namespace SharpEpub.XmlBuilder
{
	
	/// <summary>
	/// Xml builder
	/// </summary>
	public sealed class XmlBuilder : XmlBase<XmlBuilder>
	{
		#region Constructors

		public XmlBuilder(string rootElement, XmlDocument xmlDocumentInstance)
			: base(xmlDocumentInstance)
		{
			BuildXmlHead(null, null, rootElement);
		}

		public XmlBuilder(string rootElement, ICollection<KeyValuePair<string, string>> attributes, XmlDocument xmlDocumentInstance)
			: base(xmlDocumentInstance)
		{
			BuildXmlHead(null, attributes, rootElement);
		}
		
		#endregion

		private void BuildXmlHead(XmlNode documentType, ICollection<KeyValuePair<string, string>> attributes, string rootElement)
		{
			document.AppendChild(document.CreateXmlDeclaration("1.0", "UTF-8", null));
			if (documentType != null) document.AppendChild(documentType);
			XmlNode node;
			if(attributes == null || attributes.Count == 0)
			{
				node = document.CreateElement(rootElement);
			}else
			{
				node = document.CreateElement(rootElement);
				AppendAttributes(attributes, ref node);
				
			}
			element = document.AppendChild(node);
		}

		#region Overrides of XmlBase<XmlElement>

		public override XmlBuilder AppendTextElement(string elementName, string elementValue, KeyValuePair<string, string> attribute)
		{
			AppendXmlNode(elementName, elementValue, attribute);
			return this;
		}

		public override XmlBuilder AppendTextElement(string elementName, string elementValue, ICollection<KeyValuePair<string, string>> attributes)
		{
			AppendXmlNode(elementName, elementValue, attributes);
			return this;
		}

		public override XmlBuilder AppendElement(string elementName, KeyValuePair<string, string> attribute)
		{
			AppendXmlNode(elementName, null, attribute);
			return this;
		}

		public override XmlBuilder AppendElement(XmlElement xmlElement)
		{
			if (xmlElement == null) return this;
			element.AppendChild(xmlElement.GetElement());
			return this;
		}
		public override XmlBuilder AppendElement(string elementName, ICollection<KeyValuePair<string, string>> attributes)
		{
			AppendXmlNode(elementName, null, attributes);
			return this;
		}
		#endregion
	}
}
