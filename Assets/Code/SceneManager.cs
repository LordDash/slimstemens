using SManager = UnityEngine.SceneManagement.SceneManager;

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManager : MonoSingleton<SceneManager>
{
    private const string EmptyScene = "Empty";

    public void Load(string sceneName, System.Action onSceneLoaded)
    {
        Debug.LogFormat("[SceneManager] Load(sceneName = {0}, onSceneLoaded = {1})", sceneName, onSceneLoaded);
        SManager.LoadScene(EmptyScene, LoadSceneMode.Single);

        StartCoroutine(WaitUntilSceneLoaded(sceneName, onSceneLoaded));

        SManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private IEnumerator WaitUntilSceneLoaded(string sceneName, System.Action onSceneLoaded)
    {
        Debug.LogFormat("[SceneManager] WaitUntilSceneLoaded(sceneName = {0}, onSceneLoaded = {1})", sceneName, onSceneLoaded);
        while(SManager.GetActiveScene().name.Equals(sceneName) == false)
        {
            yield return null;
        }

        Debug.LogFormat("[SceneManager] Scene loaded done");

        if (onSceneLoaded != null)
        {
            onSceneLoaded();
        }
    }
}
