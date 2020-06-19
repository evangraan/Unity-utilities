using System.Collections; 
using UnityEngine; 
using UnityEngine.UI; 
 
public class UIUtilities 
{  
    public static IEnumerator DownloadTextureIntoImageFromURL(string Url, RawImage destinationImage, bool issueCommsError = false) 
    { 
        bool success = false; 
        do 
        { 
            using (WWW www = new WWW(Url)) 
            { 
                while (!www.isDone) 
                { 
                    yield return null; 
                } 
 
                if (string.IsNullOrEmpty(www.error)) 
                { 
                    GameState.Instance.CommsOK(); 
                    success = true; 
                    if ((destinationImage != null) && (www.texture != null)) 
                    { 
                        destinationImage.texture = www.texture; 
                        destinationImage.SetNativeSize(); 
                    } else 
                    { 
                        Debug.LogWarning("null encountered when setting destinationImage.texture!"); 
                    } 
                } 
                else 
                { 
                    Debug.LogError("Could not download " + Url + " : " + www.error); 
                    if (issueCommsError) 
                    { 
                        GameState.Instance.CommsError(); 
                        yield return new WaitForSeconds(1f); 
                    } 
                    else 
                    { 
                        GameController.Instance.GetPlayerToasts().PopupText(GameState.MESSAGE_CONNECTION_ISSUES); 
                    } 
                } 
            } 
        } while (issueCommsError && !success); 
 
        yield return null; 
    } 
 
    public static void HideGameObject(GameObject gameObject) // Instead of SetActive(false), allowing co-routines to still act on the object while hidden 
    { 
        if (gameObject) 
        { 
            gameObject.transform.localScale = new Vector3(0, 0, 0); 
        } 
    } 
 
    public static void ShowGameObject(GameObject gameObject, int scaleFactor = 1) 
    { 
        if (gameObject) 
        { 
            gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor); 
        } 
    } 
 
 
    public static void DeactivateObject(GameObject gameObject) 
    { 
        if (gameObject) 
        { 
            gameObject.SetActive(false); 
        } 
    } 
 
    public static string RemoveCharacters(string value, char del) 
    { 
        string result = ""; 
        foreach (char c in value) 
        { 
            if (c != del) 
            { 
                result = result + c; 
            } 
        } 
        return result; 
    } 
} 
