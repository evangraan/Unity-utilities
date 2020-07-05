using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * README
 * 
 * When creating a scroll viewport with a content control in it, the content 
 * control should be resized to fit the full content and the viewport should
 * be resized to the area that should be visible. I.e. the viewport is a mask
 * that determines which section of the content is visible and the scroll bars
 * and swipe mechanism moves this viewport up and down the content window (or
 * sideways)
 * 
 * With text as the content how-ever it is difficult to know how high the
 * content window should be. This class calculates the height and if this script
 * is placed on the Content object in Unity, it periodically recalculates and
 * sets the content control's height (only height but easily adjusted for width)
 * based on the number of lines of text in the text control (ensure Wrap is 
 * enabled.)
 * 
 * On the content control in Unity, add a vertical layput group, a content size
 * fitter (with both horizontal and vertical set to unconstrained) and this
 * script. Drag the content control's rect transform into the content rect
 * transform field and the text control into the text field. The font height
 * will depend on the font in use, so determine this first and then set the
 * field value as needed.
 * 
 */

public class ContentTextSizeFitter : MonoBehaviour {
    private float deltaTime = 0f;
    public float fontHeight = 52.2f;
    public RectTransform contentWindow;
    public Text contentText;
    
	void Update () {
        deltaTime += Time.deltaTime;
        if (deltaTime > Configuration.REFRESH_RATE)
        {
            deltaTime = 0f;
            Canvas.ForceUpdateCanvases();
            contentWindow.sizeDelta = new Vector2(contentWindow.sizeDelta.x, contentText.cachedTextGenerator.lines.Count * fontHeight);
        }
		
	}
}
