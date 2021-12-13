using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using TrainicketJSONStorage.CodeResponse;
using TrainicketJSONStorage.RegisterBody;
using UnityEngine;
using static RestAPI;

public class RegisterMenu : MenuItem<RegisterMenu>
{
    public TMP_InputField firstName;
    public TMP_InputField secondName;
    public TMP_InputField email;

    public TextMeshProUGUI childDateTxt;

    [SerializeField] GameObject childDateObj;
    [SerializeField] GameObject studentNumberObj;

    public TMP_Dropdown privilege;
    public TMP_InputField studentNumber;
    public void SendRegisteredValues() {
        var body = "";
        Privilege priv = new Privilege("", "");
        if (privilege.value == 1) {
            priv = new Privilege("child", ChildDate.Date.ToString("yy.MM.dd"));
        }
        if (privilege.value == 2) {
            priv = new Privilege("student", "BK" + UnityEngine.Random.Range(10000000, 100000000));
        }
        RegisteredData registeredData = new RegisteredData($"trainicket user({UnityEngine.Random.Range(0, 99999f)})", email.text, firstName.text, secondName.text, priv);
        body = JsonUtility.ToJson(registeredData);

        if(privilege.value == 0) {
            var mask = new Regex(@",""privilege"".*?(?=})}");
            body = mask.Replace(body, "");
        }
        string headerValue = "Bearer " + UIMenuManager.Instance.accesstoken;
        Debug.Log("VALUE:" + headerValue);
        HeaderRequest[] headers = new HeaderRequest[1] { new HeaderRequest("Authorization", "Bearer " + UIMenuManager.Instance.accesstoken) };
        StartCoroutine(POST("http://18.117.102.247:5000/api/user/register", body, GetResponse, headers));
    }
    void GetResponse(string json, long responseCode) {
        if(responseCode == 200) {
            var response = JsonUtility.FromJson<CodeCheckResponse>(json);
            if (response.success) {
                SaveToken(response.accessToken);
                SearchMenu.Show();
            }
        }
    }
    DateTime childDate;
    public DateTime ChildDate {
        get {
            return childDate;
        }
        set {
            childDate = value;
            childDateTxt.text = childDate.Date.ToString("dd.MM.yyyy");
        }
    }
    public void ShowCalendar() => CalendarMenu.ShowCalendar((selectedDate)=> ChildDate = selectedDate, true);

    void SaveToken(string token) {
        PlayerPrefs.SetString("userToken", token);
    }
    public void OnPrivilegeChanged(int i) {
        switch (i) {
            case 0:
                studentNumberObj.SetActive(false);
                childDateObj.SetActive(false);
                break;
            case 1:
                studentNumberObj.SetActive(false);
                childDateObj.SetActive(true);
                break;
            case 2:
                studentNumberObj.SetActive(true);
                childDateObj.SetActive(false);
                break;
            default:
                break;
        }
    }
}
