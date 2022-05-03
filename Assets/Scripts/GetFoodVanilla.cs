using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFoodVanilla : MonoBehaviour
{
    public int speed = 6;

    private bool looking = true;
    private Vector3 foodPosition;

    // Update is called once per frame
    void Update()
    {
        // find food objects
        GameObject[] foodObjects =  GameObject.FindGameObjectsWithTag("food");

        // if there are food objects to chase
        if (foodObjects.Length > 0) {
            PlayerPrefs.SetInt("needFood", 0);
            if (looking) {
                GameObject foodObj = foodObjects[Random.Range(0, foodObjects.Length)];
                
                // set food position
                foodPosition = foodObj.GetComponent<Transform>().position;

                // stop looking for object
                looking = false;

            }

            else {
                // move towards food target
                transform.position = Vector3.MoveTowards(transform.position, foodPosition, speed * Time.deltaTime);
            }
        }

        else {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            PlayerPrefs.SetInt("needFood", 1);
        }

        // if arrived at food, but food is not there anymore
        if (transform.position == foodPosition && !looking) {
            looking = true;
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "food" && !looking) {

            // increase global satiation
            int num_ants =  GameObject.FindGameObjectsWithTag("ant").Length;
            float currSatiation = PlayerPrefs.GetFloat("satiation");
            PlayerPrefs.SetFloat("satiation", currSatiation + 1 - 0.3429f * 0.75f * num_ants);

            // look for other food target
            looking = true;
        }
    }
}
