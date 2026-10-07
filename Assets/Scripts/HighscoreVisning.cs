using UnityEngine;
using TMPro;

// Viser den længste afstand man har kørt. Tallet gemmes af SpilStyring, når turen slutter.
[RequireComponent(typeof(TMP_Text))]
public class HighscoreVisning : MonoBehaviour
{
    private void Start()
    {
        int meter = PlayerPrefs.GetInt(SpilStyring.HighscoreNoegle, 0);
        GetComponent<TMP_Text>().text = "HIGHSCORE  " + meter + " m";
    }
}
