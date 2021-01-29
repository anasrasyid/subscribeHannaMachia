using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour
{
    private Animator animator;

    public CharacterSkin skin = default;

    void Awake()
    {
        if(skin)
            ChangeAnimator(skin.runtimeAnimator);
        else
            animator = GetComponent<Animator>();
    }

    public void ChangeAnimator(RuntimeAnimatorController runtimeAnimator)
    {
        GetComponent<Animator>().runtimeAnimatorController = runtimeAnimator;
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
