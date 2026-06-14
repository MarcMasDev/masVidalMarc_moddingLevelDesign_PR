using UnityEngine;

public class CameraFocusController : MonoBehaviour
{
    public static CameraFocusController Instance;

    [SerializeField] private Transform[] focusPoints;
    [SerializeField] private float rotationSpeed = 3f;

    private Transform currentFocus;
    private bool disableComponent = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SetFocus(int index)
    {
        disableComponent = index == -1;
        if (index < 0 || index >= focusPoints.Length)
            return;

        currentFocus = focusPoints[index];
    }

    private void LateUpdate()
    {
        if (currentFocus == null || disableComponent)
            return;

        Vector3 camPos = transform.position;

        Vector3 targetPos = new Vector3(
            currentFocus.position.x,
            currentFocus.position.y,
            currentFocus.position.z);

        Vector3 dir = targetPos - camPos;

        if (dir.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime);
    }
}
