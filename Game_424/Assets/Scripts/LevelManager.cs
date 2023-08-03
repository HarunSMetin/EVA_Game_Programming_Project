using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = GameObject.Find("/Level Manager");
                if (go == null)
                {
                    return null;
                }

                instance = go.GetComponent<LevelManager>();
                if (instance == null)
                {
                    return null;
                }
            }
            return instance;
        }
    }
    public enum Levels
    {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
    }

    public Levels CurrentLevel = Levels.Level1;

    [Header("Level 1")]

    public GameObject PopUp;
    public float waitTime = 5f;
    float timer = 0f; 
    bool flagOfPopUp = false;
    bool flagOfFirstIntro = false;
    public TMP_Text inboxButtonText;
    public Button firstMailButton;
    public TMP_Text unreadMesagesText;
    public GameObject mailPanel;
    public Button downloadButton;  
    public Slider downloadBar;



    // Start is called before the first frame update
    void Start()
    {
        switch (CurrentLevel)
        {
            case Levels.Level1:
                if (PopUp != null)
                {
                    PopUp.SetActive(false);
                }

                firstMailButton.onClick.AddListener(() => {
                    unreadMesagesText.text = "*You Have No Unread Messages";
                    mailPanel.SetActive(true);
                    inboxButtonText.text = "INBOX";
                });
                downloadButton.onClick.AddListener(() =>
                {
                    StartCoroutine(OnClick_Download());
                });
                break;
            case Levels.Level2:
                break;
            case Levels.Level3:
                break;
            case Levels.Level4:
                break;
            case Levels.Level5:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentLevel)
        {
            case Levels.Level1:
                if (!flagOfFirstIntro)
                {
                    string message = "Hi, there...\r\nWith newest Update of your OS, I have recently added your screen. \r\nAs you see, I am your AI Assistant.";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime:12f, startDelay: 2f); 
                    flagOfFirstIntro=true;
                }
                if (!flagOfPopUp && DesktopManager.Instance.CurrentState != DesktopManager.ScreenState.LockScreen)
                {
                    if (timer < waitTime)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        flagOfPopUp = true;
                        PopUp.SetActive(true);
                    }
                }
                break;
            case Levels.Level2:
                break;
            case Levels.Level3:
                break;
            case Levels.Level4:
                break;
            case Levels.Level5:
                break;
        }

    }

    public void Onclick_Yes()
    {
        PopUp.SetActive(false);
        DesktopManager.Instance.OnClicked_OpenProgram(DesktopManager.Instance.DesktopIcons[2]); // gmail icon index
    }
    IEnumerator OnClick_Download()
    {
        downloadButton.interactable = false;
        downloadBar.gameObject.SetActive(true);
        float time = 0f;
        while (time < 5f)
        {
            time += Time.deltaTime;
            downloadBar.value = time / 5f;
            yield return null;
        } 
        ChatBoxManager.Instance.AIChatBoxTextUpdate("Seems like, you have just downloaded file :) \r\n \r\nClose the mail panel and see the file in desktop", displayTime: 5f, startDelay: 1f);
    }
}
