using UnityEngine;

public class CameraBase : MonoBehaviour
{
    protected Transform target;
    protected bool isRotatingAroundCharacter = false;
    protected float zoomLevel = 0f;

    public float rotationSpeed = 50f;
    public float zoomOutDistance = 5f;
    public float zoomInSpeed = 2f;
    public float zoomOutSpeed = 1f;
    public float breathingIntensity = 0.1f;
    public float breathingSpeed = 1f;
    public float shakeIntensity = 0.05f;

    public float baseFollowSpeed = 5f;



    protected virtual void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("No tag 'Player'.");
            }
        }
    }


    protected virtual void Update()
    {

    }
}
