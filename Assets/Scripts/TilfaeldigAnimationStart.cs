using UnityEngine;

// Starter animationen et tilfældigt sted i forløbet.
// Uden den blinker alle sælerne præcis samtidig, og det ser forkert ud.
[RequireComponent(typeof(Animator))]
public class TilfaeldigAnimationStart : MonoBehaviour
{
    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play(0, 0, Random.value);
    }
}
