using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    public delegate void OnLocalization();
    public static event OnLocalization OnLocalizationChanged;

    public static LocalizationManager instance;

    [SerializeField] public Dictionary<string, string> localizaedText;

    public string curr_language = "localizedText_en";

    private string missingTextValue = "localized value missing";

    private bool isReady = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        GetSavedLaguage();
        LoadLocalization();
    }

    public void setLocalization(string language)
    {
        curr_language = language;
        LoadLocalization();
    }

    public void LoadLocalization()
    {
        localizaedText = new Dictionary<string, string>();
        //string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string filePath = Application.streamingAssetsPath + "/" + curr_language + ".json";
        Debug.Log("filePath:" + filePath);
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("android path");
            //filePath = "jar:file://" + Application.streamingAssetsPath + "!/assets/" + curr_language + ".json";
            filePath = Application.streamingAssetsPath + "/" +curr_language+ ".json";
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            www.SendWebRequest();
            while (!www.isDone)
            {
            }
            string jsonString = www.downloadHandler.text;
            LocalizationData localizationData = JsonUtility.FromJson<LocalizationData>(jsonString);
            curr_language = localizationData.items[0].value;
            for (int i = 0; i < localizationData.items.Length; i++)
            {
                if (!localizaedText.ContainsKey(localizationData.items[i].key))
                {
                    localizaedText.Add(localizationData.items[i].key, localizationData.items[i].value);
                }
            }
            
        }

        if (Application.platform != RuntimePlatform.Android && File.Exists(filePath))
        {
            Debug.Log("found file");
            string jsonData = File.ReadAllText(filePath);
            LocalizationData localizationData = JsonUtility.FromJson<LocalizationData>(jsonData);
            curr_language = localizationData.items[0].value;
            for (int i = 0; i < localizationData.items.Length; i++)
            {
                if (!localizaedText.ContainsKey(localizationData.items[i].key))
                {
                    localizaedText.Add(localizationData.items[i].key, localizationData.items[i].value);
                }
            }
        }
        else
            Debug.Log("no localization file");
        isReady = true;
        OnLocalizationChanged?.Invoke();
        SaveCurrentLanguage();
    }

    public string GetLocalizedValue(string key)
    {
        if (localizaedText.ContainsKey(key))
        {
            return localizaedText[key];
        }
        else return missingTextValue;
    }

    public bool GetIsReady()
    {
        return isReady;
    }

    public void GetSavedLaguage()
    {
        int languageIndex = PlayerPrefs.GetInt("language");
        switch (languageIndex) {
            case 0:
                curr_language = "localizedText_en";
                break;
            case 1:
                curr_language = "localizedText_it";
                break;
            case 2:
                curr_language = "localizedText_ua";
                break;
            case 3:
                curr_language = "localizedText_ru";
                break;
        }
    }

    public void SaveCurrentLanguage()
    {
        switch (curr_language) {
            case "English":
                PlayerPrefs.SetInt("language", 0);
                break;
            case "Italian":
                PlayerPrefs.SetInt("language", 1);
                break;
            case "Ukrainian":
                PlayerPrefs.SetInt("language", 2);
                break;
            case "Russian":
                PlayerPrefs.SetInt("language", 3);
                break;
        }
    }
}
