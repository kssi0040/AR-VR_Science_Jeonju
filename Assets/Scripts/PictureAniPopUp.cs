using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureAniPopUp : MonoBehaviour
{
    public StagePlay m_StagePlay;

    public GameObject button1;
    public GameObject button2;
    private GameObject[] aImgs = new GameObject[11];


    // Start is called before the first frame update
    void Start()
    {
        m_StagePlay = FindObjectOfType<StagePlay>();
        button2.SetActive(false);

        for (int i = 2; i < aImgs.Length+2; ++i)
        {
            aImgs[i - 2] = this.transform.GetChild(i).gameObject;
            aImgs[i - 2].SetActive(false);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonEvent()
    {
        Debug.Log("button 11");

        StartCoroutine(StartAnimation());
    }

    public void NextButtonEvent()
    {
        Debug.Log("button 22");
        m_StagePlay.forwardDown();
    }

    IEnumerator StartAnimation()
    {
        button1.SetActive(false);

        int iIndex = 0;
        while(aImgs.Length > iIndex-1)
        {            
            yield return new WaitForSeconds(0.5f);
            aImgs[iIndex].SetActive(true);
            iIndex++;

            if (aImgs.Length <= iIndex)
                break;
        }
        Debug.Log("hmm... end");        
        button2.SetActive(true);
    }
}
