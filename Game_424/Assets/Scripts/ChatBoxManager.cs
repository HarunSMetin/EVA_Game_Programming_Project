using System.Collections;
using System.Collections.Generic;
using TMPro; 
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxManager : MonoBehaviour
{
    private static ChatBoxManager instance;
    public static ChatBoxManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = GameObject.Find("/AI Chat Box Manager");
                if (go == null)
                {
                    return null;
                }

                instance = go.GetComponent<ChatBoxManager>();
                if (instance == null)
                {
                    return null;
                }
            }
            return instance;
        }
    }

    public GameObject AIPanel;
    public Button AILogo;
    public TMP_Text AIChatBoxText;
    
    private bool available = true;
    public void AIChatBoxTextUpdate(string text)
    {
       if (!available)
            return;
        available =false;
        AIChatBoxText.text = text;
        AIPanel.SetActive(true); 
        available = true;
    }
    public void AIChatBoxTextUpdate(string text, float startDelay = 1f)
    {
        if (!available)
            return;
        available = false;
        AIChatBoxText.text = text;
        StartCoroutine(DisableAIPanel(startDelay ));
    }
    public void AIChatBoxTextUpdate(string text, float displayTime, float startDelay = 0f)
    {
        if(!available)
            return;
        available = false;
        AIChatBoxText.text = text;
        StartCoroutine(DisableAIPanel( startDelay, displayTime));
    }
    IEnumerator DisableAIPanel(float startDelay )
    {
        yield return new WaitForSeconds(startDelay);
        AIPanel.SetActive(true); 
        available = true;
    }
    IEnumerator DisableAIPanel(float startDelay,float displayTime)
    {
        yield return new WaitForSeconds(startDelay);  
        AIPanel.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        AIPanel.SetActive(false);
        available = true;
    }
}
