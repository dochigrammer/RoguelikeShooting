using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _Instance;

    public virtual void OnInitialize() {}

    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = (T)FindObjectOfType(typeof(T));

                if (_Instance == null)
                {
                    GameObject go = new GameObject();
                    _Instance = go.AddComponent<T>();

                    go.name = typeof(T).ToString();

                    DontDestroyOnLoad(go);
                    _Instance.OnInitialize();
                }
            }
            return _Instance;
        }
    }

}