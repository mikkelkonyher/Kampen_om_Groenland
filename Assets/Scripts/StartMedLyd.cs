 using UnityEngine;
  using UnityEngine.SceneManagement;

  // Spiller en lyd der overlever sceneskiftet, og skifter med det samme.
  public class StartMedLyd : MonoBehaviour
  {
      public AudioClip lyd;

      public void AabnScene(string sceneNavn)
      {
          // Et nyt, tomt objekt kun til lyden. Det er ikke en del af nogen scene.
          GameObject lydObjekt = new GameObject("Startlyd");
          AudioSource kilde = lydObjekt.AddComponent<AudioSource>();

          // Bliv her, når scenen skifter.
          DontDestroyOnLoad(lydObjekt);

          kilde.PlayOneShot(lyd);

          // Ryd op igen, når lyden er færdig, så der ikke ligger døde objekter tilbage.
          Destroy(lydObjekt, lyd.length);

          Time.timeScale = 1f;
          SceneManager.LoadScene(sceneNavn);
      }
  }
