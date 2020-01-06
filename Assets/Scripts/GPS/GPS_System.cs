using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System;

public enum GPS_CITY
{
    NONE = -1,
    ANSAN,
    BUYEO,
    CHUN_CHEON,
    CHANG_NYEONG
}

public class GPS_System : MonoBehaviour
{
    [Header("UI")]
    public Text gpsNotice;
    public GameObject GPSButton;

    [Header("City")]
    public GameObject CityButtons;
    public GameObject BackButton;
    public GameObject Ansan;
    public GameObject Buyeo;
    public GameObject Chang_neong;
    public GameObject Chun_chen;

    private Text[] aAnsanSpotsDistance = new Text[4];
    private Text[] aBuyeoSpotsDistance = new Text[4];
    private Text[] aChang_neongSpotsDistance = new Text[4];
    private Text[] aChun_chenSpotsDistance = new Text[4];
    private GameObject[] aAnsanSpotsStampBg = new GameObject[4];
    private GameObject[] aBuyeoSpotsStampBg = new GameObject[4];
    private GameObject[] aChang_neongSpotsStampBg = new GameObject[4];
    private GameObject[] aChun_chenSpotsStampBg = new GameObject[4];
    private GameObject[] aAnsanSpotsStamp = new GameObject[4];
    private GameObject[] aBuyeoSpotsStamp = new GameObject[4];
    private GameObject[] aChang_neongSpotsStamp = new GameObject[4];
    private GameObject[] aChun_chenSpotsStamp = new GameObject[4];
    private double[] aDistance = new double[4];

    private float latitude;
    private float longitude;
    private int iCount = 0;
    private string gpsText = "";
    private GPS_CITY eCity = GPS_CITY.NONE;



    // 안산...
    // 안산 문화원
    private double dAnsanSpot1Latitude  = 37.29931;
    private double dAnsanSpot1Longitude = 126.8479;
    // 안산갈대습지공원    
    private double dAnsanSpot2Latitude = 37.27401;
    private double dAnsanSpot2Longitude = 126.8395;
    // 단원미술관    
    private double dAnsanSpot3Latitude = 37.31571;
    private double dAnsanSpot3Longitude = 126.8515;
    // 성호기념관    
    private double dAnsanSpot4Latitude = 37.315084;
    private double dAnsanSpot4Longitude = 126.859732;
    
    // 충남 부여....    
    // 무량사
    private double dBuyeoSpot1Latitude = 36.31317;
    private double dBuyeoSpot1Longitude = 126.696918;
    // 대조사
    private double dBuyeoSpot2Latitude = 36.189776;
    private double dBuyeoSpot2Longitude = 126.900293;
    // 부여송국리선사
    private double dBuyeoSpot3Latitude = 36.260491;
    private double dBuyeoSpot3Longitude = 127.03242;
    // 국립부여박물관
    private double dBuyeoSpot4Latitude = 36.274881;
    private double dBuyeoSpot4Longitude = 126.91779;

    // 충남 창녕
    // 일단 정문 gps...
    // 망우정
    private double dChang_neongSpot1Latitude = 35.401917;
    private double dChang_neongSpot1Longitude = 128.513779;
    // 만옥정
    private double dChang_neongSpot2Latitude = 35.540763;
    private double dChang_neongSpot2Longitude = 128.501329;
    // 관룡사
    private double dChang_neongSpot3Latitude = 35.531196;
    private double dChang_neongSpot3Longitude = 128.553187;
    // 우포늪
    private double dChang_neongSpot4Latitude = 35.543002;
    private double dChang_neongSpot4Longitude = 128.417631;

    // 강원 춘천    
    // 소양강댐 물 문화관
    private double dChun_chenSpot1Latitude = 37.948945;
    private double dChun_chenSpot1Longitude = 127.815004;
    // 레일 바이크
    private double dChun_chenSpot2Latitude = 37.815683;
    private double dChun_chenSpot2Longitude = 127.712145;
    // 실레 이야기길
    private double dChun_chenSpot3Latitude = 37.817943;
    private double dChun_chenSpot3Longitude = 127.714743;
    // 책과 인쇄 박물관
    private double dChun_chenSpot4Latitude = 37.819349;
    private double dChun_chenSpot4Longitude = 127.724884;


