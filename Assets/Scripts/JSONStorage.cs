using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONStorage
{
    public struct CodeResponse
    {
        public bool success;
        public string message;
        public int SMSCode;
    }
    public struct CodeCheckResponse
    {
        public bool success;
        public bool newUser;
        public string accessToken;
    }
    public struct BadCodeCheckResponse
    {
        public bool success;
        public string message;
    }
    public delegate void Response(string json, long responseCode);
}
