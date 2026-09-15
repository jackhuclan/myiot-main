// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Vegalot.External.XianjinIot.Drill;
public class RSAUtils
{
    /// <summary>
    /// 公钥加密
    /// </summary>
    /// <param name="publicKey"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string Encrypt(string content, string publicKey)
    {
        if (string.IsNullOrWhiteSpace(publicKey))
        {
            throw new Exception("公钥PublicKey不能为空");
        }
        var rsa = LoadPublicKey(publicKey);
        byte[] encrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// 私钥解密
    /// </summary>
    /// <param name="privateKey"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string Decrypt(string content, string privateKey)
    {
        if (string.IsNullOrWhiteSpace(privateKey))
        {
            throw new Exception("私钥PrivateKey不能为空");
        }
        var rsa = LoadPrivateKey(privateKey);
        byte[] encrypted = rsa.Decrypt(Encoding.UTF8.GetBytes(content), false);
        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// 加载PEM格式私钥(PKCS8编码的PEM私钥)
    /// </summary>
    /// <param name="pemKey"></param>
    /// <returns></returns>
    public static RSACryptoServiceProvider LoadPrivateKey(string pemKey)
    {
        var rsa = new RSACryptoServiceProvider();
        var keyPair = PrivateKeyFactory.CreateKey(Convert.FromBase64String(pemKey)) as RsaPrivateCrtKeyParameters;
        if (keyPair == null)
        {
            throw new Exception("加载私钥失败");
        }
        RSAParameters parameters = new RSAParameters
        {
            Modulus = keyPair.Modulus.ToByteArrayUnsigned(),
            Exponent = keyPair.PublicExponent.ToByteArrayUnsigned(),
            D = keyPair.Exponent.ToByteArrayUnsigned(),
            P = keyPair.P.ToByteArrayUnsigned(),
            Q = keyPair.Q.ToByteArrayUnsigned(),
            DP = keyPair.DP.ToByteArrayUnsigned(),
            DQ = keyPair.DQ.ToByteArrayUnsigned(),
            InverseQ = keyPair.QInv.ToByteArrayUnsigned()
        };
        rsa.ImportParameters(parameters);
        return rsa;
    }

    /// <summary>
    /// 加载PEM格式公钥(X.509格式PEM公钥)
    /// </summary>
    /// <param name="pemKey"></param>
    /// <returns></returns>
    public static RSACryptoServiceProvider LoadPublicKey(string pemKey)
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        var p = PublicKeyFactory.CreateKey(Convert.FromBase64String(pemKey)) as RsaKeyParameters;
        if (p == null)
        {
            throw new Exception("加载公钥失败");
        }
        RSAParameters rsaParameters = new RSAParameters()
        {

            Modulus = p.Modulus.ToByteArrayUnsigned(),
            Exponent = p.Exponent.ToByteArrayUnsigned(),
        };
        rsa.ImportParameters(rsaParameters);
        return rsa;
    }

}
