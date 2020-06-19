using System.Collections; 
using UnityEngine; 
 
public class InternetConnectionCheck : MonoBehaviour 
{ 
    public static float CONNECTION_QUALITY_THRESHOLD = 3f; 
    public static float CONNECTION_TIMEOUT_THRESHOLD = 6f; 
    private float deltaTime; 
    private bool reachable = true; 
    public GameObject noConnectionPanel; 
    private bool badPingLatch = false; 
 
    private void Start() 
    { 
        StartCoroutine(Ping()); 
    } 
 
    void Update() 
    { 
        if (noConnectionPanel == null) 
        { 
            return; 
        } 
 
        deltaTime += Time.deltaTime; 
        if (deltaTime > 0.5f) 
        { 
            deltaTime = 0f; 
 
            bool connected = true; 
 
 
            if (!GameState.IsInternetConnected()) 
            { 
                Debug.LogError("No connection"); 
                GameController.Instance.brokenConnectivity.SetActive(true); 
                connected = false; 
            } 
 
            if (!reachable) 
            { 
                connected = false; 
            } 
 
            if (connected) 
            { 
                noConnectionPanel.SetActive(false); 
                GameController.Instance.brokenConnectivity.SetActive(false); 
            } 
            else 
            { 
                if ((GameState.Instance != null) && 
                    (GameState.Instance.sceneManager != null) && 
                    (!GameState.Instance.sceneManager.IsArcadeScene())) 
                { 
                    noConnectionPanel.SetActive(true); 
                } 
            } 
        } 
    } 
 
    private IEnumerator Ping() 
    { 
        float deltaTime = 0f; 
        while (true) 
        { 
            deltaTime = Time.time; 
            Ping google = new Ping("8.8.8.8"); 
            bool breakout = false; 
            while (!google.isDone && !breakout) 
            { 
                if (Time.time - deltaTime > CONNECTION_TIMEOUT_THRESHOLD) 
                { 
                    breakout = true; 
                } 
                yield return null; 
            } 
 
            if (((google.time / 1000f) > CONNECTION_QUALITY_THRESHOLD) || breakout) 
            { 
                if (breakout) 
                { 
                    Debug.LogWarning("Ping unreturned (or unresolved) for " + CONNECTION_QUALITY_THRESHOLD + " seconds"); 
                } 
                else 
                { 
                    Debug.Log("Ping time is " + google.time + " ms"); 
                } 
 
                if (badPingLatch) 
                { 
                    reachable = false; 
                    badPingLatch = false; 
                } else 
                { 
                    badPingLatch = true; 
                } 
            } 
            else 
            { 
                // Debug.Log("(y) Ping time is " + google.time + " ms"); 
 
                if (badPingLatch) 
                { 
                    badPingLatch = false; 
                } 
                else 
                { 
                    reachable = true; 
                } 
            } 
 
            yield return new WaitForSeconds(1f); 
        } 
    } 
} 
