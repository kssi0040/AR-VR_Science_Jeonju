using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTextPopUp : MonoBehaviour
{
    public StagePlay m_StagePlay;
    public GameObject button;
    public Text answer;


    // Start is called before the first frame update
    void Start()
    {
        m_StagePlay = FindObjectOfType<StagePlay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnswerButtonEvent()
    {
        if ("" == answer.text)
            return;

        m_StagePlay.forwardDown();
    }
}
