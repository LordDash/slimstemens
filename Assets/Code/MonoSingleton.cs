using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = CreateInstance();
            }

            return _instance;
        }
    }

    private static T CreateInstance()
    {
        GameObject instanceGO = new GameObject(typeof(T).ToString());
        GameObject.DontDestroyOnLoad(instanceGO);

        return instanceGO.AddComponent<T>();
    }

    private void Awake ()
    {
        if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
