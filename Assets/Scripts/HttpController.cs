using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpController : MonoBehaviour
{
    public static HttpController _instance;

    public static HttpController Instance { get { return _instance; } }
    
    private static string BaseURL = "https://api.particle.io/v1/"; 
    private static readonly string DeviceURL = BaseURL + "devices/" + APIData.DeviceID + "/";

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    IEnumerator GetRequest(CloudVariableType VariableType, object inputObj)
    {
        string uri = DeviceURL + ParticleHelpers.GetVariableStringByEnum(VariableType);
        Debug.Log("uri: " + uri);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + APIData.Key);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    UpdateObjectByResult(VariableType, inputObj, webRequest.downloadHandler.text);
                    break;
                default:
                    Debug.Log("WaitWhat");
                    break;
            }
        }
    }

    private IEnumerator PostRequest(CloudVariableType VariableType, object inputObj, string arg)
    {
        string uri = DeviceURL + ParticleHelpers.GetFunctionNameByEnum(VariableType);
        Debug.Log("uri: " + uri);
        Debug.Log("Arg: " + arg);

        WWWForm form = new WWWForm();
        form.AddField("arg", arg);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + APIData.Key);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
                Debug.Log(webRequest.error);
            else
            {
                HandleUpdate(VariableType, inputObj);
                Debug.Log("Form upload complete!");
            }       
        }
    }

    private void UpdateObjectByResult(CloudVariableType VariableType, object inputObj, string result)
    {
        JObject jObj = JObject.Parse(result);

        switch (VariableType)
        {
            case CloudVariableType.Channel1:
            case CloudVariableType.Channel2:
                if(ColorUtility.TryParseHtmlString("#" + jObj["result"].ToString(), out Color _newClr))
                    ParticleHelpers.SetChannelColor(VariableType, inputObj, _newClr);
                break;
            case CloudVariableType.Animation:
                if (int.TryParse(jObj["result"].ToString(), out int _newIndex))
                    ParticleHelpers.SetButtonIndex(inputObj, _newIndex);
                break;
            case CloudVariableType.Intensity:
            case CloudVariableType.Speed:
                if (float.TryParse(jObj["result"].ToString(), out float _newVal))
                    ParticleHelpers.SetSliderObject(inputObj, _newVal);
                break;
            case CloudVariableType.Power:
                ParticleHelpers.SetPowerColor(inputObj, jObj["result"].ToString().ToLower() == "true");
                break;
        }
    }

    public void HandleUpdate(CloudVariableType VariableType, object inputObj)
    {
        Debug.Log("Send Request");
        Debug.Log(inputObj);
        StartCoroutine(GetRequest(VariableType, inputObj));
    }

    public void HandlePost(CloudVariableType VariableType, object inputObj, string arg)
    {
        Debug.Log("Posting Request");
        StartCoroutine(PostRequest(VariableType, inputObj, arg));
    }
}
