using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TrainicketJSONStorage.TrainDetailInfo;
using TrainicketJSONStorage.FoundRoutes;

public class VanTypeButton : MonoBehaviour
{
    public TextMeshProUGUI typeName;
    public TextMeshProUGUI seatsLable;
    VanTypeData vanTypeData;
    Route trainRoute;
    public void SetInfo(VanTypeData trainTypeData, Route route) {
        vanTypeData = trainTypeData;
        trainRoute = route;
        typeName.text = trainTypeData.typeName;
        seatsLable.text = trainTypeData.seatsInfo;
    }
    [SerializeField]
    public struct VanTypeData
    {
        public string trainId;
        public string typeName;
        public string seatsInfo;
        public VanTypeData(string typeName, string seatsInfo, string trainId) {
            this.typeName = typeName;
            this.seatsInfo = seatsInfo;
            this.trainId = trainId;
        }
    }
    public void OnPressed() {
        string url = "http://18.117.102.247:5000/api/train?id=" + vanTypeData.trainId + "&vanClass=" + vanTypeData.typeName;
        StartCoroutine(RestAPI.GET(url, (json, responseCode) => ShowTrainDetailInfo(json, responseCode), null, true));
    }
    void ShowTrainDetailInfo(string json, long responseCode) {
        switch (responseCode) {
            case 200:
                var trainDetailInfo = JsonUtility.FromJson<TrainDetailInfo>(json);
                TrainInfoMenu.ShowTrainDetailInfo(trainDetailInfo, trainRoute);
                break;
            default:
                break;
        }
    }
}
