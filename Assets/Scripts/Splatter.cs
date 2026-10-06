using UnityEngine;

// Skifter til splat-animationen når spilleren rammer,
// og tilbage til den normale når tingen sendes om på højre side igen.
//
// Komponenterne hentes hver gang i stedet for at blive gemt i Awake.
// Det sker kun når man rammer noget, så det koster ingenting -
// og så kan de ikke nå at være tomme.
[RequireComponent(typeof(Animator))]
public class Splatter : MonoBehaviour
{
    public RuntimeAnimatorController normal;
    public RuntimeAnimatorController splat;

    public void Splat()
    {
        if (splat == null) return;

        // starter forfra, og en pandekage kan ikke rammes igen
        Skift(splat, 0f, false);
    }

    public void Nulstil()
    {
        if (normal == null) return;

        // tilfældigt sted i forløbet, så flere ikke går i takt
        Skift(normal, Random.value, true);
    }

    private void Skift(RuntimeAnimatorController controller, float startTid, bool kanRammes)
    {
        Animator animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.runtimeAnimatorController = controller;
            animator.Play(0, 0, startTid);
        }

        Collider2D felt = GetComponent<Collider2D>();

        if (felt != null)
        {
            felt.enabled = kanRammes;
        }
    }
}
