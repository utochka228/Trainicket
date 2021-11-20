using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhoneValidator : MonoBehaviour
{
    [SerializeField] TMP_InputField phoneInputField;

    public void OnEndOfEditting() {
        bool lessThanTen = phoneInputField.text.Length < 10;
        bool isEmpty = string.IsNullOrEmpty(phoneInputField.text);
        bool filled = phoneInputField.text.StartsWith("+38");
        if (filled || isEmpty || lessThanTen)
            return;
        phoneInputField.text = "+38" + phoneInputField.text;
    }
}
