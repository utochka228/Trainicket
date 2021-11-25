using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static JSONStorage;

public class RestAPI : MonoBehaviour
{
    public static IEnumerator PostRequest(string url, string body, Response resultAction, HeaderRequest[] headers) {
        LoadingMenu.Show();
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        if (headers != null)
            SetAdditionalHeaders(headers, request);
        yield return request.SendWebRequest();

        LoadingMenu.Hide();
        if (request.error != null) {
            Debug.LogError("Error: " + request.error);
            Message.ShowNativePopUp("Error", request.error);
        } else {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log("Responsed json: " + request.downloadHandler.text);
            var response = request.downloadHandler.text;
            resultAction?.Invoke(response, request.responseCode);
        }
    }
    static void SetAdditionalHeaders(HeaderRequest[] headers, UnityWebRequest request) {
        foreach (var header in headers) {
            request.SetRequestHeader(header.headerName, header.headerValue);
        }
    }
}

public struct HeaderRequest
{
    public string headerName;
    public string headerValue;
    public HeaderRequest(string name, string value) {
        headerName = name;
        headerValue = value;
    }
}
