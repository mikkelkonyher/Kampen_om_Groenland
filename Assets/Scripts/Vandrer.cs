using UnityEngine;

// Får tingen til at vandre langsomt rundt.
//
// Vandret lægger den kun en lille forskydning oveni, så den kan sidde
// sammen med Ruller uden at de slås om positionen.
//
// Lodret kan den gøre to ting:
//   fritLodret = false  ->  lille vippen omkring det spor Ruller satte den i
//   fritLodret = true   ->  vandrer frit op og ned omkring midteY,
//                           altså hen over flere spor
public class Vandrer : MonoBehaviour
{
    public float bredde = 0.45f;
    public float hoejde = 0.30f;
    public float fart = 0.5f;

    public bool fritLodret = false;
    public float midteY = 0f;

    private float forskudt;     // så flere ikke vandrer i takt
    private Vector3 sidste;

    private void Start()
    {
        forskudt = Random.value * 100f;
    }

    // Kaldes af Ruller når tingen sendes om på højre side.
    public void Nulstil()
    {
        sidste = Vector3.zero;
    }

    private void Update()
    {
        float t = (Time.time + forskudt) * fart;

        // to forskellige takter, ellers går den bare i en ren cirkel
        float x = Mathf.Sin(t) * bredde;
        float y = Mathf.Sin(t * 1.37f) * hoejde;

        if (fritLodret)
        {
            // højden bestemmes helt af vandringen - sporet er ligegyldigt
            transform.position = new Vector3(
                transform.position.x - sidste.x + x,
                midteY + y,
                transform.position.z);

            sidste = new Vector3(x, 0f, 0f);
        }
        else
        {
            Vector3 nu = new Vector3(x, y, 0f);
            transform.position = transform.position - sidste + nu;
            sidste = nu;
        }
    }
}
