using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayer : Photon.MonoBehaviour, ICharacterStateAble
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private CharacterState state = CharacterState.normal;

    private CharacterMovement movement;

    [SerializeField] private bool isCanTouch = true;
    [SerializeField] private float delay = 0.5f;

    [SerializeField] private PhotonView photonView;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            // Get Input and Move Player
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            movement.MoveToPoint(inputX, inputY, speed, state);
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
        int idChar = PhotonNetwork.player.ID;
        photonView.RPC("ChangeState", PhotonTargets.AllBuffered, CharacterState.bomb);
    }
    
    public void ChangeStateToNormal()
    {
        int idChar = PhotonNetwork.player.ID;
        photonView.RPC("ChangeState", PhotonTargets.AllBuffered, CharacterState.normal);
    }

    [PunRPC]
    public void ChangeState(CharacterState characterState)
    {
        state = characterState;
        movement.AnimateMove(Vector3.zero, state);

        // Do Some Behaviuor
        StartCoroutine(InvicibleTouch());
    }

    public void ChangeAnimator(int id)
    {
        photonView.RPC("ChangeAnimatorOnline", PhotonTargets.AllBuffered, id);
    }

    [PunRPC]
    public void ChangeAnimatorOnline(int id)
    {
        movement.ChangeAnimator(id);
    }

    private void OnCollisionEnter2D(Collision2D other)
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

    public CharacterState GetState()
    {
        return this.state;
    }
}
