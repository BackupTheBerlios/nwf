using System;
using System.Collections;
using DotNetTools.Util;
using Spring.Collections;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet.Memory
{
	/// <summary> The MemoryPropertySet is a PropertySet implementation that
	/// will store any primitive or object in an internal Map
	/// that is stored in memory.
	/// *
	/// <p>An alternative to MemoryPropertySet is SerializablePropertySet
	/// which can be Serialized to/from a stream.</p>
	/// *
	/// </summary>
	/// <author>  <a href="mailto:joe@truemesh.com">Joe Walnes</a>
	/// </author>
	/// <author>jjx (.net)</author>
	/// <version> 
	/// $Revision: 1.1 $
	/// </version>
	/// <seealso cref="">com.opensymphony.module.propertyset.PropertySet
	/// 
	/// </seealso>
	public class MemoryPropertySet : AbstractPropertySet
	{
		protected internal virtual IDictionary Map
		{
			get { return map; }

		}

		//~ Instance fields ////////////////////////////////////////////////////////

		private IDictionary map;

		//~ Methods ////////////////////////////////////////////////////////////////

		public override ICollection GetKeys(String prefix, int type)
		{
			lock (this)
			{
				
				IList result = new LinkedList();
				foreach(String key in Map.Keys){

					if ((prefix == null) || key.StartsWith(prefix))
					{
						if (type == 0)
						{
							result.Add(key);
						}
						else
						{
							ValueEntry v = (ValueEntry) Map[key];

							if (v.type == type)
							{
								result.Add(key);
							}
						}
					}
				}

				Collections.Sort(result);

				return result;
			}
		}

		public override int GetType(String key)
		{
			lock (this)
			{
				if ((Map as Hashtable).ContainsKey(key))
				{
					return ((ValueEntry) Map[key]).type;
				}
				else
				{
					return 0;
				}
			}
		}

		public override bool Exists(String key)
		{
			lock (this)
			{
				return GetType(key) > 0;
			}
		}

		public override void Init(IDictionary config, IDictionary args)
		{
			map = new Hashtable();
		}

		
		public override void Remove(String key)
		{
			lock (this)
			{
				Map.Remove(key);
			}
		}

		public override void Remove()
		{
			map.Clear();
		}

		protected internal override void setImpl(int type, String key, Object value_Renamed)
		{
			lock (this)
			{
				if (Exists(key))
				{
					ValueEntry v = (ValueEntry) Map[key];

					if (v.type != type)
					{
						throw new DuplicatePropertyKeyException();
					}

					v.value_Renamed = value_Renamed;
				}
				else
				{
					Map.Add(key, new ValueEntry(type, value_Renamed));
				}

				return;
			}
		}


		protected internal override Object get(int type, String key)
		{
			lock (this)
			{
				if (Exists(key))
				{
					ValueEntry v = (ValueEntry) Map[key];

					if (v.type != type)
					{
						throw new InvalidPropertyTypeException();
					}

					return v.value_Renamed;
				}
				else
				{
					return null;
				}
			}
		}

		//~ Inner Classes //////////////////////////////////////////////////////////

		[Serializable]
		public sealed class ValueEntry
		{
			public  int Type
			{
				get { return type; }

				set { this.type = value; }

			}

			public  Object Value
			{
				get { return value_Renamed; }

				set { this.value_Renamed = value; }

			}

			internal Object value_Renamed;
			internal int type;

			public ValueEntry()
			{
			}

			public ValueEntry(int type, Object value_Renamed)
			{
				this.type = type;
				this.value_Renamed = value_Renamed;
			}


		}
	}
}