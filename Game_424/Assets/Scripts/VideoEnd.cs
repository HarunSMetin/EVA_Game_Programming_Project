using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public int VideoPlayed = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("VideoPlayed"))
        {
            VideoPlayed = PlayerPrefs.GetInt("VideoPlayed");
        }
        if (VideoPlayed == 0)
        { 
            VideoPlayed = 1;
            PlayerPrefs.SetInt("VideoPlayed", VideoPlayed);
        }
        else
        {
            SceneManager.LoadScene("Desktop");
        }
        videoPlayer.loopPointReached += (VideoPlayer vp) => { SceneManager.LoadScene("Desktop"); };
    } 
    
 
}
