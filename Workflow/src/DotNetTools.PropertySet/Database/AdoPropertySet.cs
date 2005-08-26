using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using log4net;
/*
* Copyright (c) 2002-2003 by OpenSymphony
* All rights reserved.
*/

namespace DotNetTools.PropertySet.Database
{
	/// <summary> This is an implementation of a property set manager for JDBC. It relies on
	/// one table, called "os_propertyset" that has four columns: "type" (integer),
	/// "keyValue" (string), "globalKey" (string), and "value" (string). This is not
	/// likely to be enough for people who store BLOBS as properties. Of course,
	/// those people need to get a life.
	/// <p>
	/// *
	/// For Postgres(?):<br>
	/// CREATE TABLE OS_PROPERTYENTRY (GLOBAL_KEY varchar(255), ITEM_KEY varchar(255), ITEM_TYPE smallint, STRING_VALUE varchar(255), DATE_VALUE timestamp, DATA_VALUE oid, FLOAT_VALUE float8, NUMBER_VALUE numeric, primary key (GLOBAL_KEY, ITEM_KEY));
	/// <p>
	/// *
	/// For Oracle (Thanks to Michael G. Slack!):<br>
	/// CREATE TABLE OS_PROPERTYENTRY (GLOBAL_KEY varchar(255), ITEM_KEY varchar(255), ITEM_TYPE smallint, STRING_VALUE varchar(255), DATE_VALUE date, DATA_VALUE long raw, FLOAT_VALUE float, NUMBER_VALUE numeric, primary key (GLOBAL_KEY, ITEM_KEY));
	/// <p>
	/// *
	/// Other databases may require small tweaks to the table creation scripts!
	/// *
	/// <p>
	/// *
	/// <b>Required Args</b>
	/// <ul>
	/// <li><b>globalKey</b> - the globalKey to use with this PropertySet</li>
	/// </ul>
	/// <p>
	/// *
	/// <b>Required Configuration</b>
	/// <ul>
	/// <li><b>datasource</b> - JNDI path for the DataSource</li>
	/// <li><b>table.name</b> - the table name</li>
	/// <li><b>col.globalKey</b> - column name for the globalKey</li>
	/// <li><b>col.itemKey</b> - column name for the itemKey</li>
	/// <li><b>col.itemType</b> - column name for the itemType</li>
	/// <li><b>col.string</b> - column name for the string value</li>
	/// <li><b>col.date</b> - column name for the date value</li>
	/// <li><b>col.data</b> - column name for the data value</li>
	/// <li><b>col.float</b> - column name for the float value</li>
	/// <li><b>col.number</b> - column name for the number value</li>
	/// </ul>
	/// *
	/// </summary>
	/// <version>  $Revision: 1.1 $
	/// </version>
	/// <author>  <a href="mailto:epesh@hotmail.com">Joseph B. Ottinger</a>
	/// </author>
	/// <author>  <a href="mailto:plightbo@hotmail.com">Pat Lightbody</a>
	/// </author>
	/// <author>jjx (.net)</author>

	public class AdoPropertySet : AbstractPropertySet
	{
		protected internal virtual String getParameterName(String parameterName)
		{
			return "@"+parameterName;
		}
		protected internal virtual IDataParameter createDataParameter(String parameterName,Object value)
		{
			return new SqlParameter("@"+parameterName,value);

		}
		protected internal virtual IDbCommand createDbCommand(String sql,IDbConnection conn)
		{
			
			IDbCommand cmd=new SqlCommand();
			cmd.CommandType=CommandType.Text;
			cmd.CommandText=sql;
			cmd.Connection=conn;
			return cmd;
		}
		protected internal virtual IDbConnection Connection
		{
			get
			{
				closeConnWhenDone = true;

				SqlConnection con=new SqlConnection(connectionString);
				con.Open();
				return con;
			}

		}

		//~ Static fields/initializers /////////////////////////////////////////////

		private static readonly ILog log;

		//~ Instance fields ////////////////////////////////////////////////////////

		// config
		protected internal String connectionString;
		protected internal String colData;
		protected internal String colDate;
		protected internal String colFloat;
		protected internal String colGlobalKey;
		protected internal String colItemKey;
		protected internal String colItemType;
		protected internal String colNumber;
		protected internal String colString;

		// args
		protected internal String globalKey;
		protected internal String tableName;
		protected internal bool closeConnWhenDone = false;

		//~ Methods ////////////////////////////////////////////////////////////////

