using UnityEngine;
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

    private int opbakning;
    private int bitcoins;
    private float afstand;
    private bool spilletKoerer;

    private void Start()
    {
        // Vigtigt: tiden blev sat til 0 da vi tabte sidst.
        Time.timeScale = 1f;

        opbakning = startOpbakning;
        bitcoins = 0;
        afstand = 0f;
        spilletKoerer = true;

        if (slutSkaerm != null)
        {
            slutSkaerm.SetActive(false);
        }

        VisTal();
    }

    private void Update()
    {
        if (spilletKoerer)
        {
            afstand = afstand + meterPrSekund * Time.deltaTime;
            VisTal();
        }
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
