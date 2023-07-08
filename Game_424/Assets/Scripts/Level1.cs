    using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level1 : MonoBehaviour
{

    public GameObject PopUp; 
    public float waitTime = 10f;

    float timer = 0f;

    bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        if(PopUp != null)
        {
            PopUp.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!flag)
        {  
            if( timer < waitTime)
            {
                timer += Time.deltaTime;
            }
            else
            { 
                flag = true;
                PopUp.SetActive(true);
            } 
        } 
    }

    public void Onclick_Yes()
    {
        PopUp.SetActive(false);
        DesktopManager.Instance.OnClicked_OpenProgram(DesktopManager.Instance.DesktopIcons[2]); // gmail icon index
    }
}
