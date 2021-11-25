using waqashaxhmi.AndroidNativePlugin;

public class Message
{
    public static void ShowNativePopUp(string title, string message_txt) {
#if UNITY_ANDROID
        AndroidNativePluginLibrary.Instance.ShowMessage(title, message_txt);
#endif
    }
}
public class NoInternetMessage : Message
{
    public NoInternetMessage() {
        ShowNativePopUp("Error!", "The internet connection appears to be offline.");
    }
}
