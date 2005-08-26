using System;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet.Verifiers
{
	/// <summary>
	/// Handles verification of numbers.
	/// Can be configured to only accept specific numeric types (int, float, etc)
	/// as well as a range for the specified number. All constraints are
	/// optional. If not specified, then any number is accepted.
	/// *
	/// </summary>
	/// <author>
	/// <a href="mailto:hani@fate.demon.co.uk">Hani Suleiman</a>
	/// </author>
	/// <author>jjx (.net)</author>
	/// <version>
	/// $Revision: 1.1 $
	/// </version>
	public class NumberVerifier : IPropertyVerifier
	{

		public virtual Decimal Max
		{
			get { return max; }
	

			set
			{
				//Should we check if(type!=null && num.getClass()==type) ? Also ensure min/max classes match?
				max = value;
			}

		}

		public virtual Decimal Min
		{
			get { return min; }

			set
			{
				//Should we check if(type!=null && num.getClass()==type) ? Also ensure min/max classes match?
				min = value;
			}

		}

		public virtual Type Type
		{
			get { return type; }

			set { this.type = value; }

		}

		//~ Instance fields ////////////////////////////////////////////////////////

		private Type type;
		private Decimal max;
		private Decimal min;

		//~ Constructors ///////////////////////////////////////////////////////////

		public NumberVerifier()
		{
			max=Decimal.MaxValue;
			min=Decimal.MinValue;
		}

	
		public virtual void Verify(Object o)
		{
			//Should we wrap up a ClassCastException here?
			Decimal num = (Decimal) o;

			if (num.GetType() != type)
			{
				throw new VerifyException("value is of type " + num.GetType() + " expected type is " + type);
			}

			//Hmm, should we convert everything to doubles (performance?) or deal with every possible
			//Number subclass that we support?
			if ((min != Decimal.MinValue) && (o != null) && (min > num))
			{
				throw new VerifyException("value " + num + " < min limit " + min);
			}

			if ((max != Decimal.MaxValue) && (o != null) && (max < num))
			{
				throw new VerifyException("value " + num + " > max limit " + max);
			}

			//Fall through case, allow any Number object.
		}
	}
}