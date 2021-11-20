using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Testscript : MonoBehaviour
{
    public Text text;
    void Start()
    {
        var body = @"{" + "\n" +
@"    ""phoneNumber"":""+380933994540""" + "\n" +
@"}";
        StartCoroutine(CallLogin("http://18.117.102.247:5000/api/auth/phone/send", body));
    }

    public IEnumerator CallLogin(string url, string logindataJsonString) {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(logindataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null) {
            Debug.Log("Erro: " + request.error);
        } else {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            text.text = request.downloadHandler.text;
        }

    }
}
