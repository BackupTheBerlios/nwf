using System;
using System.Globalization;
using System.IO;
using System.Xml;
using DotNetTools.PropertySet.Memory;
using DotNetTools.Util;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet.Xml
{
	/* ====================================================================
	* The OpenSymphony Software License, Version 1.1
	*
	* (this license is derived and fully compatible with the Apache Software
	* License - see http://www.apache.org/LICENSE.txt)
	*
	* Copyright (c) 2001 The OpenSymphony Group. All rights reserved.
	*
	* Redistribution and use in source and binary forms, with or without
	* modification, are permitted provided that the following conditions
	* are met:
	*
	* 1. Redistributions of source code must retain the above copyright
	*    notice, this list of conditions and the following disclaimer.
	*
	* 2. Redistributions in binary form must reproduce the above copyright
	*    notice, this list of conditions and the following disclaimer in
	*    the documentation and/or other materials provided with the
	*    distribution.
	*
	* 3. The end-user documentation included with the redistribution,
	*    if any, must include the following acknowledgment:
	*       "This product includes software developed by the
	*        OpenSymphony Group (http://www.opensymphony.com/)."
	*    Alternately, this acknowledgment may appear in the software itself,
	*    if and wherever such third-party acknowledgments normally appear.
	*
	* 4. The names "OpenSymphony" and "The OpenSymphony Group"
	*    must not be used to endorse or promote products derived from this
	*    software without prior written permission. For written
	*    permission, please contact license@opensymphony.com .
	*
	* 5. Products derived from this software may not be called "OpenSymphony"
	*    or "OSCore", nor may "OpenSymphony" or "OSCore" appear in their
	*    name, without prior written permission of the OpenSymphony Group.
	*
	* THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED
	* WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
	* OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	* DISCLAIMED.  IN NO EVENT SHALL THE APACHE SOFTWARE FOUNDATION OR
	* ITS CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
	* SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
	* LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF
	* USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
	* OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
	* OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
	* SUCH DAMAGE.
	* ====================================================================
	*/

	/// <summary> The XMLPropertySet behaves as an in-memory typed PropertySet, with the ability to
	/// load and save all the properties to/from an XML document.
	/// *
	/// <ul>
	/// <li>boolean, int, long, double and String properties are saved as simple Text nodes.</li>
	/// <li>text and XML properties are stored as CDATA blocks.</li>
	/// <li>java.util.Date properties are stored in <code>yyyy-mm-dd HH:mm:ss</code> format.</li>
	/// <li>java.util.Properties properties are stored in child elements.</li>
	/// <li>java.lang.Object and byte[] data properties are encoded using base64 into text and stored as CDATA blocks.</li>
	/// </ul>
	/// *
	/// <h3>Example:</h3>
	/// <blockquote><code>
	/// XMLPropertySet p = new XMLPropertySet(); // create blank property-set<br>
	/// p.load( new FileReader("my-properties.xml") ); // load properties from XML.<br>
	/// System.out.println( p.getString("name") );<br>
	/// p.setString("name","blah blah");<br>
	/// p.save( new FileWriter("my-properties.xml") ); // save properties back to XML.<br>
	/// </code></blockquote>
	/// *
	/// </summary>
	/// <author>  <a href="mailto:joe@truemesh.com">Joe Walnes</a>
	/// </author>
	/// <author>jjx (.net)</author>
	/// <version>  $Revision: 1.1 $
	/// *
	/// </version>
	/// <seealso cref="">com.opensymphony.module.propertyset.PropertySet
	/// </seealso>
	/// <seealso cref="">com.opensymphony.module.propertyset.memory.SerializablePropertySet
	/// 
	/// </seealso>
	public class XMLPropertySet : SerializablePropertySet
	{
		public XMLPropertySet()
		{
			InitBlock();
		}

		private void InitBlock()
		{
			dateFormat = "yyyy-mm-dd HH:mm:ss";
		}

		//~ Instance fields ////////////////////////////////////////////////////////

		private String dateFormat;

		//~ Methods ////////////////////////////////////////////////////////////////

		/// <summary>
		/// Load properties from XML input.
		/// </summary>
		public virtual void Load(StreamReader in_Renamed)
		{
			LoadFromDocument(XMLUtils.Parse(in_Renamed));
		}

		/// <summary>
		/// Load properties from XML input.
		/// </summary>
		public virtual void Load(Stream stream)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(stream);
			LoadFromDocument(doc);
		}

		/// <summary> 
		/// Load properties from XML document.
		/// </summary>
		public virtual void LoadFromDocument(XmlDocument doc)
		{
			try
			{
				XmlNodeList nodeList = doc.SelectNodes("/property-set/property");

				foreach (XmlNode e in nodeList)
				{
					String key = XMLUtils.GetAttribute(e, "key");
					int type = this.type(XMLUtils.GetAttribute(e, "type"));
					Object value_Renamed = loadValue(e, key, type);

					if (value_Renamed != null)
					{
						setImpl(type, key, value_Renamed);
					}
				}
			}
			catch (Exception e)
			{
				throw new PropertyImplementationException(e.Message, e);
			}
		}

		/// <summary>
		///  Save properties to XML output.
		/// </summary>
		public virtual void Save(StreamWriter writer)
		{
			SaveToDocument().Save(writer);
		
		}

		/// <summary>
		///  Save properties to XML output.
		/// </summary>
		public virtual void Save(Stream stream)
		{
			SaveToDocument().Save(stream);
		}

		/// <summary>
		///  Save properties to XML Document.
		/// </summary>
		public virtual XmlDocument SaveToDocument()
		{
			XmlDocument doc = new XmlDocument();
			
			doc.AppendChild(doc.CreateXmlDeclaration("1.0","UTF-8",""));

			XmlNode root = doc.CreateElement("proeprty-set");
			doc.AppendChild(root);

			foreach (String key in Keys)
			{
				int type = GetType(key);
				Object value_Renamed = get(type, key);
				saveValue(doc, key, type, value_Renamed);
			}

			return doc;
		}

		/// <summary>
		///  Load value from <property>...</property> tag. Null returned if value cannot be determined.
		/// </summary>
		private Object loadValue(XmlNode e, String key, int type)
		{
			String text = XMLUtils.GetText(e);

			switch (type)
			{
				case PropertySet_Fields.BOOLEAN:
					return TextUtils.ParseBoolean(text);

				case PropertySet_Fields.INT:
					return TextUtils.ParseInt(text);


				case PropertySet_Fields.LONG:
					return  TextUtils.ParseLong(text);


				case PropertySet_Fields.DOUBLE:
					return TextUtils.ParseDouble(text);


				case PropertySet_Fields.STRING:
				case PropertySet_Fields.TEXT:
					return text;


				case PropertySet_Fields.DATE:

					try
					{
						return DateTime.Parse(text, new DateTimeFormatInfo());
					}
					catch (FormatException )
					{
						return null; // if the date cannot be parsed, ignore it.
					}
					//return null;
				case PropertySet_Fields.OBJECT:

					try
					{
						return TextUtils.decodeObject(text);
					}
					catch (Exception )
					{
						return null; // if Object cannot be decoded, ignore it.
					}
				


				case PropertySet_Fields.XML:

					try
					{
						XmlDocument doc=new XmlDocument();
							doc.LoadXml(text);
						return doc;
						
					}
					catch (Exception )
					{
						return null; // if XML cannot be parsed, ignore it.
					}
				


				case PropertySet_Fields.DATA:

					try
					{
						return new Data(TextUtils.DecodeBytes(text));
					}
					catch (IOException )
					{
						return null; // if data cannot be decoded, ignore it.
					}
					//return null;


				case PropertySet_Fields.PROPERTIES:

					try
					{
						
						Properties props = new Properties();
						XmlNodeList pElements = e.SelectNodes( "properties/property");

						foreach(XmlNode pElement in pElements)
						{
							
							props.Add(XMLUtils.GetAttribute(pElement,"key"), XMLUtils.GetElementText(pElement));
						}

						return props;
					}
					catch (Exception )
					{
						return null; // could not get nodes via x-path
					}
					


				default:
					return null;

			}
		}

		/// <summary>
		///  Append <property key="..." type="...">...</property> tag to document.
		/// </summary>
		private void saveValue(XmlDocument doc, String key, int type, Object o)
		{
			XmlNode element = doc.CreateElement("property");

			element.Attributes.SetNamedItem(XMLUtils.CreateAttribute(doc,"key",key));
			element.Attributes.SetNamedItem(XMLUtils.CreateAttribute(doc,"type",this.type(type)));

			
			XmlNode valueNode;

			switch (type)
			{
				case PropertySet_Fields.BOOLEAN:
				case PropertySet_Fields.INT:
				case PropertySet_Fields.LONG:
				case PropertySet_Fields.DOUBLE:
				case PropertySet_Fields.STRING:

					valueNode = doc.CreateTextNode(o.ToString());


					break;


				case PropertySet_Fields.TEXT:

					valueNode = doc.CreateCDataSection(o.ToString());

					break;


				case PropertySet_Fields.DATE:
					valueNode = doc.CreateTextNode( Convert.ToDateTime(o).ToString(dateFormat,new DateTimeFormatInfo()));

					
					break;


				case PropertySet_Fields.OBJECT:

					try
					{
						valueNode = doc.CreateCDataSection(TextUtils.EncodeObject(o));
					}
					catch (IOException )
					{
						return; // cannot save Object - carry on with rest of properties.
					}

					break;


				case PropertySet_Fields.XML:

					try
					{
						valueNode = doc.CreateCDataSection(XMLUtils.Print((XmlDocument) o));
					}
					catch (IOException )
					{
						return; // cannot serialize XML - carry on with rest of properties.
					}

					break;


				case PropertySet_Fields.DATA:

					try
					{
						valueNode = doc.CreateCDataSection(TextUtils.EncodeBytes(((Data) o).Bytes));
					}
					catch (IOException )
					{
						return; // cannot save data - carry on with rest of properties.
					}

					break;


				case PropertySet_Fields.PROPERTIES:
					// scoping block
					valueNode = doc.CreateElement("properties");

					Properties props = (Properties) o;
					foreach (String pKey in props.Keys)
					{
						XmlNode pElement = doc.CreateElement("property");
						pElement.Attributes.Append(XMLUtils.CreateAttribute(doc, "key", pKey));
						pElement.Attributes.Append(XMLUtils.CreateAttribute(doc, "type", "string"));

						pElement.AppendChild(doc.CreateTextNode( props.GetProperty(pKey)));
						valueNode.AppendChild(pElement);
					}
					break;


				default:
					return; // if type not recognised, stop now.

			}

			element.AppendChild(valueNode);

			doc.DocumentElement.AppendChild(element);
		}
	}

	
}