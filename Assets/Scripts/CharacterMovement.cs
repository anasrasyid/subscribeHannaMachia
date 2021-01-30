using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private CharacterSkin skin = default;

    void Awake()
    {
        if(skin)
            ChangeAnimator(skin);
        else
            animator = GetComponent<Animator>();
    }

    public void ChangeAnimator(CharacterSkin characterSkin)
    {
        skin = characterSkin;
        GetComponent<Animator>().runtimeAnimatorController = skin.runtimeAnimator;
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimator(int id)
    {
        skin = PUNController.controller.playerSkins[id];
        GetComponent<Animator>().runtimeAnimatorController = skin.runtimeAnimator;
        animator = GetComponent<Animator>();
    }

    public void MoveToPoint(float x, float y, float speed, CharacterState state)
    {
        Vector3 move = new Vector3(x, y, 0);

        AnimateMove(move, state);

        transform.Translate(move * speed * Time.deltaTime);
    }

    public void AnimateMove(Vector3 velocity, CharacterState state = CharacterState.normal)
    {
        // Set Variabel in Animator
        animator.SetFloat("Vertical", velocity.y);
        animator.SetFloat("Horizontal", velocity.x);
        animator.SetFloat("Magnitude", velocity.magnitude);
        animator.SetInteger("State", (int)state);
    }
}
