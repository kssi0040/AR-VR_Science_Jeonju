using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKButtonElement : MonoBehaviour
{
    public StagePlay m_StagePlay;
    public GameObject Check;
    // Start is called before the first frame update
    void Start()
    {
        m_StagePlay = FindObjectOfType<StagePlay>();
        Check.SetActive(false);
        this.GetComponent<Button>().onClick.AddListener(delegate { this.OnButtonDown(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButtonDown()
    {
        StartCoroutine(NextPage());
    }

    public IEnumerator NextPage()
    {
        Check.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        PlayerInfo.Instance.isComplite = true;
        Check.SetActive(false);
        m_StagePlay.forwardDown();
    }
}
