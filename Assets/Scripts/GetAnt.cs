using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAnt : MonoBehaviour
{
    public int speed = 6;

    private bool looking = true;
    private Transform ant;
    private Vector3 offset = new Vector3(2, 0, -7);
    private bool antSet = false;

    // Update is called once per frame
    void Update()
    {   
        // if (antSet) {
        //     Debug.Log(ant);
        //     Debug.Log(ant.gameObject);
        // }
        if (antSet && !ant) {
            looking = true;
        }

        // Debug.Log(ant.gameObject);

        // if there are ant objects to chase
        if (looking) {
            GetNewAnt();
        }

        else {
            // move towards ant target
            transform.position = ant.position + offset;
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "ant" && !looking) {
            // look for other ant target
            looking = true;
            Destroy(other);
            GetNewAnt();
        }
    }

    public void GetNewAnt() {
        GameObject[] antObjects = GameObject.FindGameObjectsWithTag("ant");
        if (antObjects.Length > 0) {
            antSet = true;
            GameObject antObj = antObjects[Random.Range(0, antObjects.Length)];
            
            // set ant transform
            ant = antObj.GetComponent<Transform>();

            looking = false;
        }
        else {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
