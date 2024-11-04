using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 100;

    private List<GameObject> pool;
    private void Awake()
    {
        pool = new List<GameObject>();
        //メモリ空間を確保
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        //非表示のオブジェクトを周回する
        foreach (GameObject obj in pool)
        {
            if (obj.activeInHierarchy == false)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        //プールしたいが容量いっぱいなら
        GameObject newObj = Instantiate(prefab);
        pool.Add(newObj);
        return newObj;
    }

    public GameObject GetObject(Vector3 pos, Quaternion Rot)
    {
        //非表示のオブジェクトを周回する
        foreach (GameObject obj in pool)
        {
            if (obj.activeInHierarchy == false)
            {
                obj.transform.position = pos;
                obj.transform.rotation = Rot;
                obj.SetActive(true);

                return obj;
            }
        }

        //プールしたいが容量いっぱいなら
        GameObject newObj = Instantiate(prefab);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {//見えないようにする。
        obj.SetActive (false);
    }
}
