using System;
using System.Collections.Generic;
using System.Xml;

namespace SharpEpub.XmlBuilder
{
	public sealed class XmlElement : XmlBase<XmlElement>
	{
		#region Constructors

		public XmlElement(string elementName, ICollection<KeyValuePair<string, string>> attributes, XmlDocument xmlDocumentInstance)
			: base(xmlDocumentInstance)
		{
			if (string.IsNullOrEmpty(elementName)) return;
			element = document.CreateElement(elementName);
			AppendAttributes(attributes, ref element);
		}

		public XmlElement(string elementName, XmlDocument xmlDocumentInstance)
			: base(xmlDocumentInstance)
		{
			if (string.IsNullOrEmpty(elementName)) return;
			element = document.CreateElement(elementName);
		} 

		#endregion
		
		#region Overrides of XmlBase<XmlElement>

		public override XmlElement AppendTextElement(string elementName, string elementValue, KeyValuePair<string, string> attribute)
		{
			AppendXmlNode(elementName, elementValue, attribute);
			return this;
		}

		public override XmlElement AppendTextElement(string elementName, string elementValue, ICollection<KeyValuePair<string, string>> attributes)
		{
			AppendXmlNode(elementName, elementValue, attributes);
			return this;
		}

		public override XmlElement AppendElement(string elementName, KeyValuePair<string, string> attribute)
		{
			AppendXmlNode(elementName, null, attribute);
			return this;
		}

		public override XmlElement AppendElement(string elementName, ICollection<KeyValuePair<string, string>> attributes)
		{
			AppendXmlNode(elementName, null, attributes);
			return this;
		}

		public override XmlElement AppendElement(XmlElement xmlElement)
		{
			if (xmlElement != null)
			{
				element.AppendChild(xmlElement.GetElement());
			}
			return this;
		}

		#endregion
	}
}
