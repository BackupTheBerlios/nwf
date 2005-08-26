using System;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet
{
	/// <summary> Thrown if a property is set who's key matches a key of an
	/// existing property with different type.
	/// *
	/// </summary>
	/// <author>  <a href="mailto:joe@truemesh.com">Joe Walnes</a>
	/// </author>
	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	public class DuplicatePropertyKeyException : PropertyException
	{
		//~ Constructors ///////////////////////////////////////////////////////////

		public DuplicatePropertyKeyException() : base()
		{
		}

		public DuplicatePropertyKeyException(String msg) : base(msg)
		{
		}
	}
}