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

    [SerializeField] private PhotonView photonView;
    [SerializeField] private BombBehavior bombBehavior;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == CharacterState.death)
        {
            movement.AnimateDeathOnline();
            return;
        }
        if (photonView.isMine)
        {
            // Get Input and Move Player
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            movement.MoveToPoint(inputX, inputY, speed, state);
        }
        photonView.RPC("UpdateTimer", PhotonTargets.AllBuffered, -Time.deltaTime);
    }

    [PunRPC]
    public void UpdateTimer(float time)
    {
        bombBehavior.Run(time, ref state);
    }

    public void ChangeStateToBomber()
    {
        photonView.RPC("ChangeStateToBomberOnline", PhotonTargets.AllBuffered);
    }
    
    public void ChangeStateToNormal()
    {
        photonView.RPC("ChangeStateToNormalOnline", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    public void ChangeStateToBomberOnline()
    {
        bombBehavior.Active(GameManager.Manager.BombExplode, ref state, 
            GameManager.Manager.delayTouch);
    }

    [PunRPC]
    public void ChangeStateToNormalOnline()
    {
        bombBehavior.Deactive(ref state, GameManager.Manager.delayTouch);
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

    public CharacterState GetState()
    {
        return this.state;
    }
}
