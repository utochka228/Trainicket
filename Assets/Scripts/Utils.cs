using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Utils
{
    public static string ReplaceSymbol(string input, int index, char symbol) {
        StringBuilder sb = new StringBuilder(input);
        sb[index] = symbol;
        return sb.ToString();
    }
}
