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

    [SerializeField] private BombBehavior bombBehavior;

    void Start()
    {
        movement = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == CharacterState.death)
        {
            movement.AnimateDeath(true);
            return;
        }
        // Get Input and Move Player
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        movement.MoveToPoint(inputX, inputY, speed, state);

        bombBehavior.Run(-Time.deltaTime, ref state);
    }

    public CharacterState GetState() {
        return this.state;
    }

    public void ChangeStateToBomber()
    {
        bombBehavior.Active(GameManager.Manager.BombExplode, ref state, GameManager.Manager.delayTouch);
    }

    public void ChangeStateToNormal()
    {
        bombBehavior.Deactive(ref state, GameManager.Manager.delayTouch);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ICharacterStateAble otherState = collision.gameObject.GetComponent<ICharacterStateAble>();
        if (otherState != null && bombBehavior.isCanTouch)
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
