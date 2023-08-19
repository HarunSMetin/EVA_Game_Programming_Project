using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelManager;
using UnityEngine.SceneManagement;
using URPGlitch.Runtime.DigitalGlitch;
using UnityEngine.Rendering;

public class FinishAndReturnDesktop : MonoBehaviour
{

    public Volume GlitchVolume;
    public enum Levels
    {
        Level1,
        Level2,
        Level3
    }

    Levels CurrentLevel= Levels.Level1;

    public bool FinishedFlag= false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FinishedFlag)
        { 
            FinishedFlag = false;
            StartCoroutine(SceneTransition());
        }

    }
    public void SetGlitch(float value)
    {
        GlitchVolume.profile.TryGet(out DigitalGlitchVolume Glitch);
        Glitch.intensity.value = value;
    }
    IEnumerator SceneTransition()
    {
        float time = 0f;
        while (time < 6f)
        {
            time += Time.deltaTime; 
            SetGlitch(time / 6f);
            yield return null;
        }
        LoadLevel();
    }
    public void LoadLevel()
    {
        switch (CurrentLevel)
        {
            case Levels.Level1:
                PlayerPrefs.SetInt("Level",1);
                break;
            case Levels.Level2: 
                PlayerPrefs.SetInt("Level", 2);
                break;
            case Levels.Level3:
                SceneManager.LoadScene("GameEnd");
                break;
        }
        SceneManager.LoadScene("Desktop");
    }
}
