using System.Collections;
using System.Collections.Generic;
using TrainicketJSONStorage.FoundRoutes;
using TrainicketJSONStorage.GettingTickets;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserTicketsMenu : MenuItem<UserTicketsMenu>
{
    [SerializeField] Transform activeHolder;
    [SerializeField] Transform historyHolder;
    [SerializeField] RouteHolder routeHolder;
    [SerializeField] GameObject activeBtn;
    [SerializeField] GameObject historyBtn;

    UnityAction onActiveTicketPressed;
    UnityAction onHistoryTicketPressed;

    public void AddTicketToActive(UserTicket[] userTickets) {
        Route route = new Route();
        foreach (var t in userTickets) {
            route.from.name = t.from.station.name;
            route.to.name = t.to.station.name;
            route.arrivalTime = t.from.arrivalTime;
            route.departureTime = t.from.departureTime;
        }
        var ticket = Instantiate(routeHolder, activeHolder);
        ticket.InitializeRoute(route);
        ticket.expandBtn.onClick.RemoveAllListeners();
        ticket.expandBtn.onClick.AddListener(onActiveTicketPressed);
    }
    public void AddTicketToHistory(UserTicket[] userTickets) {
        Route route = new Route();
        foreach (var t in userTickets) {
            route.from.name = t.from.station.name;
            route.to.name = t.to.station.name;
            route.arrivalTime = t.from.arrivalTime;
            route.departureTime = t.from.departureTime;
        }
        var ticket = Instantiate(routeHolder, historyHolder);
        ticket.InitializeRoute(route);
        ticket.expandBtn.onClick.RemoveAllListeners();
        ticket.GetComponent<Button>().interactable = false;
        ticket.expandBtn.onClick.AddListener(onHistoryTicketPressed);
    }
    void ShowDetailActiveInfo() {
        
    }
    void ShowDetailHistoryInfo() {
        
    }
    
    public static void ShowTicketsMenu(UserTicketsResponse userTicketsResponse) {
        Show();
        i.onActiveTicketPressed += i.ShowDetailActiveInfo;
        i.onHistoryTicketPressed += i.ShowDetailHistoryInfo;
        i.AddTicketToActive(userTicketsResponse.userTickets.upcoming);
        i.AddTicketToHistory(userTicketsResponse.userTickets.past);
    }
    public void ShowBurgerMenu() {
        SideMenu.Show();
    }
}
