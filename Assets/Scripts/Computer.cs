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

    private CharacterMovement movement;
    private NavMeshAgent agent;
    private GameObject currBomber = null;

    [SerializeField] private bool isCanTouch = true;
    [SerializeField] private float delay = 0.5f;

    void Start()
    {
        movement = GetComponent<CharacterMovement>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;

        GetBomber();
    }

    private void Update()
    {
        if (state == CharacterState.bomb)
        {
            FindShortestTarget();
            agent.SetDestination(target.position);
        }
        else if (currBomber)
        {
            RaycastHit2D[] hits;

            Vector3 dir = Vector3.zero;
            // Vector3 dest = this.transform.position + dir.normalized;
            Vector3 dest = this.transform.position;
            float randValue = Random.value;
            if (randValue < 0.6f) {
                dir = (this.transform.position - currBomber.transform.position);
            } else {
                dir.x += -currBomber.transform.position.x;
                dir.y += -currBomber.transform.position.y;
            }
          
            foreach (string tag in targetTags)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
                {
                    if (obj.transform.position != currBomber.transform.position) {
                        float dist = Vector3.Distance(this.transform.position, obj.transform.position);
                        float distToBomb = Vector3.Distance(currBomber.transform.position, obj.transform.position);
                        if (dist < 5 && dist < distToBomb) {
                            dir += (transform.position - obj.transform.position);
                        }
                    }
                }
            }

            dest = dir;
            Debug.DrawLine(transform.position, dest, Color.blue, 2.5f);
            Debug.DrawLine(transform.position, currBomber.transform.position, Color.red, 2.5f);
 
            agent.SetDestination(dest);
        }

        movement.AnimateMove(agent.velocity, state);
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
                    if (newDist < dist && newDist > 0)
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
    }

    void reset() {
        target = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ICharacterStateAble otherState = collision.gameObject.GetComponent<ICharacterStateAble>();
        if (otherState != null && isCanTouch)
        {
            if (otherState.GetState() == CharacterState.bomb)
            {
                this.ChangeStateToBomber();
                otherState.ChangeStateToNormal();
            }
            else if (this.state == CharacterState.bomb)
            {
                this.ChangeStateToNormal();
                otherState.ChangeStateToBomber();
            }
        }
    }

    IEnumerator InvicibleTouch()
    {
        isCanTouch = false;
        yield return new WaitForSeconds(delay);
        isCanTouch = true;
    }

    public void ChangeStateToBomber()
    {
        state = CharacterState.bomb;

        // Do Some Behaviuor
        StartCoroutine(InvicibleTouch());
    }

    public void ChangeStateToNormal()
    {
        state = CharacterState.normal;

        // Do Some Behaviuor
        StartCoroutine(InvicibleTouch());
    }

    public void GetBomber() {
        GameObject bomber = GameObject.FindWithTag("Bomb");
        if (bomber) {
            currBomber = bomber;
        } else {
            foreach (string tag in targetTags)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
                {
                    ICharacterStateAble state = obj.GetComponent<ICharacterStateAble>();
                    if (state.GetState() == CharacterState.bomb) {
                        currBomber = obj;
                    }
                }
            }

        }
    }

    public CharacterState GetState()
    {
        return this.state;
    }
}
