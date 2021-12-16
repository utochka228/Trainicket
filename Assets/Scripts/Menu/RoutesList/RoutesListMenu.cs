using System.Collections;
using System.Collections.Generic;
using TrainicketJSONStorage.FoundRoutes;
using UnityEngine;

public class RoutesListMenu : MenuItem<RoutesListMenu>
{
    public RouteHolder routePrefab;
    public Transform wayHolder;
    // Start is called before the first frame update
    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void ShowFoundRoutes(FoundRoutes foundRoutes) {
        Show();
        foreach (var foundRoute in foundRoutes.routes) {
            var routeHolder = Instantiate(i.routePrefab, i.wayHolder).GetComponent<RouteHolder>();
            routeHolder.InitializeRoute(foundRoute);
        }
    }
}
