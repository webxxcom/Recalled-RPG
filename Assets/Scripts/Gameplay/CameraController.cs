using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject _following;

    void Update()
    {
        if (_following && _following.activeInHierarchy)
        {
            transform.position = new(
            _following.transform.position.x,
            _following.transform.position.y,
            transform.position.z);
        }
    }
}
