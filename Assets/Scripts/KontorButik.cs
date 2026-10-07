using UnityEngine;
using TMPro;

// Butikken i Det Ovale Kontor. Selve pengene og lommen ligger i SpilStyring på banen,
// som ligger frosset under kontoret - butikken spørger bare den.
public class KontorButik : MonoBehaviour
{
    public TMP_Text bitcoinTekst;
    public TMP_Text beskedTekst;
    public int elonPris = 100;

    private SpilStyring spil;

    private void Start()
    {
        spil = Object.FindFirstObjectByType<SpilStyring>();

        if (beskedTekst != null)
        {
            beskedTekst.text = "";
        }

        VisBitcoins();
    }

    public void KoebElon()
    {
        if (spil == null) return;

        string svar = spil.Koeb("Elon Musk", elonPris);

        if (beskedTekst != null)
        {
            if (svar == "")
            {
                // Købt: grøn tekst.
                beskedTekst.text = "Elon Musk Raketmotor aktiveret";
                beskedTekst.color = new Color32(80, 220, 100, 255);
            }
            else
            {
                // Ikke købt: rød tekst med grunden.
                beskedTekst.text = svar;
                beskedTekst.color = new Color32(255, 128, 128, 255);
            }
        }

        VisBitcoins();
    }

    private void VisBitcoins()
    {
        if (bitcoinTekst == null) return;

        int antal = spil != null ? spil.Bitcoins : 0;
        bitcoinTekst.text = "DU HAR " + antal + " BITCOINS";
    }
}
