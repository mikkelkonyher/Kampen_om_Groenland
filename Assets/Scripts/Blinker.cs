using UnityEngine;
using TMPro;

// Får en tekst til at blinke, som "PRESS START" i gamle arkadespil.
// Bruger unscaledTime, så den også blinker hvis tiden er sat på pause.
[RequireComponent(typeof(TMP_Text))]
public class Blinker : MonoBehaviour
{
    public float sekunderTaendt = 0.6f;
    public float sekunderSlukket = 0.4f;

    private void Update()
    {
        float runde = sekunderTaendt + sekunderSlukket;
        bool taendt = Time.unscaledTime % runde < sekunderTaendt;
        GetComponent<TMP_Text>().enabled = taendt;
    }
}
