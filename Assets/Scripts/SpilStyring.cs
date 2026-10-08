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

    // Navnet highscoren gemmes under. MainMenu læser samme navn.
    public const string HighscoreNoegle = "Highscore";

    // Små tal, der flyder op, når man rammer eller samler noget.
    public FlydeTekster flydeTekster;
    public static readonly Color PlusFarve = new Color32(80, 220, 100, 255);   // opbakning op
    public static readonly Color MinusFarve = new Color32(255, 80, 80, 255);   // opbakning ned
    public static readonly Color BitcoinFarve = new Color32(255, 212, 0, 255);

    // Lommen: den mand man har købt i Det Ovale Kontor og kan bruge med Shift.
    public TMP_Text lommeTekst;

    // Elon Musk: alt kører raketFaktor gange hurtigere i raketSekunder.
    public float raketFaktor = 2f;
    public float raketSekunder = 5f;

    // Chatboksen nederst til højre, som mændene fra kontoret taler til en i.
    public ChatBoks chat;
    public Sprite elonFoto;
    public Sprite kimFoto;

    [TextArea(2, 4)]
    public string elonBesked = "Hey Donald. Raketmotoren er tændt. Grønland er det nye Mars: Koldt, øde og ingen bor der frivilligt. Det bliver nemt at overtage.";
    public float elonBeskedSekunder = 10f;

    // Benjamin Netanyahu: Iron Dome beskytter mod alle forhindringer i domeSekunder.
    public Sprite netanyahuFoto;
    [TextArea(2, 4)]
    public string netanyahuBesked = "Iron Dome aktiveret. Hvis noget ikke virker hensigtmæssigt, så kontakt min assistent, da jeg er travlt optaget med at begå folkemord de næste par måneder.";
    public float domeSekunder = 10f;
    public GameObject kuppel;             // den blå kuppel om slæden, vises mens skjoldet er tændt

    // Kim Jong-un: kimHeldChance for at få kimOpbakning mere, ellers dør man på stedet.
    public float kimBeskedSekunder = 10f;
    public int kimOpbakning = 50;
    [Range(0f, 1f)] public float kimHeldChance = 0.7f;   // 0,7 = 70 % held, 30 % død
    [TextArea(2, 4)]
    public string kimGod = "Kim Jong-un har lært dig en vigtig lektion: \"Inderst inde er alle kritikere tilhængere. Du skal bare grave lidt. Typisk to meter.\" +50%";
    [TextArea(2, 4)]
    public string kimDaarlig = "Ups! Du sagde \"Nuuk\" i telefonen til Kim Jong-un. Forbindelsen var dårlig. Nu er forbindelsen til Grønland også dårlig. For evigt. -100%";

    private int opbakning;
    private int bitcoins;
    private float afstand;
    private bool spilletKoerer;

    private string iLommen = "";
    private float raketTidTilbage = 0f;
    private float domeTidTilbage = 0f;

    // Står der noget her, når turen slutter, vises det på slutskærmen.
    private string doedsAarsag = "";

    // Butikken i kontoret skal kunne læse dem, men ikke ændre dem direkte.
    public int Bitcoins => bitcoins;
    public string ILommen => iLommen;

    // Forhindringerne spørger her, om de må gøre skade.
    public bool Beskyttet => domeTidTilbage > 0f;

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
        domeTidTilbage = 0f;
        doedsAarsag = "";

        SkjulChat();

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

        // Shift bruger den mand, der ligger i lommen.
        if (spilletKoerer && !kontorAabent && shift)
        {
            if (iLommen == "Elon Musk")
            {
                BrugElon();
            }
            else if (iLommen == "Kim Jong-un")
            {
                BrugKim();
            }
            else if (iLommen == "Benjamin Netanyahu")
            {
                BrugNetanyahu();
            }
        }

        // Iron Dome tæller ned med spillets tid, ligesom raketfarten.
        if (domeTidTilbage > 0f)
        {
            domeTidTilbage = domeTidTilbage - Time.deltaTime;
        }

        // Kuppelen vises, mens man er beskyttet, og blinker de sidste 2 sekunder som advarsel.
        if (kuppel != null)
        {
            bool vis = Beskyttet && (domeTidTilbage > 2f || Mathf.Repeat(domeTidTilbage, 0.3f) > 0.15f);

            if (kuppel.activeSelf != vis)
            {
                kuppel.SetActive(vis);
            }
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

        if (chat != null)
        {
            chat.Vis(elonFoto, "ELON MUSK", elonBesked, Color.white, elonBeskedSekunder);
        }

        VisTal();
    }

    private void BrugNetanyahu()
    {
        iLommen = "";
        domeTidTilbage = domeSekunder;

        // Chatboksen står lige så længe som skjoldet, så man kan se, hvornår det slutter.
        if (chat != null)
        {
            chat.Vis(netanyahuFoto, "BENJAMIN NETANYAHU", netanyahuBesked, Color.white, domeSekunder);
        }

        VisTal();
    }

    private void SkjulChat()
    {
        if (chat != null)
        {
            chat.Skjul();
        }
    }

    // Wildcard: oftest hjælper han, men nogle gange er turen slut.
    private void BrugKim()
    {
        iLommen = "";

        if (Random.value < kimHeldChance)
        {
            Saml(0, kimOpbakning);

            if (chat != null)
            {
                chat.Vis(kimFoto, "KIM JONG-UN", kimGod, new Color32(80, 220, 100, 255), kimBeskedSekunder);
            }
        }
        else
        {
            // Beskeden vises på slutskærmen, hvor spillet står stille, så den kan læses i ro.
            doedsAarsag = kimDaarlig;
            opbakning = 0;
            Slut();
        }

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

    public void VisFlydeTekst(Vector3 sted, string tekst, Color farve)
    {
        if (flydeTekster != null)
        {
            flydeTekster.Vis(sted, tekst, farve);
        }
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
            // Under raketfarten fortæller Elons chatboks det, så her står intet.
            if (iLommen != "")
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

        // Highscore: gem afstanden, hvis den er længere end den bedste hidtil.
        // PlayerPrefs gemmer på computeren, så tallet overlever at spillet lukkes.
        int meter = Mathf.FloorToInt(afstand);
        int rekord = PlayerPrefs.GetInt(HighscoreNoegle, 0);
        bool nyRekord = meter > rekord;

        if (nyRekord)
        {
            rekord = meter;
            PlayerPrefs.SetInt(HighscoreNoegle, rekord);
            PlayerPrefs.Save();
        }

        if (slutTekst != null)
        {
            // Ny rekord: kun den grønne linje, ellers står samme tal to gange.
            // Ingen rekord: turens afstand og den gamle rekord i hvid.
            string tal = nyRekord
                ? "<color=#50DC64>NY REKORD!  " + rekord + " m</color>"
                : meter + " m\nHIGHSCORE  " + rekord + " m";

            // Døde man af noget særligt (fx Kim Jong-un), står grunden under overskriften.
            string aarsag = doedsAarsag != ""
                ? "<size=32><color=#FF8080>" + doedsAarsag + "</color></size>\n\n"
                : "";

            // Overskriften i gul og større, som i menuen.
            slutTekst.text = "<size=76><color=#FFD400>DU BLEV IKKE GENVALGT</color></size>\n\n"
                + aarsag
                + tal;
        }

        SkjulChat();

        if (slutSkaerm != null)
        {
            slutSkaerm.SetActive(true);
        }

        // Stopper alt der bruger Time.deltaTime - altså isen og forhindringerne.
        Time.timeScale = 0f;
    }
}