		public override ICollection GetKeys(String prefix, int type)
		{
			if (prefix == null)
			{
				prefix = "";
			}

			IDbConnection conn = null;
			IDbCommand ps = null;
			
			IDataReader rs = null;

			try
			{
				conn = Connection;

				String sql = "SELECT " + colItemKey + " FROM " + tableName + " WHERE " + colItemKey + " LIKE "+
					getParameterName(colItemKey)+" AND " +
					colGlobalKey + " = "+
					getParameterName(colGlobalKey);

				if (type == 0)
				{
					ps=createDbCommand(sql,conn);
					ps.Parameters.Add(createDataParameter(colItemKey,prefix+"%"));

					ps.Parameters.Add(createDataParameter(colGlobalKey,globalKey));
					
				}
				else
				{
					sql = sql + " AND " + colItemType + " = "+getParameterName(colItemType);

					ps=createDbCommand(sql,conn);
					ps.Parameters.Add(createDataParameter(colItemKey,prefix+"%"));

					ps.Parameters.Add(createDataParameter(colGlobalKey,globalKey));
					ps.Parameters.Add(createDataParameter(colItemType,type));

					
				}

				ArrayList list = new ArrayList();
				rs = ps.ExecuteReader();

				while (rs.Read())
				{
					list.Add(Convert.ToString(rs[(colItemKey)]));
				}

				return list;
			}
			catch (Exception e)
			{
				
				throw new PropertyException(e.Message,e);
			}
			finally
			{
				cleanup(conn, ps, rs);
			}
		}

		public override int GetType(String key)
		{
			IDbConnection conn = null;
			IDbCommand ps = null;
			
			IDataReader rs = null;

			try
			{
				conn = Connection;

				String sql = "SELECT " + colItemType + " FROM " + tableName + " WHERE " + colGlobalKey + " ="+
					getParameterName(colGlobalKey)+" AND " + colItemKey + " = "+
					getParameterName(colItemKey);

				ps=createDbCommand(sql,conn);
				ps.Parameters.Add(createDataParameter(colGlobalKey,globalKey));
				ps.Parameters.Add(createDataParameter(colItemKey,key));

				
				rs = ps.ExecuteReader();

				int type = 0;

				if (rs.Read())
				{
					type = Convert.ToInt32(rs[(colItemType)]);
				}

				return type;
			}
			catch (Exception e)
			{
				
				throw new PropertyException(e.Message,e);
			}
			finally
			{
				cleanup(conn, ps, rs);
			}
		}

		public override bool Exists(String key)
		{
			return GetType(key) != 0;
		}

		public  override void Init(IDictionary config, IDictionary args)
		{
			// args
			globalKey = (String) args["globalKey"];

			// config
			connectionString = (String) config["connectionString"];
			tableName = (String) config["table.name"];
			colGlobalKey = (String) config["col.globalKey"];
			colItemKey = (String) config["col.itemKey"];
			colItemType = (String) config["col.itemType"];
			colString = (String) config["col.string"];
			colDate = (String) config["col.date"];
			colData = (String) config["col.data"];
			colFloat = (String) config["col.float"];
			colNumber = (String) config["col.number"];
		}

		public override void Remove()
		{
			IDbConnection conn = null;
			IDbCommand ps = null;

			try
			{
				conn = Connection;

				String sql = "DELETE FROM " + tableName + " WHERE " + colGlobalKey + " = "+getParameterName(colGlobalKey);

				ps=createDbCommand(sql,conn);
				ps.Parameters.Add(createDataParameter("colGlobalKey",globalKey));

				ps.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				
				throw new PropertyException(e.Message,e);
			}
			finally
			{
				cleanup(conn, ps, null);
			}
		}

		public override void Remove(String key)
		{
			IDbConnection conn = null;
			IDbCommand ps = null;

			try
			{
				conn = Connection;

				String sql = "DELETE FROM " + tableName + " WHERE " + colGlobalKey + " = "+
						getParameterName(colGlobalKey) +" AND " + colItemKey + " = "+getParameterName(colItemKey);
				ps=createDbCommand(sql,conn);
				ps.Parameters.Add(createDataParameter(colGlobalKey,globalKey));
				ps.Parameters.Add(createDataParameter(colItemKey,key));

				ps.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				
				throw new PropertyException(e.Message,e);
			}
			finally
			{
				cleanup(conn, ps, null);
			}
		}

		public override bool SupportsType(int type)
		{
			switch (type)
			{
				case PropertySet_Fields.PROPERTIES:
				case PropertySet_Fields.TEXT:
				case PropertySet_Fields.XML:
					return false;
			}

			return true;
		}


