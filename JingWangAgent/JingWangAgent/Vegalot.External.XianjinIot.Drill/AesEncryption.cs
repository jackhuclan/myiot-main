// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;

namespace Vegalot.External.XianjinIot.Drill
{
    internal class AesEncryption
    {
        public static byte[] GenerateAESKey(int size)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = size;
                aes.GenerateKey();
                return aes.Key;
            }
        }

        public static string GenerateAESKeyString(int size)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = size;
                    aes.GenerateKey();
                    return Convert.ToBase64String(aes.Key);
                }
            }
            catch (Exception)
            {

                return "";
            }


        }
        public static string AesEncrypt(int plainInt, string key)
        {
            // 检查密钥长度
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            {
                throw new ArgumentException("AES密钥长度必须是16、24或32字节");
            }

            // 将密钥转换为字节数组
            byte[] keyBytes = Convert.FromBase64String(key);

            // 将int转换为字节数组
            byte[] plainBytes = BitConverter.GetBytes(plainInt);

            // 创建Aes对象
            using (Aes aes = Aes.Create())
            {
                // 设置密钥
                aes.Key = keyBytes;

                // 创建加密器
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // 创建内存流
                using (MemoryStream ms = new MemoryStream())
                {
                    // 将IV写入内存流
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    // 创建加密流
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // 将明文字节数组写入加密流
                        cs.Write(plainBytes, 0, plainBytes.Length);
                    }

                    // 获取加密后的字节数组
                    byte[] encryptedBytes = ms.ToArray();

                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        public static int AesDecrypt(byte[] cipherBytes, string key)
        {
            // 检查密钥长度
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            {
                throw new ArgumentException("AES密钥长度必须是16、24或32字节");
            }

            // 将密钥转换为字节数组
            byte[] keyBytes = Convert.FromBase64String(key);

            // 创建Aes对象
            using (Aes aes = Aes.Create())
            {
                // 设置密钥
                aes.Key = keyBytes;

                // 从密文字节数组中提取IV
                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(cipherBytes, iv, iv.Length);

                // 设置IV
                aes.IV = iv;

                // 创建解密器
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // 创建内存流
                using (MemoryStream ms = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length))
                {
                    // 创建解密流
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // 创建字节数组用于存储解密后的数据
                        byte[] decryptedBytes = new byte[4];
                        cs.Read(decryptedBytes, 0, decryptedBytes.Length);

                        // 将字节数组转换为int
                        return BitConverter.ToInt32(decryptedBytes, 0);
                    }
                }
            }
        }


        public static int AesDecrypt(string cipherString, string key)
        {
            // 检查密钥长度
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            {
                throw new ArgumentException("AES密钥长度必须是16、24或32字节");
            }

            // 将密钥转换为字节数组
            byte[] keyBytes = Convert.FromBase64String(key);

            // 创建Aes对象
            using (Aes aes = Aes.Create())
            {
                // 设置密钥
                aes.Key = keyBytes;

                // 从密文字节数组中提取IV
                byte[] iv = new byte[aes.BlockSize / 8];
                var cipherBytes = Convert.FromBase64String(cipherString);
                Array.Copy(cipherBytes, iv, iv.Length);

                // 设置IV
                aes.IV = iv;

                // 创建解密器
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // 创建内存流
                using (MemoryStream ms = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length))
                {
                    // 创建解密流
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // 创建字节数组用于存储解密后的数据
                        byte[] decryptedBytes = new byte[4];
                        cs.Read(decryptedBytes, 0, decryptedBytes.Length);

                        // 将字节数组转换为int
                        return BitConverter.ToInt32(decryptedBytes, 0);
                    }
                }
            }
        }


    }
}
