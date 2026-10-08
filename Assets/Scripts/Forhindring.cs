using UnityEngine;

// Noget man ikke må ramme. Trækker fra opbakningen.
[RequireComponent(typeof(BoxCollider2D))]
public class Forhindring : MonoBehaviour
{
    public int kosterOpbakning = 5;
    public AudioClip lyd;
    public SpilStyring spil;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;

        if (spil == null)
        {
            spil = Object.FindFirstObjectByType<SpilStyring>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SpillerSlaede>() == null) return;
        if (spil == null) return;

        // Iron Dome: man kører lige igennem uden skade, og forhindringen bliver stående.
        if (spil.Beskyttet) return;

        spil.MistOpbakning(kosterOpbakning);
        spil.SpilLyd(lyd);

        // Har den en Splatter, bliver den liggende og splatter ud mens
        // isen kører den ud af billedet. Ellers ryger den om med det samme.
        Splatter splatter = GetComponent<Splatter>();

        if (splatter != null)
        {
            splatter.Splat();
        }
        else
        {
            SendTilbage();
        }
    }

    private void SendTilbage()
    {
        Ruller ruller = GetComponent<Ruller>();

        if (ruller != null)
        {
            ruller.Genstart();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
