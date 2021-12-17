using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RestAPI;
using TrainicketJSONStorage.CodeResponse;
using System.Text.RegularExpressions;

public class AuthMenu : MenuItem<AuthMenu>
{
    [SerializeField] Button sendCodeBtn;
    [SerializeField] GameObject phoneBlock;
    [SerializeField] GameObject pincodeBlock;
    [SerializeField] TMP_InputField phoneField;
    [SerializeField] TMP_InputField[] pinFields;
    [SerializeField] GameObject authShowingMessage;

    string inputPhoneNumber;
    private void Start() {
        sendCodeBtn.interactable = false;
    }
    public static int LoadAuthShowing() {
        return PlayerPrefs.GetInt("showAuth");
    }
    public void SaveAuthShowing(int value) {
        PlayerPrefs.SetInt("showAuth", value);
        Close();
        SearchMenu.Show();
    }
    public void SkipAuthStep() {
        if (LoadAuthShowing() == 0 || LoadAuthShowing() == 2)
            authShowingMessage.SetActive(true);
    }
    
    public void CheckPhone() {
        Regex regex = new Regex(@"^\+?3?8?(0\d{9})$");
        if (regex.IsMatch(phoneField.text))
            sendCodeBtn.interactable = true;
        else
            sendCodeBtn.interactable = false;
    }
    public void SendPhone() {
        inputPhoneNumber = phoneField.text;
        if (inputPhoneNumber.StartsWith("+38") == false)
            inputPhoneNumber = inputPhoneNumber.Insert(0, "+38");
        string phoneNumber = "\"phoneNumber\":\"" + inputPhoneNumber + "\"";
        var body = @"{" + "\n" + phoneNumber + "\n" +
@"}";
        StartCoroutine(POST("http://18.117.102.247:5000/api/auth/phone/send", body, ShowPinCodeField, null));
    }
    string responsedCode;
    
    #region PinCode
    void ShowPinCodeField(string json, long responseCode) {
        var codeResponse = JsonUtility.FromJson<CodeResponse>(json);
        phoneBlock.SetActive(false);
        sendCodeBtn.gameObject.SetActive(false);
        pincodeBlock.SetActive(true);
        //Auto filling code
        var code = codeResponse.SMSCode.ToString();
        for (int i = 0; i < pinFields.Length; i++) {
            var pin = pinFields[i];
            pin.text = code[i].ToString();
        }
        SendEnteredCode();
        //SelectFirstPin();
    }
    public void SelectFirstPin() => pinFields[0].Select();
    
    public void SendEnteredCode() {
        string phoneNumber = "\"phoneNumber\":\"" + inputPhoneNumber + "\",";
        string pinCode = "";
        foreach (var pin in pinFields) {
            pinCode += pin.text;
        }
        var body = @"{" + "\n" + phoneNumber + "\n" + "\"code\":" + pinCode +
@"}";
        StartCoroutine(POST("http://18.117.102.247:5000/api/auth/phone/check", body, CheckResponsedCode, null));
    }
    #endregion
    void CheckResponsedCode(string json, long responseCode) {
        //Bad request
        if(responseCode == 400) {
            var response = JsonUtility.FromJson<BadCodeCheckResponse>(json);
        }
        //Ok
        if(responseCode == 200) {
            var response = JsonUtility.FromJson<CodeCheckResponse>(json);
            if (response.newUser) {
                RegisterMenu.ShowRegisterMenu(response.accessToken);
            } else {
                Close();
                SearchMenu.Show();
            }
        }
    }
    public override void OnBackPressed() {
        
    }
}
