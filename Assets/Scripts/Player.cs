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
}
