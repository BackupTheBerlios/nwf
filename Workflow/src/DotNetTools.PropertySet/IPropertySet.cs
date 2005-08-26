using System;
using System.Collections;
using System.Xml;
using DotNetTools.Util;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet
{
	/// <summary> A <code>PropertySet</code> is designed to be associated with other entities
	/// in the system for storing key/value property pairs.
	/// *
	/// <p>A key can only contain one value and a key is unique across all types. If
	/// a property is set using the same key and an already existing property of the
	/// SAME type, the new value will overwrite the old. However, if a property of
	/// DIFFERENT type attempts to overwrite the existing value, a
	/// {@link com.opensymphony.module.propertyset.DuplicatePropertyKeyException}
	/// should be thrown.</p>
	/// *
	/// <p>If a property is set of a type that is not allowed, a
	/// {@link com.opensymphony.module.propertyset.IllegalPropertyException}
	/// should be thrown.</p>
	/// *
	/// <p>If a property is retrieved that exists but contains a value of different
	/// type, a
	/// {@link com.opensymphony.module.propertyset.InvalidPropertyTypeException}
	/// should be thrown.</p>
	/// *
	/// <p>If a property is retrieved that does not exist, null (or the primitive
	/// equivalent) is returned.</p>
	/// *
	/// <p>If an Exception is encountered in the actual implementation of the
	/// PropertySet that needs to be rethrown, it should be wrapped in a
	/// {@link com.opensymphony.module.propertyset.PropertyImplementationException}
	/// .</p>
	/// *
	/// <p>Some PropertySet implementations may not store along side the data the original
	/// type it was set as. This means that it could be retrieved using a get method of
	/// a different type without throwing an InvalidPropertyTypeException (so long as the
	/// original type can be converted to the requested type.</p>
	/// *
	/// <p><b>Typed PropertySet Example</b></p>
	/// *
	/// <p><code>
	/// propertySet.setString("something","99");<br>
	/// x = propertySet.getString("something"); // throws InvalidPropertyTypeException
	/// </code></p>
	/// *
	/// <p><b>Untyped PropertySet Example</b></p>
	/// *
	/// <p><code>
	/// propertySet.setString("something","99");<br>
	/// x = propertySet.getString("something"); // returns 99.
	/// </code></p>
	/// *
	/// <p>Typically (unless otherwise stated), an implementation is typed. This can be
	/// checked by calling the {@link #supportsTypes()} method of the implementation.</p>
	/// *
	/// <p>Not all PropertySet implementations need to support setter methods (i.e.
	/// they are read only) and not all have to support storage/retrieval of specific
	/// types. The capabilities of the specific implementation can be determined by
	/// calling {@link #supportsType(int)} and {@link #isSettable(String)} .</p>
	/// *
	/// </summary>
	/// <author>  <a href="mailto:joe@truemesh.com">Joe Walnes</a>
	/// </author>
	/// <author>jjx (.net)</author>

	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	public struct PropertySet_Fields
	{
		public const int BOOLEAN = 1;
		public const int DATA = 10;
		public const int DATE = 7;
		public const int DOUBLE = 4;
		public const int INT = 2;
		public const int LONG = 3;
		public const int OBJECT = 8;
		public const int PROPERTIES = 11;
		public const int STRING = 5;
		public const int TEXT = 6;
		public const int XML = 9;
	}

	public interface IPropertySet
	{
		//UPGRADE_NOTE: Members of interface 'PropertySet' were extracted into structure 'PropertySet_Fields'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1045"'
		/// <summary>Value-type boolean 
		/// </summary>
		/// <summary>Value-type byte[] 
		/// </summary>
		/// <summary>Value-type {@link java.util.Date} 
		/// </summary>
		/// <summary>Value-type double 
		/// </summary>
		/// <summary>Value-type int 
		/// </summary>
		/// <summary>Value-type long 
		/// </summary>
		/// <summary>Value-type serializable {@link java.lang.Object} 
		/// </summary>
		/// <summary>Value-type {@link java.util.Properties} 
		/// </summary>
		/// <summary>Value-type {@link java.lang.String} (max length 255) 
		/// </summary>
		/// <summary>Value-type text (unlimited length {@link java.lang.String})  
		/// </summary>
		/// <summary>Value-type XML {@link org.w3c.dom.Document} 
		/// </summary>
		PropertySetSchema Schema { get;
			//~ Methods ////////////////////////////////////////////////////////////////
			set; }

		/// <summary> List all keys.
		/// *
		/// </summary>
		/// <returns> Unmodifiable {@link java.util.Collection} of
		/// {@link java.lang.String}s.
		/// 
		/// </returns>
		ICollection Keys { get; }

		//~ Instance fields ////////////////////////////////////////////////////////

		void SetAsActualType(String key, Object value_Renamed);
		Object GetAsActualType(String key);
		void SetBoolean(String key, bool value_Renamed);
		bool GetBoolean(String key);
		void SetData(string key, byte[] value_Renamed);
		byte[] GetData(string key);
		void SetDate(String key, DateTime value_Renamed);
		DateTime GetDate(String key);
		void SetDouble(String key, double value_Renamed);
		double GetDouble(String key);
		void SetInt(String key, int value_Renamed);
		int GetInt(String key);

		/// <summary> List all keys of certain type.
		/// *
		/// </summary>
		/// <param name="type">Type to list. See static class variables. If null, then
		/// all types shall be returned.
		/// </param>
		/// <returns> Unmodifiable {@link java.util.Collection} of
		/// {@link java.lang.String}s.
		/// 
		/// </returns>
		ICollection GetKeys(int type);

		/// <summary> List all keys starting with supplied prefix.
		/// *
		/// </summary>
		/// <param name="prefix">String that keys must start with. If null, than all
		/// keys shall be returned.
		/// </param>
		/// <returns> Unmodifiable {@link java.util.Collection} of
		/// {@link java.lang.String}s.
		/// 
		/// </returns>
		ICollection GetKeys(string prefix);

		/// <summary> List all keys starting with supplied prefix of certain type. See
		/// statics.
		/// *
		/// </summary>
		/// <param name="prefix">String that keys must start with. If null, than all
		/// keys shall be returned.
		/// </param>
		/// <param name="type">Type to list. See static class variables. If null, then
		/// all types shall be returned.
		/// </param>
		/// <returns> Unmodifiable {@link java.util.Collection} of
		/// {@link java.lang.String}s.
		/// 
		/// </returns>
		ICollection GetKeys(string prefix, int type);

		void SetLong(String key, long value_Renamed);
		long GetLong(String key);
		void SetObject(String key, Object o);
		Object GetObject(String key);
		void SetProperties(string key, Properties properties);
		Properties GetProperties(string key);

		/// <summary> Whether this PropertySet implementation allows values to be set
		/// (as opposed to read-only).
		/// </summary>
		bool IsSettable(String property);

		void SetString(String key, String value_Renamed);

		/// <summary> {@link java.lang.String} of maximum 255 chars.
		/// </summary>
		String GetString(String key);

		void SetText(String key, String value_Renamed);

		/// <summary> {@link java.lang.String} of unlimited length.
		/// </summary>
		String GetText(String key);

		/// <summary> Returns type of value.
		/// *
		/// </summary>
		/// <returns> Type of value. See static class variables.
		/// 
		/// </returns>
		int GetType(String key);

		void SetXML(string key, XmlDocument doc);
		XmlDocument GetXML(string key);

		/// <summary> Determine if property exists.
		/// </summary>
		bool Exists(String key);

		void Init(IDictionary config, IDictionary args);

		/// <summary> Removes property.
		/// </summary>
		void Remove(String key);

		/// <summary> Remove the propertyset and all it associated keys.
		/// @throws PropertyException if there is an error removing the propertyset.
		/// </summary>
		void Remove();

		/// <summary> Whether this PropertySet implementation allows the type specified
		/// to be stored or retrieved.
		/// </summary>
		bool SupportsType(int type);

		/// <summary> Whether this PropertySet implementation supports types when storing values.
		/// (i.e. the type of data is stored as well as the actual value).
		/// </summary>
		bool SupportsTypes();
	}
}