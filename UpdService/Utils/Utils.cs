using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace UpdService.Utils
{
    public static class Utils
    {
        public static string ComputeMD5Checksum(byte[] fileData)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checkSum = md5.ComputeHash(fileData);
            string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
            return result;
        }

        public static string ComputeMD5Checksum(Stream fileData)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checkSum = md5.ComputeHash(fileData);
            string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
            return result;
        }
    }
}
