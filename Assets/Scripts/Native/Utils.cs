using System.Collections;
using TMPro;
using UnityEngine;

static public class Utils
{
    static public IEnumerator RevealTextOverTime(
        TextMeshProUGUI textMeshpro,
        float timeDelay,
        string text)
    {
        textMeshpro.text = text;
        textMeshpro.maxVisibleCharacters = 0;

        while (textMeshpro.maxVisibleCharacters < textMeshpro.text.Length)
        {
            textMeshpro.maxVisibleCharacters++;

            yield return new WaitForSecondsRealtime(timeDelay);
        }
    }
}
