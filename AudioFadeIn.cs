using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
[RequireComponent(typeof(AudioSource))] 
public class AudioFadeIn : MonoBehaviour { 
    bool interrupt = false; 
 
    [SerializeField] 
    private int m_FadeInTime = 3; 
    private AudioSource m_AudioSource; 
 
    void Awake () { 
        m_AudioSource = GetComponent<AudioSource>(); 
        m_AudioSource.volume = 0; 
    } 
 
    public void Stop() 
    { 
        interrupt = true;    
    } 
 
    void Update () { 
        if (interrupt) 
        { 
            return; 
        } 
 
        if (m_AudioSource.volume < 1) 
        { 
            m_AudioSource.volume = m_AudioSource.volume + (Time.deltaTime / (m_FadeInTime + 1)); 
        } 
    } 
} 
