using SManager = UnityEngine.SceneManagement.SceneManager;

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManager : MonoSingleton<SceneManager>
{
    private const string EmptyScene = "Empty";

    public void Load(string sceneName, System.Action onSceneLoaded)
    {
        SManager.LoadScene(EmptyScene, LoadSceneMode.Single);

        StartCoroutine(WaitUntilSceneLoaded(sceneName, onSceneLoaded));

        SManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private IEnumerator WaitUntilSceneLoaded(string sceneName, System.Action onSceneLoaded)
    {
        while(SManager.GetActiveScene().name.Equals(sceneName) == false)
        {
            yield return null;
        }

        if (onSceneLoaded != null)
        {
            onSceneLoaded();
        }
    }
}
