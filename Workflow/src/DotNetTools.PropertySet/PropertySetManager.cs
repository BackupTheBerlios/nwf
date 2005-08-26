using System;
using System.Collections;
using DotNetTools.PropertySet.Config;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet
{
	/// <summary> The PropertySetManager is a factory for all the different types of
	/// propertysets registered.
	/// *
	/// </summary>
	/// <author>  $author$
	/// </author>
	/// <author>jjx (.net)</author>
	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	public class PropertySetManager
	{
		//~ Methods ////////////////////////////////////////////////////////////////

		/// <summary> Get a propertyset by name.
		/// </summary>
		/// <param name="name">The name of the propertyset as registered in propertyset.xml.
		/// For example 'ejb', or 'memory'.
		/// </param>
		/// <param name="args">The arguments to pass to the propertyset for initialization.
		/// Consult the javadocs for a particular propertyset to see what arguments
		/// it requires and supports.
		/// 
		/// </param>
		public static IPropertySet GetInstance(String name, IDictionary args)
		{
			PropertySetConfig psc = PropertySetConfig.Config;
			String clazz = psc.GetTypeName(name);
			IDictionary config = psc.getArgs(name);
			Type psClass;

			try
			{
			
				psClass = Type.GetType(clazz);
			}
			catch (Exception )
			{
				return null;
			}

			try
			{
				IPropertySet ps = (IPropertySet) SupportClass.CreateNewInstance(psClass);
				ps.Init(config, args);

				return ps;
			}
			catch (UnauthorizedAccessException e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
			}
			catch (Exception e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
			}
			

			return null;
		}


		

		/// <summary> Copy the contents of one propertyset into another.
		/// </summary>
		/// <param name="src">The propertyset to copy from.
		/// </param>
		/// <param name="dest">The propertyset to copy into.
		/// 
		/// </param>
		public static void Clone(IPropertySet src, IPropertySet dest)
		{
			PropertySetCloner cloner = new PropertySetCloner();
			cloner.Source = src;
			cloner.Destination = dest;
			cloner.cloneProperties();
		}
	}
}