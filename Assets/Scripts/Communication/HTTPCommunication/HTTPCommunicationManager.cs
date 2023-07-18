using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPCommunicationManager : MonoBehaviour
{
    [SerializeField] private string baseURL;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public HTTPCommunicationManager(string baseURL)
    {
        this.baseURL = baseURL;
    }

    public IEnumerator PostUserSignup(string userName, string password, Action<string, string> userUuidAndTokenSetter, Action callback)
    {
        string deviceSecret = "hogehoge";
        PostUserSignupRequestBody body = new(userName, password, deviceSecret);
        string bodyJson = JsonUtility.ToJson(body);
        byte[] postData = Encoding.UTF8.GetBytes(bodyJson);

        string url = baseURL + "/api/v1/user/signup";

        UnityWebRequest request = new(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            PostUserSignupResponseBody responseBody = JsonUtility.FromJson<PostUserSignupResponseBody>(responseJson);

            Debug.Log(responseBody.userUuid);
            // TODO: データを適切な場所に保存する。

            userUuidAndTokenSetter(responseBody.userUuid, responseBody.token);

            callback.Invoke();
        }
        else
        {
            Debug.LogError("HTTP POST error: " + request.error);
        }
    }

    public IEnumerator PostUserSignin(string userName, string password, Action<string, string> userUuidAndTokenSetter, Action callback)
    {
        string deviceSecret = "hogehoge";
        PostUserSigninRequestBody body = new(userName, password, deviceSecret);
        string bodyJson = JsonUtility.ToJson(body);
        byte[] postData = Encoding.UTF8.GetBytes(bodyJson);

        string url = baseURL + "/api/v1/user/signin";

        UnityWebRequest request = new(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            PostUserSigninResponseBody responseBody = JsonUtility.FromJson<PostUserSigninResponseBody>(responseJson);

            Debug.Log(responseBody.userUuid);

            userUuidAndTokenSetter(responseBody.userUuid, responseBody.token);

            callback.Invoke();
        }
        else
        {
            Debug.LogError("HTTP POST error: " + request.error);
        }
    }
}
