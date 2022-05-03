using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 8;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Time.deltaTime * speed;
    }

    public void OnCollisionEnter(Collision collider) {
        if (collider.gameObject.tag == "ant") {
            Destroy(collider.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "food") {
            float currSatiation = PlayerPrefs.GetFloat("satiation");
            PlayerPrefs.SetFloat("satiation", currSatiation + 1);
            Destroy(other.gameObject);
        }
    }
}
