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
    namespace FoundRoutes
    {
        [Serializable]
        public struct FoundRoutes
        {
            public bool success;
            public Route[] routes;
        }
        [Serializable]
        public struct Route
        {
            public string _id;
            public Way from;
            public Way to;
            public Train train;
            [SerializeField]
            public string departureTime;
            [SerializeField]
            public string arrivalTime;
            public TimeRoute route;
            public string time;
        }
        [Serializable]
        public struct Way
        {
            public StationsSearcher.Location location;
            public string _id;
            public string name;
        }
        [Serializable]
        public struct TimeRoute
        {
            public StationTime from;
            public StationTime to;
        }
        [Serializable]
        public struct StationTime
        {
            public string departureTime;
            public string arrivalTime;
        }
        [Serializable]
        public struct Train
        {
            public string _id;
            public string name;
            public string type;
            public int[] seats;
            public int[] freeSeats;
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
        public struct TrainDetailInfo
        {
            public bool success;
            public Train train;
        }
        [Serializable]
        public struct Train
        {
            public string _id;
            public string name;
            public string type;
            public Van[] vans;
            public int[] seats;
            public int[] freeSeats;
        }
        [Serializable]
        public struct Van
        {
            public Seat[] seats;
            public string _id;
            public string vanClass;
            public int number;
        }
        [Serializable]
        public struct Seat
        {
            public bool occupied;
            public string _id;
            public int number;
        }
    }
    namespace PopularWays
    {
        [Serializable]
        public struct PopularWays
        {
            public bool success;
            public Station[] stations;
        }
        [Serializable]
        public struct Station
        {
            public string _id;
            public Way from;
            public Way to;
        }
        [Serializable]
        public struct Way
        {
            public string _id;
            public string name;
        }
    }
}
public delegate void Response(string json, long responseCode);

