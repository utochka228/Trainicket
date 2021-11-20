using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static JSONStorage;

public class AuthMenu : MenuItem<AuthMenu>
{
    [SerializeField] GameObject sendCodeBtn;
    [SerializeField] GameObject continueBtn;
    [SerializeField] TMP_Dropdown language;
    [SerializeField] TMP_InputField phoneField;
    // Start is called before the first frame update
    void Start()
    {
        sendCodeBtn.SetActive(true);
    }
    public void SkipAuthStep() {
        SearchMenu.Show();
    }
    public void ChangeLocalization() {
        switch (language.value) {
            case 0:
                Debug.Log("Localized to English.");
                UIMenuManager.Instance.ChangeLocalization("localizedText_en");
                break;
            case 1:
                Debug.Log("Localized to Ukrainian.");
                UIMenuManager.Instance.ChangeLocalization("localizedText_ua");
                break;
            case 2:
                Debug.Log("Localized to Russian.");
                UIMenuManager.Instance.ChangeLocalization("localizedText_ru");
                break;
            default:
                break;
        }
    }
    public void SendPhone() {
        string phoneNumber = "\"phoneNumber\":\"" + phoneField.text + "\"";
        var body = @"{" + "\n" + phoneNumber + "\n" +
@"}";
        StartCoroutine(CallLogin("http://18.117.102.247:5000/api/auth/phone/send", body));
    }
    string responsedCode;
    public IEnumerator CallLogin(string url, string logindataJsonString) {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(logindataJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null) {
            Debug.Log("Erro: " + request.error);
        } else {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.downloadHandler.text);
            var response = JsonUtility.FromJson<CodeResponse>(request.downloadHandler.text);
        }

    }
}
