using UnityEngine;
using TMPro;

// Butikken i Det Ovale Kontor. Selve pengene og lommen ligger i SpilStyring på banen,
// som ligger frosset under kontoret - butikken spørger bare den.
public class KontorButik : MonoBehaviour
{
    public TMP_Text bitcoinTekst;
    public TMP_Text beskedTekst;
    public int elonPris = 100;
    public int kimPris = 400;
    public int netanyahuPris = 600;

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
        Koeb("Elon Musk", elonPris, "Elon Musk Raketmotor aktiveret");
    }

    public void KoebKim()
    {
        Koeb("Kim Jong-un", kimPris, "Kim Jong-un er i lommen. Gud hjælpe os alle");
    }

    public void KoebNetanyahu()
    {
        Koeb("Benjamin Netanyahu", netanyahuPris, "Iron Dome er klar. Tryk SHIFT på isen");
    }

    // Fælles for alle mændene: spørg SpilStyring og vis svaret.
    private void Koeb(string navn, int pris, string succesTekst)
    {
        if (spil == null) return;

        string svar = spil.Koeb(navn, pris);

        if (beskedTekst != null)
        {
            if (svar == "")
            {
                // Købt: grøn tekst.
                beskedTekst.text = succesTekst;
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
