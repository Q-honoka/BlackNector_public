using UnityEngine;

public class Image_CharacterManager : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetpos = Vector3.zero;
    private Vector3 resetpos = new Vector3(-5.0f, -3f, 0.0f);

    Image_CameraMovement cameraMovement;
    [SerializeField] private float smoothTime = 0.5f;
    [SerializeField] public GameObject meincamera;
    private bool stand;
    private Vector3 plpos;

    [SerializeField] ChildController ChildController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = resetpos;
        stand = false;
        plpos = this.transform.position;
        cameraMovement = meincamera.GetComponent<Image_CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.R))
        {
            cameraMovement.ChameraPosReset();
            this.transform.position = resetpos;
            ChildController.ResetPos();
        }
        if(stand)
        {
            Stageswitching();
        }

        plpos = this.transform.position;

    }

    /// <summary>
    /// Player‚ÌˆÊ’u‚ðŽæ“¾
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerPos() { return plpos; }
    public void SetPlayerPos_Stageswitching(Vector3 setpos)
    {
        targetpos = setpos;
        stand = true;
    }

    public void Stageswitching()
    {
        if (targetpos == null) { return; }

        transform.position = Vector3.SmoothDamp(
           transform.position,
           targetpos,
           ref velocity,
           smoothTime);


        if (Vector3.Distance(transform.position, targetpos) < 0.3f)
        {
            //Debug.Log("ˆ—I‚í‚è");
            transform.position = targetpos;
            velocity = Vector3.zero;
            stand = false;
        }
    }
}
