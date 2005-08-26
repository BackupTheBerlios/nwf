using System;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet
{
	/// <summary> Parent class of all exceptions thrown by PropertySet.
	/// *
	/// </summary>
	/// <author>  
	/// <a href="mailto:joe@truemesh.com">Joe Walnes</a>
	/// </author>
	/// <author>jjx (.net)</author>
	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	public class PropertyException : SystemException
	{
		//~ Constructors ///////////////////////////////////////////////////////////

		public PropertyException() : base()
		{
		}

		public PropertyException(String msg) : base(msg)
		{
		}
		public PropertyException (String msg,Exception inner):base(msg,inner){}
	}
}