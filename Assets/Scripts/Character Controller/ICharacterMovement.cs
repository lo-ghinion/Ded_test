using UnityEngine;

public interface ICharacterMovement
{
    void Move(Vector3 direction, float speed);
}

public interface ICharacterActions
{
    void Jump();
    void Crouch();
    void Sprint();
}
