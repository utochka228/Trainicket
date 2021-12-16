using TrainicketJSONStorage.BookingInfo;
using UnityEngine;
using UnityEngine.UI;

public class PaymentsMenu : MenuItem<PaymentsMenu>
{
    [SerializeField] Button payButton;
    BookResponse[] bookResponse;
    public void PayForTickets() {
        //string url = "http://18.117.102.247:5000/api/ticket/pay?id=" + id + "&payToken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJudW1iZXIiOiI0MjQyNDI0MjQyNDI0MjQyIiwiZXhwaXJhdGlvbiI6IjQyLzQyIiwiY3Z2IjoiNDI0IiwidHlwZSI6Ik1hc3RlcmNhcmQiLCJpYXQiOjE1MTYyMzkwMjJ9.2e2nBNcog83Dcp-ydqnqeiUGO3RNBXhFOebJ5vG9tmI";
        //StartCoroutine(RestAPI.POST(url, null, PaidSuccessfully, new HeaderRequest[1] { new HeaderRequest("Authorization", "Bearer " + AccountMenu.accessToken)))
    }
    void PaidSuccessfully() {
        UIMenuManager.Instance.CloseTopMenus(4);
    }
    public static void ShowPayments(BookResponse[] bookResponse) {
        Show();
        i.bookResponse = bookResponse;
    }
}
