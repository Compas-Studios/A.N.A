using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class ObjectPoolItem //pequeñaEstructura para los items
{
    public GameObject objeto;
    public int cantidad;
    public bool debeExpandir;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;

    void Awake() {
        if (SharedInstance == null)
            SharedInstance = this;
        else
            Destroy(gameObject);
    }

    void Start() {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool) {
            for (int i = 0; i < item.cantidad; i++) {
                GameObject obj = (GameObject)Instantiate(item.objeto);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag) {
        for (int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag) {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool) {
            if (item.objeto.tag == tag) {
                if (item.debeExpandir) {
                    GameObject obj = (GameObject)Instantiate(item.objeto);
                    obj.SetActive(true);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

}
