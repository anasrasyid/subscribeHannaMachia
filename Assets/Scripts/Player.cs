using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class Player : MonoBehaviour, ICharacterStateAble
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private CharacterState state = CharacterState.normal;

    private CharacterMovement movement;

    [SerializeField] private bool isCanTouch = true;
    [SerializeField] private float delay = 0.5f;

    void Start()
    {
        movement = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get Input and Move Player
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        movement.MoveToPoint(inputX, inputY, speed, state);
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

    public CharacterState GetState() {
        return this.state;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ICharacterStateAble otherState = other.gameObject.GetComponent<ICharacterStateAble>();
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


}
