using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void MoveToPoint(float x, float y, float speed, CharacterState state)
    {
        Vector3 move = new Vector3(x, y, 0);

        animator.SetFloat("Vertical", move.y);
        animator.SetFloat("Horizontal", move.x);
        animator.SetFloat("Magnitude", move.magnitude);
        animator.SetInteger("State", (int)state);

        transform.Translate(move * speed * Time.deltaTime);
    }
}