    private int iStampCount = 0;
    private int iGPS_PosCount = 0;

    private double[] aSpotLatitude = new double[4];
    private double[] aSpotLongitude  = new double[4];

    // Start is called before the first frame update
    void Start()
    {        
        for(int i = 0; i < aAnsanSpotsDistance.Length; ++i)
        {
            // distance text...
            aAnsanSpotsDistance[i] = Ansan.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.transform.GetComponent<Text>();
            aBuyeoSpotsDistance[i] = Buyeo.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.transform.GetComponent<Text>();
            aChang_neongSpotsDistance[i] = Chang_neong.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.transform.GetComponent<Text>();
            aChun_chenSpotsDistance[i] = Chun_chen.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.transform.GetComponent<Text>();

            // stamp bg
            aAnsanSpotsStampBg[i] = Ansan.transform.GetChild(i).gameObject.transform.GetChild(2).gameObject;
            aBuyeoSpotsStampBg[i] = Buyeo.transform.GetChild(i).gameObject.transform.GetChild(2).gameObject;
            aChang_neongSpotsStampBg[i] = Chang_neong.transform.GetChild(i).gameObject.transform.GetChild(2).gameObject;
            aChun_chenSpotsStampBg[i] = Chun_chen.transform.GetChild(i).gameObject.transform.GetChild(2).gameObject;

            // stamp
            aAnsanSpotsStamp[i] = Ansan.transform.GetChild(i).gameObject.transform.GetChild(3).gameObject;
            aBuyeoSpotsStamp[i] = Buyeo.transform.GetChild(i).gameObject.transform.GetChild(3).gameObject;
            aChang_neongSpotsStamp[i] = Chang_neong.transform.GetChild(i).gameObject.transform.GetChild(3).gameObject;
            aChun_chenSpotsStamp[i] = Chun_chen.transform.GetChild(i).gameObject.transform.GetChild(3).gameObject;
        }

        //모바일 location 정보에 대한 권한 설정...
        if (Application.platform == RuntimePlatform.Android)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
            }
            Invoke("ReStartGPS", 2f);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(GPS_CITY.ANSAN == eCity)                                
            AnsanCalculateDistance();        
        else if (GPS_CITY.BUYEO == eCity)        
            BuyeoCalculateDistance();        
        else if (GPS_CITY.CHUN_CHEON == eCity)        
            Chang_neongCalculateDistance();        
        else if (GPS_CITY.CHANG_NYEONG == eCity)        
            Chun_chenCalculateDistance();        
        */         
    }

    //==========================================================================================================//
    //================================================= BUTTON ===================================================//
    //========================================================================================================//
    // 안산시 버튼을 클리시에....
    public void AnsanButtonEvent()
    {
        // 총 4개를 받아온다.
        // 여기서 시작해서,,,
        // 아래처럼.....

        string appID = PlayerInfo.Instance.GetAppID();
        string userID = PlayerInfo.Instance.GetUserID();
        //string kind = "social_sticker_first";
        string kind = "";
        string get_flag = "";
        string edu_type = "social_live";
        string game_type = "VR";
        int timeInfo = 101;

        /*
        if (0 == iStampCount)
            kind = "social_sticker_first";
        else if (1 == iStampCount)
            kind = "social_sticker_second";
        else if (2 == iStampCount)
            kind = "social_sticker_third";
        else if (3 == iStampCount)
            kind = "social_sticker_fourth";
        else if (4 == iStampCount)
            return;
        */

        get_flag = "Y";
        kind = "social_sticker_first";


        Debug.Log("app ID: " + appID);
        Debug.Log("user ID: " + userID);

        // 여기서 먼저 체크를 해야 한다.
        // first ~ fourth 까지 각각 받아온다.
        // 총 4개의 코루틴 함수를 받아온다.

        // db 에서 stamp 결과 받아오기....
        // 성공...
        //DatabaseManager.Instance.GetStamp(appID, userID, kind, edu_type);
        
        // test 1 update...
        // 성공...
        DatabaseManager.Instance.GpsInfoUpdate(appID, userID, kind, get_flag, edu_type, game_type, timeInfo);

        /*
        CityButtons.SetActive(false);        
        Buyeo.SetActive(false);
        Chang_neong.SetActive(false);
        Chun_chen.SetActive(false);
        Ansan.SetActive(true);
        BackButton.SetActive(true); 
        
        eCity = GPS_CITY.ANSAN;
        */
    }

    // 아래는 작업해야 함....
    // DatabaseManager  에서 받아옴....
    public void CatchStampInfo(string _strAnswer)
    {
        Debug.Log("answer: " + _strAnswer);
        string strAnswer = _strAnswer;
        if("Y" == strAnswer)
        {
            // 이미 획득한 Stamp..
            Debug.Log("already have");
        }
        else
        {
            // 획득 안한 Stamp...
            Debug.Log("none");
        }

        // 여기서 call....
        //iStampCount++;
        //AnsanButtonEvent();
    }

    public void CatchGpsUpdate(string _strAnswer)
    {
        Debug.Log("answer: " + _strAnswer);
        string strAnswer = _strAnswer;
        
        // hmm.. 갱신해야 한다.
        if ("Y" == strAnswer)
        {
            // 이미 획득한 Stamp..
        }
        else
        {
            // 획득 안한 Stamp...
        }
    }


    //================================================= GPS ===================================================//
    // GPS 정보 받기...
    // 성공.....
    public void RequestGPS_Pos()
    {
        string structure = "";
        if (0 == iGPS_PosCount)
            structure = "안산문화원";
        else if (1 == iGPS_PosCount)
            structure = "안산갈대습지공원";
        else if (2 == iGPS_PosCount)
            structure = "단원미술관";
        else if (3 == iGPS_PosCount)
            structure = "성호기념관";
        else if (4 == iGPS_PosCount)
            return;

        DatabaseManager.Instance.GetGPS_Info(structure);
    }
    public void GetGPS_Pos_1(string _strAnswer)
    {
        //Debug.Log("answer: " + _strAnswer);
        string strAnswer = _strAnswer;
        if(strAnswer != "")
        {            
            int location = strAnswer.IndexOf(",");
            int length = strAnswer.Length;            
            int minusLength = length - location;
            string strLatitude = strAnswer.Substring(0, location);
            string strLongitute = strAnswer.Substring(location+1, minusLength-1);


            aSpotLatitude[iGPS_PosCount] = double.Parse(strLatitude);
            aSpotLongitude[iGPS_PosCount] = double.Parse(strLongitute);
            AnsanCalculateDistance2(iGPS_PosCount);

            // 계속 반복한다.
            iGPS_PosCount++;
            RequestGPS_Pos();
        }
        else
        {
            // 에러....
        }
    }

    void AnsanCalculateDistance2(int _iCount)
    {
        int iTempCount = _iCount;
        // 일단 여기서 켠다....
        CityButtons.SetActive(false);
        Buyeo.SetActive(false);
        Chang_neong.SetActive(false);
        Chun_chen.SetActive(false);
        Ansan.SetActive(true);
        BackButton.SetActive(true);

        // test 용...        
        double dMyLatitude = 37.274092;
        double dMyLogitude = 126.839966;
        
        aDistance[iTempCount] = GetDistance(dMyLatitude, dMyLogitude, aSpotLatitude[iTempCount], aSpotLongitude[iTempCount]);
        if (1000 <= aDistance[iTempCount])                                                                            // 1000 미터 이상, 즉 kilo 로 표시...
        {
            double dDist22 = aDistance[iTempCount] / 1000;
            string strDistance = dDist22.ToString("N1");
            strDistance += "kilo";
            aAnsanSpotsDistance[iTempCount].text = strDistance;
        }
        else
        {
            if (100 > aDistance[iTempCount])                                                                              // 100 미터 이하일 경우 도착으로 표시...
            {
                // arrived...
                Debug.Log("arrived");
                aAnsanSpotsDistance[iTempCount].text = "";
                aAnsanSpotsStampBg[iTempCount].SetActive(true);
                aAnsanSpotsStamp[iTempCount].SetActive(true);

                // 그리고 db 에 send 한다....
                // 도착...
                RequestCheckArrive(iTempCount, dMyLatitude, dMyLogitude);
            }
            else
            {
                string strDistance = aDistance[iTempCount].ToString("N0");
                aAnsanSpotsDistance[iTempCount].text = strDistance;
            }
        }
    }

    // 유저 id, 현재 gps, appID, 장소...
    void RequestCheckArrive(int _iTmeCount, double _latitude, double _longitute)
    {
        int iTmpCount = _iTmeCount;
        string dCurrentLatitude = _latitude.ToString();
        string dCurrentLongitude = _longitute.ToString();
        string dUserGPS = dCurrentLatitude + "," + dCurrentLongitude;

        string appID = PlayerInfo.Instance.GetAppID();
        string userID = PlayerInfo.Instance.GetUserID();

        string structure = "";
        if (0 == iTmpCount)
            structure = "안산문화원";
        else if (1 == iTmpCount)
            structure = "안산갈대습지공원";
        else if (2 == iTmpCount)
            structure = "단원미술관";
        else if (3 == iTmpCount)
            structure = "성호기념관";
        else if (4 <= iTmpCount)
            return;

        DatabaseManager.Instance.GPS_ArriveUpdate(appID, userID, structure, dUserGPS);
    }




































    public void BuyeoButtonEvent()
    {        
        CityButtons.SetActive(false);
        Ansan.SetActive(false);        
        Chang_neong.SetActive(false);
        Chun_chen.SetActive(false);
        Buyeo.SetActive(true);
        BackButton.SetActive(true);
        
        eCity = GPS_CITY.BUYEO;
    }
    public void ChunChenButtonEvent()
    {        
        CityButtons.SetActive(false);
        Ansan.SetActive(false);
        Buyeo.SetActive(false);
        Chang_neong.SetActive(false);
        Chun_chen.SetActive(true);
        BackButton.SetActive(true);
        
        eCity = GPS_CITY.CHUN_CHEON;
    }
    public void Chang_neongButtonEvent()
    {        
        CityButtons.SetActive(false);
        Ansan.SetActive(false);
        Buyeo.SetActive(false);        
        Chun_chen.SetActive(false);
        Chang_neong.SetActive(true);
        BackButton.SetActive(true);
        
        eCity = GPS_CITY.CHANG_NYEONG;
    }
    public void BackButtonEvent()
    {
        CityButtons.SetActive(true);
        Buyeo.SetActive(false);
        Chang_neong.SetActive(false);
        Chun_chen.SetActive(false);
        Ansan.SetActive(false);        
        BackButton.SetActive(false);

        eCity = GPS_CITY.NONE;
    }

    //============================================= GPS INITIALIZE =================================================//   
    void ReStartGPS()
    {
        StartCoroutine(GpsStart());        
    }
    IEnumerator GpsStart()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
            gpsText = "User has not enabled GPS";
            gpsNotice.text = gpsText;
            Invoke("ReStartGPS", 4f);
            yield break;
        }        
        Input.location.Start(5, 10);
        int maxWait = 20;

        gpsText = Input.location.status.ToString();        
        gpsNotice.text = gpsText;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            //Input.location.status
            gpsText = Input.location.status.ToString();
            gpsNotice.text = gpsText;
            maxWait--;
        }

        while (true)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            gpsText = "lat: " + latitude.ToString() + "   long: " + longitude.ToString() + "  State: " + iCount;
            gpsNotice.text = gpsText;            
            iCount++;
            yield return new WaitForSeconds(0.5f);
        }
    }








    //=========================================================================================================//    
    // 일단 각자의 거리가 나타난다.    
    void AnsanCalculateDistance()
    {
        // test 용...        
        double dMyLatitude = 37.274094;
        double dMyLogitude = 126.839968;

        if (Application.platform == RuntimePlatform.Android)
        {            
            aDistance[0] = GetDistance(latitude, longitude, dAnsanSpot1Latitude, dAnsanSpot1Longitude);
            aDistance[1] = GetDistance(latitude, longitude, dAnsanSpot2Latitude, dAnsanSpot2Longitude);
            aDistance[2] = GetDistance(latitude, longitude, dAnsanSpot3Latitude, dAnsanSpot3Longitude);
            aDistance[3] = GetDistance(latitude, longitude, dAnsanSpot4Latitude, dAnsanSpot4Longitude);
        }
        else
        {                                    
            aDistance[0] = GetDistance(dMyLatitude, dMyLogitude, dAnsanSpot1Latitude, dAnsanSpot1Longitude);
            aDistance[1] = GetDistance(dMyLatitude, dMyLogitude, dAnsanSpot2Latitude, dAnsanSpot2Longitude);
            aDistance[2] = GetDistance(dMyLatitude, dMyLogitude, dAnsanSpot3Latitude, dAnsanSpot3Longitude);
            aDistance[3] = GetDistance(dMyLatitude, dMyLogitude, dAnsanSpot4Latitude, dAnsanSpot4Longitude);
        }

        for(int i = 0; i < aDistance.Length; ++i)
        {
            if(1000 <= aDistance[i])                                                                            // 1000 미터 이상, 즉 kilo 로 표시...
            {
                double dDist22 = aDistance[i] / 1000;
                string strDistance = dDist22.ToString("N1");
                strDistance += "kilo";
                aAnsanSpotsDistance[i].text = strDistance;
            }
            else
            {
                if(100 > aDistance[i])                                                                              // 100 미터 이하일 경우 도착으로 표시...
                {
                    // arrived...
                    Debug.Log("arrived");


                    aAnsanSpotsDistance[i].text = "";
                    aAnsanSpotsStampBg[i].SetActive(true);
                    aAnsanSpotsStamp[i].SetActive(true);
                }
                else
                {
                    string strDistance = aDistance[i].ToString("N0");
                    aAnsanSpotsDistance[i].text = strDistance;
                }                
            }
        }
    }    
    void BuyeoCalculateDistance()
    {
        // test 용...        
        double dMyLatitude = 37.274094;
        double dMyLogitude = 126.839968;

        if (Application.platform == RuntimePlatform.Android)
        {
            aDistance[0] = GetDistance(latitude, longitude, dBuyeoSpot1Latitude, dBuyeoSpot1Longitude);
            aDistance[1] = GetDistance(latitude, longitude, dBuyeoSpot2Latitude, dBuyeoSpot2Longitude);
            aDistance[2] = GetDistance(latitude, longitude, dBuyeoSpot3Latitude, dBuyeoSpot3Longitude);
            aDistance[3] = GetDistance(latitude, longitude, dBuyeoSpot4Latitude, dBuyeoSpot4Longitude);
        }
        else
        {
            aDistance[0] = GetDistance(dMyLatitude, dMyLogitude, dBuyeoSpot1Latitude, dBuyeoSpot1Longitude);
            aDistance[1] = GetDistance(dMyLatitude, dMyLogitude, dBuyeoSpot2Latitude, dBuyeoSpot2Longitude);
            aDistance[2] = GetDistance(dMyLatitude, dMyLogitude, dBuyeoSpot3Latitude, dBuyeoSpot3Longitude);
            aDistance[3] = GetDistance(dMyLatitude, dMyLogitude, dBuyeoSpot4Latitude, dBuyeoSpot4Longitude);
        }

        for (int i = 0; i < aDistance.Length; ++i)
        {
            if (1000 <= aDistance[i])                                                                                           // 1000 미터 이상, 즉 kilo 로 표시...
            {
                double dDist22 = aDistance[i] / 1000;
                string strDistance = dDist22.ToString("N1");
                strDistance += "kilo";
                aBuyeoSpotsDistance[i].text = strDistance;
            }
            else
            {
                if (100 > aDistance[i])                                                                                             // 100 미터 이하일 경우 도착으로 표시...
                {
                    // arrived...
                    Debug.Log("arrived");
                    aBuyeoSpotsDistance[i].text = "";
                    aBuyeoSpotsStampBg[i].SetActive(true);
                    aBuyeoSpotsStamp[i].SetActive(true);
                }
                else
                {
                    string strDistance = aDistance[i].ToString("N0");
                    aBuyeoSpotsDistance[i].text = strDistance;
                }
            }
        }
    }
    void Chang_neongCalculateDistance()
    {
        // test 용...        
        double dMyLatitude = 37.274094;
        double dMyLogitude = 126.839968;

        if (Application.platform == RuntimePlatform.Android)
        {
            aDistance[0] = GetDistance(latitude, longitude, dChang_neongSpot1Latitude, dChang_neongSpot1Longitude);
            aDistance[1] = GetDistance(latitude, longitude, dChang_neongSpot2Latitude, dChang_neongSpot2Longitude);
            aDistance[2] = GetDistance(latitude, longitude, dChang_neongSpot3Latitude, dChang_neongSpot3Longitude);
            aDistance[3] = GetDistance(latitude, longitude, dChang_neongSpot4Latitude, dChang_neongSpot4Longitude);
        }
        else
        {
            aDistance[0] = GetDistance(dMyLatitude, dMyLogitude, dChang_neongSpot1Latitude, dChang_neongSpot1Longitude);
            aDistance[1] = GetDistance(dMyLatitude, dMyLogitude, dChang_neongSpot2Latitude, dChang_neongSpot2Longitude);
            aDistance[2] = GetDistance(dMyLatitude, dMyLogitude, dChang_neongSpot3Latitude, dChang_neongSpot3Longitude);
            aDistance[3] = GetDistance(dMyLatitude, dMyLogitude, dChang_neongSpot4Latitude, dChang_neongSpot4Longitude);
        }

        for (int i = 0; i < aDistance.Length; ++i)
        {
            if (1000 <= aDistance[i])                                                                                                               // 1000 미터 이상, 즉 kilo 로 표시...
            {
                double dDist22 = aDistance[i] / 1000;
                string strDistance = dDist22.ToString("N1");
                strDistance += "kilo";
                aChang_neongSpotsDistance[i].text = strDistance;
            }
            else
            {
                if (100 > aDistance[i])                                                                                                              // 100 미터 이하일 경우 도착으로 표시...
                {
                    // arrived...
                    Debug.Log("arrived");
                    aChang_neongSpotsDistance[i].text = "";
                    aChang_neongSpotsStampBg[i].SetActive(true);
                    aChang_neongSpotsStamp[i].SetActive(true);
                }
                else
                {
                    string strDistance = aDistance[i].ToString("N0");
                    aChang_neongSpotsDistance[i].text = strDistance;
                }
            }
        }
    }
    void Chun_chenCalculateDistance()
    {
        // test 용...        
        double dMyLatitude = 37.274094;
        double dMyLogitude = 126.839968;

        if (Application.platform == RuntimePlatform.Android)
        {
            aDistance[0] = GetDistance(latitude, longitude, dChun_chenSpot1Latitude, dChun_chenSpot1Longitude);
            aDistance[1] = GetDistance(latitude, longitude, dChun_chenSpot2Latitude, dChun_chenSpot2Longitude);
            aDistance[2] = GetDistance(latitude, longitude, dChun_chenSpot3Latitude, dChun_chenSpot3Longitude);
            aDistance[3] = GetDistance(latitude, longitude, dChun_chenSpot4Latitude, dChun_chenSpot4Longitude);
        }
        else
        {
            aDistance[0] = GetDistance(dMyLatitude, dMyLogitude, dChun_chenSpot1Latitude, dChun_chenSpot1Longitude);
            aDistance[1] = GetDistance(dMyLatitude, dMyLogitude, dChun_chenSpot2Latitude, dChun_chenSpot2Longitude);
            aDistance[2] = GetDistance(dMyLatitude, dMyLogitude, dChun_chenSpot3Latitude, dChun_chenSpot3Longitude);
            aDistance[3] = GetDistance(dMyLatitude, dMyLogitude, dChun_chenSpot4Latitude, dChun_chenSpot4Longitude);
        }

        for (int i = 0; i < aDistance.Length; ++i)
        {
            if (1000 <= aDistance[i])                                                                                                            // 1000 미터 이상, 즉 kilo 로 표시...
            {
                double dDist22 = aDistance[i] / 1000;
                string strDistance = dDist22.ToString("N1");
                strDistance += "kilo";
                aChun_chenSpotsDistance[i].text = strDistance;
            }
            else
            {
                if (100 > aDistance[i])                                                                                                             // 100 미터 이하일 경우 도착으로 표시...
                {
                    // arrived...
                    Debug.Log("arrived");
                    aChun_chenSpotsDistance[i].text = "";
                    aChun_chenSpotsStampBg[i].SetActive(true);
                    aChun_chenSpotsStamp[i].SetActive(true);
                }
                else
                {
                    string strDistance = aDistance[i].ToString("N0");
                    aChun_chenSpotsDistance[i].text = strDistance;
                }
            }
        }
    }

    //=========================================================================================================//
    //================================================= EXTRA FUNCTION ===========================================//
    //=========================================================================================================//    
    // 거리 계산 Function...
    double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double theta = lon1 - lon2;
        double dLat1 = deg2rad(lat1);
        double dLat2 = deg2rad(lat2);
        double dTheta = deg2rad(theta);

        double dist = Math.Sin(dLat1) * Math.Sin(dLat2) + Math.Cos(dLat1) * Math.Cos(dLat2) * Math.Cos(dTheta);
        dist = Math.Acos(dist);
        double dDistResult = rad2deg(dist);

        dDistResult = dDistResult * 60 * 1.1515;        
        dDistResult = dDistResult * 1.6093344;
        dDistResult = dDistResult * 1000.0;
        return dDistResult;
    }
    // 방향 각도 구하기....
    public short BearingP1toP2(double P1_latitude, double P1_longitude, double P2_latitude, double P2_longitude)
    {
        // 현재 위치 : 위도나 경도는 지구 중심을 기반으로 하는 각도이기 때문에 라디안 각도로 변환한다.
        double cur_Lat_radian = P1_latitude * (3.141592 / 180);
        double cur_Lon_radian = P1_longitude * (3.141592 / 180);
        // 목표 위치 : 위도나 경도는 지구 중심을 기반으로 하는 각도이기 때문에 라디안 각도로 변환한다.
        double Dest_Lat_radian = P2_latitude * (3.141592 / 180);
        double Dest_Lon_radian = P2_longitude * (3.141592 / 180);
        
        // radian distance               
        //radian_distance = Mathf.Acos(Mathf.Sin(DoubleToFloat(cur_Lat_radian)) * Mathf.Sin(DoubleToFloat(Dest_Lat_radian)) + Mathf.Cos(DoubleToFloat(cur_Lat_radian)) * Mathf.Cos(DoubleToFloat(Dest_Lat_radian)) * Mathf.Cos(DoubleToFloat(cur_Lon_radian - Dest_Lon_radian)));
        double radian_distance = Math.Acos(Math.Sin(cur_Lat_radian)) * Math.Sin(Dest_Lat_radian) + Math.Cos(cur_Lat_radian) * Math.Cos(Dest_Lat_radian) * Math.Cos(cur_Lon_radian - Dest_Lon_radian);

        // 목적지 이동 방향을 구한다.(현재 좌표에서 다음 좌표로 이동하기 위해서는 방향을 설정해야 한다. 라디안값이다.
        //double radian_bearing = Mathf.Acos((Mathf.Sin(DoubleToFloat(Dest_Lat_radian)) - Mathf.Sin(DoubleToFloat(cur_Lat_radian)) * Mathf.Cos(DoubleToFloat(radian_distance))) / (Mathf.Cos(DoubleToFloat(cur_Lat_radian)) * Mathf.Sin(DoubleToFloat(radian_distance))));
        double radian_bearing = Math.Acos(Math.Sin(Dest_Lat_radian)) - Math.Sin(cur_Lat_radian) * Math.Cos(radian_distance) / Math.Cos(cur_Lat_radian) * Math.Sin(radian_distance);

        // acos의 인수로 주어지는 x는 360분법의 각도가 아닌 radian(호도)값이다.       
        double true_bearing = 0;
        if (Math.Sin(Dest_Lon_radian - cur_Lon_radian) < 0)
        {
            true_bearing = radian_bearing * (180 / 3.141592);
            true_bearing = 360 - true_bearing;
        }
        else
        {
            true_bearing = radian_bearing * (180 / 3.141592);
        }
        return (short)true_bearing;
    }
    static double deg2rad(double _deg)
    {
        return (_deg * Mathf.PI / 180d);
    }
    static double rad2deg(double _rad)
    {
        return (_rad * 180d / Mathf.PI);
    }    
}
