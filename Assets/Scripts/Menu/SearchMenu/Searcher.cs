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

    bool canAddChildren;
    public void SetAddingChildrenState(bool state) => canAddChildren = state;

    public void OnSymbolsInputFrom(string from) {
        if (string.IsNullOrEmpty(from)) {
            SearchMenu.i.targetSearch.from = "";
            SearchMenu.i.targetSearch.fromName = "";
        }
        if (canAddChildren == false)
            return;
        DestroyDropdowns();
        if (from.Length >= 2) {
            //Hide elements
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
        if (string.IsNullOrEmpty(where)) {
            SearchMenu.i.targetSearch.where = "";
            SearchMenu.i.targetSearch.whereName = "";
        }
        if (canAddChildren == false)
            return;
        DestroyDropdowns();
        if (where.Length >= 2) {
            //Hide elements
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
