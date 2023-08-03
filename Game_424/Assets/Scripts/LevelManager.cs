using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                var go = GameObject.Find("/Level Manager ");
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
        Level3
    }

    public Levels CurrentLevel = Levels.Level1;

    [Header("Level Icons")]
    public GameObject[] LevelIcons;


    [Header("Level 1\n")]

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
    public Button SetupVirus; 
    public GameObject Ransomware;
     

    // Start is called before the first frame update
    void Start()
    {
        CurrentLevel= (Levels)DesktopManager.Instance.level;
        switch (CurrentLevel)
        {
            case Levels.Level1:
                LevelIcons[0].SetActive(false);
                LevelIcons[1].SetActive(false);
                LevelIcons[2].SetActive(false);
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
                SetupVirus.onClick.AddListener(() =>
                {
                    StartCoroutine(OnClick_SetupVirus());
                }); 
                break;
            case Levels.Level2:
                LevelIcons[0].SetActive(true);
                LevelIcons[1].SetActive(false);
                LevelIcons[2].SetActive(false);
                level1FinishedStates();

                break;
            case Levels.Level3:
                LevelIcons[0].SetActive(true);
                LevelIcons[1].SetActive(true);
                LevelIcons[2].SetActive(false);
                break; 
        }
    }
    void level1FinishedStates()
    { 
        PopUp.SetActive(false);
        SetupVirus.interactable = false;
        downloadButton.interactable = false;
        Ransomware.SetActive(false);
        unreadMesagesText.text = "*You Have No Unread Messages";
        mailPanel.SetActive(true);
        inboxButtonText.text = "INBOX";
    }
    public void level2FinishedStates()
    { 
        //TODO: Level2 things which are finished
    }
    void level3StartLoad()
    {
        level1FinishedStates();
        level2FinishedStates();
    } 
    void Update()
    {
        switch (CurrentLevel)
        {
            case Levels.Level1:

                if (!flagOfFirstIntro)
                {
                    string message = "Hi, there...\r\nWith newest Update of your OS, I have recently added your screen. \r\nAs you see, I am your AI Assistant.";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime:8f, startDelay: 2f); 
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

        ChatBoxManager.Instance.AIChatBoxTextUpdate("Seems like, you have just downloaded file :) \r\n \r\nClose the mail program and see the file in desktop", displayTime: 5f, startDelay: 1f);
        LevelIcons[0].SetActive(true);
    }
    IEnumerator OnClick_SetupVirus()
    {
        SetupVirus.interactable = false; 
        ChatBoxManager.Instance.AIChatBoxTextUpdate("OH NO! \r\n \r\nTHIS IS VIRUS", displayTime: 6f, startDelay: 1f);
        float time = 0f;
        while (time < 8f)
        {
            time += Time.deltaTime;
            DesktopManager.Instance.SetGlitch( time / 80f);
            yield return null;
        }
        Ransomware.SetActive(true); 
        ChatBoxManager.Instance.AILogo.onClick.AddListener(() =>
        { 
            DesktopManager.Instance.SetGlitch(1);
            new WaitForSecondsRealtime(1f);
            Ransomware.SetActive(false);
            LoadLevel();
        });
        ChatBoxManager.Instance.AIChatBoxTextUpdate("ARE YOU THERE??? \r\nPRESS TO MY ICON AND LETS CLEAN THIS THING",  startDelay: 5f);
 
        
    }
    public void LoadLevel()
    {
        switch (CurrentLevel)
        {
            case Levels.Level1:
                SceneManager.LoadScene("Level1");
                break;
            case Levels.Level2:
                SceneManager.LoadScene("Level2");
                break;
            case Levels.Level3:
                SceneManager.LoadScene("Level3");
                break;
        }
    }
}
