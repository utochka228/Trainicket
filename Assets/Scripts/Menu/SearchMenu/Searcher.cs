using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TrainicketJSONStorage.StationsSearcher;
using UnityEngine;
using static RestAPI;
public class Searcher : MonoBehaviour
{
    [SerializeField] TMP_InputField fromField;
    [SerializeField] Transform fromHolder;
    [SerializeField] TMP_InputField whereField;
    [SerializeField] Transform whereHolder;
    [SerializeField] SearchedDropdown dropdown;

    [SerializeField] List<GameObject> toHide = new List<GameObject>();

    bool canAddChildren;
    public void SetAddingChildrenState(bool state) => canAddChildren = state;

    public void OnSymbolsInputFrom(string from) {
        if (canAddChildren == false)
            return;
        DestroyDropdowns();
        if (from.Length >= 3) {
            //Hide elements
            HideElements(new GameObject[] { fromField.gameObject});
            var url = "http://18.117.102.247:5000/api/stations/search?search=" + from;
            StartCoroutine(GET(url, (json, responseCode) => {
                var way = JsonUtility.FromJson<WaySearched>(json);

                if(way.stations != null)
                    if(way.stations.Length > 0)
                        CreateSearchedDropdown(way, fromField, fromHolder, true);

            }, null));
        }
    }
    public void OnSymbolsInputWhere(string where) {
        if (canAddChildren == false)
            return;
        DestroyDropdowns();
        if (where.Length >= 3) {
            //Hide elements
            HideElements(new GameObject[] { whereField.gameObject });
            var url = "http://18.117.102.247:5000/api/stations/search?search=" + where;
            StartCoroutine(GET(url, (json, responseCode) => {
                var way = JsonUtility.FromJson<WaySearched>(json);

                if (way.stations != null)
                    if (way.stations.Length > 0)
                        CreateSearchedDropdown(way, whereField, whereHolder, false);

            }, null));
        }
    }
    public void CreateSearchedDropdown(WaySearched way, TMP_InputField inputField, Transform holder, bool fromField) {
        var count = way.stations.Length;
        for (int i = 0; i < count; i++) {
            var _dropDown = Instantiate(dropdown, holder).GetComponent<SearchedDropdown>();
            _dropDown.Initialize(way.stations[i]._id, way.stations[i].city, fromField, inputField);
        }
    }
    public void HideElements(GameObject[] notHide) {
        foreach (var element in toHide) {
            if (notHide.Contains(element))
                continue;

            element.SetActive(false);
        }
    }
    public void ShowHiddenElements() {
        foreach (var element in toHide) {
            element.SetActive(true);
        }
    }
    public void DestroyDropdowns() {
        StartCoroutine(CheckIfDropdownWasPressed());
    }
    IEnumerator CheckIfDropdownWasPressed() {
        yield return new WaitForSeconds(0.1f);
        if (SearchedDropdown.wasPressed == false) {
            for (int i = 0; i < fromHolder.childCount; i++) {
                var obj = fromHolder.GetChild(i);
                Destroy(obj.gameObject);
            }
            for (int i = 0; i < whereHolder.childCount; i++) {
                var obj = whereHolder.GetChild(i);
                Destroy(obj.gameObject);
            }
        }
    }
}
