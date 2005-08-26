using System;
using System.Collections;
using System.Reflection;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet.Memory
{
	/// <summary> The SerializablePropertySet is a PropertySet implementation that
	/// will store any primitive of serializable object in an internal Map
	/// which is stored in memory and can be loaded/saved by serializing the
	/// entire SerializablePropertySet.
	/// *
	/// <p>This offers the most basic form of persistence. Note that
	/// <code>setObject()</code> will throw an IllegalPropertyException if
	/// the passed object does not implement Serializable.</p>
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
	/// <seealso cref="">com.opensymphony.module.propertyset.memory.MemoryPropertySet
	/// 
	/// </seealso>
	[Serializable]
	public class SerializablePropertySet : MemoryPropertySet
	{
		protected internal override IDictionary Map
		{
			get { return serialMap; }

		}

		//~ Instance fields ////////////////////////////////////////////////////////

		private IDictionary serialMap;

		//~ Methods ////////////////////////////////////////////////////////////////

		public override void Init(IDictionary config, IDictionary args)
		{
			serialMap = new Hashtable();
		}

		public override void Remove()
		{
			serialMap.Clear();
		}

		
		protected internal override void setImpl(int type, String key, Object o)
		{
			lock (this)
			{
				if ((o != null) && (type==PropertySet_Fields.OBJECT && (o.GetType().Attributes & TypeAttributes.Serializable)!=TypeAttributes.Serializable))
				{
					throw new IllegalPropertyException("Cannot set " + key + ". Value type " + o.GetType() + " not Serializable");
				}

				base.setImpl(type, key, o);
			}
		}

	}
}