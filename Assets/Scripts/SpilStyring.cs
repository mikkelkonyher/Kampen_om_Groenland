using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

// Reglerne og tallene på skærmen.
// Opbakning er liv. Rammer den 0, er turen slut.
public class SpilStyring : MonoBehaviour
{
    public int startOpbakning = 100;
    public int maksOpbakning = 100;
    public float meterPrSekund = 10f;

    public TMP_Text opbakningTekst;
    public TMP_Text bitcoinTekst;
    public TMP_Text afstandTekst;

    public GameObject slutSkaerm;
    public TMP_Text slutTekst;

    // Én fælles AudioSource til alle korte lyde.
    // PlayOneShot kan spille flere oven i hinanden uden at afbryde.
    public AudioSource lydKilde;

    // Scenen der åbnes med O. Den lægges oven på banen, så turen ikke går tabt.
    public string kontorScene = "OvaleKontor";

    // Lommen: den mand man har købt i Det Ovale Kontor og kan bruge med Shift.
    public TMP_Text lommeTekst;

    // Elon Musk: alt kører raketFaktor gange hurtigere i raketSekunder.
    public float raketFaktor = 2f;
    public float raketSekunder = 5f;

    private int opbakning;
    private int bitcoins;
    private float afstand;
    private bool spilletKoerer;

    private string iLommen = "";
    private float raketTidTilbage = 0f;

    // Butikken i kontoret skal kunne læse dem, men ikke ændre dem direkte.
    public int Bitcoins => bitcoins;
    public string ILommen => iLommen;

    private void Start()
    {
        // Vigtigt: tiden blev sat til 0 da vi tabte sidst.
        Time.timeScale = 1f;

        opbakning = startOpbakning;
        bitcoins = 0;
        afstand = 0f;
        spilletKoerer = true;

        // Fartfaktoren er fælles og overlever sceneskift, så den skal nulstilles her.
        Ruller.fartFaktor = 1f;
        iLommen = "";
        raketTidTilbage = 0f;

        if (slutSkaerm != null)
        {
            slutSkaerm.SetActive(false);
        }

        VisTal();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        bool kontorAabent = SceneManager.GetSceneByName(kontorScene).isLoaded;

        if (spilletKoerer && !kontorAabent && keyboard != null && keyboard.oKey.wasPressedThisFrame)
        {
            AabnKontor();
        }

        bool shift = keyboard != null && (keyboard.leftShiftKey.wasPressedThisFrame || keyboard.rightShiftKey.wasPressedThisFrame);

        if (spilletKoerer && !kontorAabent && shift && iLommen == "Elon Musk")
        {
            BrugElon();
        }

        // Raketfarten tæller ned med spillets tid, så den står stille i kontoret og efter game over.
        if (raketTidTilbage > 0f)
        {
            raketTidTilbage = raketTidTilbage - Time.deltaTime;

            if (raketTidTilbage <= 0f)
            {
                raketTidTilbage = 0f;
                Ruller.fartFaktor = 1f;
            }
        }

        if (spilletKoerer)
        {
            afstand = afstand + meterPrSekund * Ruller.fartFaktor * Time.deltaTime;
            VisTal();
        }
    }

    private void BrugElon()
    {
        iLommen = "";
        raketTidTilbage = raketSekunder;
        Ruller.fartFaktor = raketFaktor;
        VisTal();
    }

    // Kaldes af butikken. Tom tekst betyder at købet gik igennem,
    // ellers er teksten grunden til at det ikke gjorde.
    public string Koeb(string navn, int pris)
    {
        if (iLommen != "")
        {
            return "Du har allerede " + iLommen + " i lommen";
        }

        if (bitcoins < pris)
        {
            return "Du mangler " + (pris - bitcoins) + " bitcoins";
        }

        bitcoins = bitcoins - pris;
        iLommen = navn;
        VisTal();
        return "";
    }

    private void AabnKontor()
    {
        // Banen fryses og bliver liggende under kontoret.
        Time.timeScale = 0f;
        SceneManager.LoadScene(kontorScene, LoadSceneMode.Additive);
    }

    public void MistOpbakning(int antal)
    {
        if (!spilletKoerer) return;

        opbakning = opbakning - antal;

        if (opbakning <= 0)
        {
            opbakning = 0;
            Slut();
        }

        VisTal();
    }

    public void Saml(int bitcoin, int opbakningOp)
    {
        if (!spilletKoerer) return;

        bitcoins = bitcoins + bitcoin;
        opbakning = opbakning + opbakningOp;

        // Loft på 100 procent. Uden det er der ingen fare tilbage i spillet.
        if (opbakning > maksOpbakning)
        {
            opbakning = maksOpbakning;
        }

        VisTal();
    }

    // styrke = 1 er fuld styrke. Angiver man ingen, bruges 1.
    public void SpilLyd(AudioClip klip, float styrke = 1f)
    {
        if (lydKilde == null || klip == null) return;

        lydKilde.PlayOneShot(klip, styrke);
    }

    private void VisTal()
    {
        if (opbakningTekst != null)
        {
            opbakningTekst.text = "OPBAKNING  " + opbakning + "%";
        }

        if (bitcoinTekst != null)
        {
            bitcoinTekst.text = "BITCOIN  " + bitcoins;
        }

        if (afstandTekst != null)
        {
            afstandTekst.text = "AFSTAND  " + Mathf.FloorToInt(afstand) + " m";
        }

        if (lommeTekst != null)
        {
            if (raketTidTilbage > 0f)
            {
                lommeTekst.text = "RAKETFART!";
            }
            else if (iLommen != "")
            {
                lommeTekst.text = iLommen.ToUpper() + "  ·  SHIFT";
            }
            else
            {
                lommeTekst.text = "";
            }
        }
    }

    private void Slut()
    {
        spilletKoerer = false;

        if (slutTekst != null)
        {
            // Overskriften i gul og større, som i menuen.
            slutTekst.text = "<size=76><color=#FFD400>DU BLEV IKKE GENVALGT</color></size>\n\n"
                + Mathf.FloorToInt(afstand) + " m   -   " + bitcoins + " bitcoins";
        }

        if (slutSkaerm != null)
        {
            slutSkaerm.SetActive(true);
        }

        // Stopper alt der bruger Time.deltaTime - altså isen og forhindringerne.
        Time.timeScale = 0f;
    }
}
