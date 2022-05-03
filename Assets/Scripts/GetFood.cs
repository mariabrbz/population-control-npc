using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GetFood : Agent
{
    public Transform foodTransform;
    public float speed = 1f;
    public float[] foodX = new float[2];
    public float[] foodY = new float[2];
    public float[] agentX = new float[2];
    public float[] agentY = new float[2];
    public MeshRenderer floorMesh;
    public Material normalMaterial;
    public Material loseMaterial;
    public Material winMaterial;

    public override void OnEpisodeBegin() {
        // spawn at random localPosition
        transform.localPosition = new Vector3(Random.Range(agentX[0], agentX[1]), 1, Random.Range(agentY[0], agentY[1]));
        foodTransform.localPosition = new Vector3(Random.Range(foodX[0], foodX[1]), 0, Random.Range(foodY[0], foodY[1]));
    }

    public override void CollectObservations(VectorSensor sensor) {
        // agent localPosition
        sensor.AddObservation(transform.localPosition);

        // food localPosition
        sensor.AddObservation(foodTransform.localPosition);
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
            AddReward(2f);
            floorMesh.material = winMaterial;
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "wall") {
            AddReward(-0.1f);
            // floorMesh.material = loseMaterial;
            // EndEpisode();
        }
        
        if (collision.gameObject.tag == "out_wall") {
            AddReward(-1f);
            floorMesh.material = loseMaterial;
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxisRaw("Horizontal");
        actions[1] = Input.GetAxisRaw("Vertical");
    }

}
