using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading.Tasks;

public class AssetBundleMgr : MonoBehaviour
{
    public Text noticeText;
    private bool bLoading = false;
    string fileName = "mobile_ver";

    // Start is called before the first frame update
    void Start()
    {
        //CheckAssetBundleVersion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 먼저 경로 확인.... 즉 버전 확인...
    public void CheckAssetBundleVersion()
    {        
        string dir = "";
        if (Application.platform == RuntimePlatform.Android)
        {            
            dir = Application.persistentDataPath + "/" + "AssetBundle";
            if (true != Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        else
        {
            // pc 테스트용....
            dir = "C:/Users/Gana/Downloads/AssetBundle/";
        }
        
        
        if (Directory.Exists(dir))
        {
            string version = "";
            DirectoryInfo di = new DirectoryInfo(dir);            

            foreach(var item in di.GetDirectories())
            {
                Debug.Log("item: " + item.Name);
                version = item.Name;
            }

            if("" == version)
            {
                Debug.Log("none assetbundle");
                
                version = "0.0";
                Directory.CreateDirectory(dir + version);
                string appVersion = "1.0";
                string appID = PlayerInfo.Instance.GetAppID();

                DatabaseManager.Instance.RequestAssetBundleVersionCheck(appVersion, version, appID);
            }
            else
            {
                // 만약에 파일이 없다면...???                
                if (Application.platform == RuntimePlatform.Android)
                {
                    dir = Application.persistentDataPath + "/" + "AssetBundle" + "/" + "assetbundle_" + version;
                    if (true != File.Exists(dir))
                    {
                        version = "0.0";
                    }
                }

                string appVersion = "1.0";
                string appID = PlayerInfo.Instance.GetAppID();
                DatabaseManager.Instance.RequestAssetBundleVersionCheck(appVersion, version, appID);
            }             
        }        
    }
    // 답변 받음...
    public void GetAssetBundleMsg(string _strMsg)
    {
        string strMsg = _strMsg;
        Debug.Log(strMsg);

        if("None" == strMsg)
        {
            // 없데이트 필요없음..
            // 바로 로그인... or 무엇인가...
        }
        else
        {
            // 일단 자르기...
            int location1 = strMsg.IndexOf(":");                   
            string strCategory = strMsg.Substring(0, location1);
            //Debug.Log("cut 1: " + strCategory);

            int location2 = strMsg.IndexOf(",");            
            int minusLength = location2 - location1;
            string strVersion = strMsg.Substring(location1+1, minusLength-1);
            //Debug.Log("cut 2: " + strVersion);

            int length = strMsg.Length;
            int minusLength2 = length - location2;            
            string strLongitute = strMsg.Substring(location2 + 1, minusLength2 - 1);
            //Debug.Log("cut 3: " + strLongitute);

            int location3 = strMsg.IndexOf("/");
            int minusLength3 = length - location3;
            string strBundleName = strMsg.Substring(location3 + 1, minusLength3 - 1);
            //Debug.Log("cut 4: " + strBundleName);
            
            if ("res" == strCategory)
            {
                // 아래는 확인해야 한다.. 
                //string dir = "C:/Users/Gana/Downloads/AssetBundle/";
                string dir = "";
                if (Application.platform == RuntimePlatform.Android)
                {
                    dir = Application.persistentDataPath + "/AssetBundle/";
                }
                else
                {
                    dir = "C:/Users/Gana/Downloads/AssetBundle/";
                }

                // 아래에서 굳이 확인할 필요는 없음.. 위에서 생성함...
                if (Directory.Exists(dir))
                {
                    string version = "";
                    DirectoryInfo di = new DirectoryInfo(dir);

                    foreach (var item in di.GetDirectories())
                    {                        
                        version = item.Name;
                        Directory.CreateDirectory(dir + strVersion);
                        
                        // 안에 무엇인가가 있을 경우...
                        DirectoryInfo di2 = new DirectoryInfo(dir+ version);
                        System.IO.FileInfo[] test = di2.GetFiles();                        
                        foreach(var item2 in test)
                        {
                            File.Delete(dir + version + "/" + item2.Name);
                        }

                        Directory.Delete(dir + version);
                        // 그리고 다운로드....                        
                        RequestAssetBundleDownLoad(strLongitute, strVersion, strBundleName);
                        break;
                    }   
                }
            }
            else if ("software" == strCategory)
            {                
                // 그냥 경고 메시지만 팝업...
            }
        }
    }

    void RequestAssetBundleDownLoad(string _strUrl, string _strDir, string _strBundleName)
    {        
        StartCoroutine(AssetBudleDownLoad(_strUrl, _strDir, _strBundleName));
    }
    // 파이어 베이스에서 내려받음....
    IEnumerator AssetBudleDownLoad(string _strUrl, string _strDir, string _strBundleName)
    {        
        string assetBundleName = _strBundleName;
        bLoading = true;
        Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        // Create a storage reference from our storage service
        Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl("gs://sohn123-f1d8d.appspot.com/");        
        // Create a reference to the file you want to upload
        //Firebase.Storage.StorageReference rivers_ref = storage_ref.Child("AssetBundle/" + assetBundleName);                            // 업로드 확인해야 함...
        Firebase.Storage.StorageReference rivers_ref = storage_ref.Child(_strUrl);                                                                              // 업로드 확인해야 함...

    
        string local_file = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            // 아래 수정 해야 함....
            if (!Directory.Exists(Application.persistentDataPath + "/AssetBundle/ " + _strDir))                                                    //폴더가 있는지 체크하고 없으면 만든다.
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/AssetBundle/ " + _strDir);
            }            
            local_file = Application.persistentDataPath + "/AssetBundle/" + _strDir + "/" + _strBundleName;
        }
        else
        {            
            local_file = "C:/Users/Gana/Downloads/AssetBundle/" + _strDir + "/"+ _strBundleName;                                                // PC ....
        }

        Task TmpTask = rivers_ref.GetFileAsync(local_file, new Firebase.Storage.StorageProgress<Firebase.Storage.DownloadState>(state =>
        {
            // 다운로드 진행률....
            //Debug.Log(string.Format("Progress: {0} of {1} bytes transferred.", state.BytesTransferred, state.TotalByteCount));
            PercentView(state.BytesTransferred, state.TotalByteCount);

        })).ContinueWith(task => {
            //Debug.Log(string.Format("OnClickDownload::IsCompleted:{0} IsCanceled:{1} IsFaulted:{2}", task.IsCompleted, task.IsCanceled, task.IsFaulted));
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
                bLoading = false;
            }
            else
            {
                //Debug.Log("Finished downloading...");
            }
        });

        yield return new WaitUntil(() => TmpTask.IsCompleted);
        //PrintState("Downloading Complete");
        //StartCoroutine(AssetBundleLoadFromLocal());
        Debug.Log("Finished");
    }
    // 밑에는 필요 없을 듯....
    // 답변 받음...
    public void GetAssetBundleVersionURL(string _strMsg)
    {
        string strMsg = _strMsg;
        Debug.Log("res: " + strMsg);
    }

