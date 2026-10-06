using UnityEngine;

// Baggrundsmusik der kører i loop hele tiden, på tværs af alle scener.
// Der må kun findes én: kommer man tilbage til en scene med en kopi,
// fjerner kopien sig selv, så musikken ikke starter forfra eller spiller dobbelt.
[RequireComponent(typeof(AudioSource))]
public class Baggrundsmusik : MonoBehaviour
{
    private static Baggrundsmusik instans;

    public AudioClip musik;
    [Range(0f, 1f)] public float lydstyrke = 0.5f;

    private void Awake()
    {
        if (instans != null)
        {
            Destroy(gameObject);
            return;
        }

        instans = this;
        DontDestroyOnLoad(gameObject);

        AudioSource kilde = GetComponent<AudioSource>();
        kilde.clip = musik;
        kilde.loop = true;
        kilde.volume = lydstyrke;
        kilde.playOnAwake = false;

        if (musik != null)
        {
            kilde.Play();
        }
    }
}
