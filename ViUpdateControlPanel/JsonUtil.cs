using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ViUpdateControlPanel
{
    public static class JsonUtil
    {

        //todo объединить эти методы, думаю и один справится 
        public static string ToJsonString<T>(this T o)
        {
            return JsonUtil<T>.ObjToStr(o);
        }

        public static string ToJsonString(this object o, Type t)
        {
            DataContractJsonSerializer Ser = new DataContractJsonSerializer(t);
            MemoryStream stream1 = new MemoryStream();
            Ser.WriteObject(stream1, o);
            stream1.Position = 0;
            StreamReader reader = new StreamReader(stream1);
            string text = reader.ReadToEnd();
            return text;
        }

        public static object ToObject(string s, Type t)
        {
            DataContractJsonSerializer Ser = new DataContractJsonSerializer(t);
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return Ser.ReadObject(stream);
        }
    }

    public class JsonUtil<T>
    {
        private static readonly DataContractJsonSerializer Ser = new DataContractJsonSerializer(typeof(T));

        public static string ObjToStr(T o)
        {
            MemoryStream stream1 = new MemoryStream();
            Ser.WriteObject(stream1, o);
            stream1.Position = 0;
            StreamReader reader = new StreamReader(stream1);
            string text = reader.ReadToEnd();
            return text;
        }

        public static T ObjFromStr(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return (T)Ser.ReadObject(stream);
        }
    }
}
