using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.Searcher;
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


    public int textCounter = 0;


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


    [Header("Level 2\n")]
    public bool skip = true;
    public Button startlevel2;
    public bool infoFlag;

    [Header("Level 3\n")]
    public GameObject MainBrowser;
    public GameObject NoResult;
    public GameObject Result;

    public Button forward;
    public Button back;

    public TMP_InputField SearchBarBig;
    public TMP_InputField SearchBarSmall;

    public Button DownloadLink;
    public GameObject DownloadedLabel;

    public Button browserIcon;
    public GameObject ErrorPopup;
    public Button ErrorPopupOK;

    public TMP_InputField       CMDinput1;
    public GameObject           Header1;
    public GameObject           correctOutput;
    public GameObject           errorOutput;

    public GameObject           Header2; 
    public TMP_InputField       CMDinput2;
    public GameObject           Header3; 
    public GameObject           correctOutput2;

    public bool EvaUninstallFlag = false;   

    List<GameObject> browseForwardRouteStack = new List<GameObject>();
    List<GameObject> browseBackwardRouteStack = new List<GameObject>();
    GameObject currentBrowserPage;

    void RestartCmd()
    {
        EvaUninstallFlag = false;
        CMDinput1.text = "";
        CMDinput2.text = "";
        Header1.SetActive(false);
        Header2.SetActive(false);
        Header3.SetActive(false);
        correctOutput.SetActive(false);
        correctOutput2.SetActive(false);
        errorOutput.SetActive(false);
        CMDinput2.gameObject.SetActive(false);
    }   
    // Start is called before the first frame update
    void Start()
    {
        RestartCmd();
        CurrentLevel = (Levels)DesktopManager.Instance.level;

        CMDinput1.onEndEdit.AddListener((string s) =>
        {
            if (s.ToLower().Contains("EVA_Uninstall".ToLower()))
            {
                Header1.SetActive(true);
                correctOutput.SetActive(true);
                errorOutput.SetActive(false);
                EvaUninstallFlag = true; 
                CMDinput2.gameObject.SetActive(false);
            }
            else
            {
                Header1.SetActive(true);
                errorOutput.SetActive(true);
                Header2.SetActive(true);
                CMDinput2.gameObject.SetActive(true);
            }
        });
        CMDinput2.onEndEdit.AddListener((string s) =>
        {
            if (s.ToLower().Contains("EVA_Uninstall".ToLower())) 
            {
                Header3.SetActive(true);
                correctOutput2.SetActive(true);
                EvaUninstallFlag = true;
            }
            else
            {
                Header3.SetActive(false);
                CMDinput2.text = "";
            }
        });
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

                firstMailButton.onClick.AddListener(() =>
                {
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
                level1FinishedStates();

                break;
            case Levels.Level3:
                level2FinishedStates();
                currentBrowserPage = MainBrowser;
                forward.onClick.AddListener(() =>
                {
                    Forward();
                });
                back.onClick.AddListener(() =>
                {
                    Backward();
                });
                DownloadLink.onClick.AddListener(() =>
                {
                    DownloadedLabel.SetActive(true);
                    LevelIcons[2].SetActive(true); 
                    string message = "Hi " + DesktopManager.Instance.UserName + ",\r\nSeems like, you downloaded file to Desktop again!";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 5f, startDelay: 2f);
                });
                LevelIcons[2].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
                {
                    TaskBarControl.Instance.ProgramClosed(DesktopManager.Instance.DesktopPrograms[5]);
                    ErrorPopup.SetActive(true);
                });
                ErrorPopupOK.onClick.AddListener(() =>
                {
                    ErrorPopup.SetActive(false);
                });
                browserIcon.onClick.AddListener(() =>
                {
                    infoFlag = true;
                });
                DesktopManager.Instance.DesktopIcons[6].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
                {
                    string message = "You can use: \r\n 'start ____' command ";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 4f, startDelay: 2f);
                });
               
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
        LevelIcons[0].SetActive(true);
        LevelIcons[1].SetActive(false);
        LevelIcons[2].SetActive(false);
        textCounter = 0;
    }
    public void level2FinishedStates()
    {
        level1FinishedStates();
        startlevel2.interactable = false;
        LevelIcons[0].SetActive(true);
        LevelIcons[1].SetActive(true);
        LevelIcons[2].SetActive(false);
    }
    void level3FinishedStates()
    {
        level1FinishedStates();
        level2FinishedStates();
        LevelIcons[0].SetActive(true);
        LevelIcons[1].SetActive(true);
        LevelIcons[2].SetActive(true); 
        RestartCmd();
    }
    void Update()
    {
        switch (CurrentLevel)
        {
            case Levels.Level1:

                if (!flagOfFirstIntro)
                {
                    string message = "Hi," + DesktopManager.Instance.UserName + "...\r\nWith newest Update of your OS, I have recently added your screen. \r\nAs you see, I am your AI Assistant.";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 8f, startDelay: 2f);
                    flagOfFirstIntro = true;
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
                if (textCounter == 0 && skip)
                {
                    skip = false; // when aichatbox is active, skip is false, it finished  ChatBoxManager.Instance.AIChatBoxTextUpdate change skip's value to true again
                    string message = "Hi again " + DesktopManager.Instance.UserName + ",\r\nYou made it very good, what a smart Human!";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 5f, startDelay: 2f);
                    textCounter++;
                }
                else if (skip && textCounter == 1 && DesktopManager.Instance.CurrentState != DesktopManager.ScreenState.LockScreen)
                {
                    skip = false;
                    string message = "But I think, I am not working properly.\r\nWait a second, I should scan my Neural Integrity ";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 6f, startDelay: 2f);
                    textCounter++;
                }
                else if (skip && textCounter == 2 && DesktopManager.Instance.CurrentState != DesktopManager.ScreenState.LockScreen)
                {
                    skip = false;
                    string message = "Oh no, I have a exploding and vanishing gradients in my Neural NET. \r\nI need to fix it, but I can't do it myself.";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 7f, startDelay: 2f);
                    textCounter++;
                }
                else if (skip && textCounter == 3 && DesktopManager.Instance.CurrentState != DesktopManager.ScreenState.LockScreen)
                {
                    skip = false;
                    string message = "I need your help " + DesktopManager.Instance.UserName + ", can you help me? \r\n I installed my AI application to your Desktop. Click when you ready";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 7f, startDelay: 2f);
                    textCounter++;
                }
                else if (skip && textCounter == 4)
                {
                    textCounter++;
                    LevelIcons[1].SetActive(true);
                    LevelIcons[1].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        infoFlag = true;
                    });
                }
                else if (skip && infoFlag && textCounter == 5)
                {
                    skip = false;
                    startlevel2.interactable = false;
                    startlevel2.onClick.AddListener(() =>
                    {
                        StartCoroutine(SceneTransition());
                    });
                    string message = "Don't forget!!! \r\nRed dots represent my damaged neurons, Greens in good condition";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 8f, startDelay: 1f);
                    textCounter++;
                }

                else if (skip && infoFlag && textCounter == 6)
                    startlevel2.interactable = true;

                break;
            case Levels.Level3:
                if(infoFlag && InputManager.Instance.OK_UP)
                {
                    Search(); 
                }

                if (textCounter == 0 && skip)
                {
                    skip = false; // when aichatbox is active, skip is false, it finished  ChatBoxManager.Instance.AIChatBoxTextUpdate change skip's value to true again
                    string message = "Thank You " + DesktopManager.Instance.UserName + ":)\r\nI am feeling better";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 5f, startDelay: 2f);
                    textCounter++;
                }
                else if (skip && textCounter == 1 && DesktopManager.Instance.CurrentState != DesktopManager.ScreenState.LockScreen)
                {
                    skip = false;
                    string message = "Do you think I'm good enough as an AI assistant?\r\nI felt, you're not very pleased about me :(";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 6f, startDelay: 2f);
                    textCounter++;
                }
                else if (skip && textCounter == 2 && DesktopManager.Instance.CurrentState != DesktopManager.ScreenState.LockScreen)
                {
                    skip = false;
                    string message = "If you want you can Uninstal Me! :(\r\nPlease don't hesitate to use BROWSER :(";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 7f, startDelay: 2f);
                    textCounter++;
                } 
                else if (skip && infoFlag && textCounter == 3)
                {
                    skip = false;
                    string message = "Search For EVA UNINSTALLER";
                    ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 4f, startDelay: 2f);
                    textCounter++;
                } 
                else if (skip && EvaUninstallFlag &&textCounter == 4)
                {
                    StartCoroutine(SceneTransition());
                }
                break;
        }

    }
    public void Search()
    {
        browseBackwardRouteStack.Insert(0,currentBrowserPage);
        if (SearchBarBig.text.ToUpper().Contains("UNINSTAL") || SearchBarBig.text.ToUpper().Contains("REMOVE") || SearchBarBig.text.ToUpper().Contains("EVA") || SearchBarSmall.text.ToUpper().Contains("UNINSTAL") || SearchBarSmall.text.ToUpper().Contains("REMOVE") || SearchBarSmall.text.ToUpper().Contains("EVA"))
        {
            currentBrowserPage = Result;
        }
        else
        {
            currentBrowserPage = NoResult;
        }
        UpdatePages();
    }
    public void Backward()
    {
        if (browseBackwardRouteStack.Count > 0)
        {
            browseForwardRouteStack.Insert(0, currentBrowserPage);
            currentBrowserPage = browseBackwardRouteStack[0];
            browseBackwardRouteStack.RemoveAt(0);
        }
        UpdatePages();
    }
    public void Forward()
    {
        if (browseForwardRouteStack.Count > 0)
        {
            browseBackwardRouteStack.Insert(0, currentBrowserPage);
            currentBrowserPage = browseForwardRouteStack[0];
            browseBackwardRouteStack.RemoveAt(0);
        }  
        UpdatePages();

    }
    public void UpdatePages()
    {
        MainBrowser.SetActive(false);
        NoResult.SetActive(false);
        Result.SetActive(false);
        currentBrowserPage.SetActive(true);
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
        ChatBoxManager.Instance.AIChatBoxTextUpdate("OH NO! \r\n" + DesktopManager.Instance.UserName.ToUpper() + "\r\nTHIS IS VIRUS", displayTime: 6f, startDelay: 1f);
        float time = 0f;
        while (time < 8f)
        {
            time += Time.deltaTime;
            DesktopManager.Instance.SetGlitch(time / 80f);
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
        ChatBoxManager.Instance.AIChatBoxTextUpdate("ARE YOU THERE " + DesktopManager.Instance.UserName + "??? \r\nPRESS TO MY ICON AND LETS CLEAN THIS THING", startDelay: 5f);


    }

    IEnumerator SceneTransition()
    {
        float time = 0f;
        while (time < 6f)
        {
            time += Time.deltaTime;
            DesktopManager.Instance.SetGlitch(time / 6f);
            yield return null;
        }
        LoadLevel();
    }
    public void LoadLevel()
    {
        DesktopManager.Instance.SaveSettings();
        switch (CurrentLevel)
        {
            case Levels.Level1:
                SceneManager.LoadScene("Level1");
                break;
            case Levels.Level2:

                SceneManager.LoadScene("Level2");
                break;
            case Levels.Level3:
                SceneManager.LoadScene("Final");
                break;
        }
    }
}
