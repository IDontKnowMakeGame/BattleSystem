using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class ResourceManagers : IManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            //GameObject go = Managers
            //GameObject go = .GetOriginal(name);
            GameObject go = (GameManagement.Instance.GetManager<PoolManager>() as PoolManager).GetOriginal(name);
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }
        if (original.GetComponent<Poolable>() != null)
            return (GameManagement.Instance.GetManager<PoolManager>() as PoolManager).Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            (GameManagement.Instance.GetManager<PoolManager>() as PoolManager).Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
