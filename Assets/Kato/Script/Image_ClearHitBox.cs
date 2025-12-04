using UnityEngine;

public class Image_ClearHitBox : MonoBehaviour
{
    Vector3 clearpos;
    Vector3 nextpos = new Vector3(11.0f,-2.7f,0);
    Image_CharacterManager characterManager;
    [SerializeField] GameObject character;

    Image_CameraMovement maincamera;
    [SerializeField] GameObject cameraObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clearpos = this.transform.position;
        characterManager = character.GetComponent<Image_CharacterManager>();
        maincamera = cameraObj.GetComponent<Image_CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Child")
        {
            maincamera.MoveTo();
            characterManager.SetPlayerPos_Stageswitching(nextpos);
        }
    }
}
