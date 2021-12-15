using System.Collections;
using System.Collections.Generic;
using TrainicketJSONStorage.TrainDetailInfo;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VanButton : MonoBehaviour
{
    public Van myVan;
    public TextMeshProUGUI number;
    public TextMeshProUGUI freeSeats;
    [SerializeField] Outline outline;
    public void OnPressed() {
        foreach (Transform child in TrainInfoMenu.i.vanHolder.transform) {
            child.GetComponent<Outline>().enabled = false;
        }
        outline.enabled = true;
        TrainInfoMenu.i.UpdateVanInfo(myVan);
    }
}
