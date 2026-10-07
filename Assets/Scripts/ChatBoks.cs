using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Lille chatboks nederst på skærmen: foto, navn og en besked.
// Bruges af alle mændene fra Det Ovale Kontor.
public class ChatBoks : MonoBehaviour
{
    public Image foto;
    public TMP_Text navn;
    public TMP_Text besked;

    private float tidTilbage = 0f;

    public void Vis(Sprite billede, string afsender, string tekst, Color farve, float sekunder)
    {
        if (foto != null)
        {
            foto.sprite = billede;
        }

        if (navn != null)
        {
            navn.text = afsender;
        }

        if (besked != null)
        {
            besked.text = tekst;
            besked.color = farve;
        }

        tidTilbage = sekunder;
        gameObject.SetActive(true);
    }

    public void Skjul()
    {
        tidTilbage = 0f;
        gameObject.SetActive(false);
    }

    // Tæller med spillets tid, så den står stille i kontoret og efter game over.
    private void Update()
    {
        if (tidTilbage <= 0f) return;

        tidTilbage = tidTilbage - Time.deltaTime;

        if (tidTilbage <= 0f)
        {
            Skjul();
        }
    }
}
