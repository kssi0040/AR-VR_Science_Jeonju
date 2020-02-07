using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollRectSnap : MonoBehaviour
{
    public RectTransform panel;                             // to hold the scroll panel;
    public Button[] aButtons;
    //20191126 추가사항
    public GameObject Sphere;
    public Texture[] Textures;
    public string[] Names;
    public new Text name;
    public new Camera camera;
    public int Temp;
    //여기까지
    public RectTransform center;                           // center to compare the distance for each button

    public float[] aDistances;                               // all button's distance to the center..
    private bool bDragging = false;                          // will be true,  while we drag the panel
    private int iBtnDistance;                                   // will hold the distance between the buttons
    public int iMinButtonNum;                               // to hold the number of the button, with smallest distance to center

    public GameObject NoticePopup;

    // Start is called before the first frame update
    void Start()
    {
        int iBtnLength = aButtons.Length;
        aDistances = new float[iBtnLength];
        // get distance between buttons
        iBtnDistance = (int)Mathf.Abs(aButtons[1].GetComponent<RectTransform>().anchoredPosition.x - aButtons[0].GetComponent<RectTransform>().anchoredPosition.x);        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < aButtons.Length; ++i)
        {
            aDistances[i] = Mathf.Abs(center.transform.position.x - aButtons[i].transform.position.x);            
        }

        float minDistance = Mathf.Min(aDistances);                              // Get the min Distance        
        for (int a = 0; a < aButtons.Length; ++a)
        {
            if(minDistance == aDistances[a])
            {
                /*
                //20191126 변경사항
                Temp = iMinButtonNum;
                iMinButtonNum = a;
                if (Temp != iMinButtonNum)
                {
                    if (camera != null)
                    {
                        camera.transform.rotation = Quaternion.Euler(Vector3.zero);
                    }
                }
                */
                if (iMinButtonNum < Textures.Length)
                {
                    Sphere.GetComponent<Renderer>().material.mainTexture = Textures[iMinButtonNum];                    
                }
                name.text = Names[iMinButtonNum];
            }
        }

        if(!bDragging)
        {
            LerpToButton(iMinButtonNum * -iBtnDistance);
        }
    }


    void LerpToButton(int position)
    {
        float fPosX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 10f);
        Vector2 newPosition = new Vector2(fPosX, panel.anchoredPosition.y);
        panel.anchoredPosition = newPosition;
    }

    //================================================ EVENT TRIGGER ==================================================//
    // Event Trigger 에서 call...
    public void StartDrag()
    {
        bDragging = true;
    }

    public void EndDrag()
    {
        bDragging = false;       
    }


    public void LeftButtonDown()
    {
        iMinButtonNum--;
        if (iMinButtonNum < 0)
        {
            iMinButtonNum = 0;
        }
    }

    public void RightButtonDown()
    {
        iMinButtonNum++;
        if (iMinButtonNum >= Names.Length - 1)
        {
            iMinButtonNum = Names.Length - 1;
        }
    }

    public void NoticeButtonEvent()
    {
        NoticePopup.SetActive(false);
    }

    // stage button....
    public void StageButtonEvent()
    {
        if (0 == iMinButtonNum)
        {
            // stage 1 load...
            if (Quiz_XML_Reader.Instance.readCompleted == true && XML_Reader.Instance.readCompleted == true)
            {
                SceneManager.LoadScene("Stage1");
            }
        }
        else if (1 == iMinButtonNum)
        {
            // stage 1 load...
            if (Quiz_XML_Reader.Instance.readCompleted == true && XML_Reader.Instance.readCompleted == true)
            {
                SceneManager.LoadScene("Stage2");
            }
        }
        else if (2 == iMinButtonNum)
        {
            // stage 1 load...
            if (Quiz_XML_Reader.Instance.readCompleted == true && XML_Reader.Instance.readCompleted == true)
            {
                //SceneManager.LoadScene("GPS_Scene");
            }
        }
        else if (3 == iMinButtonNum)
        {
            // stage 1 load...
            if (Quiz_XML_Reader.Instance.readCompleted == true && XML_Reader.Instance.readCompleted == true)
            {
                SceneManager.LoadScene("Record");
            }
        }
    }

    public void HomeButtonEvent()
    {
        SceneManager.LoadScene("Start");
    }


    public void MapButtonEvent()
    {
        if (0 == iMinButtonNum)
        {
            //35.815485, 127.153451
            //string strUrl = "https://www.google.com/maps/place/35.815485,127.153451/";
            string strUrl = "https://www.google.com/maps/place/%EC%A0%84%EC%A3%BC+%ED%95%9C%EC%98%A5%EB%A7%88%EC%9D%84/@35.8154261,127.151268,17z/data=!4m5!3m4!1s0x3570236212eebd4b:0x8ef4be1bc6d0848e!8m2!3d35.8154261!4d127.1534567";
            Application.OpenURL(strUrl);
        }
        else if (1 == iMinButtonNum)
        {
            //35.815392, 127.149788
            //string strUrl = "https://www.google.com/maps/place/35.815392,127.149788";
            string strUrl = "https://www.google.com/maps/place/%EA%B2%BD%EA%B8%B0%EC%A0%84/@35.8153246,127.1486981,18z/data=!3m1!4b1!4m5!3m4!1s0x357023601cf26ae5:0x1f95dfca67044b31!8m2!3d35.8153224!4d127.1497924";
            Application.OpenURL(strUrl);
        }
        else if (2 == iMinButtonNum)
        {
            //36.845262, 126.196726            
            //string strUrl = "https://www.google.com/maps/place/36.835972,126.195911";
            //Application.OpenURL(strUrl);
        }
    }






}
