using System;
using System.Collections;
using DotNetTools.PropertySet.Verifiers;
using DotNetTools.Util;
using Spring.Collections;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet
{
	/// <summary> Describes the meta data for a given property.
	/// The meta data for a property includes its type as well as
	/// any verifiers that constrain it.
	/// *
	/// todo: add multiplicity?
	/// *
	/// </summary>
	/// <author>  <a href="mailto:hani@fate.demon.co.uk">Hani Suleiman</a>
	/// </author>
	/// <author>jjx (.net)</author>
	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	[Serializable]
	public class PropertySchema
	{
		public virtual String PropertyName
		{
			get { return name; }
			//~ Methods ////////////////////////////////////////////////////////////////


			set { name = value; }

		}

		public virtual int Type
		{
			get { return type; }

			set { this.type = value; }

		}

		/// <summary> Returns unmodifiable List of verifiers.
		/// </summary>
		public virtual ICollection Verifiers
		{
			get { return Collections.UnmodifiableCollection(verifiers); }

		}

		//~ Instance fields ////////////////////////////////////////////////////////

		private ICollection verifiers;
		private String name;
		private int type;

		//~ Constructors ///////////////////////////////////////////////////////////

		public PropertySchema() : this(null)
		{
		}

		public PropertySchema(String name) : base()
		{
			this.name = name;
			verifiers = new HashedSet();
		}


		public virtual bool addVerifier(IPropertyVerifier pv)
		{
			return (verifiers as HashedSet).Add(pv);
		}

		public virtual bool removeVerifier(IPropertyVerifier pv)
		{
			return(verifiers as HashedSet).Remove(pv);
		}

		/// <summary> Validate a given value against all verifiers.
		/// Default behaviour is to AND all verifiers.
		/// </summary>
		public virtual void validate(Object value_Renamed)
		{

			foreach(IPropertyVerifier pv in verifiers)
			{
				//Hmm, do we need a try/catch?
				try
				{
					pv.Verify(value_Renamed);
				}
				catch (VerifyException ex)
				{
					throw new IllegalPropertyException(ex.Message);
				}
			}
		}
	}
}