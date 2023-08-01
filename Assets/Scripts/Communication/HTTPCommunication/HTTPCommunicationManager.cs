using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPCommunicationManager : MonoBehaviour
{
    private const string STR_CONTENT_TYPE = "Content-Type";
    private const string STR_APPLICATION_JSON = "application/json";
    private const string STR_AUTHORIZATION = "Authorization";

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

    public IEnumerator PostUserSignup(string userName, string password, Action<string, string> userUuidAndTokenSetter, Action onSuccessed, Action onFailed)
    {
        PostUserSignupRequestBody body = new(userName, password);
        string bodyJson = JsonUtility.ToJson(body);
        byte[] postData = Encoding.UTF8.GetBytes(bodyJson);

        string url = baseURL + "/api/v1/user/signup";

        UnityWebRequest request = new(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader(STR_CONTENT_TYPE, STR_APPLICATION_JSON);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            PostUserSignupResponseBody responseBody = JsonUtility.FromJson<PostUserSignupResponseBody>(responseJson);

            userUuidAndTokenSetter(responseBody.userUuid, responseBody.token);

            onSuccessed.Invoke();
        }
        else
        {
            onFailed.Invoke();
            Debug.LogError("HTTP POST error: " + request.error);
        }
    }

    public IEnumerator PostUserSignin(string userName, string password, Action<string, string> userUuidAndTokenSetter, Action onSuccessed, Action onFailed)
    {
        PostUserSigninRequestBody body = new(userName, password);
        string bodyJson = JsonUtility.ToJson(body);
        byte[] postData = Encoding.UTF8.GetBytes(bodyJson);

        string url = baseURL + "/api/v1/user/signin";

        UnityWebRequest request = new(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader(STR_CONTENT_TYPE, STR_APPLICATION_JSON);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            PostUserSigninResponseBody responseBody = JsonUtility.FromJson<PostUserSigninResponseBody>(responseJson);

            userUuidAndTokenSetter(responseBody.userUuid, responseBody.token);

            onSuccessed.Invoke();
        }
        else
        {
            onFailed.Invoke();
            Debug.LogError("HTTP POST error: " + request.error);
        }
    }

    public IEnumerator GetRehabilitationSave()
    {
        string url = baseURL + "/api/v1/rehabilitaion-save";

        UnityWebRequest request = new(url, "GET");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            
        }
    }

    public IEnumerator PostRehabilitationSave(string userUuid, RehabilitationSaveDataContent saveData, Action onSuccessed, Action onFailed)
    {
        PostRehabilitationSaveRequestBody body = new(userUuid, saveData);
        string bodyJson = JsonUtility.ToJson(body);
        byte[] postData = Encoding.UTF8.GetBytes(bodyJson);

        string url = baseURL + "/api/v1/rehabilitation-save";

        UnityWebRequest request = new(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader(STR_CONTENT_TYPE, STR_APPLICATION_JSON);
        request.SetRequestHeader(STR_AUTHORIZATION, $"Bearer {SingletonDatabase.Instance.myToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            PostRehabilitationSaveResponseBody responseBody = JsonUtility.FromJson<PostRehabilitationSaveResponseBody>(responseJson);

            onSuccessed.Invoke();
        }
        else
        {
            onFailed.Invoke();
        }
    }

    public IEnumerator PostRehabilitationResult(string userUuid, RehabilitationResultContent result, Action onSuccessed, Action onFailed)
    {
        Debug.Log("issued");
        PostRehabilitationResultRequestBody body = new(userUuid, result);
        string bodyJson = JsonUtility.ToJson(body);
        byte[] postData = Encoding.UTF8.GetBytes(bodyJson);

        string url = baseURL + "/api/v1/rehabilitation-result";

        UnityWebRequest request = new(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader(STR_CONTENT_TYPE, STR_APPLICATION_JSON);
        request.SetRequestHeader(STR_AUTHORIZATION, $"Bearer {SingletonDatabase.Instance.myToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("success");
            string responseJson = request.downloadHandler.text;
            PostRehabilitationResultResponseBody responseBody = JsonUtility.FromJson<PostRehabilitationResultResponseBody>(responseJson);

            onSuccessed.Invoke();
        }
        else
        {
            Debug.Log("failed");
            Debug.Log(request.downloadHandler.error);
            onFailed.Invoke();
        }
    }
}
