using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class CustomTypeSerializer
{
    public static byte[] ObjectToByteArray(object obj)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (MemoryStream memoryStream = new MemoryStream())
        {
            binaryFormatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }
    }

    public static object ByteArrayToObject(byte[] byteArr)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (MemoryStream memoryStream = new MemoryStream(byteArr))
        {
            return binaryFormatter.Deserialize(memoryStream);
        }
    }
}