		protected internal override void setImpl(int type, String key, Object value_Renamed)
		{
			if (value_Renamed == null)
			{
				throw new PropertyException("AdoPropertySet does not allow for null values to be stored");
			}

			IDbConnection conn = null;
			IDbCommand ps = null;

			try
			{
				conn = Connection;

				String sql = "UPDATE " + tableName + " SET " + colString + " = "+
						getParameterName(colString)+", " + colDate + " = "+
						getParameterName(colDate)+", " + colData + " = "+
						getParameterName(colData) +", " + colFloat + " = "+
						getParameterName(colFloat)+", " + colNumber + " = "+
						getParameterName(colNumber)+", " + colItemType + " = "+
						getParameterName(colItemType)+ " WHERE " + colGlobalKey + " = "+
						getParameterName(colGlobalKey)+" AND " + colItemKey + " = "+getParameterName(colItemKey);
				if (log.IsDebugEnabled)
					log.Debug(sql);
				ps=createDbCommand(sql,conn);

				

				setValues(ps, type, key, value_Renamed);

				int rows = ps.ExecuteNonQuery();

				if (rows != 1)
				{
					// ok, this is a new value, insert it
					sql = "INSERT INTO " + tableName + " (" + colString + ", " +
						colDate + ", " + 
						colData + ", " + 
						colFloat + ", " + 
						colNumber + ", " +
						colItemType + ", " + 
						colGlobalKey + ", " +
						colItemKey + ") VALUES ("+
						getParameterName(colString)+","+
						getParameterName(colDate)+","+
						getParameterName(colData)+","+
						getParameterName(colFloat)+","+
						getParameterName(colNumber)+","+
						getParameterName(colItemType)+","+
						getParameterName(colGlobalKey)+","+
						getParameterName(colItemKey)+")";

						
					if (log.IsDebugEnabled)
						log.Debug(sql);
					ps.Dispose();
					ps=createDbCommand(sql,conn);
					setValues(ps, type, key, value_Renamed);
					ps.ExecuteNonQuery();
				}
			}
			catch (Exception e)
			{
				throw new PropertyException(e.Message,e);
			}
			finally
			{
				cleanup(conn, ps, null);
			}
		}

		protected internal virtual void cleanup(IDbConnection connection, IDbCommand statement, IDataReader result)
		{
			if (result != null)
			{
				try
				{
					result.Close();
				}
				catch (Exception ex)
				{
					log.Error("Error closing resultset", ex);
				}
			}

			if (statement != null)
			{
				try
				{
					
					statement.Dispose();
				}
				catch (Exception ex)
				{
					log.Error("Error closing statement", ex);
				}
			}

			if ((connection != null) && closeConnWhenDone)
			{
				try
				{
					connection.Close();
				}
				catch (Exception ex)
				{
					log.Error("Error closing connection", ex);
				}
			}
		}

		private byte[] readBlob(IDataReader dr,String columnName)
		{
			int index=dr.GetOrdinal(columnName);
			for(int i=1;i<index;i++)
			{
				Object o=dr[i];

			}
			int buffSize=4096;
			byte[] buff=new byte[buffSize];
			MemoryStream ms=new MemoryStream();
			long startIndex=0;
			long numread=dr.GetBytes(index,startIndex,buff,0,buffSize);
			while(numread==buffSize)
			{
				ms.Write(buff,0,buff.Length);
				ms.Flush();
				startIndex+=buffSize;
				numread=dr.GetBytes(index,startIndex,buff,0,buffSize);
			}
			ms.Write(buff,0,(int)numread);
			byte[] data=ms.ToArray();
			ms.Close();
			return data;





			
		}
		protected internal override Object get(int type, String key)
		{
			String sql = "SELECT " + colItemType + ", " + colString + ", " + colDate + ", " + colData + ", " + colFloat + ", " + colNumber + " FROM " + tableName + " WHERE " +
				colItemKey + " = "+getParameterName(colItemKey) +
					" AND " + colGlobalKey + " = "+getParameterName(colGlobalKey);

			Object o = null;
			IDbConnection conn = null;
			IDbCommand ps = null;
			
			IDataReader rs = null;

			try
			{
				conn = Connection;
				ps=createDbCommand(sql,conn);
				ps.Parameters.Add(createDataParameter(colItemKey,key));
				ps.Parameters.Add(createDataParameter(colGlobalKey,globalKey));

				int propertyType;
				if (type==PropertySet_Fields.OBJECT || type==PropertySet_Fields.DATA)
					rs=ps.ExecuteReader(CommandBehavior.SequentialAccess);
				else
					rs=ps.ExecuteReader();
				
				if (rs.Read())
				{
					propertyType = Convert.ToInt32(rs[(colItemType)]);

					if (propertyType != type)
					{
						throw new InvalidPropertyTypeException();
					}

					switch (type)
					{
						case PropertySet_Fields.BOOLEAN:

							int boolVal = Convert.ToInt32(rs[(colNumber)]);
							o = boolVal == 1;

							break;


						case PropertySet_Fields.DATA:
							o = readBlob(rs,colData);

							break;


						case PropertySet_Fields.DATE:
							o = Convert.ToDateTime(rs[(colDate)]);

							break;


						case PropertySet_Fields.OBJECT:


							try
							{
								BinaryReader is_Renamed = new BinaryReader(new MemoryStream(readBlob(rs,colData)));
								
								o = SupportClass.Deserialize(is_Renamed);
							}
							catch (IOException e)
							{
								throw new PropertyException("Error de-serializing object for key '" + key + "' from store:" + e);
							}
							catch (Exception e)
							{
								SupportClass.WriteStackTrace(e, Console.Error);
							}

							break;


						case PropertySet_Fields.DOUBLE:
							o = Convert.ToDouble(rs[(colFloat)]);

							break;


						case PropertySet_Fields.INT:
							o = Convert.ToInt32(rs[(colNumber)]);

							break;


						case PropertySet_Fields.LONG:
							o = Convert.ToInt64(rs[(colNumber)]);

							break;


						case PropertySet_Fields.STRING:
							o = Convert.ToString(rs[(colString)]);

							break;


						default:
							throw new InvalidPropertyTypeException("AdoPropertySet doesn't support this type yet.");

					}
				}
			}
			
			catch (FormatException e)
			{
				
				throw new PropertyException(e.Message,e);
			}
			catch (Exception e)
			{
				
				throw new PropertyException(e.Message,e);
			}
			finally
			{
				cleanup(conn, ps, rs);
			}

			return o;
		}

