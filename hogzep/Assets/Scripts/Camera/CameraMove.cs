using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    RaycastHit hit;
    RaycastHit hitUpdate;
    Ray ray;
    Ray rayUpdate;
    Vector3 lastMousePos = Vector3.zero;
    Vector3 mouseDelta = Vector3.zero;
    Vector3 targPosition;
    Vector3 posDifference;
    public float cameraEaseTime = 0.5f;
    public float WASD_smooth = 0.5f;
    public float WASD_speed = 1.7f;
    public float mouseScreenEdgeSmooth = 0.5f;
    public float mouseScreenEdgeSpeed = 2.6f;
    public bool isDragging = false;
    public bool isEdging = false;
    public bool isKeying = false;
    public LayerMask RaycastLayer;

    //Debug
    public Vector3 cubeGizmoSize = new Vector3(0.2f, 0.2f, 0.2f);
    public float SphereGizmoSize = 0.05f;
    public float debugRayDuration = 1f;
    public bool enableRaycastLines;
    public bool enableDebugMode;

    void Update()
    {
        CameraDrag();
        CameraScreenEdge();
        CameraWASD();
        MouseTracked();
    }

    void CameraDrag()
    {
        if (!isEdging && !isKeying)
        {
            if (Input.GetButtonDown("Camera Drag"))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, RaycastLayer))
                {
                    if (enableRaycastLines) { Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.cyan, debugRayDuration); }
                    isDragging = true;
                }
            }

            else if (Input.GetButtonUp("Camera Drag"))
            {
                isDragging = false;
            }

            if (isDragging && mouseDelta != Vector3.zero)
            {
                rayUpdate = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(rayUpdate, out hitUpdate, Mathf.Infinity, RaycastLayer))
                {
                    posDifference = hit.point - hitUpdate.point;
                    targPosition = new Vector3(this.transform.position.x + posDifference.x, this.transform.position.y, this.transform.position.z + posDifference.z);
                    this.transform.DOMove(new Vector3(targPosition.x, targPosition.y, targPosition.z), cameraEaseTime, false).SetEase(Ease.OutExpo);
                    if (enableRaycastLines) { Debug.DrawRay(rayUpdate.origin, rayUpdate.direction * hitUpdate.distance, Color.blue); }
                }
            }
        }
    }

    void CameraWASD()
    {
        if (!isDragging && !isEdging)
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                this.transform.DOMoveZ(this.transform.position.z + Input.GetAxisRaw("Vertical") * WASD_speed, WASD_smooth, false).SetEase(Ease.OutExpo);
                isKeying = true;
            }
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                this.transform.DOMoveX(this.transform.position.x - Input.GetAxisRaw("Horizontal") * WASD_speed, WASD_smooth, false).SetEase(Ease.OutExpo);
                isKeying = true;
            }
            else if (Input.GetAxisRaw("Vertical") == 0)
            {
                isKeying = false;
            }
        }
    }

    void CameraScreenEdge()
    {
        if (!isDragging && !isKeying)
        {
            if (Input.mousePosition.x >= Screen.width - 5f)
            {
                this.transform.DOMoveX(this.transform.position.x + 1 * mouseScreenEdgeSpeed, mouseScreenEdgeSmooth, false).SetEase(Ease.OutExpo);
                isEdging = true;
            }
            else if (Input.mousePosition.x <= Screen.width - Screen.width + 5f)
            {
                this.transform.DOMoveX(this.transform.position.x - 1 * mouseScreenEdgeSpeed, mouseScreenEdgeSmooth, false).SetEase(Ease.OutExpo);
                isEdging = true;
            }
            else if (Input.mousePosition.x < Screen.width - 5f || Input.mousePosition.x > Screen.width - Screen.width + 5f)
            {
                isEdging = false;
            }
            if (Input.mousePosition.y >= Screen.height - 5f)
            {
                this.transform.DOMoveZ(this.transform.position.z + 1 * mouseScreenEdgeSpeed, mouseScreenEdgeSmooth, false).SetEase(Ease.OutExpo);
                isEdging = true;
            }
            else if (Input.mousePosition.y <= Screen.height - Screen.height + 5f)
            {
                this.transform.DOMoveZ(this.transform.position.z - 1 * mouseScreenEdgeSpeed, mouseScreenEdgeSmooth, false).SetEase(Ease.OutExpo);
                isEdging = true;
            }
        }
    }

    void MouseTracked()
    {
        mouseDelta = Input.mousePosition - lastMousePos;
        lastMousePos = Input.mousePosition;
    }

    void OnDrawGizmos()
    {
        if (enableDebugMode)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hit.point, SphereGizmoSize);
            Gizmos.DrawWireCube(hit.point, cubeGizmoSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(hitUpdate.point, SphereGizmoSize);
            Gizmos.DrawWireCube(hitUpdate.point, cubeGizmoSize);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(hit.point + Vector3.up * 0.1f, hitUpdate.point + Vector3.up * 0.1f);
        }
    }
}
