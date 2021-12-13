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
    namespace RegisterBody
    {
        [Serializable]
        public struct RegisteredData
        {
            public string userName;
            public string email;
            public string firstName;
            public string lastName;
            public Privilege privilege;

            public RegisteredData(string name, string email, string first, string second, Privilege privilege) {
                userName = name;
                this.email = email;
                firstName = first;
                lastName = second;
                this.privilege = privilege;
            }
        }
        [Serializable]
        public struct Privilege
        {
            public string type;
            public string data;

            public Privilege(string type, string data) {
                this.type = type;
                this.data = data;
            }
        }
    }
    namespace TrainDetailInfo
    {
        [Serializable]
        public struct TrainInfo
        {
            public bool success;
        }
        [Serializable]
        public struct Train
        {
            public string _id;
            public string name;
            public string type;
            public Van[] vans;
            public VanClassSeats seats;
            public Seat[] freeSeats;
        }
        [Serializable]
        public struct Van
        {
            public Seat[] seats;
            public string _id;
            public string VanClass;
        }
        [Serializable]
        public struct Seat
        {
            public bool occupied;
            public string _id;
            public int number;
        }
        [Serializable]
        public struct VanClassSeats
        {
            public int zero;
            public int first;
            public int second;
            public int third;
        }
    }
}
public delegate void Response(string json, long responseCode);

