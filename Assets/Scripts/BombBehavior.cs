using System.Collections;
using UnityEngine;
using TMPro;

public class BombBehavior : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private TextMeshPro timeText = default;
    [SerializeField] public bool isCanTouch = true;

    public void Run(float timeAdd, ref CharacterState state)
    {
        if (state != CharacterState.bomb)
            return;
        time += timeAdd;
        timeText.text = Mathf.CeilToInt(time).ToString();
        if (time <= 0)
        {
            state = CharacterState.death;
        }
    }


    public void Active(float start, ref CharacterState state, float delay)
    {
        state = CharacterState.bomb;

        time = start;
        timeText.text = time.ToString();
        timeText.enabled = true;

        StartCoroutine(InvicibleTouch(delay));
    }

    public void Deactive(ref CharacterState state, float delay)
    {
        state = CharacterState.normal;

        timeText.enabled = false;

        StartCoroutine(InvicibleTouch(delay));
    }

    IEnumerator InvicibleTouch(float delay)
    {
        isCanTouch = false;
        yield return new WaitForSeconds(delay);
        isCanTouch = true;
    }
}
