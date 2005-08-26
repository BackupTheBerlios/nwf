using System;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Xml;
using DotNetTools.Util;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet
{
	/// <summary> Base implementation of PropertySet.
	/// *
	/// <p>Performs necessary casting for get???/set??? methods which wrap around the
	/// following 2 methods which are declared <code>protected abstract</code> and need
	/// to be implemented by subclasses:</p>
	/// *
	/// <ul>
	/// <li> {@link #get(int,java.lang.String)} </li>
	/// <li> {@link #setImpl(int,java.lang.String,java.lang.Object)} </li>
	/// </ul>
	/// *
	/// <p>The following methods are declared <code>public abstract</code> and are the
	/// remainder of the methods that need to be implemented at the very least:</p>
	/// *
	/// <ul>
	/// <li> {@link #exists(java.lang.String)} </li>
	/// <li> {@link #remove(java.lang.String)} </li>
	/// <li> {@link #getType(java.lang.String)} </li>
	/// <li> {@link #getKeys(java.lang.String,int)} </li>
	/// </ul>
	/// *
	/// <p>The <code>supports???</code> methods are implemented and all return true by default.
	/// Override if necessary.</p>
	/// *
	/// </summary>
	/// <author>  <a href="mailto:joe@truemesh.com">Joe Walnes</a>
	/// </author>
	/// <author>  <a href="mailto:hani@fate.demon.co.uk">Hani Suleiman</a>
	/// </author>
	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	public abstract class AbstractPropertySet : IPropertySet
	{
		public abstract ICollection GetKeys(string prefix, int type);

		public abstract int GetType(String key);

		public abstract bool Exists(String key);

		public abstract void Remove(String key);

		public virtual void Remove()
		{
			throw new NotImplementedException();
		}

		/// <summary> Calls <code>getKeys(null,0)</code>
		/// </summary>
		public virtual ICollection Keys
		{
			get { return GetKeys(null, 0); }

		}

		public virtual PropertySetSchema Schema
		{
			get { return schema; }

			set { this.schema = value; }

		}

		//~ Instance fields ////////////////////////////////////////////////////////

		protected internal PropertySetSchema schema;

		//~ Methods ////////////////////////////////////////////////////////////////

		public virtual void SetAsActualType(String key, Object o)
		{
			int type;

			if (o is Boolean)
			{
				type = PropertySet_Fields.BOOLEAN;
			}
			else if (o is Int32)
			{
				type = PropertySet_Fields.INT;
			}
			else if (o is Int64)
			{
				type = PropertySet_Fields.LONG;
			}
			else if (o is Double)
			{
				type = PropertySet_Fields.DOUBLE;
			}
			else if (o is String)
			{
				if (o.ToString().Length > 255)
				{
					type = PropertySet_Fields.TEXT;
				}
				else
				{
					type = PropertySet_Fields.STRING;
				}
			}
			else if (o is DateTime)
			{
				type = PropertySet_Fields.DATE;
			}
			else if (o is XmlDocument)
			{
				type = PropertySet_Fields.XML;
			}
			else if (o is byte[])
			{
				type = PropertySet_Fields.DATA;
			}
			else if (o is AppSettingsReader)
			{
				type = PropertySet_Fields.PROPERTIES;
			}
			else
			{
				type = PropertySet_Fields.OBJECT;
			}

			set(type, key, o);
		}

		public virtual Object GetAsActualType(String key)
		{
			int type = GetType(key);
			Object value_Renamed = null;

			switch (type)
			{
				case PropertySet_Fields.BOOLEAN:
					value_Renamed = GetBoolean(key);

					break;


				case PropertySet_Fields.INT:
					value_Renamed = GetInt(key);

					break;


				case PropertySet_Fields.LONG:
					value_Renamed = GetLong(key);

					break;


				case PropertySet_Fields.DOUBLE:
					value_Renamed = GetDouble(key);

					break;


				case PropertySet_Fields.STRING:
					value_Renamed = GetString(key);

					break;


				case PropertySet_Fields.TEXT:
					value_Renamed = GetText(key);

					break;


				case PropertySet_Fields.DATE:
					value_Renamed = GetDate(key);

					break;


				case PropertySet_Fields.XML:
					value_Renamed = GetXML(key);

					break;


				case PropertySet_Fields.DATA:
					value_Renamed = GetData(key);

					break;


				case PropertySet_Fields.PROPERTIES:
					value_Renamed = GetProperties(key);

					break;


				case PropertySet_Fields.OBJECT:
					value_Renamed = GetObject(key);

					break;
			}

			return value_Renamed;
		}

		public virtual void SetBoolean(String key, bool value_Renamed)
		{
			set(PropertySet_Fields.BOOLEAN, key, value_Renamed ? true : false);
		}

		public virtual bool GetBoolean(String key)
		{
			try
			{
				return ((Boolean) get(PropertySet_Fields.BOOLEAN, key));
			}
			catch (NullReferenceException)
			{
				return false;
			}
		}

		/// <summary> Constructs {@link com.opensymphony.util.Data} wrapper around bytes.
		/// </summary>
		public virtual void SetData(string key, byte[] value_Renamed)
		{
			set(PropertySet_Fields.DATA, key, new Data(value_Renamed));
		}

		/// <summary> Casts to {@link com.opensymphony.util.Data} and returns bytes.
		/// </summary>
		public virtual byte[] GetData(string key)
		{
			try
			{
				Object data = get(PropertySet_Fields.DATA, key);

				if (data is Data)
				{
					return ((Data) data).Bytes;
				}
				else if (data is byte[])
				{
					return (byte[]) data;
				}
				
			}
			catch (NullReferenceException)
			{
				return null;
			}

			return null;
		}

		public virtual void SetDate(String key, DateTime value_Renamed)
		{
			set(PropertySet_Fields.DATE, key, value_Renamed);
		}

		public virtual DateTime GetDate(String key)
		{
			try
			{
				return (DateTime) get(PropertySet_Fields.DATE, key);
			}
			catch (NullReferenceException)
			{
				return DateTime.MinValue;
			}
		}

		public virtual void SetDouble(String key, double value_Renamed)
		{
			set(PropertySet_Fields.DOUBLE, key, value_Renamed);
		}

		public virtual double GetDouble(String key)
		{
			try
			{
				return ((Double) get(PropertySet_Fields.DOUBLE, key));
			}
			catch (NullReferenceException)
			{
				return 0.0;
			}
		}

		public virtual void SetInt(String key, int value_Renamed)
		{
			set(PropertySet_Fields.INT, key, value_Renamed);
		}

		public virtual int GetInt(String key)
		{
			try
			{
				return ((Int32) get(PropertySet_Fields.INT, key));
			}
			catch (NullReferenceException)
			{
				return 0;
			}
		}


		/// <summary> Calls <code>getKeys(null,type)</code>
		/// </summary>
		public virtual ICollection GetKeys(int type)
		{
			return GetKeys(null, type);
		}

		/// <summary> Calls <code>getKeys(prefix,0)</code>
		/// </summary>
		public virtual ICollection GetKeys(string prefix)
		{
			return GetKeys(prefix, 0);
		}

		public virtual void SetLong(String key, long value_Renamed)
		{
			set(PropertySet_Fields.LONG, key, value_Renamed);
		}

		public virtual long GetLong(String key)
		{
			try
			{
				return ((long) get(PropertySet_Fields.LONG, key));
			}
			catch (NullReferenceException)
			{
				return 0L;
			}
		}

		public virtual void SetObject(String key, Object value_Renamed)
		{
			set(PropertySet_Fields.OBJECT, key, value_Renamed);
		}

		public virtual Object GetObject(String key)
		{
			try
			{
				return get(PropertySet_Fields.OBJECT, key);
			}
			catch (NullReferenceException)
			{
				return null;
			}
		}

		public virtual void SetProperties(string key, Properties value_Renamed)
		{
			set(PropertySet_Fields.PROPERTIES, key, value_Renamed);
		}

		public virtual Properties GetProperties(string key)
		{
			try
			{
				return (Properties) get(PropertySet_Fields.PROPERTIES, key);
			}
			catch (NullReferenceException)
			{
				return null;
			}
		}


		/// <summary> Returns true.
		/// </summary>
		public virtual bool IsSettable(String property)
		{
			return true;
		}

		/// <summary> Throws IllegalPropertyException if value length greater than 255.
		/// </summary>
		public virtual void SetString(String key, String value_Renamed)
		{
			if ((value_Renamed != null) && (value_Renamed.Length > 255))
			{
				throw new IllegalPropertyException("String exceeds 255 characters.");
			}

			set(PropertySet_Fields.STRING, key, value_Renamed);
		}

		public virtual String GetString(String key)
		{
			try
			{
				return (String) get(PropertySet_Fields.STRING, key);
			}
			catch (NullReferenceException)
			{
				return null;
			}
		}

		public virtual void SetText(String key, String value_Renamed)
		{
			set(PropertySet_Fields.TEXT, key, value_Renamed);
		}

		public virtual String GetText(String key)
		{
			try
			{
				return (String) get(PropertySet_Fields.TEXT, key);
			}
			catch (NullReferenceException)
			{
				return null;
			}
		}

		public virtual void SetXML(string key, XmlDocument doc)
		{
			set(PropertySet_Fields.XML, key, doc);
		}

		public virtual XmlDocument GetXML(string key)
		{
			try
			{
				return (XmlDocument) get(PropertySet_Fields.XML, key);
			}
			catch (NullReferenceException)
			{
				return null;
			}
		}

		public virtual void Init(IDictionary config, IDictionary args)
		{
			// nothing
		}

		/// <summary> Returns true.
		/// </summary>
		public virtual bool SupportsType(int type)
		{
			return true;
		}

		/// <summary> Returns true.
		/// </summary>
		public virtual bool SupportsTypes()
		{
			return true;
		}

		/// <summary> Simple human readable representation of contents of PropertySet.
		/// </summary>
		public override String ToString()
		{
			StringBuilder result = new StringBuilder();
			result.Append(GetType().FullName);
			result.Append(" {\n");

			try
			{
				foreach (String key in Keys)
				{
					int type = GetType(key);

					if (type > 0)
					{
						result.Append('\t');
						result.Append(key);
						result.Append(" = ");
						result.Append(get(type, key));
						result.Append('\n');
					}
				}
			}
			catch (PropertyException)
			{
				// toString should never throw an exception.
			}

			result.Append("}\n");

			return result.ToString();
		}

		protected internal abstract void setImpl(int type, String key, Object value_Renamed);

		protected internal abstract Object get(int type, String key);

		protected internal virtual String type(int type)
		{
			switch (type)
			{
				case PropertySet_Fields.BOOLEAN:
					return "boolean";


				case PropertySet_Fields.INT:
					return "int";


				case PropertySet_Fields.LONG:
					return "long";


				case PropertySet_Fields.DOUBLE:
					return "double";


				case PropertySet_Fields.STRING:
					return "string";


				case PropertySet_Fields.TEXT:
					return "text";


				case PropertySet_Fields.DATE:
					return "date";


				case PropertySet_Fields.OBJECT:
					return "object";


				case PropertySet_Fields.XML:
					return "xml";


				case PropertySet_Fields.DATA:
					return "data";


				case PropertySet_Fields.PROPERTIES:
					return "properties";


				default:
					return null;

			}
		}

		protected internal virtual int type(String type)
		{
			if (type == null)
			{
				return 0;
			}

			type = type.ToLower();

			if (type.Equals("boolean"))
			{
				return PropertySet_Fields.BOOLEAN;
			}

			if (type.Equals("int"))
			{
				return PropertySet_Fields.INT;
			}

			if (type.Equals("long"))
			{
				return PropertySet_Fields.LONG;
			}

			if (type.Equals("double"))
			{
				return PropertySet_Fields.DOUBLE;
			}

			if (type.Equals("string"))
			{
				return PropertySet_Fields.STRING;
			}

			if (type.Equals("text"))
			{
				return PropertySet_Fields.TEXT;
			}

			if (type.Equals("date"))
			{
				return PropertySet_Fields.DATE;
			}

			if (type.Equals("object"))
			{
				return PropertySet_Fields.OBJECT;
			}

			if (type.Equals("xml"))
			{
				return PropertySet_Fields.XML;
			}

			if (type.Equals("data"))
			{
				return PropertySet_Fields.DATA;
			}

			if (type.Equals("properties"))
			{
				return PropertySet_Fields.PROPERTIES;
			}

			return 0;
		}

		private void set(int type, String key, Object value_Renamed)
		{
			//If we have a schema, validate data against it.
			if (schema != null)
			{
				PropertySchema ps = schema.getPropertySchema(key);

				//Restricted schemas have to explicitly list all permissible values
				if ((ps == null) && schema.Restricted)
				{
					throw new IllegalPropertyException("Property " + key + " not explicitly specified in restricted schema.");
				}

				//Check the property type matches
				if (SupportsTypes() && (ps.Type != type))
				{
					throw new InvalidPropertyTypeException("Property " + key + " has invalid type " + type + " expected type=" + ps.Type);
				}

				ps.validate(value_Renamed);
			}

			//we're ok this far, so call the actual setter.
			setImpl(type, key, value_Renamed);
		}
	}
}