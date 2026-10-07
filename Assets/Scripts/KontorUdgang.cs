using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Lukker Det Ovale Kontor og lader turen fortsætte, hvor den slap.
public class KontorUdgang : MonoBehaviour
{
    public GameObject tilbageKnap;

    private void Start()
    {
        // Markér knappen, så Enter virker med det samme.
        if (EventSystem.current != null && tilbageKnap != null)
        {
            EventSystem.current.SetSelectedGameObject(tilbageKnap);
        }
    }

    public void TilbageTilIsen()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
