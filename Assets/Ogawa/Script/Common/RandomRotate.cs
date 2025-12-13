using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    [SerializeField, Tooltip("óhÇÁÇ∑äÑçá")] private float amount;
    private Vector3 random;

    private void Start()
    {
        random = new Vector3(amount, amount, amount);
    }

    private void Update()
    {
        float nowtime = Time.time;
        Vector3 nowRotation = transform.localEulerAngles;
        nowRotation.x += Mathf.Sin(nowtime) * random.x;
        nowRotation.y += Mathf.Sin(nowtime) * random.y;
        nowRotation.z += Mathf.Sin(nowtime) * random.z;
        transform.localEulerAngles = nowRotation;
    }
}
