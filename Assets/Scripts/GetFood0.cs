using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GetFood0 : Agent
{
    public float speed = 1f;
    public float[] agentX = new float[2];
    public float[] agentY = new float[2];
    public GameObject foodPrefab;
    public Transform foodParent;

    public override void OnEpisodeBegin() {
        // spawn at random localPosition
        transform.localPosition = new Vector3(Random.Range(agentX[0], agentX[1]), 1, Random.Range(agentY[0], agentY[1]));

        // spawn food
        GameObject[] foodObjects =  GameObject.FindGameObjectsWithTag("food");
        foreach (GameObject foodObject in foodObjects) {
           Destroy(foodObject); 
        }
        for(int i = 0; i < 30; i += 1) {
            GameObject food = Instantiate(foodPrefab, new Vector3(Random.Range(-42f, 43f), 1, Random.Range(-46f, 46)), Quaternion.identity) as GameObject;
            food.transform.parent = foodParent;
        }
    }

    public override void CollectObservations(VectorSensor sensor) {
        // agent localPosition
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        // get move vector
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        // move agent
        transform.localPosition += new Vector3(moveX, 0, moveY) * Time.deltaTime * speed;
    
        // give negative reward to encourage fast learning
        AddReward(-1 / 1000);
    }

    // check collisions
    private void OnTriggerEnter(Collider other) {
        // if we hit food
        if (other.tag == "food") {
            AddReward(1f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "out_wall") {
            AddReward(-1f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxisRaw("Horizontal");
        actions[1] = Input.GetAxisRaw("Vertical");
    }

    public void Update() {
        int num_foods =  GameObject.FindGameObjectsWithTag("food").Length;
        if (num_foods == 0) {
            EndEpisode();
        }
    }
}
