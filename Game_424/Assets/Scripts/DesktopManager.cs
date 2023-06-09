using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public GameObject ScreenPanel;
    public GameObject DesktopPanel;
    public GameObject LockScreenPanel; 
    public GameObject TaskBarPanel;

    public enum ScreenState
    {
        LockScreen,
        Desktop,
        Program
    }
    public ScreenState CurrentState = ScreenState.LockScreen;


    private TMP_InputField passwordInputField;
    private string password = "1234";
    private bool isLocked = false; 
    private string typedPassword = "1234";

    // Start is called before the first frame update
    void Start()
    {
        LockPc();
        passwordInputField = LockScreenPanel.GetComponentInChildren<TMP_InputField>();
        typedPassword = passwordInputField.text;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentState == ScreenState.LockScreen)
        {
            if (InputManager.Instance.OK)
                OnClicked_LockScreenOk();
        }
    }

    #region ActionsMethods
    public void OnClicked_LockScreenOk()
    {
        Debug.Log("LockScreenOk");
        typedPassword = LockScreenPanel.GetComponentInChildren<TMP_InputField>().text.Trim();
        if (String.Equals(password, typedPassword) )
            UnlockPc();
        else
            passwordInputField.text = "";
    }
    #endregion

    #region PrivateMethods
    private void UnlockPc()
    {
        LockScreenPanel.SetActive(false);
        ScreenPanel.SetActive(true);
        DesktopPanel.SetActive(true);
        TaskBarPanel.SetActive(true);
        CurrentState = ScreenState.Desktop;
    }
    private void LockPc()
    {
        LockScreenPanel.SetActive(true); 
        ScreenPanel.SetActive(true);
        DesktopPanel.SetActive(false);
        TaskBarPanel.SetActive(false);

        CurrentState = ScreenState.LockScreen;
    }
    #endregion                                              
}
