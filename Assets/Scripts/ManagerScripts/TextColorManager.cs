using TMPro;
using UnityEngine;

public class TextColorManager : MonoBehaviour
{
    public TextMeshProUGUI sprintText;
    public TextMeshProUGUI rotateText;
    public CameraController cameraController;

    private void Update()
    {
        if (cameraController == null || sprintText == null || rotateText == null)
        {
            Debug.LogWarning("CameraController sau TMPro!?");
            return;
        }

        bool isZoomingOut = cameraController.IsZooming;
        bool isRotatingAroundCharacter = cameraController.IsRotating;

        if (isRotatingAroundCharacter)
        {
            rotateText.color = Color.green;
        }
        else if (isZoomingOut)
        {
            rotateText.color = Color.red;
        }
        else
        {
            rotateText.color = Color.white;
        }

        sprintText.color = isZoomingOut ? Color.green : Color.white;
    }
}
