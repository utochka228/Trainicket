using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TrainicketJSONStorage.CodeResponse;
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
        if (privilege.value == 1) {
            body = @"{" + "\n" +
@$"    ""userName"":""trainicket user({UnityEngine.Random.Range(0, 99999f)})""," + "\n" +
@$"    ""email"":""{email.text}""," + "\n" +
@"" + "\n" +
@"    " + "\n" +
@$"    ""firstName"":""{firstName.text}""," + "\n" +
@$"    ""lastName"":""{secondName.text}""," + "\n" +
@"    ""privilege"":{" + "\n" +
@"        ""type"":""child""," + "\n" +
@$"        ""data"":""{ChildDate.Date.ToString().Replace('/', '.')}""" + "\n" +
@"    }" + "\n" +
@"" + "\n" +
@"}";
        }
        if (privilege.value == 2) {
            body =  @"{" + "\n" +
@$"    ""userName"":""trainicket user({UnityEngine.Random.Range(0, 99999f)})""," + "\n" +
@$"    ""email"":""{email.text}""," + "\n" +
@"" + "\n" +
@"    " + "\n" +
@$"    ""firstName"":""{firstName.text}""," + "\n" +
@$"    ""lastName"":""{secondName.text}""," + "\n" +
@"    ""privilege"":{" + "\n" +
@"        ""type"":""student""," + "\n" +
@$"        ""data"":""BK{UnityEngine.Random.Range(10000000, 100000000)}""" + "\n" +
@"    }" + "\n" +
@"" + "\n" +
@"}";
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
    public void ShowCalendar() => CalendarMenu.ShowCalendar((selectedDate)=> ChildDate = selectedDate);

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
