using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFood : MonoBehaviour
{
    public int foodAmount = 20;
    public GameObject foodPrefab;
    public int ants = 5;
    public GameObject antPrefab;
    public int targetPopulation = 5;
    
    private bool createFood = true;
    private bool destroyFood = true;

    // Start is called before the first frame update
    void Start()
    {
        // create food objects
        for(int i = 0; i < foodAmount; i += 1) {
            GameObject food = Instantiate(foodPrefab, new Vector3(Random.Range(-42f, 43f), 1, Random.Range(-46f, 46)), Quaternion.identity);
        }

        for(int i = 0; i < ants; i += 1) {
            GameObject ant = Instantiate(antPrefab, new Vector3(Random.Range(-42f, 43f), 1, Random.Range(-46f, 46)), Quaternion.identity);
        }

        PlayerPrefs.SetInt("needFood", 0);
    }

    // Update is called once per frame
    void Update()
    {
        float currSatiation = PlayerPrefs.GetFloat("satiation");
        GameObject[] antObjects =  GameObject.FindGameObjectsWithTag("ant");
        int numAnts = antObjects.Length;

        if (numAnts < targetPopulation / 1.5f && createFood) {
            createFood = false;
            StartCoroutine(CreateFood(numAnts * 3, 10));
        }

        else if (numAnts < targetPopulation && createFood) {
            createFood = false;
            StartCoroutine(CreateFood(numAnts, 10));
        }

        else if (numAnts > 1.5 * targetPopulation && destroyFood) {
            createFood = false;
            StartCoroutine(DestroyFood(10));
        }
    }

    public IEnumerator CreateFood(int amount, int wait)
    {   
        yield return new WaitForSeconds(wait);
        for (int i = 0; i < amount; i += 1) {
            GameObject food = Instantiate(foodPrefab, new Vector3(Random.Range(-42f, 43f), 1, Random.Range(-46f, 46)), Quaternion.identity);
        }
        createFood = true;
    }

    public IEnumerator DestroyFood(int wait)
    {   
        yield return new WaitForSeconds(wait);
        GameObject[] foodObjects =  GameObject.FindGameObjectsWithTag("food");
        int numFood = foodObjects.Length;

        for (int i = 0; i < numFood / 4; i += 1) {

            Destroy(foodObjects[Random.Range(0, numFood)]);
        }
        createFood = true;
    }
}
