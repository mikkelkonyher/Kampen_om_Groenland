using UnityEngine;
using UnityEngine.SceneManagement;

public class CourseSceneButtons : MonoBehaviour
{
    public void OpenScene(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogWarning("Tilfoej scenen til Scene List: " + sceneName);
            return;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene()
    {
        OpenScene(SceneManager.GetActiveScene().name);
    }
}