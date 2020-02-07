using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoHleperPopUp : MonoBehaviour
{    
    public StagePlay m_StagePlay;
    public string clipName = "";
    public Image bg;
    public GameObject Button_1;
    public GameObject Button_2;
    public GameObject Button_3;
    public GameObject Img1;

    private VideoPlayerHelper videoPlayerSc;

    // Start is called before the first frame update
    void Start()
    {
        m_StagePlay = FindObjectOfType<StagePlay>();
        videoPlayerSc = FindObjectOfType<VideoPlayerHelper>();
        Button_2.SetActive(false);
        Button_3.SetActive(false);
    }

    private void OnEnable()
    {
        Button_2.SetActive(false);
        Button_3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(true == videoPlayerSc.GetPlayCheck())
        {
            bg.enabled = true;
            Img1.SetActive(true);
            Button_2.SetActive(true);
            Button_3.SetActive(true);
        }
       
    }

    public void VideoPlayButtonEvent()
    {
        if ("" == clipName)
            return;

        //videoPlayerSc.SetVideoClip(clipName);
        videoPlayerSc.SetVideoUrl(clipName);
        videoPlayerSc.VideoPlayEvent();

        //this.gameObject.SetActive(false);
        bg.enabled = false;
        Img1.SetActive(false);
        Button_1.SetActive(false);
        Button_2.SetActive(false);
        Button_3.SetActive(false);        
    }


    public void RePlayButtonEvent()
    {
        if ("" == clipName)
            return;

        //videoPlayerSc.SetVideoClip(clipName);        
        videoPlayerSc.SetVideoUrl(clipName);
        videoPlayerSc.VideoPlayEvent();

        //this.gameObject.SetActive(false);
        bg.enabled = false;
        Img1.SetActive(false);
        Button_1.SetActive(false);
        Button_2.SetActive(false);
        Button_3.SetActive(false);
    }

    public void NextButtonEvent()
    {
        m_StagePlay.forwardDown();
    }

}
