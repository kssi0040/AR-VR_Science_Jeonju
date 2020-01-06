using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicturePopUp : MonoBehaviour
{
    public StagePlay m_StagePlay;

    private List<GameObject> aButtons = new List<GameObject>();
    private int iBtnCount = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        m_StagePlay = FindObjectOfType<StagePlay>();

        for (int i = 0; i < this.transform.childCount; ++i)
        {
            aButtons.Add(this.transform.GetChild(i).gameObject);
            aButtons[i].SetActive(false);
        }
        aButtons[iBtnCount].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ImgButtonEvent()
    {
        aButtons[iBtnCount].SetActive(false);
        iBtnCount++;
        if (this.transform.childCount <= iBtnCount)
        {
            // hmm... 그냥 넘기기..???
            m_StagePlay.forwardDown();
            iBtnCount = 0;
            return;
        }
        aButtons[iBtnCount].SetActive(true);
    }
}
