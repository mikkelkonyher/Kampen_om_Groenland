using UnityEngine;

// Flytter tingen mod venstre. Når den er ude af billedet, sendes den
// tilbage til højre igen - så kan de samme få objekter bruges for evigt.
public class Ruller : MonoBehaviour
{
    public float fart = 6f;
    public float slutX = -12f;
    public float startX = 12f;
    public float spredning = 8f;
    public bool skiftSporVedStart = true;
    public float[] sporHoejder = new float[] { 2.4f, 0f, -2.4f };

    private void Update()
    {
        transform.position = transform.position + Vector3.left * fart * Time.deltaTime;

        if (transform.position.x < slutX)
        {
            Genstart();
        }
    }

    // Kaldes også udefra, når spilleren har ramt eller samlet tingen.
    public void Genstart()
    {
        float hoejde = transform.position.y;

        if (skiftSporVedStart && sporHoejder.Length > 0)
        {
            hoejde = sporHoejder[Random.Range(0, sporHoejder.Length)];
        }

        // Lidt tilfældig afstand, ellers samler alle ting sig i en klump.
        float x = startX + Random.Range(0f, spredning);

        transform.position = new Vector3(x, hoejde, transform.position.z);
        gameObject.SetActive(true);

        // Var den splattet ud, skal den være hel igen når den kommer tilbage.
        Splatter splatter = GetComponent<Splatter>();
        if (splatter != null)
        {
            splatter.Nulstil();
        }

        // Vandreren skal starte forfra, ellers ligger tingen skævt i sporet.
        Vandrer vandrer = GetComponent<Vandrer>();
        if (vandrer != null)
        {
            vandrer.Nulstil();
        }

        // Sporskifteren skal også vide at vi er i et nyt spor nu.
        SporSkifter skifter = GetComponent<SporSkifter>();
        if (skifter != null)
        {
            skifter.Nulstil();
        }
    }
}
