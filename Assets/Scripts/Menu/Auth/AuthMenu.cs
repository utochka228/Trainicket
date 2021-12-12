using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RestAPI;
using TrainicketJSONStorage.CodeResponse;

public class AuthMenu : MenuItem<AuthMenu>
{
    [SerializeField] GameObject sendCodeBtn;
    [SerializeField] Button continueBtn;
    [SerializeField] GameObject phoneBlock;
    [SerializeField] GameObject pincodeBlock;
    [SerializeField] TMP_InputField phoneField;
    [SerializeField] TMP_InputField[] pinFields;

    string inputPhoneNumber;
    // Start is called before the first frame update
    void Start()
    {
        sendCodeBtn.SetActive(true);
    }
    public void SkipAuthStep() {
        SearchMenu.Show();
    }
    
    public void SendPhone() {
        inputPhoneNumber = phoneField.text;
        string phoneNumber = "\"phoneNumber\":\"" + phoneField.text + "\"";
        var body = @"{" + "\n" + phoneNumber + "\n" +
@"}";
        StartCoroutine(POST("http://18.117.102.247:5000/api/auth/phone/send", body, ShowPinCodeField, null));
    }
    string responsedCode;
    
    #region PinCode
    void ShowPinCodeField(string json, long responseCode) {
        phoneBlock.SetActive(false);
        sendCodeBtn.SetActive(false);
        continueBtn.gameObject.SetActive(true);
        continueBtn.interactable = false;
        pincodeBlock.SetActive(true);
    }
    int pinCountEntered;
    string maskedCode = "xxxx";
    public void OnPinEntered(int index) {
        var pinField = pinFields[index];
        if (string.IsNullOrEmpty(pinField.text)) {
            if (maskedCode[index] != 'x')
                pinCountEntered--;
            maskedCode = Utils.ReplaceSymbol(maskedCode, index, 'x');
            if (pinCountEntered < 4)
                continueBtn.interactable = false;
            return;
        }
        if (maskedCode[index] != 'x')
            return;
        maskedCode = Utils.ReplaceSymbol(maskedCode, index, pinField.text[0]);
        pinCountEntered++;
        if(pinCountEntered == 4)
            continueBtn.interactable = true;
    }
    public void SendEnteredCode() {
        string phoneNumber = "\"phoneNumber\":\"" + phoneField.text + "\",";
        var body = @"{" + "\n" + phoneNumber + "\n" + "\"code\":" + int.Parse(maskedCode) +
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
                UIMenuManager.Instance.accesstoken = response.accessToken;
                RegisterMenu.Show();
            }
            else
                SearchMenu.Show();
        }
    }
}
