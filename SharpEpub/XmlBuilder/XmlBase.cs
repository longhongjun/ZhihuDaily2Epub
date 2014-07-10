using System;
using System.Collections.Generic;
using System.Xml;

namespace SharpEpub.XmlBuilder
{
	/// <summary>
	/// Extension for XmlDocument. Allows build any XmlStructure very easy. Provides fluent interface.
	/// </summary>
	public static class XmlExtender
	{
		public static XmlBuilder CreateXmlBuilder(this XmlDocument document, string rootElement)
		{
			return new XmlBuilder(rootElement, document);
		}

		public static XmlBuilder CreateXmlBuilder(this XmlDocument document, string rootElement, ICollection<KeyValuePair<string,string>> attributes)
		{
			return new XmlBuilder(rootElement, attributes, document);
		}

		public static XmlElement CreateXmlElement(this XmlDocument document, string elementName, ICollection<KeyValuePair<string,string>> attributes)
		{
			return new XmlElement(elementName, attributes, document);
		}

		public static XmlElement CreateXmlElement(this XmlDocument document, string elementName)
		{
			return new XmlElement(elementName, document);
		}
	}


	public abstract class XmlBase<T> where T:class
	{
		protected XmlDocument document;
		protected XmlNode element;
		public string NamespaceUri { get; set; }
		public string AttributesNamespaceUri { get; set; }
		private bool isDirty = false;
		
		protected XmlBase(XmlDocument document)
		{
			this.document = document;
		}

		#region Abstract methods - for fluent support

		public abstract T AppendTextElement(string elementName, string elementValue, KeyValuePair<string, string> attribute);
		public abstract T AppendTextElement(string elementName, string elementValue, ICollection<KeyValuePair<string, string>> attributes);
		public abstract T AppendElement(string elementName, KeyValuePair<string, string> attribute);
		public abstract T AppendElement(string elementName, ICollection<KeyValuePair<string, string>> attributes);

		public abstract T AppendElement(XmlElement xmlElement);

		#endregion

		public T AppendElement(string elementName)
		{
			return AppendElement(elementName, null);
		}

		public T AppendTextElement(string elementName, string elementValue)
		{
			return AppendTextElement(elementName, elementValue, null);
		}

		protected void AppendAttributes(ICollection<KeyValuePair<string, string>> attributes, ref XmlNode childElement)
		{
			if (attributes == null || attributes.Count == 0 || childElement.Attributes == null) return;
			foreach (KeyValuePair<string, string> attribute in attributes)
			{
				childElement = AppendAttributeToNode(attribute, childElement);
			}
		}

		protected void AppendXmlNode(string elementName, string elementValue, KeyValuePair<string, string> attribute)
		{
			if (string.IsNullOrEmpty(elementName)) return;
			XmlNode node = document.CreateElement(elementName, NamespaceUri);
			node = AppendAttributeToNode(attribute, node);
			if (!string.IsNullOrEmpty(elementValue)) node.InnerText = elementValue;
			element.AppendChild(node);
		}

		protected void AppendXmlNode(string elementName, string elementValue, ICollection<KeyValuePair<string, string>> attributes)
		{
			if (string.IsNullOrEmpty(elementName)) return;
			XmlNode node = document.CreateElement(elementName, NamespaceUri);
			AppendAttributes(attributes, ref node);
			if (!string.IsNullOrEmpty(elementValue)) node.InnerText = elementValue;
			element.AppendChild(node);
		}

		private XmlNode AppendAttributeToNode(KeyValuePair<string, string> attribute, XmlNode childElement)
		{
			if (childElement.Attributes[attribute.Key] != null)
			{
				childElement.Attributes[attribute.Key].Value = attribute.Value;
			}
			else
			{
				XmlAttribute xmlAttribute = string.IsNullOrEmpty(AttributesNamespaceUri)
				                            	? document.CreateAttribute(attribute.Key)
				                            	: document.CreateAttribute(attribute.Key, AttributesNamespaceUri);
				xmlAttribute.Value = attribute.Value;				
				childElement.Attributes.Append(xmlAttribute);
			}
			return childElement;
		}

		public void SetDocument(XmlDocument document)
		{
			this.document = document;
		}
		public XmlNode GetElement()
		{
			return element;
		}
		public XmlDocument GetDocument()
		{
			if(!isDirty)
			{
				document.AppendChild(element);
				isDirty = true;
			}
			return document;
		}
	}
}
