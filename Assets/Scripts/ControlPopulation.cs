using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPopulation : MonoBehaviour
{
    public float satiation = 70;
    public GameObject antPrefab;
    public GameObject satiationText;
    public GameObject populationText;
    public int targetPopulation = 5;

    private bool instantiated = false;
    private bool killed = false;
    private bool hunger = true;
    private bool instantiateAntBool = true;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("satiation", satiation);
    }

    // Update is called once per frame
    void Update()
    {

        // find ants
        GameObject[] antObjects =  GameObject.FindGameObjectsWithTag("ant");
        populationText.GetComponent<TMPro.TextMeshProUGUI>().text = "Ants: " + antObjects.Length.ToString();

        // get satiation
        float currSatiation = PlayerPrefs.GetFloat("satiation");
        satiationText.GetComponent<TMPro.TextMeshProUGUI>().text = "Satiation: " + currSatiation.ToString("00.0") + "%";
        
        // if colony is satiated, reproduce
        if (currSatiation >= 75 && !instantiated) {
            instantiated = true;
            GameObject newAnt = Instantiate(antPrefab, new Vector3(Random.Range(-42f, 43f), 1, Random.Range(-46f, 46)), Quaternion.identity);
            StartCoroutine(CreateAnt(30));
        }

        // if colony is hungry, kill random ant
        if (currSatiation < 25 && !killed) {
            killed = true;
            GameObject[] ants =  GameObject.FindGameObjectsWithTag("ant");
            if (ants.Length > 0) {
                Destroy(ants[Random.Range(0, ants.Length)]);
                StartCoroutine(DestroyAnt());
            }
        }

        if (PlayerPrefs.GetInt("needFood") == 1 && hunger) {
            hunger = false;
            PlayerPrefs.SetFloat("satiation", currSatiation - antObjects.Length * 0.3429f * 0.75f);
            StartCoroutine(ConsumeFood(5));
        }

        if (antObjects.Length == 0 && instantiateAntBool) {
            instantiateAntBool = false;
            GameObject newAnt = Instantiate(antPrefab, new Vector3(Random.Range(-42f, 43f), 1, Random.Range(-46f, 46)), Quaternion.identity);
            StartCoroutine(InstantiateAnt(5));
        }
    }

    public IEnumerator CreateAnt(int wait)
    {   
        yield return new WaitForSeconds(wait);
        instantiated = false;
    }

    public IEnumerator InstantiateAnt(int wait)
    {   
        yield return new WaitForSeconds(wait);
        instantiateAntBool = true;
    }

    public IEnumerator DestroyAnt()
    {   
        yield return new WaitForSeconds(30);
        killed = false;
    }

    public IEnumerator ConsumeFood(int wait) {
        yield return new WaitForSeconds(wait);
        hunger = true;
    }
}
