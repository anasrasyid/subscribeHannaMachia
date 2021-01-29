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

    private void OnCollisionEnter2D(Collision2D other)
    {
        Bomber bomber = other.gameObject.GetComponent<Bomber>();
        if (bomber)
        {
            this.ChangeStateToBomber();
        } else {
            ICharacterStateAble state = other.gameObject.GetComponent<ICharacterStateAble>();
            if (state != null) {
                if (state.GetState() == CharacterState.bomb) {
                    this.ChangeStateToBomber();
                } else {
                    if (this.state == CharacterState.bomb) {
                        this.ChangeStateToNormal();
                    }
                }
            }
        }

        Debug.Log(this.state);
    }

    public CharacterState GetState() {
        return this.state;
    }
}
