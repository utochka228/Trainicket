using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopularWay : MonoBehaviour
{
    public TextMeshProUGUI fromWhereTxt;
    public TargetSearch targetSearch;

    public void OnPressed() {
        SearchMenu.i.targetSearch = targetSearch;
        SearchMenu.i.from.text = targetSearch.fromName;
        SearchMenu.i.where.text = targetSearch.whereName;
    }
}
