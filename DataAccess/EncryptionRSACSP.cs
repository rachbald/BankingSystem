using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Security.Cryptography;

namespace Utilities
{
    public class EncryptionRSACSP
    {
        public static string roundtrip;
        public static ASCIIEncoding textConverter = new ASCIIEncoding();
        public static RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
        public static byte[] fromEncrypt;
        public static byte[] encrypted;
        public static byte[] toEncrypt;
        //    public static byte[] key;
        //public static byte[] IV;


        private static byte[] IV = 
			{
				68,161,244,217,250,221,92,227
			};

        private static byte[] key = 
           {
                111,21,12,65,21,12,2,1,
               1,2,12,21,65,12,21,111,
           };

        public static String EncryptTripleDES(String vl)
        {

            //encrypt
            if (vl != "")
            { //if
                ICryptoTransform encryptor = rc2CSP.CreateEncryptor(key, IV);

                TripleDESCryptoServiceProvider cryptoprovider = new TripleDESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
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
        {
            String roundtrip = "";
            try
            {

                fromEncrypt = textConverter.GetBytes(vl);

                //Get a decryptor that uses the same key and IV as the encryptor.
                ICryptoTransform decryptor = rc2CSP.CreateDecryptor(key, IV);

                //Now decrypt the previously encrypted message using the decryptor
                // obtained in the above step.
                MemoryStream msDecrypt = new MemoryStream(encrypted);
                CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

                fromEncrypt = new byte[encrypted.Length];

                //Read the data out of the crypto stream.
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                //Convert the byte array back into a string.
                roundtrip = textConverter.GetString(fromEncrypt);

                return roundtrip;
            }
            catch (Exception e)
            { }

            return roundtrip;
            return "";
        } //decrypt
    }
}
