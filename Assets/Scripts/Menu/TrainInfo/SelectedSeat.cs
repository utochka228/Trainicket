using TMPro;
using UnityEngine;

public class SelectedSeat : Seat
{
    [SerializeField] TextMeshProUGUI vanNumberTxt;
    public override int VanNumber { 
        get => base.VanNumber; 
        set { 
            vanNumber = value;
            vanNumberTxt.text = value.ToString();
        } 
    }
    public override void OnPressed() {
        TrainInfoMenu.i.RemoveSelectedSeat(this);
    }
}
