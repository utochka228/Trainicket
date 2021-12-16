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
    string accessToken;
    public static void ShowRegisterMenu(string accessToken) {
        Show();
        i.accessToken = accessToken;
    }
    public void SendRegisteredValues() {
        var body = "";

        var studentData = "BK" + UnityEngine.Random.Range(10000000, 100000000);
        var childData = ChildDate.Date.ToString("yy.MM.dd");
        Privilege priv = GetPrivilegeByIndex(privilege.value, childData, studentData);
        
        RegisteredData registeredData = new RegisteredData($"trainicket user({UnityEngine.Random.Range(0, 99999f)})", email.text, firstName.text, secondName.text, priv);
        AccountMenu.SetRegisteredData(registeredData);

        body = JsonUtility.ToJson(registeredData);

        if(privilege.value == 0) {
            var mask = new Regex(@",""privilege"".*?(?=})}");
            body = mask.Replace(body, "");
        }
        string headerValue = "Bearer " + accessToken;
        Debug.Log("VALUE:" + headerValue);
        HeaderRequest[] headers = new HeaderRequest[1] { new HeaderRequest("Authorization", "Bearer " + accessToken) };
        StartCoroutine(POST("http://18.117.102.247:5000/api/user/register", body, GetResponse, headers));
    }
    void GetResponse(string json, long responseCode) {
        var response = JsonUtility.FromJson<CodeCheckResponse>(json);
        SaveToken(response.accessToken);
        SearchMenu.Show();
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
        AccountMenu.accessToken = token;
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
    public static Privilege GetPrivilegeByIndex(int index, string dateChild, string dateStudent) {
        Privilege priv = new Privilege("", "");
        if (index == 1) {
            priv.type = "child";
            priv.data = dateChild;
        }
        if (index == 2) {
            priv.type = "student";
            priv.data = dateStudent;
        }
        return priv;
    }
}
