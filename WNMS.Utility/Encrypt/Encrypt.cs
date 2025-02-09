﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WNMS.Utility.Encrypt
{
    public class Encrypt
    {
        /// <summary> 
        /// MD5 加密字符串 
        /// </summary> 
        /// <param name="rawPass">源字符串</param> 
        /// <returns>加密后字符串</returns> 
        private static string MD5Encoding(string rawPass)
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider 
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(rawPass);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化 
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary> 
        /// MD5盐值加密 
        /// </summary> 
        /// <param name="rawPass">源字符串</param> 
        /// <param name="salt">盐值</param> 
        /// <returns>加密后字符串</returns> 
        public static string MD5Encoding(string rawPass, object salt)
        {
            string md5 = MD5Encoding(rawPass);
            if (salt == null)
            {
                return md5;
            }
            return MD5Encoding(md5 + "{" + salt.ToString() + "}");
        }
    }
}
