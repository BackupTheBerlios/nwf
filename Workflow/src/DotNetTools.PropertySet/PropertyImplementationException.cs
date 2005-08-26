using System;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet
{
	/// <summary> Thrown if a specific implementation exception is thrown
	/// (such as EJBException, RemoteException, NamingException, IOException, etc).
	/// *
	/// <p>A specific Exception can be wrapped in this Exception, by being
	/// passed to the constructor. It can be retrieved via
	/// {@link #getRootCause()} .</p>
	/// *
	/// </summary>
	/// <author>  <a href="mailto:joe@truemesh.com">Joe Walnes</a>
	/// </author>
	/// <author>jjx (.net)</author>
	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	public class PropertyImplementationException : PropertyException
	{
		/// <summary> Retrieve original Exception.
		/// </summary>
		//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
		public virtual Exception RootCause
		{
			get { return original; }

		}

		//~ Instance fields ////////////////////////////////////////////////////////

		//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
		protected internal Exception original;

		//~ Constructors ///////////////////////////////////////////////////////////

		public PropertyImplementationException() : base()
		{
		}

		public PropertyImplementationException(String msg) : base(msg)
		{
		}

		//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
		public PropertyImplementationException(String msg, Exception original) : base(msg)
		{
			this.original = original;
		}

		//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1100"'
		public PropertyImplementationException(Exception original) : this(original.Message, original)
		{
		}

		//~ Methods ////////////////////////////////////////////////////////////////

	}
}