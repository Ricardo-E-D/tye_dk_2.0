using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace tye {
	public class Helpers {

		public static byte[] Serialize(object data) {
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream streamMemory = new MemoryStream();
			formatter.Serialize(streamMemory, data);
			byte[] binaryData = streamMemory.GetBuffer();
			streamMemory.Close();
			return binaryData;
		}

		public static object Deserialize(byte[] binaryData) {
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream streamMemory = new MemoryStream(binaryData);
			object data = formatter.Deserialize(streamMemory);
			streamMemory.Close();
			return data;
		}

	}
}
