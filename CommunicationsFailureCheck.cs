using UnityEngine; 
 
public class CommunicationsFailureCheck : MonoBehaviour { 
  private float deltaTime; 
  public GameObject communicationsFailurePanel; 
 
  // Update is called once per frame 
  void Update () { 
    if (communicationsFailurePanel == null) { 
      return; 
    } 
 
    deltaTime += Time.deltaTime; 
    if (deltaTime > 0.5f) 
    { 
      deltaTime = 0f; 
      if ((GameState.Instance.communicationsIssues) && 
          (GameState.Instance != null) && 
          (GameState.Instance.sceneManager != null) && 
          (!GameState.Instance.sceneManager.IsArcadeScene())) 
      { 
        communicationsFailurePanel.SetActive(true); 
      } 
      else 
      { 
        communicationsFailurePanel.SetActive(false); 
      } 
    } 
  } 
} 
