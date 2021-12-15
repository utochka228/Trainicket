using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Seat : MonoBehaviour
{
    int seatNumber;
    public int SeatNumber {
        get => seatNumber;
        set {
            seatNumber = value;
            number.text = seatNumber.ToString();
        }
    }
    [SerializeField] TextMeshProUGUI number;
    protected int vanNumber;
    public virtual int VanNumber {
        get => vanNumber;
        set {
            vanNumber = value;
        }
    }

    public GameObject up;
    public GameObject down;
    public GameObject selector;
    public Button button;
    protected bool selected = false;
    public virtual void OnPressed() {
        selected = !selected;
        TrainInfoMenu.i.SeatPressed(this, selected, SeatNumber, up.activeSelf);
        if (selected)
            selector.SetActive(true);
        else
            selector.SetActive(false);
    }
    public void ResetSeat() {
        selected = false;
        selector.SetActive(false);
    }
    public void SelectSeat() {
        selected = true;
        selector.SetActive(true);
    }
}
