using System;
using System.Collections;
using System.Collections.Generic;
using TrainicketJSONStorage.GettingTickets;
using UnityEngine;
using UnityEngine.UI;

public class SideMenu : MenuItem<SideMenu>
{
    [SerializeField] LayoutElement layoutElement;
    [Range(0f, 1f)]
    [SerializeField] float menuPerWidth;

    [SerializeField] Image darkRegion;
    // Start is called before the first frame update
    void Start()
    {
        ShowMenu();
    }
    void ShowMenu(bool hide = false) {
        float endWidth = 0f;
        float startWidth = 0f;

        float startAlpha = 0f;
        float endAlpha = 0f;
        if (hide) {
            startWidth = layoutElement.preferredHeight;
            endWidth = 0f;

            startAlpha = 0.5f;
            endAlpha = 0f;
        } else {
            startWidth = 0f;
            endWidth = Screen.width * menuPerWidth;

            startAlpha = 0f;
            endAlpha = 0.5f;
        }
        float duration = 0.2f;
        StartCoroutine(Utils.Lerp(startWidth, endWidth, duration, (lerpWidth, end) => {
            layoutElement.preferredWidth = lerpWidth;
            if (end && hide)
                Close();
        }));
        StartCoroutine(Utils.Lerp(startAlpha, endAlpha, duration, (lerpAlpha, end) => darkRegion.ChangeAlpha(lerpAlpha)));
    }
    public override void OnBackPressed() {
        HideMenu();
    }
    public void HideMenu() => ShowMenu(true);

    public void ShowTicketsMenu() {
        if (UserTicketsMenu.i != null && UserTicketsMenu.i.gameObject.activeSelf) {
            Close();
            return;
        }
        if (string.IsNullOrEmpty(AccountMenu.accessToken)) {
            Debug.LogError("Unregistered user - You are not registered!" +
                "Please, register in system for watching your tickets.");
            return;
        }
        else {
            string url = "http://18.117.102.247:5000/api/user/tickets";
            StartCoroutine(RestAPI.GET(url, (json, code) => {
                var ticketsResponse = JsonUtility.FromJson<UserTicketsResponse>(json);
                Close();
                UserTicketsMenu.ShowTicketsMenu(ticketsResponse);
            }, new HeaderRequest[1] { new HeaderRequest("Authorization", "Bearer " + AccountMenu.accessToken) }, true));
        }
    }
}
