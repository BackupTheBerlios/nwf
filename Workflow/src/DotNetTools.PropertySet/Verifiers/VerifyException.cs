using System;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet.Verifiers
{
	/// <summary>*
	/// </summary>
	/// <author>  <a href="mailto:hani@fate.demon.co.uk">Hani Suleiman</a>
	/// </author>
	/// <author>jjx (.net)</author>
	/// <version> 
	///  $Revision: 1.1 $
	/// </version>
	public class VerifyException : SystemException
	{
		//~ Constructors ///////////////////////////////////////////////////////////

		public VerifyException() : base()
		{
		}

		public VerifyException(String msg) : base(msg)
		{
		}
	}
}