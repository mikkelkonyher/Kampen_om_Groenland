using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Samlestykke : MonoBehaviour
{
    public int giverBitcoins = 0;
    public int giverOpbakning = 0;
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

        spil.Saml(giverBitcoins, giverOpbakning);
        spil.SpilLyd(lyd);
        SendTilbage();
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
