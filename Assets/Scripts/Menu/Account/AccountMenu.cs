using System.Collections;
using System.Collections.Generic;
using TrainicketJSONStorage.RegisterBody;
using TrainicketJSONStorage.UserProfileData;
using UnityEngine;

public class AccountMenu : MenuItem<AccountMenu>
{
    public static string accessToken;
    public static RegisteredData RegisteredData { get; private set; }
    public static void SetRegisteredData(RegisteredData registeredData) {
        RegisteredData = registeredData;
    }
    public static void SetRegisteredData(UserResponseProfile registeredData) {
        RegisteredData data;
        data.userName = registeredData.profile.userName;
        data.email = registeredData.profile.email;
        data.firstName = registeredData.profile.firstName;
        data.lastName = registeredData.profile.lastName;
        data.privilege = registeredData.profile.privilege;
        RegisteredData = data;
    }
}
