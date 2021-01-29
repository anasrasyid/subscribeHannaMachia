using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bomber : MonoBehaviour
{
    [SerializeField]
    private Transform target = null;
    [SerializeField]
    private string[] targetTags;

    [SerializeField]
    private float delayTime;

    [SerializeField]
    private float speed;

    private NavMeshAgent agent;
    private CharacterMovement movement;

    void Start()
    {
        movement = GetComponent<CharacterMovement>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
    }

    private void FixedUpdate() {
        Invoke("FindShortestTarget", delayTime);
    }

    void Update()
    {
        if (target && agent)
        {
            // Update Target Position and Animate Move
            agent.SetDestination(target.position);
            movement.AnimateMove(agent.velocity);
        }
    }

    void FindShortestTarget()
    {
        Transform currentTarget = null;
        float dist = 0;

        // Find Shortest Target 
        foreach(string tag in targetTags)
        {
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag(tag))
            {
                float newDist = Vector3.Distance(transform.position, obj.transform.position);
                if (currentTarget)
                {
                    // Compare current minimun distance with new distance
                    if(newDist < dist)
                    {
                        currentTarget = obj.transform;
                        dist = newDist;
                    }
                }
                else
                {
                    currentTarget = obj.transform;
                    dist = newDist;
                }
            }
        }

        target = currentTarget;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (string tag in targetTags)
        {
            // Check if other collision in bomber targets tag
            if (collision.gameObject.CompareTag(tag))
            {
                ICharacterStateAble other = collision.gameObject.GetComponent<ICharacterStateAble>();
                Debug.Log(tag);
                // Change other State and disable this game object
                other.ChangeStateToBomber();
                gameObject.SetActive(false);
                agent = null;
                break;
            }
        }
    }
}
