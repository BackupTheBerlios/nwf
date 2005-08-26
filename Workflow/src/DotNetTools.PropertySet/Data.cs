namespace DotNetTools.PropertySet
{
	public sealed class Data
	{
		//~ Instance fields ////////////////////////////////////////////////////////

		private byte[] bytes;

		//~ Constructors ///////////////////////////////////////////////////////////

		public Data()
		{
		}

		public Data(byte[] bytes)
		{
			this.bytes = bytes;
		}

		//~ Methods ////////////////////////////////////////////////////////////////


		public byte[] Bytes
		{
			set{this.bytes=value;}
			get{return this.bytes;}
		}
	
	}

}