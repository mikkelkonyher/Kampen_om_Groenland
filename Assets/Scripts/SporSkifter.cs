using UnityEngine;

// Får en forhindring til langsomt at glide over i nabosporet.
// Den lander altid PÅ et spor og bliver der lidt - ellers kan
// spilleren ikke nå at se hvilke spor der er frie.
//
// Ligesom Vandrer lægger den kun en forskydning oveni, så den
// kan sidde sammen med Ruller uden at de slås om positionen.
public class SporSkifter : MonoBehaviour
{
    public float[] sporHoejder = new float[] { 2.4f, 0f, -2.4f };
    public float pauseMin = 1.5f;
    public float pauseMax = 4.5f;
    public float skifteTid = 2f;      // hvor længe et skift tager

    private int basisSpor;            // det spor Ruller satte os i
    private int fraSpor;
    private int tilSpor;
    private float t = 1f;             // 0 til 1 gennem skiftet
    private float venter;
    private float sidste;             // sidste forskydning vi lagde til

    private void Start()
    {
        Nulstil();
    }

    // Kaldes af Ruller når tingen sendes om på højre side.
    public void Nulstil()
    {
        sidste = 0f;
        basisSpor = NaermesteSpor(transform.position.y);
        fraSpor = basisSpor;
        tilSpor = basisSpor;
        t = 1f;
        venter = Random.Range(pauseMin, pauseMax);
    }

    private void Update()
    {
        if (sporHoejder == null || sporHoejder.Length < 2) return;

        if (t >= 1f)
        {
            venter = venter - Time.deltaTime;

            if (venter <= 0f)
            {
                fraSpor = tilSpor;
                tilSpor = NaboSpor(tilSpor);
                t = 0f;
            }
        }
        else
        {
            t = t + Time.deltaTime / skifteTid;

            if (t >= 1f)
            {
                t = 1f;
                venter = Random.Range(pauseMin, pauseMax);
            }
        }

        // SmoothStep sætter blidt i gang og bremser blidt af.
        // Uden den ser skiftet ud som et ryk.
        float glat = Mathf.SmoothStep(0f, 1f, t);
        float hoejde = Mathf.Lerp(sporHoejder[fraSpor], sporHoejder[tilSpor], glat);
        float forskydning = hoejde - sporHoejder[basisSpor];

        transform.position = transform.position + new Vector3(0f, forskydning - sidste, 0f);
        sidste = forskydning;
    }

    private int NaboSpor(int nu)
    {
        if (nu <= 0) return 1;
        if (nu >= sporHoejder.Length - 1) return sporHoejder.Length - 2;
        return Random.value < 0.5f ? nu - 1 : nu + 1;
    }

    private int NaermesteSpor(float y)
    {
        int bedst = 0;

        for (int i = 1; i < sporHoejder.Length; i++)
        {
            if (Mathf.Abs(y - sporHoejder[i]) < Mathf.Abs(y - sporHoejder[bedst])) bedst = i;
        }

        return bedst;
    }
}
