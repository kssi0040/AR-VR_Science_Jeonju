using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class VideoPopup : MonoBehaviour
{
    private VideoPlayer videoPlayerSc;
    private string videoURL = "";
    private bool bPlay = false;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayerSc = this.transform.GetComponent<VideoPlayer>();
        videoPlayerSc.targetCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(true == videoPlayerSc.isPlaying)
        {
            bPlay = true;
            Debug.Log("state: " + videoPlayerSc.isPlaying);
        }


        if(true == bPlay)
        {
            if (false == videoPlayerSc.isPlaying)
            {
                videoPlayerSc.Stop();
                Debug.Log("state: " + videoPlayerSc.isPlaying);
            }   
        }   
    }

    public void VideoPlayButtonEvent()                                                                                                      // button event...
    {
        StartCoroutine(PlayVideoPlayer());
    }
    IEnumerator PlayVideoPlayer()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        //videoPlayerSc.url = videoURL;

        while (!videoPlayerSc.isPrepared)
        {           
            yield return waitTime;
            break;
        }

        videoPlayerSc.Play();               
    }
}
