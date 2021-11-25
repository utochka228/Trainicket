using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterMenu : MenuItem<RegisterMenu>
{
    public void SendRegisteredValues() {
        var body = @"{" + "\n" +
@"    ""userName"":""test username""," + "\n" +
@"    ""email"":""test@gmail.com""," + "\n" +
@"" + "\n" +
@"    " + "\n" +
@"    ""firstName"":"" test first""," + "\n" +
@"    ""lastName"":""last test""," + "\n" +
@"    ""privilege"":{" + "\n" +
@"        ""type"":""child""," + "\n" +
@"        ""data"":""21.2.2222""" + "\n" +
@"    }" + "\n" +
@"" + "\n" +
@"}";
        string headerValue = "Bearer " + UIMenuManager.Instance.accesstoken;
        Debug.Log("VALUE:" + headerValue);
        HeaderRequest[] headers = new HeaderRequest[1] { new HeaderRequest("Authorization", "Bearer " + UIMenuManager.Instance.accesstoken) };
        StartCoroutine(RestAPI.PostRequest("http://18.117.102.247:5000/api/user/register", body, GetResponse, headers));
    }
    void GetResponse(string json, long responseCode) {
        
    }
}
