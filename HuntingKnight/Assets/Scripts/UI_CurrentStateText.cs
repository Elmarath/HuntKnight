using UnityEngine;
using TMPro;

public class UI_CurrentStateText : MonoBehaviour
{
    public TextMeshProUGUI textmeshPro;

    public void Init()
    {
        textmeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        textmeshPro.SetText(text);
    }
}
