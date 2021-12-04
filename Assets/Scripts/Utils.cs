using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static string ReplaceSymbol(string input, int index, char symbol) {
        StringBuilder sb = new StringBuilder(input);
        sb[index] = symbol;
        return sb.ToString();
    }
    public static T ChangeAlpha<T>(this T g, float newAlpha)
         where T : Graphic {
        var color = g.color;
        color.a = newAlpha;
        g.color = color;
        return g;
    }
}
