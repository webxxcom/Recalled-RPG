using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    public static class TypeWriter
    {
        public static IEnumerator TypeWriteWithSkip(
            TMP_Text tmpText,
            float delay,
            string text,
            InputAction action)
        {
            tmpText.text = text;
            tmpText.maxVisibleCharacters = 0;

            // Consume first frame skip action pressing
            yield return null;

            float elapsed = 0;
            while (tmpText.maxVisibleCharacters < tmpText.text.Length)
            {
                tmpText.maxVisibleCharacters++;
                while (elapsed < delay)
                {
                    if (action.WasReleasedThisFrame())
                    {
                        tmpText.maxVisibleCharacters = tmpText.text.Length;
                        action.Reset(); // Consume current action state not to propagate it
                        break;
                    }

                    elapsed += Time.unscaledDeltaTime;
                    yield return null;
                }
                elapsed = 0;
            }
        }

        public static void FinishTypeWrite(TMP_Text tmpText, string text)
        {
            tmpText.maxVisibleCharacters = text.Length;
        }
    }
}
