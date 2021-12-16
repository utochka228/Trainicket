using System;
using System.Collections;
using System.Collections.Generic;
using TrainicketJSONStorage.SelectedForBooking;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TrainicketJSONStorage.BookingInfo;
using System.Text.RegularExpressions;

[Serializable]
public class Ticket
{
    public bool ready;
    public BookInfo bookInfo;
    public UserBookingData userBookingData;
    public Ticket(BookInfo bookInfo, UserBookingData userBookingData) {
        this.bookInfo = bookInfo;
        this.userBookingData = userBookingData;
    }
}
public class BookingMenu : MenuItem<BookingMenu>
{
    [SerializeField] TMP_Dropdown privilege;
    [SerializeField] GameObject studentObj;
    [SerializeField] TMP_InputField studentNumber;
    [SerializeField] GameObject birthdayObj;
    [SerializeField] TextMeshProUGUI birthdayDate;
    [SerializeField] Toggle bedclothes;
    [SerializeField] Toggle drinks;
    [SerializeField] TMP_InputField firstName;
    [SerializeField] TMP_InputField secondName;
    [SerializeField] TMP_InputField email;
    [SerializeField] Button next;

    [SerializeField] Toggle fillAccountData;

    public List<Ticket> tickets = new List<Ticket>();
    [SerializeField] HorizontalLayoutGroup horizontalLayout;
    [SerializeField] GameObject tabPrefab;
    public static void ShowBookingMenu(UserBookingData[] selectedTickets) {
        Show();
        if(string.IsNullOrEmpty(AccountMenu.accessToken))
            i.fillAccountData.gameObject.SetActive(false);
        //Tabs spawning
        for (int k = 0; k < selectedTickets.Length; k++) {
            var tab = Instantiate(i.tabPrefab, i.horizontalLayout.transform);
            int index = k;
            tab.GetComponent<Button>().onClick.AddListener(() => i.OpenTicket(index));
            tab.GetComponentInChildren<TextMeshProUGUI>().text = (k + 1).ToString();
            i.tickets.Add(new Ticket(new BookInfo(), selectedTickets[k]));
        }
        i.OpenTicket(0);
    }
    int currentTab;
    Ticket currentTicket;
    DateTime lastSelectedDate;
    void OpenTicket(int index) {
        var ticket = tickets[index];
        currentTicket = ticket;
        ticket.bookInfo.editted = true;
        currentTab = index;
        foreach (Transform tab in horizontalLayout.transform) {
            tab.Find("Selector").gameObject.SetActive(false);
        }
        notUpdateValues = true;
        horizontalLayout.transform.GetChild(index).Find("Selector").gameObject.SetActive(true);
        fillAccountData.isOn = false;

        //Update values
        SetFilledValues();
        bedclothes.isOn = currentTicket.bookInfo.additional.bedclothes;
        drinks.isOn = currentTicket.bookInfo.additional.drinks;
        notUpdateValues = false;
    }
    public void UpdateValues() {
        if (notUpdateValues)
            return;
        var studentData = studentNumber.text;
        var childData = lastSelectedDate.Date.ToString("yy.MM.dd");
        var priv = RegisterMenu.GetPrivilegeByIndex(privilege.value, childData, studentData);
        currentTicket.bookInfo.privilege = priv.type;
        currentTicket.bookInfo.childBirthday = priv.data;
        currentTicket.bookInfo.studentNumber = priv.data;
        currentTicket.bookInfo.additional.bedclothes = bedclothes.isOn;
        currentTicket.bookInfo.additional.drinks = drinks.isOn;
        currentTicket.bookInfo.firstName = firstName.text;
        currentTicket.bookInfo.secondName = secondName.text;
        currentTicket.bookInfo.email = email.text;
        tickets[currentTab] = currentTicket;

        CheckTicketReady();
    }
    void CheckTicketReady() {
        if (currentTicket.bookInfo.privilege == "student") {
            if (string.IsNullOrEmpty(studentNumber.text)) {
                ChangeTicketState(false);
                return;
            }
        }
        if (string.IsNullOrEmpty(currentTicket.bookInfo.firstName)) {
            ChangeTicketState(false);
            return;
        }
        if (string.IsNullOrEmpty(currentTicket.bookInfo.secondName)) {
            ChangeTicketState(false);
            return;
        }
        if (string.IsNullOrEmpty(currentTicket.bookInfo.email)) {
            ChangeTicketState(false);
            return;
        }
        ChangeTicketState(true);
    }
    void ChangeTicketState(bool ready) {
        var currentTabBtn = horizontalLayout.transform.GetChild(currentTab).GetComponent<Button>();
        if (ready) {
            var image = currentTabBtn.image;
            image.color = Color.green;
        } else {
            var image = currentTabBtn.image;
            image.color = Color.white;
        }
        currentTicket.ready = ready;
    }
    public void ShowCalendar() {
        CalendarMenu.ShowCalendar((selectedDate) => {
            lastSelectedDate = selectedDate;
            UpdateValues();
        }, true);
    }
    public void OnPrivilegeChanged(int index) {
        if(index == 1) {
            birthdayObj.SetActive(true);
            studentObj.SetActive(false);
            return;
        }
        if(index == 2) {
            studentObj.SetActive(true);
            birthdayObj.SetActive(false);
            return;
        }
    }
    public void OnNextPressed() {
        for (int k = 0; k < tickets.Count; k++) {
            if(tickets[k].ready == false) {
                OpenTicket(k);
                return;
            }
        }
        BookTickets();
    }
    int ticketsResponded;
    BookResponse[] bookResponses;
    void BookTickets() {
        //Process userBookingData
        ticketsResponded = 0;
        bookResponses = new BookResponse[tickets.Count];
        for (int j = 0; j < tickets.Count; j++) {
            var ticket = tickets[j];
            BookData bookData = new BookData();
            bookData.route = ticket.userBookingData.route;
            bookData.from = ticket.userBookingData.from;
            bookData.to = ticket.userBookingData.to;
            bookData.van = ticket.userBookingData.van;
            bookData.seat = ticket.userBookingData.seat;
            bookData.firstName = ticket.bookInfo.firstName;
            bookData.lastName = ticket.bookInfo.secondName;
            bookData.email = ticket.bookInfo.email;
            List<Service> services = new List<Service>();
            if (ticket.bookInfo.additional.bedclothes)
                services.Add(new Service() { service = "bedclothes", amount = UnityEngine.Random.Range(1, 4) });
            if (ticket.bookInfo.additional.drinks)
                services.Add(new Service() { service = "tea", amount = UnityEngine.Random.Range(1, 4) });
            bookData.services = services.ToArray();

            bookData.privilege.type = ticket.bookInfo.privilege;
            if (ticket.bookInfo.privilege == "child")
                bookData.privilege.data = ticket.bookInfo.childBirthday;
            if (ticket.bookInfo.privilege == "student")
                bookData.privilege.data = ticket.bookInfo.studentNumber;

            var body = JsonUtility.ToJson(bookData);

            if(services.Count == 0) {
                var mask = new Regex(@",""services"".*?(?=])]");
                body = mask.Replace(body, "");
            }
            if (ticket.bookInfo.privilege == "Full" || string.IsNullOrEmpty(ticket.bookInfo.privilege)) {
                var mask = new Regex(@",""privilege"".*?(?=})}");
                body = mask.Replace(body, "");
            }
            int index = j;
            HeaderRequest[] headers = null;
            if (!string.IsNullOrEmpty(AccountMenu.accessToken))
                headers = new HeaderRequest[1] { new HeaderRequest("Authorization", "Bearer " + AccountMenu.accessToken) };
            StartCoroutine(RestAPI.POST("http://18.117.102.247:5000/api/ticket/book", body,
                (json, code) => {
                    bookResponses[index] = JsonUtility.FromJson<BookResponse>(json);
                    ticketsResponded++;
            }, headers));
        }
        StartCoroutine(ShowPayments());
    }
    IEnumerator ShowPayments() {
        yield return new WaitUntil(() => ticketsResponded == bookResponses.Length);
        PaymentsMenu.ShowPayments(bookResponses);
    }
    bool notUpdateValues;
    public void FillRegisteredInfo(bool isOn) {
        if (isOn)
            FillRegInfo();
        else
            ResetAllValues();
        UpdateValues();
    }
    void SetFilledValues() {
        firstName.text = currentTicket.bookInfo.firstName;
        secondName.text = currentTicket.bookInfo.secondName;
        email.text = currentTicket.bookInfo.email;
        if (currentTicket.bookInfo.privilege == "child") {
            privilege.value = 1;
            birthdayDate.text = currentTicket.bookInfo.childBirthday;
            birthdayObj.SetActive(true);
        }
        if (currentTicket.bookInfo.privilege == "student") {
            privilege.value = 2;
            studentObj.SetActive(true);
            studentNumber.text = currentTicket.bookInfo.studentNumber;
        }
    }
    void FillRegInfo() {
        notUpdateValues = true;
        var registeredData = AccountMenu.RegisteredData;
        currentTicket.bookInfo.privilege = registeredData.privilege.type;
        currentTicket.bookInfo.studentNumber = registeredData.privilege.data;
        currentTicket.bookInfo.childBirthday = DateTime.Parse(registeredData.privilege.data).ToString("yy.MM.dd");
        currentTicket.bookInfo.firstName = registeredData.firstName;
        currentTicket.bookInfo.secondName = registeredData.lastName;
        currentTicket.bookInfo.email = registeredData.email;
        SetFilledValues();
        notUpdateValues = false;
    }
    public void ResetAllValues() {
        notUpdateValues = true;
        privilege.value = 0;
        birthdayDate.text = "select date";
        birthdayObj.SetActive(false);
        studentObj.SetActive(false);
        studentNumber.text = "";
        bedclothes.isOn = false;
        drinks.isOn = false;
        firstName.text = "";
        secondName.text = "";
        email.text = "";
        notUpdateValues = false;
    }
}

[Serializable]
public struct BookInfo
{
    public bool editted;
    public string privilege;
    public string childBirthday;
    public string studentNumber;
    public Additional additional;
    public string firstName;
    public string secondName;
    public string email;
}
[Serializable]
public struct Additional
{
    public bool bedclothes;
    public bool drinks;
}
