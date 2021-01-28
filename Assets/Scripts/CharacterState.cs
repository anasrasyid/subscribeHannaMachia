public enum CharacterState
{
    normal = 0,
    bomb = 1,
    death = 2
};

public interface ICharacterStateAble
{
    void ChangeStateToBomber();
    void ChangeStateToNormal();
}