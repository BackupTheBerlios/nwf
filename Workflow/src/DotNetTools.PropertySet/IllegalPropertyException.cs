using System;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet
{
	/// <summary> Thrown if a property is set which is not allowed.
	/// *
	/// <p><i>e.g.</i> non-serializable Object is passed to SerializablePropertySet,
	/// or field is persisted that cannot be stored in database.</p>
	/// *
	/// </summary>
	/// <author>  <a href="mailto:joe@truemesh.com">Joe Walnes</a>
	/// </author>
	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	public class IllegalPropertyException : PropertyException
	{
		//~ Constructors ///////////////////////////////////////////////////////////

		public IllegalPropertyException() : base()
		{
		}

		public IllegalPropertyException(String msg) : base(msg)
		{
		}
	}
}