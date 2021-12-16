using TrainicketJSONStorage.BookingInfo;
using UnityEngine;
using UnityEngine.UI;

public class PaymentsMenu : MenuItem<PaymentsMenu>
{
    [SerializeField] Button payButton;
    [SerializeField] GameObject successImage;
    BookResponse[] bookResponse;
    public void PayForTickets() {
        foreach (var respone in bookResponse) {
            string url = "http://18.117.102.247:5000/api/ticket/pay?id=" + respone.id + "&payToken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJudW1iZXIiOiI0MjQyNDI0MjQyNDI0MjQyIiwiZXhwaXJhdGlvbiI6IjQyLzQyIiwiY3Z2IjoiNDI0IiwidHlwZSI6Ik1hc3RlcmNhcmQiLCJpYXQiOjE1MTYyMzkwMjJ9.2e2nBNcog83Dcp-ydqnqeiUGO3RNBXhFOebJ5vG9tmI";
            HeaderRequest[] headers = null;
            //if (!string.IsNullOrEmpty(AccountMenu.accessToken))
            //    headers = new HeaderRequest[1] { new HeaderRequest("Authorization", "Bearer " + AccountMenu.accessToken) };
            //else
                headers = new HeaderRequest[1] { new HeaderRequest("Authorization", "Bearer " + respone.accessToken) };
            StartCoroutine(RestAPI.POST(url, null, PaidSuccessfully, headers));
        }
    }
    void PaidSuccessfully(string json, long code) {
        successImage.SetActive(true);
    }
    public void OnCardNumberChanged(string value) {
        if (value.Length == 16)
            cardNumber = true;
        else
            cardNumber = false;
        CheckPayButton();
    }
    public void OnMonthYearChanged(string value) {
        if (value.Length == 4)
            monthYear = true;
        else
            monthYear = false;
        CheckPayButton();
    }
    public void OnPinChanged(string value) {
        if (value.Length == 3)
            pinCode = true;
        else
            pinCode = false;
        CheckPayButton();
    }
    bool cardNumber;
    bool monthYear;
    bool pinCode;
    void CheckPayButton() {
        if (cardNumber && monthYear && pinCode)
            payButton.interactable = true;
        else
            payButton.interactable = false;
    }
    public void GoSearchMenu() {
        Close();
        UIMenuManager.Instance.CloseTopMenus(4);
        SearchMenu.Show();
    } 
    public static void ShowPayments(BookResponse[] bookResponse) {
        Show();
        i.bookResponse = bookResponse;
    }
}
