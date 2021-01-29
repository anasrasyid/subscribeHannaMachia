using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterMovement))]
public class Computer : MonoBehaviour, ICharacterStateAble
{

    [SerializeField]
    private Transform target = null;
    [SerializeField]
    private string[] targetTags;


    [SerializeField]
    private float speed = 3;

    [SerializeField]
    private CharacterState state = CharacterState.normal;

    [SerializeField]
    private float delayTime = 0;


    private CharacterMovement movement;
    private NavMeshAgent agent;

    void Start()
    {
        movement = GetComponent<CharacterMovement>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;

    }

    private void FixedUpdate()
    {
        if (state == CharacterState.bomb)
        {
            Invoke("FindShortestTarget", delayTime);
        }
        else
        {

        }
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
        foreach (string tag in targetTags)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
            {
                float newDist = Vector3.Distance(transform.position, obj.transform.position);
                if (currentTarget)
                {
                    // Compare current minimun distance with new distance
                    if (newDist < dist)
                    {
                        currentTarget = obj.transform;
                        dist = newDist;
                    }

                }
                else if (newDist > 0)
                {
                    currentTarget = obj.transform;
                    dist = newDist;
                }
            }
        }

        target = currentTarget;
        if (agent)
        {
            agent.SetDestination(target.position);
        }
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        Bomber bomber = other.gameObject.GetComponent<Bomber>();
        if (bomber)
        {
            this.ChangeStateToBomber();
        }
        else
        {
            ICharacterStateAble state = other.gameObject.GetComponent<ICharacterStateAble>();
            if (state != null)
            {
                if (state.GetState() == CharacterState.bomb)
                {
                    this.ChangeStateToBomber();
                }
                else
                {
                    if (this.state == CharacterState.bomb)
                    {
                        this.ChangeStateToNormal();
                    }
                }
            }
        }

        Debug.Log("computer:" + this.state);
    }



    public void ChangeStateToBomber()
    {
        state = CharacterState.bomb;

        // Do Some Behaviuor
    }

    public void ChangeStateToNormal()
    {
        state = CharacterState.normal;

        // Do Some Behaviuor
    }

    public CharacterState GetState()
    {
        return this.state;
    }
}
