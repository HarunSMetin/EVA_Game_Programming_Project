using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using URPGlitch.Runtime.DigitalGlitch;

public class DesktopManager : MonoBehaviour
{
    //Singleton class
    private static DesktopManager instance;
    public static DesktopManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = GameObject.Find("/Desktop Manager");
                if (go == null)
                {
                    return null;
                }

                instance = go.GetComponent<DesktopManager>();
                if (instance == null)
                {
                    return null;
                }
            }
            return instance;
        }
    }
    public enum ScreenState
    {
        LockScreen,
        Desktop,
        Program
    } 
    public string UserName = "User";
    public float SoundVolume =0.5f;
    public int Difficulty = 0;
    public int level = 0;

    [Header(" ")]
    public Volume GlitchVolume;
    public Canvas mainCanvas;

    [Header(" ")]
    public GameObject ScreenPanel;
    [Header("Desktop Panel Objects")]
    [Header("-------------------------------------------------------------")]
    public GameObject DesktopPanel;
    public GameObject[] DesktopIcons;
    public GameObject[] DesktopPrograms;
    public GameObject Programs;

    public Dictionary<GameObject, GameObject> DesktopIconsDict = new Dictionary<GameObject, GameObject>();  //DesktopIconsDict.Add(DesktopIcons[i], DesktopPrograms[i]);
    [Header("Lock Screen Panel Objects")]
    [Header("-------------------------------------------------------------")]
    public GameObject LockScreenPanel;
    public TMP_Text UserNameTmpText;
    public TMP_Text WrongPassTmpText;
    public TMP_InputField passwordInputField;
    [Header("Taskbar Panel Objects")]
    [Header("-------------------------------------------------------------")]
    public GameObject TaskBarPanel;
    public GameObject StartUpPanel;

    [Header("User Preferences")]
    public Slider SoundBarSlider;
    public TMP_Dropdown DifficultyDropdown;
    public TMP_InputField UserNameTMP;
   
    [Header(" ")]
    public ScreenState CurrentState = ScreenState.LockScreen;
    public ScreenState OldState = ScreenState.Desktop;


    private string password = "1234";
    private string typedPassword = "1234";
     
    private void Awake()
    { 
        LoadSettings();
    }
    void Start()
    { 
        LockPc(false);
        passwordInputField = LockScreenPanel.GetComponentInChildren<TMP_InputField>();
        typedPassword = passwordInputField.text;

        if (DesktopIcons.Length == DesktopPrograms.Length)
        {
            for (int i = 0; i < DesktopIcons.Length; i++)
                DesktopIconsDict.Add(DesktopIcons[i], DesktopPrograms[i]);

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (CurrentState)
        {
            case ScreenState.LockScreen:
                if (InputManager.Instance.OK)
                    OnClicked_LockScreenOk();
                break;
            case ScreenState.Desktop:
                if (InputManager.Instance.lockKey)
                {
                    LockPc();
                    InputManager.Instance.lockKey = false;
                }
                break;
            case ScreenState.Program:
                if (InputManager.Instance.lockKey)
                {
                    LockPc();
                    InputManager.Instance.lockKey = false;
                }
                break;
        }
    }

    #region ActionsMethods
    public void OnClicked_ForgotPassword()
    {
        string message = "Can't you remember your password? \r\nHere is a tip for you: \r\nYour password is most common 4 digit password in the world";
        ChatBoxManager.Instance.AIChatBoxTextUpdate(message, displayTime: 7f ,startDelay: 0f); 
    }
    public void OnClicked_LockScreenOk()
    {
        typedPassword = passwordInputField.text;
        if (String.Equals(password, typedPassword))
        {
            WrongPassTmpText.gameObject.SetActive(false);
            UnlockPc();
        }

        else
        {
            passwordInputField.text = "";
            WrongPassTmpText.gameObject.SetActive(true);

        }

    }
    public void OnClicked_CloseProgram(GameObject Program)
    {
        OldState = CurrentState;
        CurrentState = ScreenState.Desktop;
        Debug.Log("CloseProgram");
        TaskBarControl.Instance.ProgramClosed(Program);
    }
    public void OnClicked_MinimizeProgram(GameObject Program)
    {
        Debug.Log("MinimizeProgram");

        OldState = CurrentState;
        CurrentState = ScreenState.Desktop;
        TaskBarControl.Instance.MinimizeProgram(Program);
    }
    public void OnClicked_MaximizeProgram(GameObject Program)
    {
        OldState = CurrentState;
        CurrentState = ScreenState.Program;
        TaskBarControl.Instance.MaximizeProgram(Program);
    }
    public void OnClicked_OpenProgram(GameObject button)
    {
        Debug.Log("1: " + OldState.ToString() + ' ' + CurrentState.ToString());
        if (CurrentState == ScreenState.LockScreen)
        { 
            OldState = ScreenState.Program; 
            Debug.Log("2: " + OldState.ToString() + ' ' + CurrentState.ToString());
        }
        else
        {
            OldState = CurrentState;
            Debug.Log("3: " + OldState.ToString() + ' ' + CurrentState.ToString());
            CurrentState = ScreenState.Program;
            Debug.Log("4: " + OldState.ToString() + ' ' + CurrentState.ToString());
        }
        DesktopIconsDict[button].SetActive(true);
        TaskBarControl.Instance.NewProgramOpen(button.transform.GetChild(0).GetComponent<Image>().sprite, DesktopIconsDict[button]);
    }
    public void OnClicked_Shutdown()
    {
        Debug.Log("Shutdown");
        ExitGame();
    }
    public void OnClicked_StartUpPanel()
    {
        StartUpPanel.SetActive(!StartUpPanel.activeInHierarchy);
        SaveSettings(); 

    }
    #endregion

    public void ExitGame()
    { 
        SaveSettings();

        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Debug.Log("Game in Editor");
        }
        else
        {
            Application.Quit();
            Debug.Log("Game is exiting");
        }

    }
    #region PrivateMethods
    private void UnlockPc()
    {
        passwordInputField.text = "";
        LockScreenPanel.SetActive(false);
        ScreenPanel.SetActive(true);
        DesktopPanel.SetActive(true);
        TaskBarPanel.SetActive(true);
        CurrentState = OldState;
        OldState = ScreenState.LockScreen;
        WrongPassTmpText.gameObject.SetActive(false);
    }
    #endregion 
    public void LockPc(bool justForStart=true)
    {
        LockScreenPanel.SetActive(true);
        ScreenPanel.SetActive(true);
        DesktopPanel.SetActive(false);
        TaskBarPanel.SetActive(false);

        if(justForStart)OldState = CurrentState;
        CurrentState = ScreenState.LockScreen;
        WrongPassTmpText.gameObject.SetActive(false);
    }
    public void LoadSettings()
    {
        // Load settings using PlayerPrefs and apply them to the game
        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            SoundVolume = PlayerPrefs.GetFloat("SoundVolume");
        }

        if (PlayerPrefs.HasKey("Difficulty"))
        {
            Difficulty = PlayerPrefs.GetInt("Difficulty"); 
        }

        if (PlayerPrefs.HasKey("UserName"))
        {
            UserName = PlayerPrefs.GetString("UserName");
        }
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level"); 
        } 
        SoundBarSlider.value = SoundVolume;
        DifficultyDropdown.value = Difficulty;
        UserNameTMP.text = UserName; 
        UserNameTmpText.text = UserName;
        LevelManager.Instance.CurrentLevel = (LevelManager.Levels)level;

    }
    public void SaveSettings()
    {
        // Save settings using PlayerPrefs
        UserName=UserNameTMP.text;
        SoundVolume = SoundBarSlider.value;
        Difficulty = DifficultyDropdown.value;
        level = (int)LevelManager.Instance.CurrentLevel; 
        PlayerPrefs.SetFloat("SoundVolume", SoundVolume);
        PlayerPrefs.SetInt("Difficulty", Difficulty);
        PlayerPrefs.SetString("UserName", UserName);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();

    }
    public void SetGlitch(float value)
    {
        GlitchVolume.profile.TryGet(out DigitalGlitchVolume Glitch);
        Glitch.intensity.value = value;
    }

}
