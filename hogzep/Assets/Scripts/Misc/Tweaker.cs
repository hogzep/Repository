using UnityEngine;

public class Tweaker : MonoBehaviour
{
    public int frameRateLimit = 120;
    public bool limitFrameRate;
    public bool hideCursor;
    public bool lockCursor;

    void Start()
    {
        if (limitFrameRate == true)
        {
            Application.targetFrameRate = frameRateLimit;
            Debug.Log("FPS limit is set to: " + frameRateLimit);
        }
        else { Debug.Log("FPS limit is disabled."); }

        if (hideCursor == true)
        {
            Cursor.visible = false;
        }

        if (lockCursor == true)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
