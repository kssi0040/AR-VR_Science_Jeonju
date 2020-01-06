using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerHelper : MonoBehaviour
{
    private VideoPlayer videoPlayerSc;
    private string videoURL = "";
    private bool bPlay = false;
    private bool bPlayEnd = false;

    public GameObject renderPlane = null;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayerSc = this.transform.GetComponent<VideoPlayer>();
        renderPlane.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (true == videoPlayerSc.isPlaying)
        {
            bPlay = true;            
        }

        if (true == bPlay)
        {
            if (false == videoPlayerSc.isPlaying)
            {
                videoPlayerSc.Stop();                
                renderPlane.SetActive(false);
                bPlayEnd = true;
            }
        }
    }

    public void SetVideoUrl(string _url)
    {
        //videoPlayerSc.clip = Resources.Load(_cileName) as VideoClip;
        videoPlayerSc.url = _url;
    }


    public void SetVideoClip(string _cileName)
    {
        videoPlayerSc.clip = Resources.Load(_cileName) as VideoClip;
    }

    public void VideoPlayEvent()                                                                                                      // button event...
    {
        StartCoroutine(PlayVideoPlayer());
    }
    IEnumerator PlayVideoPlayer()
    {        
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);

        bPlay = false;
        bPlayEnd = false;
        renderPlane.SetActive(true);
        //videoPlayerSc.renderMode = VideoRenderMode.MaterialOverride;
        //videoPlayerSc.targetMaterialRenderer = GameObject.Find("Video_Renderer").GetComponent<MeshRenderer>();        
        while (!videoPlayerSc.isPrepared)
        {
            yield return waitTime;
            break;
        }        
        videoPlayerSc.Play();
    }


    public bool GetPlayCheck()
    {
        return bPlayEnd;
    }
}
