using UnityEngine;
using TMPro;

// Et lille tal, fx "+5%", der dukker op hvor man ramte noget,
// flyder op og toner ud. Genbruges af FlydeTekster.
[RequireComponent(typeof(TMP_Text))]
public class FlydeTekst : MonoBehaviour
{
    public float varighed = 1f;
    public float stigning = 1.2f;   // hvor langt den flyder op i alt

    private TMP_Text tekst;
    private Color farve;
    private float tid;

    public bool Ledig => !gameObject.activeSelf;

    public void Vis(Vector3 sted, string besked, Color nyFarve)
    {
        if (tekst == null)
        {
            tekst = GetComponent<TMP_Text>();
        }

        transform.position = sted;
        tekst.text = besked;
        farve = nyFarve;
        tekst.color = farve;
        tid = 0f;
        gameObject.SetActive(true);
    }

    // Spillets tid, så tallet står stille i kontoret og efter game over.
    private void Update()
    {
        tid = tid + Time.deltaTime;

        transform.position = transform.position + Vector3.up * (stigning / varighed) * Time.deltaTime;

        // Fuldt synlig det første halve sekund, toner derefter ud.
        float andel = tid / varighed;
        Color c = farve;
        c.a = andel < 0.5f ? 1f : Mathf.Lerp(1f, 0f, (andel - 0.5f) / 0.5f);
        tekst.color = c;

        if (tid >= varighed)
        {
            gameObject.SetActive(false);
        }
    }
}
