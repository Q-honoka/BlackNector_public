using UnityEngine;

/*
 * ƒJƒƒ‰‚Ì•ûŒü‚ÉŒü‚©‚¹‚é
 */

public class Billboard : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Quaternion rotation = transform.rotation;
        rotation = _camera.transform.rotation;
        transform.rotation = rotation;
    }
}
