using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.Networking;

public class JoinMemberManager : MonoBehaviour
{

    [Header("Sign Up")]
    public InputField signupID;        
    public InputField signupPassword;
    public InputField signupConfirmPassword;

    public InputField signupEmail;
    public InputField signupPhone;

    public Dropdown groupDropdown;
    public Dropdown genderDropdown;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }






    public void ConfirmButtonEvent()
    {
        string tmpGroup = groupDropdown.transform.GetChild(0).gameObject.GetComponent<Text>().text;
        string tmpGender = genderDropdown.transform.GetChild(0).gameObject.GetComponent<Text>().text;

        string tmpId = signupID.text;
        string tmpPw = signupPassword.text;
        string tmpConirmId = signupConfirmPassword.text;
        string tmpEmail = signupEmail.text;
        string tmpPhone = signupPhone.text;

        Debug.Log("haha");

        //StartCoroutine(MySqlSignUp(tmpId, tmpPw, tmpConirmId, tmpGroup, tmpGender, tmpEmail, tmpPhone));
    }




    IEnumerator MySqlSignUp(string _strUserID, string _strUserPW, string _strConfirmPW, string _strType, string _strGender, string _strEmail, string _strPhone)
    {
        if (_strUserPW != _strConfirmPW)
        {
            // 비번과 비번확인 일치하지 않음...
            Debug.Log("pass word not match!");
            yield break;
        }

        bool bCheckExist = false;
        WWWForm form2 = new WWWForm();
        form2.AddField("userID", _strUserID);
        // 먼저 아이디를 중복검사한다.
        using (UnityWebRequest www2 = UnityWebRequest.Post("http://192.168.1.183/ARVR/pages/unity_php/checkid_unity.php", form2))
        {
            yield return www2.SendWebRequest();
            if (www2.isNetworkError || www2.isHttpError)
            {
                Debug.Log(www2.error);
            }
            else
            {
                Debug.Log(www2.downloadHandler.text);
                string strHandlerText = www2.downloadHandler.text;
                string resultText = strHandlerText.Trim();

                if ("exist" == resultText)                                                              // 해당 아이디 이미 존재함...
                {
                    bCheckExist = true;
                    Debug.Log("already exsit");
                }
                else
                {
                    bCheckExist = false;
                    Debug.Log("ok to use");
                }
            }
        }

        if (true == bCheckExist)
        {
            // 팝업 오픈 등으로 notice 한다.
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("userID", _strUserID);
        form.AddField("userPW", _strUserPW);
        form.AddField("userType", _strType);
        form.AddField("userGender", _strGender);
        form.AddField("userEmail", _strEmail);
        form.AddField("userPhone", _strPhone);

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.1.183/ARVR/pages/unity_php/admission_unity.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string strHandlerText = www.downloadHandler.text;
                string resultText = strHandlerText.Trim();

                if (_strUserID == resultText)
                {
                    // 아이디 생성 후에 로그인...
                    Debug.Log("login... success");
                    //lobbyUI.ShowDatabaseLoggedinPanel();
                    //lobbyUI.databaseLoggedinID.text = resultText;
                    //lobbyUI.databaseLoggedinText.text = "Log in Success";
                }
                else
                {
                    Debug.Log("login... fail");
                    // 로그인 실패.... 일단 무조건 password error 라고 뜨는 듯...
                    //authUI.ShowNotice(resultText);
                }
            }
        }
    }















}
