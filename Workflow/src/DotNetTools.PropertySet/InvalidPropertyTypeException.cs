using System;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet
{
	/// <summary> Thrown if a property is attempted to be retrieved that
	/// does exist but is of different type.
	/// *
	/// </summary>
	/// <author>  <a href="mailto:joe@truemesh.com">Joe Walnes</a>
	/// </author>
	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	public class InvalidPropertyTypeException : PropertyException
	{
		//~ Constructors ///////////////////////////////////////////////////////////

		public InvalidPropertyTypeException() : base()
		{
		}

		public InvalidPropertyTypeException(String msg) : base(msg)
		{
		}
	}
}