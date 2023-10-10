using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPCommunicationManager : MonoBehaviour
{
    private const string STR_CONTENT_TYPE = "Content-Type";
    private const string STR_APPLICATION_JSON = "application/json";
    private const string STR_AUTHORIZATION = "Authorization";

    private string baseURL;

    void Awake()
    {
        this.baseURL = "http://" + Config.serverIP + ":" + Config.serverHttpPort;
    }

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

        UnityWebRequest request = GenerateStandardPostRequest("/api/v1/user/signup", body);

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

        UnityWebRequest request = GenerateStandardPostRequest("/api/v1/user/signin", body);

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

    public IEnumerator PostUserSignupWithTemporaryAccount(string userName, Action<string, string> userUuidAndTokenSetter, Action onSuccessed, Action onFailed)
    {
        PostUserSignupWithTemporaryAccountRequestBody body = new(userName);

        UnityWebRequest request = GenerateStandardPostRequest("/api/v1/user/signup-with-temporary-account", body);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            PostUserSignupWithTemporaryAccountResponseBody responseBody = JsonUtility.FromJson<PostUserSignupWithTemporaryAccountResponseBody>(responseJson);

            userUuidAndTokenSetter(responseBody.userUuid, responseBody.token);

            onSuccessed.Invoke();
        }
        else
        {
            onFailed.Invoke();
            Debug.LogError("HTTP POST error: " + request.error);
        }
    }

    public IEnumerator GetRehabilitationSave(string userUuid, Action<RehabilitationSaveDataContent> loadedSaveDataSetter, Action onSuccessed, Action onFailed)
    {
        string url = $"{baseURL}/api/v1/rehabilitation-save?userUuid={userUuid}";

        UnityWebRequest request = new(url, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader(STR_CONTENT_TYPE, STR_APPLICATION_JSON);
        request.SetRequestHeader(STR_AUTHORIZATION, $"Bearer {SingletonDatabase.Instance.myToken}");
        
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            GetRehabilitationSaveResponseBody responseBody = JsonUtility.FromJson<GetRehabilitationSaveResponseBody>(responseJson);

            loadedSaveDataSetter(responseBody.saveData);

            onSuccessed.Invoke();
        }
        else
        {
            onFailed.Invoke();
        }
    }

    public IEnumerator PostRehabilitationSave(string userUuid, RehabilitationSaveDataContent saveData, Action onSuccessed, Action onFailed)
    {
        PostRehabilitationSaveRequestBody body = new(userUuid, saveData);

        UnityWebRequest request = GenerateStandardPostRequest("/api/v1/rehabilitation-save", body);
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
        PostRehabilitationResultRequestBody body = new(userUuid, result);

        UnityWebRequest request = GenerateStandardPostRequest("/api/v1/rehabilitation-result", body);
        request.SetRequestHeader(STR_AUTHORIZATION, $"Bearer {SingletonDatabase.Instance.myToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            PostRehabilitationResultResponseBody responseBody = JsonUtility.FromJson<PostRehabilitationResultResponseBody>(responseJson);

            onSuccessed.Invoke();
        }
        else
        {
            Debug.Log(request.downloadHandler.error);
            onFailed.Invoke();
        }
    }

    private UnityWebRequest GenerateStandardPostRequest(string endPoint, object serializableTypeBody)
    {
        string url = baseURL + endPoint;

        string bodyJson = JsonUtility.ToJson(serializableTypeBody);
        byte[] postData = Encoding.UTF8.GetBytes(bodyJson);

        UnityWebRequest request = new(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader(STR_CONTENT_TYPE, STR_APPLICATION_JSON);

        return request;
    }
}
