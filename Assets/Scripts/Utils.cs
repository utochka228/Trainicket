using System.Collections;
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
    public static IEnumerator Lerp(float startValue, float endValue, float duration, LerpDelegate action) {
        float timeElapsed = 0;
        float valueToLerp = startValue;

        while (timeElapsed < duration) {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            action?.Invoke(valueToLerp, false);
            yield return null;
        }
        valueToLerp = endValue;
        action?.Invoke(valueToLerp, true);
    }
    public delegate void LerpDelegate(float lerpValue, bool isEnd);
}
