using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketsListMenu : MenuItem<TicketsListMenu>
{
    public GameObject detailInfoPrefab;
    public Transform wayHolder;
    // Start is called before the first frame update
    void OnEnable()
    {
        detailInfoPrefab = (GameObject)Resources.Load("UIElements/TicketsList/DetailInfo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
