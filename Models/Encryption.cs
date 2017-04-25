using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Utilities
{
    public class Encryption
    {
        private static byte[] KEY_192 = 
			{
				111,21,12,65,21,12,2,1,
				5,30,34,78,98,1,32,122,
				123,124,125,126,212,212,213,214
			};

        private static byte[] IV_192 = 
			{
				1,2,3,4,5,12,13,14,
				13,14,15,13,17,21,22,23,
				24,25,121,122,122,123,124,124
			};

        public static String EncryptTripleDES(String vl)
        {

            //encrypt
            if (vl != "")
            { //if
                TripleDESCryptoServiceProvider cryptoprovider = new TripleDESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, cryptoprovider.CreateEncryptor(KEY_192, IV_192), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);
                sw.Write(vl);
                sw.Flush();
                cs.FlushFinalBlock();
                ms.Flush();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            } //if
            return "";
        } //encrypt

        public static String DecryptTripleDES(String vl)
        { //encrypt
            if (vl != "")
            { //if
                TripleDESCryptoServiceProvider cryptoprovider = new TripleDESCryptoServiceProvider();
                Byte[] buffer = Convert.FromBase64String(vl);
                MemoryStream ms = new MemoryStream(buffer);
                CryptoStream cs = new CryptoStream(ms, cryptoprovider.CreateDecryptor(KEY_192, IV_192), CryptoStreamMode.Read);
                StreamReader sw = new StreamReader(cs);
                return sw.ReadToEnd();

            } //if
            return "";
        } //decrypt

    } //class
}