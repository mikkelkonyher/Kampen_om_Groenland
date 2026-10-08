using UnityEngine;

// En lille fast pulje af flydende tal. Der oprettes aldrig nye under spil:
// er alle i brug, genbruges den der har været vist længst.
public class FlydeTekster : MonoBehaviour
{
    public FlydeTekst[] tekster;

    private int naeste = 0;

    public void Vis(Vector3 sted, string besked, Color farve)
    {
        if (tekster == null || tekster.Length == 0) return;

        // Først en ledig, ellers den næste i ringen.
        for (int i = 0; i < tekster.Length; i++)
        {
            int nr = (naeste + i) % tekster.Length;

            if (tekster[nr] != null && tekster[nr].Ledig)
            {
                naeste = (nr + 1) % tekster.Length;
                tekster[nr].Vis(sted, besked, farve);
                return;
            }
        }

        FlydeTekst aeldste = tekster[naeste];
        naeste = (naeste + 1) % tekster.Length;

        if (aeldste != null)
        {
            aeldste.Vis(sted, besked, farve);
        }
    }
}
