using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Lukker Det Ovale Kontor og lader turen fortsætte, hvor den slap.
public class KontorUdgang : MonoBehaviour
{
    public GameObject tilbageKnap;

    // Er ingen knap markeret, markeres tilbageKnap, så Enter altid virker.
    // Det tjekkes hele tiden og ikke kun ved start, fordi markeringen kan
    // forsvinde lige når kontoret åbner, eller hvis man klikker ved siden af knapperne.
    private void Update()
    {
        EventSystem es = EventSystem.current;

        if (es != null && tilbageKnap != null && es.currentSelectedGameObject == null)
        {
            es.SetSelectedGameObject(tilbageKnap);
        }
    }

    public void TilbageTilIsen()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
