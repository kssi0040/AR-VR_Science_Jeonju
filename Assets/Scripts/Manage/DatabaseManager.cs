using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DatabaseManager : MonoBehaviour
{
    // gps 를 어떻게 받아올것인가...?
    private static DatabaseManager instance = null;
    private static readonly object padlock = new object();


    // 아래도 전부 삭제 예정....
    public AuthUIMgr lobbyUI;
    // login..
    [Header("Login")]
    public InputField userId;
    public InputField userPassword;
    public Text tempText;
    // sign up..
    [Header("Sign Up")]
    public InputField signupID;
    public InputField signupEmail;
    public InputField signupPassword;
    public InputField signupConfirmPassword;
    public InputField signupPhone;
    public Dropdown typeDropdown;
    public Dropdown genderDropdown;



     //========================================================================================================//
    public static DatabaseManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new DatabaseManager();
                }
                return instance;
            }
        }
    }

    private void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
    //========================================================================================================//



    //================================================= MYSQL =================================================//    
    //========================================================================================================//        
    //================================================= LOGIN ==================================================//
    //========================================================================================================//        
    public void LogInSystemEvent(string _strUserID, string _strUserPW)
    {
        StartCoroutine(MySqlLogIn(_strUserID, _strUserPW));        
    }
    IEnumerator MySqlLogIn(string _strUserID, string _strUserPW)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", _strUserID);
        form.AddField("userPW", _strUserPW);

        // 아래에서 php 바꿔서 처리한다.
        //using (UnityWebRequest www = UnityWebRequest.Post("http://eduarvr.dlinkddns.com/pages/unity_php/loginTest.php", form))
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.1.183/pages/unity_php/loginTest.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string strHandlerText = www.downloadHandler.text;
                string resultText = strHandlerText.Trim();
                if ("password ERROR" == resultText)
                {
                    Debug.Log("password error");
                }
                else
                {
                    if (_strUserID == resultText)
                    {
                        //Debug.Log("login... sucesss ID: "+ resultText);
                        PlayerInfo.Instance.SetUserID(resultText);
                        //SceneManager.LoadScene("GPS_Scene");          
                        GameObject.Find("ButtonManager").SendMessage("LoginEventCallBack", resultText);
                    }
                }
            }
        }
    }
    //================================================================================================//        
    //============================================== GPS ===============================================//
    //================================================================================================//        
    // 스탬프 정보 Update or Insert...    
    public void GpsInfoUpdate(string _strAppID, string _strUserID, string _strBadgeType, string _strFlag, string _strEduType, string _strGameType, int _strTime)
    {
        StartCoroutine(GpsResultUpdate(_strAppID, _strUserID, _strBadgeType, _strFlag, _strEduType, _strGameType, _strTime));        
    }
    // 스탬프 정보 Update or Insert...
    IEnumerator GpsResultUpdate(string _strAppID,  string _strUserID, string _strBadgeType, string _strFlag, string _strEduType, string _strGameType, int _strTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("appID", _strAppID);
        form.AddField("userID", _strUserID);

        form.AddField("userBadgeType", _strBadgeType);
        form.AddField("userBadgeFlag", _strFlag);
        form.AddField("userEduType", _strEduType);
        form.AddField("userGameType", _strGameType);
        form.AddField("userTime", _strTime);

        using (UnityWebRequest www = UnityWebRequest.Post("http://175.198.74.238/pages/unity_php/unity_sticker.php", form))
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
                
                GameObject.Find("GPS_System").SendMessage("CatchGpsUpdate", resultText);
            }
        }
    }

    //============================================== 도착시에 ===============================================//
    public void GPS_ArriveUpdate(string _strAppID, string _strUserID, string _strLocation, string _strGPSinfo)
    {
        StartCoroutine(ArriveUpdate(_strAppID, _strUserID, _strLocation, _strGPSinfo));
    }
    IEnumerator ArriveUpdate(string _strAppID, string _strUserID, string _strLocation, string _strGPSinfo)
    {
        WWWForm form = new WWWForm();
        // 아래부터 작업해야 함....
        form.AddField("appID", _strAppID);
        form.AddField("userID", _strUserID);
        form.AddField("structure", _strLocation);
        form.AddField("GPSinfo", _strGPSinfo);

        // 아래에서 php 바꿔서 처리한다.
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.1.183/pages/unity_php/unity_GPS_log.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string strHandlerText = www.downloadHandler.text;
                string resultText = strHandlerText.Trim();

                // 결과를 보내는 것 처리해야 함...
                //SendGpsResult(resultText);
                Debug.Log(resultText);
            }
        }
    }

    //============================================== 스탬프 정보 받기 ===============================================//
    // 새로 만듬. 스탬프 정보를 받아오는 함수....
    public void GetStamp(string _strAppID, string _strUserID, string _strBadgeType, string _strEduType)
    {
        StartCoroutine(GetStameInfo(_strAppID, _strUserID, _strBadgeType, _strEduType));
    }
    // 새로만듬. 스탬프 정보를 받아오는 함수....
    IEnumerator GetStameInfo(string _strAppID, string _strUserID, string _strBadgeType, string _strEduType)
    {
        WWWForm form = new WWWForm();
        form.AddField("appID", _strAppID);
        form.AddField("userID", _strUserID);
        form.AddField("userBadgeType", _strBadgeType);        
        form.AddField("userEduType", _strEduType);
        
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.1.183/pages/unity_php/sticker_check.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                //tempText.text = www.error;
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string strHandlerText = www.downloadHandler.text;
                string resultText = strHandlerText.Trim();
                GameObject.Find("GPS_System").SendMessage("CatchStampInfo", resultText);                
            }
        }
    }    

    //============================================== GPS 위도, 경도 받아오기 ===============================================//
    // gps 값 받아오기...
    public void GetGPS_Info(string _structure)
    {
        StartCoroutine(GetGPS_Pos(_structure));
    }
    IEnumerator GetGPS_Pos(string _structure)
    {
        WWWForm form = new WWWForm();
        form.AddField("structure", _structure);        

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.1.183/pages/unity_php/GPS_state_check.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string strHandlerText = www.downloadHandler.text;
                string resultText = strHandlerText.Trim();
                GameObject.Find("GPS_System").SendMessage("GetGPS_Pos_1", resultText);
            }
        }
    }

    //================================================================================================//        
    //========================================== ASSET BUNDLE ===========================================//
    //================================================================================================//            ..
    // 에셋번들 버전 보내기...
    public void RequestAssetBundleVersionCheck(string _appVersion, string _resVersion, string _appID)
    {
        StartCoroutine(AssetBundleVersionCheck(_appVersion, _resVersion, _appID));
    }
    IEnumerator AssetBundleVersionCheck(string _appVersion, string _resVersion, string _appID)
    {
        WWWForm form = new WWWForm();
        form.AddField("appID", _appID);
        form.AddField("appVersion", _appVersion);
        form.AddField("resVersion", _resVersion);

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.1.183/pages/unity_php/unity_app_check.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string strHandlerText = www.downloadHandler.text;
                string resultText = strHandlerText.Trim();
                //ResponseAssetBundleVersion(resultText);                
                GameObject.Find("AssetBundleManager").SendMessage("GetAssetBundleMsg", resultText);
            }
        }
    }
    
    // 에셋번들 URL 받아오기...
    public void RequestAssetBundleVersionURL(string _appVersion, string _resVersion, string _appID)
    {
        StartCoroutine(AssetBundleVersionURL(_appVersion, _resVersion, _appID));
    }
    IEnumerator AssetBundleVersionURL(string _appVersion, string _resVersion, string _appID)
    {
        WWWForm form = new WWWForm();
        form.AddField("appID", _appID);
        form.AddField("appVersion", _appVersion);
        form.AddField("resVersion", _resVersion);

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.1.183/pages/unity_php/unity_app_check.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string strHandlerText = www.downloadHandler.text;
                string resultText = strHandlerText.Trim();
                GameObject.Find("AssetBundleManager").SendMessage("GetAssetBundleVersionURL", resultText);                
            }
        }
    }
    
}
