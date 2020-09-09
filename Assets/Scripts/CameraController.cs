using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the camera movement and rotation independently of the rest of the game logics.
/// Player can rotate the view by click and drag.
/// </summary>
public class CameraController : MonoBehaviour
{
    public Transform followTransform;
    public float movementSpeed;

    private Vector3 _rotateStartPosition;
    private Vector3 _rotateCurrentPosition;

    private Vector3 _newPosition;
    private Quaternion _newRotation;

    void Start()
    {
        _newPosition = transform.position;
        _newRotation = transform.rotation;
    }

    void Update()
    {
        if (followTransform != null)
        {
            MoveToFollowTransform();
        }

        _newPosition = transform.position;

        HandleRotation();
        ApplyMovement();
    }

    void HandleRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            // Prevents rotation if interacting with UI
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            _rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = _rotateStartPosition - _rotateCurrentPosition;

            _rotateStartPosition = _rotateCurrentPosition;

            _newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }

    void MoveToFollowTransform()
    {
        transform.position = Vector3.Lerp(transform.position, followTransform.position, Time.deltaTime * movementSpeed);
    } 

    void ApplyMovement()
    {
        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * movementSpeed);
    }
}
