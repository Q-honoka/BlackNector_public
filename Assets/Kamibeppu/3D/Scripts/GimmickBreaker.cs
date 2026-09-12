using RaruLib;
using UnityEngine;
using UnityEngine.Playables;

public class GimmickBreaker : GimmickBase
{
    [SerializeField]
    GimmickCollision collision;
    [SerializeField]
    bool myState = true;
    [SerializeField]
    PlayableDirector endFinalDirector;

    private const int ActivePickCount = 3;  // 起動させるのに必要な回数
    private Animator anim;
    private Sound _sound => Sound.instance;

    int pickedCount = 0;

    private void Start()
    {
        isState = false;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (collision == null) { return; }

        if (collision.awakeGimmick)
        {
            myState ^= true;
            isState = myState;
            collision.awakeGimmick = false;
        }
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        pickedCount++;
        anim.SetTrigger("Picked");
        anim.SetInteger("PickedCount", pickedCount);

        // 指定回数つついたらライトをつける
        if (pickedCount == ActivePickCount)
        {
            _sound.Play("SE", "Breaker_finish");
            if (endFinalDirector != null)
            {
                endFinalDirector.Play();
            }
        }
        else
        {
            _sound.Play("SE","Breaker_middle");
        }
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {

    }

}
