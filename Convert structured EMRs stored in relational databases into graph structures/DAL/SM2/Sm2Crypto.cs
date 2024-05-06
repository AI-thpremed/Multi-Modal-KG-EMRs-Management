using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;
using System;

using System.Text;

/// <summary>
/// Sm2算法   
/// 对标国际RSA算法
/// </summary>
public class Sm2Crypto
{
    /// <summary>
    /// 数据
    /// </summary>
    public string Str { get; set; }
    /// <summary>
    /// 数据
    /// </summary>
    public byte[] Data { get; set; }
    /// <summary>
    /// 公钥
    /// </summary>
    //public string PublicKey { get; set; }

    public string PublicKey = "04F06034E316A92C5AA903DCFE4C6AB6F183EB3714A04619C54AD5004E198CB2D975CC173FDBA02A2BF8B189407398A1E548FC3A2B0AF37C869F06344FF6022AB6";



    /// <summary>
    /// 私钥
    /// </summary>
    //public string PrivateKey { get; set; }
    public string PrivateKey= "00C0D0139BFEF574170D62CCBBEB51C927A03E305AA49413351695EC6E97804E86";




    /// <summary>
    /// 获取密钥
    /// </summary>
    /// <param name="privateKey">私钥</param>
    /// <param name="publicKey">公钥</param>
    public static void GetKey(out string privateKey, out string publicKey)
    {
        SM2 sm2 = SM2.Instance;
        AsymmetricCipherKeyPair key = sm2.EccKeyPairGenerator.GenerateKeyPair();
        ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
        ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
        publicKey = Encoding.Default.GetString(Hex.Encode(ecpub.Q.GetEncoded())).ToUpper();
        privateKey = Encoding.Default.GetString(Hex.Encode(ecpriv.D.ToByteArray())).ToUpper();
    }

    #region 解密
    public object Decrypt(Sm2Crypto entity)
    {
        if (String.IsNullOrEmpty(entity.Str))
        {
            return "";

        }
        else
        {

            var data = !string.IsNullOrEmpty(entity.Str) ?
                Hex.Decode(entity.Str) :
                entity.Data;
            return Encoding.Default.GetString(Decrypt(Hex.Decode(entity.PrivateKey), data));

        }

    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="privateKey"></param>
    /// <param name="encryptedData"></param>
    /// <returns></returns>
    private static byte[] Decrypt(byte[] privateKey, byte[] encryptedData)
    {
        if (null == privateKey || privateKey.Length == 0)
        {
            return null;
        }
        if (encryptedData == null || encryptedData.Length == 0)
        {
            return null;
        }

        String data = Encoding.Default.GetString(Hex.Encode(encryptedData));

        byte[] c1Bytes = Hex.Decode(Encoding.Default.GetBytes(data.Substring(0, 130)));
        int c2Len = encryptedData.Length - 97;
        byte[] c2 = Hex.Decode(Encoding.Default.GetBytes(data.Substring(130, 2 * c2Len)));
        byte[] c3 = Hex.Decode(Encoding.Default.GetBytes(data.Substring(130 + 2 * c2Len, 64)));

        SM2 sm2 = SM2.Instance;
        BigInteger userD = new BigInteger(1, privateKey);

        ECPoint c1 = sm2.EccCurve.DecodePoint(c1Bytes);
        //c1.Normalize();
        Cipher cipher = new Cipher();
        cipher.InitDec(userD, c1);
        cipher.Decrypt(c2);
        cipher.Dofinal(c3);

        return c2;
    }
    #endregion

    #region 加密
    public string Encrypt(Sm2Crypto entity)
    {
        if (String.IsNullOrEmpty(entity.Str))
        {
            return "";
        }
        else
        {
            var data = !string.IsNullOrEmpty(entity.Str) ?
                Encoding.Default.GetBytes(entity.Str) :
                entity.Data;
            return Encrypt(Hex.Decode(entity.PublicKey), data);

        }


    }
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="publicKey"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private static string Encrypt(byte[] publicKey, byte[] data)
    {
        if (null == publicKey || publicKey.Length == 0)
        {
            return null;
        }
        if (data == null || data.Length == 0)
        {
            return null;
        }

        byte[] source = new byte[data.Length];
        Array.Copy(data, 0, source, 0, data.Length);

        Cipher cipher = new Cipher();
        SM2 sm2 = SM2.Instance;

        ECPoint userKey = sm2.EccCurve.DecodePoint(publicKey);
        //userKey.Normalize();
        ECPoint c1 = cipher.InitEnc(sm2, userKey);
        cipher.Encrypt(source);

        byte[] c3 = new byte[32];
        cipher.Dofinal(c3);

        String sc1 = Encoding.Default.GetString(Hex.Encode(c1.GetEncoded()));
        String sc2 = Encoding.Default.GetString(Hex.Encode(source));
        String sc3 = Encoding.Default.GetString(Hex.Encode(c3));

        return (sc1 + sc2 + sc3).ToUpper();
    }
    #endregion
}