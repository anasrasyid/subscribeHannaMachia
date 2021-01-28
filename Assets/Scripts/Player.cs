using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    normal = 0,
    bomb = 1,
    death = 2
};

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private CharacterState state = CharacterState.normal;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(inputX, inputY, 0);

        animator.SetFloat("Vertical", move.y);
        animator.SetFloat("Horizontal", move.x);
        animator.SetFloat("Magnitude", move.magnitude);
        animator.SetInteger("State", (int)state);

        transform.Translate(move * speed * Time.deltaTime);
    }
}
