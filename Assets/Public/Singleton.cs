using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static object _lock = new object();
    private static volatile T instance = null;

    protected Singleton() { }
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    instance = FindObjectOfType(typeof(T)) as T;
                    if (instance == null)
                    {
                        GameObject gameObject = new GameObject(typeof(T).ToString(), typeof(T));
                        instance = gameObject.GetComponent<T>();
                        if (instance == null)
                        {
                            Debug.LogError(typeof(T).ToString() + " : Failed to get component <T> from new object");
                        }
                    }
                }
            }
            return instance;
        }
    }
    [SerializeField] private int Instance_ID;

    private void Awake()
    {
        if(instance == null)
        {
            Instance_ID = GetInstanceID();
            Debug.Log("Awake singleton class <" + this.name + ">, Instance ID : " + Instance_ID);
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        if (instance != this)
        {
            Debug.LogWarning(this.name + " : There are 2 same singleton class. Force delete this one.");
            DestroyImmediate(this.gameObject);
        }
    }
    protected virtual void OnDestroy()
    {
        Debug.Log("Destroyed singleton class <" + this.name + ">, Instance ID :" + Instance_ID);
        instance = null;
    }
    protected virtual void OnApplicationQuit()
    {
        DestroyImmediate(base.gameObject);
        instance = null;
    }
}
