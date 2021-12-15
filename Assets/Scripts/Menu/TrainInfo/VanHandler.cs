using System.Collections;
using System.Collections.Generic;
using TrainicketJSONStorage.TrainDetailInfo;
using UnityEngine;

public class VanHandler : MonoBehaviour
{
    [SerializeField] List<Seat> vanSeats;
    Van currentVan;
    public void UpdateVan(Van van) {
        currentVan = van;
        SetSeatInfo();
    }
    void SetSeatInfo() {
        for (int i = 0; i < vanSeats.Count; i++) {
            var seat = vanSeats[i];
            seat.number.text = i.ToString();
            if(i % 2 == 0) {
                seat.up.SetActive(true);
            } else {
                seat.down.SetActive(true);
            }
            if (currentVan.seats[i].occupied)
                seat.button.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
