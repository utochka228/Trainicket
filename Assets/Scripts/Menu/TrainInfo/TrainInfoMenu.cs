using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TrainInfoMenu : MenuItem<TrainInfoMenu>
{
    [SerializeField] TextMeshProUGUI wayName;
    [SerializeField] TextMeshProUGUI vanType;

    [SerializeField] TextMeshProUGUI dateFrom;
    [SerializeField] TextMeshProUGUI dateWhere;

    [SerializeField] TextMeshProUGUI from;
    [SerializeField] TextMeshProUGUI where;

    [SerializeField] TextMeshProUGUI price;

    [SerializeField] GameObject vanPrefab;
    [SerializeField] Transform vanHolder;

    [SerializeField] Tooltip toolTip;
    [SerializeField] Transform selectedSeatsHolder;
    [SerializeField] GameObject selectedSeat;

    [SerializeField] Transform mainVanHolder;

    [SerializeField] GameObject platscartPrefab;
    [SerializeField] GameObject kupePrefab;

    [SerializeField] Button continueBtn;
}
