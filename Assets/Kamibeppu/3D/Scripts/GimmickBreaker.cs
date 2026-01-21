using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GimmickBreaker : GimmickBase
{
    [SerializeField]
    GimmickCollision collision;
    [SerializeField]
    GameObject[] lights;    // 消したいライトすべて
    [SerializeField]
    GameObject nestors;     // 追いかけるネスター
    bool isChaseStart;      // 追いかけ開始
    bool myState = true;
    private Animator anim;

    int pickedCount = 0;

    private void Start()
    {
        isState = false;
        anim = GetComponent<Animator>();
        StartCoroutine(LightOffAnimation());
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

        // 最終ステージについたら
        if (isChaseStart && GameData.instance.saveSpot == SaveSpotKind.Stage8_1)
        {
            nestors.SetActive(true);
            nestors.GetComponent<Animator>().SetTrigger("Chase");
            isChaseStart = false;
        }
    }

    // ライトを消すコルーチン
    IEnumerator LightOffAnimation()
    {
        foreach (var light in lights)
        {
            yield return new WaitForSeconds(3.0f);
            if (nestors.activeSelf == false)
            {
                isChaseStart = true;
            }
            light.transform.GetComponentInChildren<Animator>().SetTrigger("Off");
        }
    }

    // タイトルに戻る
    IEnumerator ToTitle()
    {
        yield return new WaitForSeconds(15.0f);
        SceneController.instance.SceneChange(0);
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected override void OnStateTrue()
    {
        pickedCount++;
        anim.SetTrigger("Picked");
        anim.SetInteger("PickedCount", pickedCount);

        // 3回つついたらライトをつける
        if (pickedCount == 3)
        {
            foreach (var light in lights)
            {
                light.transform.GetComponentInChildren<Animator>().SetTrigger("On");
                nestors.SetActive(false);
            }
            pickedCount = 0;
            StartCoroutine(ToTitle());
        }
    }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected override void OnStateFalse()
    {

    }

}
