using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPopUp : MonoBehaviour
{
    public StagePlay m_StagePlay;

    private List<GameObject> aButtons = new List<GameObject>();
    private List<GameObject> aShadowImgs = new List<GameObject>();

    private int iIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        m_StagePlay = FindObjectOfType<StagePlay>();
        
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            aButtons.Add(this.transform.GetChild(i).gameObject);            
            aButtons[i].transform.GetComponent<Button>().onClick.AddListener(delegate { this.OnButtonDown(); });
            aShadowImgs.Add(aButtons[i].transform.GetChild(1).gameObject);
        }

        for (int i = 0; i < this.transform.childCount; ++i)
        {
            aButtons[i].transform.GetComponent<Button>().enabled = false;
            aShadowImgs[i].SetActive(true);
        }
        aButtons[0].transform.GetComponent<Button>().enabled = true;
        aShadowImgs[0].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButtonDown()
    {
        // hmm/...
        if (false == PlayerInfo.Instance.isComplite)
            return;

        if (true == m_StagePlay.Narration.isPlaying)
            return;


        // hmm...
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            aButtons[i].transform.GetComponent<Button>().enabled = false;
            aShadowImgs[i].SetActive(true);
        }

        iIndex++;
        if (this.transform.childCount <= iIndex)
        {            
            m_StagePlay.forwardDown();
            return;
        }
        
        aButtons[iIndex].transform.GetComponent<Button>().enabled = true;
        aShadowImgs[iIndex].SetActive(false);
        PlayerInfo.Instance.isComplite = true;
        m_StagePlay.forwardDown();
    }
}
