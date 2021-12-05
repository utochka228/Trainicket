using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TrainicketJSONStorage
{
    namespace CodeResponse
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
    }
    namespace StationsSearcher
    {
        [Serializable]
        public struct WaySearched
        {
            public bool success;
            public Station[] stations;
        }
        [Serializable]
        public struct Station
        {
            public Location location;
            public string _id;
            public string city;
            public string name;
            public string country;
        }
        [Serializable]
        public struct Location
        {
            public double longitude;
            public double latitude;
        }
    }
}
public delegate void Response(string json, long responseCode);

