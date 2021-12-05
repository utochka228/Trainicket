using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static RestAPI;

public class RegisterMenu : MenuItem<RegisterMenu>
{
    public TMP_InputField firstName;
    public TMP_InputField secondName;
    public TMP_InputField email;

    public TMP_Dropdown privilege;
    public TMP_InputField day;
    public TMP_InputField month;
    public TMP_InputField year;
    public TMP_InputField studentNumber;
    public void SendRegisteredValues() {
        string privilegeValue = "none";
        if (privilege.value == 1)
            privilegeValue = "child";
        if (privilege.value == 2)
            privilegeValue = "student";
        var body = @"{" + "\n" +
@$"    ""userName"":""trainicket user({Random.Range(0, 99999f)})""," + "\n" +
@$"    ""email"":""{email.text}""," + "\n" +
@"" + "\n" +
@"    " + "\n" +
@$"    ""firstName"":""{firstName.text}""," + "\n" +
@$"    ""lastName"":""{secondName.text}""," + "\n" +
@"    ""privilege"":{" + "\n" +
@$"        ""type"":""{privilegeValue}""," + "\n" +
@"        ""data"":""21.2.2222""" + "\n" +
@"    }" + "\n" +
@"" + "\n" +
@"}";
        string headerValue = "Bearer " + UIMenuManager.Instance.accesstoken;
        Debug.Log("VALUE:" + headerValue);
        HeaderRequest[] headers = new HeaderRequest[1] { new HeaderRequest("Authorization", "Bearer " + UIMenuManager.Instance.accesstoken) };
        StartCoroutine(POST("http://18.117.102.247:5000/api/user/register", body, GetResponse, headers));
    }
    void GetResponse(string json, long responseCode) {
        
    }
}
