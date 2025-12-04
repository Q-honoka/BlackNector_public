using UnityEngine;

public class Image_CameraMovement : MonoBehaviour
{
    [SerializeField] private float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPos = new Vector3(18f,0,-10);
    private Vector3 resetPos = new Vector3(0,0,-10);
    private bool isMoving;
    private Vector3 pos;

    private void Start()
    {
        /* カメラの位置をリセット */
        this.transform.position = resetPos;
        isMoving = false;
    }

    void Update()
    {
        pos = transform.position;

        if (isMoving)
        {
            transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime);


            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;
                velocity = Vector3.zero;
            }
        }
    }

    public void MoveTo()
    {
        isMoving = true;
    }

    public void ChameraPosReset()
    {
        this.transform.position = resetPos;
        isMoving = false;
    }
}
