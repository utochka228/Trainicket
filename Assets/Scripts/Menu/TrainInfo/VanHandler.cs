using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            seat.ResetSeat();

            seat.SeatNumber = i;
            seat.VanNumber = currentVan.number;
            seat.van = currentVan;

            if(i % 2 == 0) {
                seat.up.SetActive(true);
            } else {
                seat.down.SetActive(true);
            }
            if (currentVan.seats[i].occupied) {
                seat.button.interactable = false;
                seat.selector.SetActive(false);
            }
            else
                seat.button.interactable = true;
            
            //Enable selector if this seat was selected prev
            if(TrainInfoMenu.i.selectedSeats.Any(x => x.seat == seat && 
            x.selectedSeat.VanNumber == currentVan.number))
                seat.SelectSeat();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
