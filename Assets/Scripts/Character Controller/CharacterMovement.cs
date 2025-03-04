using UnityEngine;

public class CharacterMovement : ICharacterMovement
{
    private readonly CharacterController characterController;
    private float currentSpeed = 0f;
    private float stopFriction = 5f;

    public CharacterMovement(CharacterController characterController)
    {
        this.characterController = characterController;
    }

    public void Move(Vector3 direction, float targetSpeed)
    {
        if (direction.magnitude > 0)
        {
            currentSpeed = targetSpeed;
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * stopFriction);
        }

        // Aplic?m miscarea doar pe axa orizontal?
        Vector3 movement = direction.normalized * currentSpeed * Time.deltaTime;
        movement.y = characterController.isGrounded ? 0 : Physics.gravity.y * Time.deltaTime;
        characterController.Move(movement);
    }
}
