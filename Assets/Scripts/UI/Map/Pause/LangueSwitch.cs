using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LangueSwitch : MonoBehaviour
{
    private int id;
    [SerializeField] private TMP_Text text;
    
    public void SwitchLangue(bool _isLeft)
    {
        if (_isLeft)
        {
            if (id <= 0) return;
            id--;
            text.text = "Francais";
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == "fr");
        }
        else
        {
            if (id >= 1) return;
            id++;
            text.text = "Anglais";
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == "en");
        }
    }
}
