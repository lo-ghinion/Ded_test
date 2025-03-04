using UnityEngine;

public class CharacterActions : ICharacterActions
{
    private readonly CharacterController characterController;
    private float crouchHeight = 1.0f;
    private float originalHeight;
    private Animator animator;

    public CharacterActions(CharacterController characterController)
    {
        this.characterController = characterController;
        originalHeight = characterController.height;
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            animator.SetTrigger("Jump");
            Vector3 jumpVelocity = Vector3.up * 5f;
            characterController.Move(jumpVelocity * Time.deltaTime);
        }
    }


    public void Crouch()
    {
        characterController.height = Mathf.Approximately(characterController.height, crouchHeight)
            ? originalHeight
            : crouchHeight;
    }

    public void Sprint()
    {
        //Debug.Log("Sprint activated");
    }
}
