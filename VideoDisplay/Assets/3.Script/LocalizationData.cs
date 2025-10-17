using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Icons;

[CreateAssetMenu(fileName = "LocalizationData", menuName = "Localization/Data")]
public class LocalizationData : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public string korean;
        public string english;
        public Sprite koreanSprite;
        public Sprite englishSprite;
    }
    public List<Entry> entries = new();
    public List<TMP_Text> languageText = new();
    public List<Image> languageSprite = new();

    public (string text , Sprite sprite)  GetLocaliztionLanguage(Language lang, Entry entry)
    {
        return lang == Language.Korean ? (entry.korean, entry.koreanSprite) : (entry.english, entry.englishSprite);

    }
    public void Localiztion(Language lang)
    {
        Debug.Log($"¹Ù²ï lang : {lang}");
        for (int i = 0; i < entries.Count; i++)
        {
            if(i< languageText.Count)
            {
                languageText[i].text = GetLocaliztionLanguage(lang, entries[i]).text;
            }
            else
            {
                languageSprite[i].sprite = GetLocaliztionLanguage(lang, entries[i]).sprite;
            }
            
        }
    }
}
