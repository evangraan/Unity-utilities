using System.Collections.Generic; 
using UnityEngine; 
using System; 
using System.Text; 
using System.Security.Cryptography; 
 
public class Crypto 
{ 
    public static MD5 md5 = null; 
    public static long FLOAT_PRECISION = 10000000; 
    public static int INT4_SATURATED = -1; 
    public static Dictionary<string, string> headers = null; 
 
    public static int[] EncryptDecrypt(int[] data, string keys) 
    { 
        byte[] key = Encoding.UTF8.GetBytes(keys); 
        byte[] input = Crypto.IntArrayToByteArray(data); 
 
        for (int i = 0; i < input.Length; i++) 
            input[i] = (byte)(input[i] ^ key[i % key.Length]); 
 
        int[] output = new int[data.Length]; 
        for (int i = 0; i < output.Length; i++) 
        { 
            output[i] = BitConverter.ToInt32(input, i * 4); 
        } 
 
        return output; 
    } 
 
    public static string SHA1FromString(string value) 
    { 
        return Crypto.ByteArrayToBase64String(Crypto.IntArrayToByteArray(Crypto.SHA1(Crypto.ByteArrayToIntArray(Crypto.Base64StringToByteArray(value))))); 
    } 
 
    public static int[] SHA1(int[] data) 
    { 
        SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider(); 
        byte[] input = Crypto.IntArrayToByteArray(data); 
        string hash = Convert.ToBase64String(sha1.ComputeHash(input)); 
 
        int[] output = new int[hash.Length]; 
        int i = 0; 
        foreach (char c in hash) 
        { 
            output[i] = Convert.ToInt32(c); 
            i++; 
        } 
        return output; 
    } 
 
    public static bool Matched(int[] a, int[] b) 
    { 
        if (a == null) 
        { 
            return false; 
        } 
 
        if (b == null) 
        { 
            return false; 
        } 
 
        if (a.Length != b.Length) 
        { 
            return false; 
        } 
 
        int n = a.Length; 
        for (int i = 0; i < n; i++) 
        { 
            if (a[i] != b[i]) 
            { 
                return false; 
            } 
        } 
 
        return true; 
    } 
 
    public static string IntArrayToString(int[] data) 
    { 
        byte[] byteArray = Crypto.IntArrayToByteArray(data); 
        return Crypto.ByteArrayToBase64String(byteArray); 
    } 
 
    public static int[] StringToIntArray(string value) 
    { 
        byte[] byteArray = Crypto.Base64StringToByteArray(value); 
        return Crypto.ByteArrayToIntArray(byteArray); 
    } 
 
    public static string IntArrayToStringPadded(int[] data) 
    { 
        byte[] byteArray = Crypto.IntArrayToByteArrayPadded(data); 
        return Encoding.UTF8.GetString(byteArray); 
    } 
 
    public static int[] StringToIntArrayPadded(string value) 
    { 
        byte[] byteArray = Encoding.UTF8.GetBytes(value); 
        return Crypto.ByteArrayToIntArrayPadded(byteArray); 
    } 
 
