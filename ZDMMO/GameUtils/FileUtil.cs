using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ZDMMO
{
    public class FileUtil
    {
        public static void SaveData(string filePath, Object serializableObj)
        {
            Stream fStream = null;
            try
            {
                fStream = new FileStream(filePath, FileMode.OpenOrCreate);
                BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
                binFormat.Serialize(fStream, serializableObj);
                fStream.Flush();
            }
            finally
            {
                if (fStream != null)
                    fStream.Close();
            }
        }

        public static T LoadData<T>(string filePath)
        {
            Stream fStream = null;
            try {
                fStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
                fStream.Position = 0;//重置流位置
                return (T)binFormat.Deserialize(fStream);//反序列化对象
            }
            finally
            {
                if (fStream != null)
                    fStream.Close();
            }
        }

        public static string ReadFile(string filePath)
        {
            StringBuilder str = new StringBuilder();
            using (FileStream fs = File.OpenRead(filePath))
            {
                long left = fs.Length;
                int maxLength = 100;//每次读取的最大长度  
                int start = 0;//起始位置  
                int num = 0;//已读取长度  
                while (left > 0)
                {
                    byte[] buffer = new byte[maxLength];//缓存读取结果  
                    char[] cbuffer = new char[maxLength];
                    fs.Position = start;//读取开始的位置  
                    num = 0;
                    if (left < maxLength)
                    {
                        num = fs.Read(buffer, 0, Convert.ToInt32(left));
                    }
                    else
                    {
                        num = fs.Read(buffer, 0, maxLength);
                    }
                    if (num == 0)
                    {
                        break;
                    }
                    start += num;
                    left -= num;
                    str = str.Append(Encoding.UTF8.GetString(buffer));
                }
            }
            return str.ToString();
        }
    }
}
