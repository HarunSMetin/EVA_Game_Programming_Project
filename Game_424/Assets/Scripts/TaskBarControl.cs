using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TaskBarControl : MonoBehaviour
{
    private static TaskBarControl instance;
    public static TaskBarControl Instance
    {
        get
        {
            if (instance == null)
            {
                var go = GameObject.Find("/MainCanvas/Screen/TaskBar");
                if (go == null)
                {
                    return null;
                }

                instance = go.GetComponent<TaskBarControl>();
                if (instance == null)
                {
                    return null;
                }
            }
            return instance;
        }
    }

    public int currentTaskBarIndex = 0;
    public GameObject[] taskBarIcons;

    private Dictionary<GameObject, GameObject> taskBarIconsDict = new Dictionary<GameObject, GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void NewProgramOpen(Sprite SourceImage, GameObject ProgramObject)
    {
        if (taskBarIconsDict.ContainsValue(ProgramObject))
        {
            GameObject newProg = Instantiate(ProgramObject,DesktopManager.Instance.mainCanvas.pixelRect.size/2,Quaternion.identity,ProgramObject.transform.parent);
            newProg.GetComponent<RectTransform>().offsetMin = new Vector2(newProg.GetComponent<RectTransform>().offsetMin.x, 0);
            newProg.GetComponent<RectTransform>().offsetMax = new Vector2(newProg.GetComponent<RectTransform>().offsetMax.x, 0);
            taskBarIconsDict.Add(taskBarIcons[currentTaskBarIndex], newProg);
            taskBarIcons[currentTaskBarIndex].transform.GetChild(0).GetComponent<Image>().sprite = SourceImage;
            taskBarIcons[currentTaskBarIndex].SetActive(true);
        }else
        {
            taskBarIconsDict.Add(taskBarIcons[currentTaskBarIndex], ProgramObject);
            taskBarIcons[currentTaskBarIndex].transform.GetChild(0).GetComponent<Image>().sprite = SourceImage;
            taskBarIcons[currentTaskBarIndex].SetActive(true);
            currentTaskBarIndex++;
        }
    }
    public void ProgramClosed(GameObject ProgramObject)
    {

        if (taskBarIconsDict.ContainsValue(ProgramObject))
        {
            ProgramObject.SetActive(false);

            int count = taskBarIconsDict.Count;
            int index = 0;

            foreach (KeyValuePair<GameObject, GameObject> entry in taskBarIconsDict)
            {
                if (entry.Value == ProgramObject)
                {

                    while (index < count - 1)
                    {
                        taskBarIconsDict[taskBarIcons[index]] = taskBarIconsDict[taskBarIcons[index + 1]];
                        taskBarIcons[index].transform.GetChild(0).GetComponent<Image>().sprite = taskBarIcons[index + 1].transform.GetChild(0).GetComponent<Image>().sprite;
                        index++;
                    }
                    currentTaskBarIndex--;
                    taskBarIconsDict.Remove(taskBarIcons[count - 1]);
                    taskBarIcons[count - 1].SetActive(false);
                    break;
                }
                index++;
            }
        }
        else
        {
            Debug.Log("Program not found in taskbar");


        }
    }

    public void MaximizeProgram(GameObject taskBarIcon)
    {
        taskBarIconsDict[taskBarIcon].SetActive(true);
    }
    public void MinimizeProgram(GameObject ProgramObject)
    {
        ProgramObject.SetActive(false);


    }

}
