using System; 
using UnityEngine; 
 
public class EncryptedPlayerPrefs 
{ 
    public static string NOT_FOUND = "encrypted_player_prefs_get_string_not_found"; 
    public static void DeleteAll() 
    { 
        PlayerPrefs.DeleteAll(); 
    } 
 
    public static void DeleteKey(string key) 
    { 
        PlayerPrefs.DeleteKey(key); 
    } 
 
    public static float GetFloat(string key, float defaultValue) 
    { 
        try { 
            string value = PlayerPrefs.GetString(key, NOT_FOUND); 
            if (value.Equals(NOT_FOUND)) 
            { 
                return defaultValue; 
            } 
            return Crypto.DecryptFloat(value); 
        } catch (FormatException e) { 
            return defaultValue; 
        } 
} 
 
    public static int GetInt(string key, int defaultValue) 
    { 
        try {  
            string value = PlayerPrefs.GetString(key, NOT_FOUND); 
            if (value.Equals(NOT_FOUND)) 
            { 
                return defaultValue; 
            } 
            return Crypto.DecryptInt(value); 
        } catch (FormatException e) { 
            return defaultValue; 
        } 
    } 
 
    private static bool ValidUID(string uID) 
    { 
        return uID.StartsWith("i") && uID.EndsWith("i") && (uID.Length == 28); 
    } 
 
    public string[] Unique(string[] list) 
    { 
        System.Collections.ArrayList newList = new System.Collections.ArrayList(); 
 
        foreach (string str in list) 
            if (!newList.Contains(str)) 
                newList.Add(str); 
        return (string[])newList.ToArray(typeof(string)); 
    } 
 
    public static string GetStringAsSetUsingDelimiter(string key, char separator, string defaultValue) 
    { 
        string list = EncryptedPlayerPrefs.GetString(key, ""); 
 
        if (list.Equals("")) 
        { 
            return defaultValue; 
        } 
 
        string[] entries = list.Split(separator); 
 
        System.Collections.ArrayList newList = new System.Collections.ArrayList(); 
 
        foreach (string str in entries) 
            if (!newList.Contains(str)) 
                newList.Add(str); 
 
        string result = ""; 
        foreach (string entry in newList) 
        { 
            result = result + entry + separator; 
        } 
 
        result.TrimEnd(separator); 
        return result; 
    } 
 
    public static string GetString(string key, string defaultValue) 
    { 
        try 
        { 
            string value = PlayerPrefs.GetString(key, NOT_FOUND); 
 
            if (key.Equals("uID") && (!ValidUID(Crypto.DecryptString(value)))) 
            { 
                Debug.LogWarning("PlayerPrefs.GetString returned " + value + " which we think is garbage. Defauting to NOT_FOUND"); 
                value = NOT_FOUND; 
            } 
            if (value.Equals(NOT_FOUND)) 
            { 
                return defaultValue; 
            } 
            return Crypto.DecryptString(value); 
        } catch (Exception e) { 
            Debug.LogWarning("PlayerPrefs.GetString Exception " + e); 
            return defaultValue; 
        } 
    } 
 
    public static bool HasKey(string key) 
    { 
        return PlayerPrefs.HasKey(key); 
    } 
 
    public static void Save() 
    { 
        PlayerPrefs.Save(); 
    } 
 
    public static void SetFloat(string key, float value) 
    { 
        PlayerPrefs.SetString(key, Crypto.EncryptFloat (value)); 
    } 
 
    public static void SetInt(string key, int value) 
    { 
        PlayerPrefs.SetString(key, Crypto.EncryptInt(value)); 
    } 
 
    public static void SetString(string key, string value) 
    { 
        PlayerPrefs.SetString(key, Crypto.EncryptString(value)); 
    } 
} 
