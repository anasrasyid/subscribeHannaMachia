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

    public CharacterState GetState() {
        return this.state;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (this.state == CharacterState.bomb) {
            ICharacterStateAble other = collision.gameObject.GetComponent<ICharacterStateAble>();
            Debug.Log(other.GetState());
            // Change other State and disable this game object
            other.ChangeStateToBomber();
            this.ChangeStateToNormal();
        }
    }


}
