using UnityEngine;

public class CamFocusPoint : MonoBehaviour
{
    public int index;
    public bool onStaySet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CameraFocusController.Instance?.SetFocus(index);
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || !onStaySet)
            return;

        CameraFocusController.Instance?.SetFocus(index);
    }
}
