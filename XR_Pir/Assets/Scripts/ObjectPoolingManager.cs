using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();

    private const string CLONE_KEYWORD = "(Clone)";

    public static GameObject SpawnObject(GameObject objToSpawn, Vector3 spawnPos, Quaternion spawnRot)
    {
        //look through pools and check if obj is the one we want to spawn
        PooledObjectInfo pool = null;
        foreach(PooledObjectInfo p in objectPools)
        {
            if(p.LookupString == objToSpawn.name)
            {
                pool = p;
                break;
            }
        }

        //if no pool, create one
        if(pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objToSpawn.name };
            objectPools.Add(pool);
        }

        //check for inactive obj in the pool
        GameObject spawnableObj = null;
        foreach(GameObject obj in pool.InactiveObjects)
        {
            if(obj != null)
            {
                spawnableObj = obj;
                break;
            }
        }

        if(spawnableObj == null)
        {
            //if no inactive objects, create one
            spawnableObj = Instantiate(objToSpawn, spawnPos, spawnRot);
        }
        else
        {
            //if there is an inactive obj, use it
            spawnableObj.transform.position = spawnPos;
            spawnableObj.transform.rotation = spawnRot;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnToPool(GameObject obj)
    {
        string goName = obj.name.Replace(CLONE_KEYWORD, string.Empty);

        PooledObjectInfo pool = null;
        foreach (PooledObjectInfo p in objectPools)
        {
            if (p.LookupString == goName)
            {
                pool = p;
                break;
            }
        }

        if(pool == null)
        {
            Debug.LogWarning($"Trying to use non-pooled object: {obj.name}");
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }

    }
}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