    // 아래는 수정 hmm....
    // 보유하고 있는 Assetbundle 로드 하는 function....
    IEnumerator AssetBundleLoadFromLocal()
    {
        string assetBundleDirectory = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            assetBundleDirectory = Application.persistentDataPath + "/" + "AssetBundle";            
        }
        else
        {
            assetBundleDirectory = "C:/Users/Gana/Downloads/AssetBundle/AssetBundle_PC";
        }

        //var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(assetBundleDirectory, "assetbundle_0"));
        var myLoadedAssetBundle2 = AssetBundle.LoadFromFileAsync(Path.Combine(assetBundleDirectory, fileName));
        if (!myLoadedAssetBundle2.isDone)
        {                        
            yield return myLoadedAssetBundle2;
        }

        /*
        // 성공... pc and mobile...
        WWW www = WWW.LoadFromCacheOrDownload(new System.Uri(assetBundleDirectory + "/" + "mobile_ver").AbsoluteUri, 0);        
        if(!www.isDone)
        {
            Debug.Log("progress: " + www.progress);
            PrintState("progress: " + www.progress);
            yield return www;
        }        
        var myLoadedAssetBundle = www.assetBundle;
        yield return myLoadedAssetBundle;
        
        //AssetBundleRequest bundleRequest = myLoadedAssetBundle.LoadAllAssetsAsync(typeof(GameObject));
        //yield return bundleRequest;
        
        //var myLoadedAssetBundle = myLoadedAssetBundle2.assetBundle;
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Fail Load");
            PrintState("Fail Load");
            bLoading = false;
            yield break;
        }        
        else
        {
            Debug.Log("Successed to load AssetBundle");
        }
        */