		private void setValues(IDbCommand ps, int type, String key, Object o)
		{
			
			
			IDataParameter stringParameter=createDataParameter(this.colString,DBNull.Value);
			IDataParameter datetimeParameter=createDataParameter(this.colDate,DBNull.Value);
			//datetimeParameter.DbType=DbType.DateTime;
			IDataParameter dataParameter=createDataParameter(this.colData,DBNull.Value);
			dataParameter.DbType=DbType.Binary;
			IDataParameter singleParameter=createDataParameter(this.colFloat,DBNull.Value);
			//dataParameter.DbType=DbType.Single;
			IDataParameter numberParameter=createDataParameter(this.colNumber,DBNull.Value);
			//numberParameter.DbType=DbType.Decimal;
			IDataParameter itemKeyParameter=createDataParameter(this.colItemKey,key);
			IDataParameter globalKeyParameter=createDataParameter(this.colGlobalKey,globalKey);
			IDataParameter itemTypeParameter=createDataParameter(this.colItemType,type);

			

			
			switch (type)
			{
				case PropertySet_Fields.BOOLEAN:

					Boolean boolVal = (Boolean) o;
					numberParameter.Value= boolVal ? 1 : 0;

					
					break;


				case PropertySet_Fields.DATA:

					if (o is Data)
					{
						Data data = (Data) o;
						dataParameter.Value=data.Bytes;
						
						
					}

					if (o is byte[])
					{
						//Data data = (Data) value_Renamed;
						dataParameter.Value=(byte[]) o;
						
						
					}

					break;


				case PropertySet_Fields.OBJECT:
					if ((o.GetType().Attributes & TypeAttributes.Serializable)!=TypeAttributes.Serializable)
						throw new PropertyException(o.GetType() + " does not mark Serializable attribute");


					MemoryStream bos = new MemoryStream();

					try
					{
						BinaryWriter os = new BinaryWriter(bos);
						SupportClass.Serialize(os, o);
						dataParameter.Value=bos.ToArray();
						os.Close();

						//dataParameter=createDataParameter(this.colData,SupportClass.ToSByteArray(bos.ToArray()));
						
					}
					catch (IOException e)
					{
						throw new PropertyException("I/O Error when serializing object:" + e);
					}

					break;


				case PropertySet_Fields.DATE:

					DateTime date = (DateTime) o;
					datetimeParameter.Value=date;
					

					break;


				case PropertySet_Fields.DOUBLE:

					Double d = (Double) o;

					singleParameter=createDataParameter(this.colFloat,d);
					

					break;


				case PropertySet_Fields.INT:

					Int32 i = (Int32) o;
					numberParameter=createDataParameter(this.colNumber,i);
					

					break;


				case PropertySet_Fields.LONG:

					Int64 l = (Int64) o;
					numberParameter.Value=l;


					break;


				case PropertySet_Fields.STRING:
					stringParameter.Value=(String)o;
					

					break;


				default:
					throw new PropertyException("This type isn't supported!");

			}
			ps.Parameters.Add(stringParameter);
			ps.Parameters.Add(datetimeParameter);
			ps.Parameters.Add(dataParameter);
			ps.Parameters.Add(singleParameter);
			ps.Parameters.Add(numberParameter);
		
			ps.Parameters.Add(itemKeyParameter);
			ps.Parameters.Add(globalKeyParameter);
			ps.Parameters.Add(itemTypeParameter);
		}

		
		static AdoPropertySet()
		{
			log = LogManager.GetLogger(typeof (AdoPropertySet));
		}
	}
}