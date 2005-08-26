using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;
using DotNetTools.Util;
using log4net;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet.Config
{
	/// <summary> DOCUMENT ME!
	/// *
	/// </summary>
	/// <author>  $author$
	/// </author>
	/// <author>jjx (.net)</author>
	/// <version>  $Revision: 1.1 $
	/// 
	/// </version>
	public class PropertySetConfig
	{
		private static ILog log;
		static PropertySetConfig()
		{
			log=LogManager.GetLogger(typeof(PropertySetConfig))	;
		}
		private void InitBlock()
		{
			propertySetArgs = new Hashtable();
			propertySets = new Hashtable();
		}

		public static PropertySetConfig Config
		{
			//~ Methods ////////////////////////////////////////////////////////////////


			get
			{
				// check one more time, another thread may have finished
				lock (lock_Renamed)
				{
					if (config == null)
					{
						config = new PropertySetConfig();
					}

					return config;
				}
			}

		}

		//~ Static fields/initializers /////////////////////////////////////////////

		private static PropertySetConfig config;
		private static readonly Object lock_Renamed = new Object();
		private static readonly String[] CONFIG_LOCATIONS = new String[] {"propertyset.xml"};

		//~ Instance fields ////////////////////////////////////////////////////////

		private IDictionary propertySetArgs;
		private IDictionary propertySets;

		//~ Constructors ///////////////////////////////////////////////////////////

		private PropertySetConfig()
		{
			InitBlock();
			Stream is_Renamed = load();

			

			XmlDocument doc = new XmlDocument();

			try
			{
				doc.Load(is_Renamed);
			}
			catch (Exception e)
			{
				log.Error(e.Message,e);
			}
			
			finally
			{
				//close the input stream
				if (is_Renamed != null)
				{
					try
					{
						is_Renamed.Close();
					}
					catch (IOException )
					{
						/* ignore */
					}
				}
			}

			// get propertysets
			XmlNode root =  doc.SelectSingleNode("propertysets");
			XmlNodeList propertySets = root.SelectNodes("propertyset");
			foreach(XmlNode propertySet in propertySets){

				String name = XMLUtils.GetAttribute(propertySet,"name");
				
				String clazz =XMLUtils.GetAttribute(propertySet,"type");
				this.propertySets[name]= clazz;

				// get args now

				XmlNodeList args = propertySet.SelectNodes("arg");
				IDictionary argsMap = new Hashtable();

				foreach(XmlNode arg in args){
					String argName =XMLUtils.GetAttribute(arg,"name");
					String argValue = XMLUtils.GetAttribute(arg,"value");
					argsMap[argName]= argValue;
				}

				this.propertySetArgs[name]= argsMap;
			}
		}


		public virtual IDictionary getArgs(String name)
		{
			return (IDictionary) propertySetArgs[name];
		}

		public virtual String GetTypeName(String name)
		{
			return (String) propertySets[name];
		}

		
		/// <summary>
		///  Load the config from locations found in {@link #CONFIG_LOCATIONS}
		/// </summary>
		/// <returns>
		///   An inputstream to load from
		/// </returns>
		
		private Stream load()
		{
			
			for (int i = 0; i < CONFIG_LOCATIONS.Length; i++)
			{
				String location = CONFIG_LOCATIONS[i];

				try
				{
					
				
					FileInfo file=new FileInfo(location);	
					if (file.Exists)
						return file.OpenRead();

				}
				catch (Exception )
				{
					//do nothing.
				}
			}

			Stream stream = ResourceUtils.GetGetManifestResourceStreamFromAppDomain("proeprtyset.xml");
			if (stream!=null)			
				return stream;
			//TODO: StringUtils.ArrayToCommaDelimitedString (in spring.net)
			String exceptionMessage = "Could not load propertyset config using '" + CONFIG_LOCATIONS + " '.  Please check your path.";
			throw new ArgumentException(exceptionMessage);
			

			
		}
	}
}