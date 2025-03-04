using System.Collections;
using UnityEngine;
using UnityEngine.VFX;


public class CameraController : CameraBase
{
    public VisualEffect zoomEffect;

    private Vector3 cameraOffset;
    public float followSpeed = 5f;
    private bool isZoomingOut = false;
    private bool isZooming = false;

    private float originalDistance = 4.5f;

    public KeyCode rotateCameraKey = KeyCode.C;

    public bool IsZooming => isZooming;
    public bool IsRotating => isRotatingAroundCharacter;

    protected override void Start()
    {
        base.Start();
        zoomEffect.Stop();
    }

    protected override void Update()
    {
        base.Update();

        if (!isRotatingAroundCharacter)
        {
            FollowPlayer();
        }

        if (Input.GetKeyDown(rotateCameraKey) && !isZooming)
        {
            ToggleCameraRotation();
        }
    }

    private void FollowPlayer()
    {
        if (target != null)
        {
            float distance = isZoomingOut ? originalDistance + zoomOutDistance : originalDistance;
            Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * 3f;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);
            transform.LookAt(target.position + Vector3.up * 0.8f);
        }
    }

    public void ZoomOutEffect()
    {
        if (!isZoomingOut)
        {
            StopAllCoroutines();
            StartCoroutine(ApplyZoomEffect(originalDistance + zoomOutDistance));
            isZoomingOut = true;

            if (zoomEffect != null)
            {
                zoomEffect.Play();
            }

            if (target.TryGetComponent(out CharacterManager characterManager))
            {
                characterManager.SetZoomSprintMultiplier(true);
            }
        }
    }

    public void ZoomInEffect()
    {
        if (isZoomingOut)
        {
            StopAllCoroutines();
            StartCoroutine(ApplyZoomEffect(originalDistance));
            isZoomingOut = false;
            isZooming = false;

            if (zoomEffect != null)
            {
                zoomEffect.Stop();
            }

            if (target.TryGetComponent(out CharacterManager characterManager))
            {
                characterManager.SetZoomSprintMultiplier(false);
            }
        }
    }

    private IEnumerator ApplyZoomEffect(float targetDistance)
    {
        isZooming = true;
        bool wasRotating = isRotatingAroundCharacter;
        isRotatingAroundCharacter = false;

        float zoomSpeed = 2f;
        float startDistance = cameraOffset.magnitude;

        while (Mathf.Abs(cameraOffset.magnitude - targetDistance) > 0.1f)
        {
            cameraOffset = cameraOffset.normalized * Mathf.Lerp(cameraOffset.magnitude, targetDistance, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        isZooming = false;
        isRotatingAroundCharacter = wasRotating;
    }

    private void ToggleCameraRotation()
    {
        if (!isZooming)
        {
            isRotatingAroundCharacter = !isRotatingAroundCharacter;

            if (isRotatingAroundCharacter)
            {
                StartCoroutine(RotateAroundCharacter());
            }
            else
            {
                StopAllCoroutines();
            }
        }
    }

    private IEnumerator RotateAroundCharacter()
    {
        if (target == null)
        {
            Debug.LogError("null");
            yield break;
        }

        while (isRotatingAroundCharacter)
        {
            transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
