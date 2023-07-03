using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    public Canvas mainCanvas;

    [Header(" ")]
    public GameObject ScreenPanel; 
    [Header("Desktop Panel Objects")]
    [Header("-------------------------------------------------------------")]
    public GameObject DesktopPanel; 
    public GameObject[] DesktopIcons;
    public GameObject[] DesktopPrograms;

    private Dictionary<GameObject, GameObject> DesktopIconsDict = new Dictionary<GameObject, GameObject>();
    [Header("Lock Screen Panel Objects")]
    [Header("-------------------------------------------------------------")]
    public GameObject LockScreenPanel;
    public TMP_Text WrongPassTmpText;
    public TMP_InputField passwordInputField; 
    [Header("Taskbar Panel Objects")]
    [Header("-------------------------------------------------------------")]
    public GameObject TaskBarPanel;

    public enum ScreenState
    {
        LockScreen,
        Desktop,
        Program
    }
    public ScreenState CurrentState = ScreenState.LockScreen;

  
    private string password = "1234"; 
    private string typedPassword = "1234";

    // Start is called before the first frame update
    void Start()
    {
        LockPc();
        passwordInputField = LockScreenPanel.GetComponentInChildren<TMP_InputField>();
        typedPassword = passwordInputField.text;

        if(DesktopIcons.Length == DesktopPrograms.Length){
            for (int i = 0; i < DesktopIcons.Length; i++)
                DesktopIconsDict.Add(DesktopIcons[i], DesktopPrograms[i]);

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(CurrentState == ScreenState.LockScreen)
        {
            if (InputManager.Instance.OK)
                OnClicked_LockScreenOk();
        }
        if(CurrentState == ScreenState.Desktop){
            if (InputManager.Instance.lockKey)
            {
                LockPc();
                InputManager.Instance.lockKey = false;
            }
        }
    }

    #region ActionsMethods
    public void OnClicked_LockScreenOk()
    {
        Debug.Log("LockScreenOk"); 
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
        Debug.Log("CloseProgram");
        TaskBarControl.Instance.ProgramClosed(Program);
    }
    public void OnClicked_MinimizeProgram(GameObject Program)
    {
        Debug.Log("MinimizeProgram");
        TaskBarControl.Instance.MinimizeProgram(Program);
    }
    public void OnClicked_MaximizeProgram(GameObject Program)
    {
        Debug.Log("MaximizeProgram");
        TaskBarControl.Instance.MaximizeProgram (Program);
    } 
    public void OnClicked_OpenProgram(GameObject button)
    {
       DesktopIconsDict[button].SetActive(true);
       TaskBarControl.Instance.NewProgramOpen(button.transform.GetChild(0).GetComponent<Image>().sprite, DesktopIconsDict[button]);
       CurrentState = ScreenState.Program;
        
    }
    #endregion

    #region PrivateMethods
    private void UnlockPc()
    {
        passwordInputField.text="";
        LockScreenPanel.SetActive(false);
        ScreenPanel.SetActive(true);
        DesktopPanel.SetActive(true);
        TaskBarPanel.SetActive(true);
        CurrentState = ScreenState.Desktop;
        WrongPassTmpText.gameObject.SetActive(false); 
    }
    private void LockPc()
    {
        LockScreenPanel.SetActive(true); 
        ScreenPanel.SetActive(true);
        DesktopPanel.SetActive(false);
        TaskBarPanel.SetActive(false);

        CurrentState = ScreenState.LockScreen;
        WrongPassTmpText.gameObject.SetActive(false); 
    }
    #endregion                                              
}
