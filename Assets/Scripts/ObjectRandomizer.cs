using UnityEngine;
using System.Collections.Generic;

public class ObjectRandomizer : MonoBehaviour
{
    public List<GameObject> objectsToRandomize;

    void Start()
    {
        ShuffleObjects();
    }

    public void ShuffleObjects()
    {
        int objectCount = objectsToRandomize.Count;
        for (int i = 0; i < objectCount - 1; i++)
        {
            int randomIndex = Random.Range(i, objectCount);
            objectsToRandomize[randomIndex].transform.SetSiblingIndex(i);
        }
    }
}