    public static byte[] IntArrayToByteArray(int[] data) 
    { 
        byte[] bytes = new byte[data.Length * sizeof(int)]; 
        Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length); 
        return bytes; 
    } 
 
    public static int[] ByteArrayToIntArray(byte[] data) 
    { 
        int[] output = new int[data.Length / sizeof(int)]; 
        for (int i = 0; i < output.Length; i++) 
        { 
            output[i] = BitConverter.ToInt32(data, i * sizeof(int)); 
        } 
        return output; 
    } 
 
    public static byte[] IntArrayToByteArrayPadded(int[] data) 
    { 
        int padding = 0; 
        if (data.Length >= 5) 
        { 
            if ((data[data.Length - 2] == INT4_SATURATED) && (data[data.Length - 3] == INT4_SATURATED) && (data[data.Length - 4] == INT4_SATURATED)) 
            { 
                padding = data[data.Length - 1]; 
            } 
        } 
 
        int size = data.Length; 
        int n = 0; 
        if (padding > 0) 
        { 
            size -= 5; 
            n = padding; 
        } 
        n = n + size * sizeof(int); 
        byte[] bytes = new byte[n]; 
        Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length); 
 
        return bytes; 
    } 
 
    public static int[] ByteArrayToIntArrayPadded(byte[] data) 
    { 
        int size = data.Length / sizeof(int); 
        int mod = data.Length % sizeof(int); 
        if (mod > 0) { 
            size = size + 5; 
            byte[] padded = new byte[size * sizeof(int)]; 
            for (int i = 0; i < data.Length; i++) 
            { 
                padded[i] = data[i]; 
            } 
            int paddedSize = size * sizeof(int) - sizeof(int); 
            for (int j = data.Length; j < paddedSize; j++) 
            { 
                padded[j] = 255; 
            } 
 
            int[] padmoda = new int[1]; 
            padmoda[0] = mod; 
            byte[] padmod = new byte[sizeof(int)]; 
            Buffer.BlockCopy(padmoda, 0, padmod, 0, 1); 
            padded[paddedSize] = padmod[0]; 
            padded[paddedSize + 1] = padmod[1]; 
            padded[paddedSize + 2] = padmod[2]; 
            padded[paddedSize + 3] = padmod[3]; 
 
            data = (byte[])padded.Clone(); 
        } 
        int[] output = new int[size]; 
        for (int i = 0; i < output.Length; i++) 
        { 
            output[i] = BitConverter.ToInt32(data, i * sizeof(int)); 
        } 
        return output; 
    } 
 
    public static string ByteArrayToBase64String(byte[] data) 
    { 
        return System.Convert.ToBase64String(data); 
    } 
 
    public static byte[] Base64StringToByteArray(string base64) 
    { 
        return System.Convert.FromBase64String(base64); 
    } 
 
    public static MD5 GetMD5() 
    { 
        if (Crypto.md5 == null) 
        { 
            Crypto.md5 = new MD5CryptoServiceProvider(); 
        } 
        return Crypto.md5; 
    } 
 
    public static string UniqueDeviceIdentifier() 
    { 
        return SystemInfo.deviceUniqueIdentifier; 
    } 
 
    public static string GetAnonymousDeviceIdentifier() 
    { 
        return Crypto.ByteArrayToBase64String(Crypto.GetMD5().ComputeHash(Encoding.UTF8.GetBytes(Crypto.UniqueDeviceIdentifier()))); 
    } 
 
    public static Dictionary<string, string> GetHeaders() 
    { 
        if (headers == null) 
        { 
            headers = new Dictionary<string, string>(); 
            headers.Add("Content-Type", "application/json"); 
            headers.Add("X-API-KEY", "EXAMPLE-API-KEY"); 
        } 
        return headers; 
    } 
 
    public static double TimeSinceEpoch() 
    { 
        return DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; 
    } 
 
    public static string EncryptString(string value) 
    { 
        int[] data = StringToIntArrayPadded(value); 
        int[] encrypted = Crypto.EncryptDecrypt(data, CharacterPool.POOL); 
        int[] encrypted2 = Crypto.EncryptDecrypt(encrypted, Crypto.UniqueDeviceIdentifier()); 
        data = (int[])encrypted2.Clone(); 
        return IntArrayToString(data); 
    } 
 
    public static string DecryptString(string value) 
    { 
        int[] data = Crypto.StringToIntArray(value); 
        int[] decrypted = Crypto.EncryptDecrypt(data, Crypto.UniqueDeviceIdentifier()); 
        int[] decrypted2 = Crypto.EncryptDecrypt(decrypted, CharacterPool.POOL); 
        int[] sData = (int[])decrypted2.Clone(); 
        return IntArrayToStringPadded(sData); 
    } 
 
    public static string EncryptFloat(float value) 
    { 
        long lValue = (long)(value * FLOAT_PRECISION); 
        string sValue = lValue.ToString(); 
        return EncryptString(sValue); 
    } 
 
    public static float DecryptFloat(string value) 
    { 
        string sValue = DecryptString(value); 
        long lValue = Convert.ToInt64(sValue); 
        return (float)(lValue / FLOAT_PRECISION); 
    } 
 
    public static string EncryptInt(int value) 
    { 
        string sValue = value.ToString(); 
        return EncryptString(sValue); 
    } 
 
    public static int DecryptInt(string value) 
    { 
        string sValue = DecryptString(value); 
        int iValue = Convert.ToInt32(sValue); 
        return iValue; 
    } 
 
    public static void TestCrypto() 
    { 
        string a = "a1234567890 098765434"; 
        string b = Crypto.EncryptString(a); 
        string c = Crypto.DecryptString(b); 
        Debug.Log("a:[" + a + "] b:[" + b + "] c:[" + c + "]"); 
 
        int d = -151425; 
        string e = Crypto.EncryptInt(d); 
        int f = Crypto.DecryptInt(e); 
        Debug.Log("d[" + d + "] e:[" + e + "] f:[" + f + "]"); 
 
        float g = -15.142534567f; 
        string h = Crypto.EncryptFloat(g); 
        float i = Crypto.DecryptInt(h); 
        Debug.Log("g[" + g + "] h:[" + h + "] i:[" + i + "]"); 
    } 
} 
 
[Serializable] 
public class SHAVerified{ 
  public int[] verifyData; 
 
  public SHAVerified(int[] theVerifyData){ 
    verifyData = theVerifyData; 
  } 
} 
