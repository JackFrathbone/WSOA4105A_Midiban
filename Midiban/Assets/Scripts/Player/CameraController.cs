using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float smoother;
    [SerializeField] Transform target;

    [Header("References")]
    private Camera _cam;
    private Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        _cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        if (target)
        {
            Vector3 point = _cam.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, smoother);
        }
    }
}
