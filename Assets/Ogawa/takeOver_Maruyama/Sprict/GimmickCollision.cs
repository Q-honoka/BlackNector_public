using System;
using UnityEngine;

public class GimmickCollision : MonoBehaviour
{
    GameObject player;

    [NonSerialized]
    public bool awakeGimmick = false;   //ƒMƒ~ƒbƒN”­“®

    void Update()
    {
        if (player == null) { return; }
        if (player.GetComponent<PlayerController>().peckInput)
        {
            awakeGimmick = true;
            player.GetComponent<PlayerController>().peckInput = false;
        }
    }


    private void OnTriggerStay(Collider collision)
    {
        Debug.Log(collision.name);
        if (collision.gameObject.tag != "Player") { return; }
        player = collision.gameObject;

    }

    private void OnTriggerExit(Collider collision)
    {
        if (player != null)
        {
            player = null;
        }
    }
}
