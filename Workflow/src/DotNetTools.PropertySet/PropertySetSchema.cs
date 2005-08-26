using System;
using System.Collections;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet
{
	/// <summary>*
	/// </summary>
	/// <author>  <a href="mailto:hani@fate.demon.co.uk">Hani Suleiman</a>
	/// </author>
	/// <author>jjx (.net)</author>

	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	[Serializable]
	public class PropertySetSchema 
	{
		public virtual bool Restricted
		{
			get { return restricted; }

			set { restricted = value; }

		}

		//~ Instance fields ////////////////////////////////////////////////////////

		private IDictionary propertySchemas;
		private bool restricted;

		//~ Constructors ///////////////////////////////////////////////////////////

		public PropertySetSchema()
		{
			propertySchemas = new Hashtable();
		}

		//~ Methods ////////////////////////////////////////////////////////////////

		public virtual void setPropertySchema(String key, PropertySchema ps)
		{
			if (ps.PropertyName == null)
			{
				ps.PropertyName = key;
			}

			propertySchemas.Add(key, ps);
		}

		public virtual PropertySchema getPropertySchema(String key)
		{
			return (PropertySchema) propertySchemas[key];
		}


		public virtual void addPropertySchema(PropertySchema ps)
		{
			propertySchemas.Add(ps.PropertyName, ps);
		}
	}
}