using System;
using Unity.VisualScripting;
using UnityEngine;

public class GimmickThread : GimmickBase
{
    [SerializeField] GameObject actionParticle;     // エフェクトオブジェクト
    bool connectMarionntte = true;
    GimmickCollision collision;

    private void Awake()
    {
        isState = true;
    }

    private void Start()
    {
        collision = transform.GetComponent<GimmickCollision>();
        if (collision == null)
        {
            transform.AddComponent<GimmickCollision>();
            collision = transform.GetComponent<GimmickCollision>();
        }
    }

    private void Update()
    {
        if (collision == null) { return; }

        if (collision.awakeGimmick) { isState = false; }
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue() { }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {
        connectMarionntte = false;
        if(actionParticle != null)
        {
            actionParticle.SetActive(false);
        }
    }

    public bool GetConnctMarionnet() { return connectMarionntte; }
}


