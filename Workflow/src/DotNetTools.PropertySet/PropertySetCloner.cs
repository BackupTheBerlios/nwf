using System;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet
{
	/// <summary> The PropertySetCloner is used to copy all the properties from one PropertySet into another.
	/// *
	/// <h3>Example</h3>
	/// *
	/// <blockquote><code>
	/// EJBPropertySet source = new EJBPropertySet("ejb/PropertyStore","MyEJB",7);<br>
	/// XMLPropertySet dest   = new XMLPropertySet();<br>
	/// <br>
	/// PropertySetCloner cloner = new PropertySetCloner();<br>
	/// cloner.setSource( source );<br>
	/// cloner.setDestination( dest );<br>
	/// <br>
	/// cloner.cloneProperties();<br>
	/// dest.save( new FileWriter("propertyset-MyEJB-7.xml") );<br>
	/// </code></blockquote>
	/// *
	/// <p>The above example demonstrates how a PropertySetCloner can be used to export properties
	/// stores in an EJBPropertySet to an XML file.</p>
	/// *
	/// <p>If the destination PropertySet contains any properties, they will be cleared before
	/// the source properties are copied across.</p>
	/// *
	/// </summary>
	/// <author>  <a href="mailto:joe@truemesh.com">Joe Walnes</a>
	/// </author>
	/// <author>jjx (.net)</author>

	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	public class PropertySetCloner
	{
		public virtual IPropertySet Destination
		{
			get { return destination; }
			//~ Methods ////////////////////////////////////////////////////////////////


			set { this.destination = value; }

		}

		public virtual IPropertySet Source
		{
			get { return source; }

			set { this.source = value; }

		}

		//~ Instance fields ////////////////////////////////////////////////////////

		private IPropertySet destination;
		private IPropertySet source;


		public virtual void cloneProperties()
		{
			clearDestination();

			foreach(String key in source.Keys)
			{
				cloneProperty(key);
			}
			
		}

		/// <summary> Clear all properties that already exist in destination PropertySet.
		/// </summary>
		private void clearDestination()
		{
			foreach(String key in destination.Keys)
			{
				destination.Remove(key);
			}
		}

		/// <summary> Copy individual property from source to destination.
		/// </summary>
		private void cloneProperty(String key)
		{
			switch (source.GetType(key))
			{
				case PropertySet_Fields.BOOLEAN:
					destination.SetBoolean(key, source.GetBoolean(key));

					break;


				case PropertySet_Fields.INT:
					destination.SetInt(key, source.GetInt(key));

					break;


				case PropertySet_Fields.LONG:
					destination.SetLong(key, source.GetLong(key));

					break;


				case PropertySet_Fields.DOUBLE:
					destination.SetDouble(key, source.GetDouble(key));

					break;


				case PropertySet_Fields.STRING:
					destination.SetString(key, source.GetString(key));

					break;


				case PropertySet_Fields.TEXT:
					destination.SetText(key, source.GetText(key));

					break;


				case PropertySet_Fields.DATE:
					destination.SetDate(key, source.GetDate(key));

					break;


				case PropertySet_Fields.OBJECT:
					destination.SetObject(key, source.GetObject(key));

					break;


				case PropertySet_Fields.XML:
					destination.SetXML(key, source.GetXML(key));

					break;


				case PropertySet_Fields.DATA:
					destination.SetData(key, source.GetData(key));

					break;


				case PropertySet_Fields.PROPERTIES:
					destination.SetProperties(key, source.GetProperties(key));

					break;
			}
		}
	}
}