using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private ICharacterMovement movement;
    private ICharacterActions actions;
    private CameraController cameraController;
    private CharacterController characterController;
    private Animator animator;

    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float mouseSensitivity = 2f;
    public float maxVerticalAngle = 60f;

    private float zoomSprintMultiplier = 1.5f;
    private bool isZoomingOut = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        cameraController = FindObjectOfType<CameraController>();

        movement = new CharacterMovement(characterController);
        actions = new CharacterActions(characterController);

        if (cameraController == null)
        {
            Debug.LogError("CameraController!?");
        }
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Verific?m dac? anima?ia curent? este "Run"
        bool isRunning = animator.GetCurrentAnimatorStateInfo(0).IsName("Run");

        // Permitem sprint doar dac? personajul alearg? deja
        bool isSprinting = isRunning && Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        if (isZoomingOut)
        {
            currentSpeed *= zoomSprintMultiplier;
        }

        float rotationSpeed = 100f;
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);
        }

        // Blocare mi?care înapoi
        Vector3 direction = vertical > 0 ? transform.forward * vertical : Vector3.zero;
        movement.Move(direction, currentSpeed);

        animator.SetFloat("Speed", vertical > 0 ? vertical * currentSpeed : 0);
        animator.SetBool("IsGrounded", characterController.isGrounded);

        if (Mathf.Abs(vertical) < 0.1f && animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            animator.SetBool("IsStopping", true);
        }
        else
        {
            animator.SetBool("IsStopping", false);
        }

        if (Input.GetKeyDown(KeyCode.Space)) actions.Jump();
        if (Input.GetKeyDown(KeyCode.LeftControl)) actions.Crouch();

        // Sprint ?i ZoomOut sunt permise doar dac? personajul este în anima?ia "Run"
        if (isRunning && Input.GetKey(KeyCode.LeftShift))
        {
            if (!isZoomingOut)
            {
                cameraController.ZoomOutEffect();
            }
        }
        else if (isZoomingOut)
        {
            cameraController.ZoomInEffect();
        }

        if (isSprinting) actions.Sprint();
    }


    public void SetZoomSprintMultiplier(bool isZoomingOut)
    {
        this.isZoomingOut = isZoomingOut;

        animator.speed = isZoomingOut ? 1.3f : 1.0f;
    }
}