        var myLoadedAssetBundle = myLoadedAssetBundle2.assetBundle;
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Fail Load");            
            bLoading = false;
            yield break;
        }
        else
        {
            Debug.Log("Successed to load AssetBundle");            
        }


        /*
        AssetBundleRequest assetImg = myLoadedAssetBundle.LoadAssetAsync<GameObject>("Image");
        yield return assetImg;
        
        GameObject rw = assetImg.asset as GameObject;
        GameObject child = Instantiate(rw) as GameObject;

        child.transform.parent = FindObjectOfType<Canvas>().transform;
        child.name = "Kei";
        child.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        child.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        child.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        child.GetComponent<RectTransform>().anchoredPosition = new Vector2(-250.0f, -80.0f);
        //child.GetComponent<RectTransform>().sizeDelta = new Vector2(300.0f, 260.0f);
        child.GetComponent<RectTransform>().sizeDelta = new Vector2(900.0f, 600.0f);
        //child.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-250.0f, -80.0f, 0.0f);
        child.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, 0.0f, 0.0f);
        child.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        child.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, 0f);
        */
        //myLoadedAssetBundle.Unload(false);

        bLoading = false;        
    }



    //================================================ PERCENT ==============================================//
    // 프로그래스 바 등을 위한 percent 찍는 함수...
    void PercentView(long _presentTransfer, long _transferCount)
    {
        double presentTransfer = _presentTransfer;
        double transferCount = _transferCount;
        double percent = (presentTransfer / transferCount) * 100;
        percent = Mathf.Round((float)percent);
        string strPercent = percent.ToString();
        string stringPercent = "Progress : " + strPercent + " %";
        Debug.Log(stringPercent);
    }























    // 아래는 삭제 예정....
    //================================================ useless ==============================================//
    public void AssetBundleLoadButtonEvent()
    {
        Debug.Log("click load");

        if (true == bLoading)
            return;

        //StartCoroutine(AssetBundleLoadFromLocal());
        StartCoroutine(AssetBudleDownloadLocal());
    }
    // download from Firebase Storage..
    IEnumerator AssetBudleDownloadLocal()
    {
        bLoading = true;

        // 로컬 방식....
        Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        // Create a storage reference from our storage service
        Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl("gs://fir-authtest22.appspot.com");
        // Create a reference to the file you want to upload
        Firebase.Storage.StorageReference rivers_ref = storage_ref.Child("AssetBundleTest1/" + fileName);

        string local_file = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/" + "AssetBundle"))            //폴더가 있는지 체크하고 없으면 만듭니다.
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/" + "AssetBundle");
            }
            if (File.Exists(Application.persistentDataPath + "/" + "AssetBundle" + "/" + fileName))
            {
                StartCoroutine(AssetBundleLoadFromLocal());
                yield break;
            }
            local_file = Application.persistentDataPath + "/" + "AssetBundle" + "/" + fileName;
        }
        else
        {
            if (File.Exists("C:/Users/Gana/Downloads/AssetBundle/AssetBundle_PC" + "/" + fileName))
            {
                StartCoroutine(AssetBundleLoadFromLocal());
                yield break;
            }
            local_file = "C:/Users/Gana/Downloads/AssetBundle/AssetBundle_PC" + "/" + fileName;
        }

        //var TmpTask = rivers_ref.GetFileAsync(local_file).ContinueWith(task => {
        var TmpTask = rivers_ref.GetFileAsync(local_file, new Firebase.Storage.StorageProgress<Firebase.Storage.DownloadState>(state =>
        {
            Debug.Log(string.Format("Progress: {0} of {1} bytes transferred.", state.BytesTransferred, state.TotalByteCount));
            PercentView(state.BytesTransferred, state.TotalByteCount);

        })).ContinueWith(task => {
            Debug.Log(string.Format("OnClickDownload::IsCompleted:{0} IsCanceled:{1} IsFaulted:{2}", task.IsCompleted, task.IsCanceled, task.IsFaulted));
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
                // Uh-oh, an error occurred!
                //authUI.ShowNotice("Error....");
                Debug.Log("Oops,, Error..");
                bLoading = false;
            }
            else
            {
                Debug.Log("Finished downloading...");
            }
        });

        yield return new WaitUntil(() => TmpTask.IsCompleted);
        StartCoroutine(AssetBundleLoadFromLocal());
    }




}
