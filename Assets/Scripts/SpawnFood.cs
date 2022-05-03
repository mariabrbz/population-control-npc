using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    public GameObject foodPrefab;
    public float[] y = new float[2];
    public float[] x = new float[2];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 100; i += 1) {
            GameObject food = Instantiate(foodPrefab, new Vector3(Random.Range(x[0], x[1]), 1, Random.Range(y[0], y[1])), Quaternion.identity) as GameObject;
            UnityEditor.Selection.activeObject = food;
        }
    }
}
