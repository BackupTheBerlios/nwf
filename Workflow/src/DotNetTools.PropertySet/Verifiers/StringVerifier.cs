using System;
using Spring.Collections;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet.Verifiers
{
	/// <summary> Handles verification of Strings.
	/// Can be configured to only accept only strings within a given
	/// length range. Omitted values are assumed to be unconstrained.
	/// For example:<br><code>
	/// StringVerifier sv = new StringVerifier();
	/// sv.setMaxLength(50);
	/// </code><br>
	/// Will accept any string that is less than 50 characters in length.<p>
	/// Note though that the default max length of a string is 255 chars.
	/// *
	/// </summary>
	/// <author>  <a href="mailto:hani@fate.demon.co.uk">Hani Suleiman</a>
	/// </author>
	/// <author>jjx (.net)</author>
	/// <version>  
	/// $Revision: 1.1 $
	/// </version>
	public class StringVerifier : IPropertyVerifier
	{
		public virtual String[] AllowableValues
		{
			//~ Methods ////////////////////////////////////////////////////////////////


			set
			{
				allowableStrings = new HashedSet();

				//Store the array in a set, since all we'll be doing is lookups.
				for (int i = 0; i < value.Length; i++)
				{
					allowableStrings.Add(value[i]);
				}
			}

		}

		public virtual String Contains
		{
			get { return contains; }

			set { contains = value; }

		}

		public virtual int MaxLength
		{
			get { return max; }

			set { this.max = value; }

		}

		public virtual int MinLength
		{
			get { return min; }

			set { this.min = value; }

		}

		public virtual String Prefix
		{
			get { return prefix; }

			set { prefix = value; }

		}

		public virtual String Suffix
		{
			get { return suffix; }

			set { suffix = value; }

		}

		//~ Instance fields ////////////////////////////////////////////////////////

		private Set allowableStrings;
		private String contains;
		private String prefix;
		private String suffix;
		private int max = 255;
		private int min = 0;

		//~ Constructors ///////////////////////////////////////////////////////////

		public StringVerifier()
		{
		}

		/// <summary> Create a StringVerifier with the specified min and max lengths.
		/// </summary>
		/// <param name="min">The minimum allowable string length.
		/// </param>
		/// <param name="max">The maximum allowable string length.
		/// 
		/// </param>
		public StringVerifier(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		public StringVerifier(String[] allowable)
		{
			AllowableValues = allowable;
		}


		public virtual void Verify(Object o)
		{
			String s = (String) o;

			if (s.Length < min)
			{
				throw new VerifyException("String " + s + " too short, min length=" + min);
			}

			if (s.Length > max)
			{
				throw new VerifyException("String " + s + " too long, max length=" + max);
			}

			if ((suffix != null) && !s.EndsWith(suffix))
			{
				throw new VerifyException("String " + s + " has invalid suffix (suffix must be \"" + suffix + "\")");
			}

			if ((prefix != null) && !s.StartsWith(prefix))
			{
				throw new VerifyException("String " + s + " has invalid prefix (prefix must be \"" + prefix + "\")");
			}

			if ((contains != null) && (s.IndexOf(contains) == - 1))
			{
				throw new VerifyException("String " + s + " does not contain required string \"" + contains + "\"");
			}

			if ((allowableStrings != null) && !allowableStrings.Contains(s))
			{
				throw new VerifyException("String " + s + " not in allowed set for this property");
			}
		}
	}
}