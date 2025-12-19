using UnityEngine;
using UniRx;
using UnityEngine.Events;

[RequireComponent(typeof(Marionette))]
[RequireComponent(typeof(Command))]
public class MarionetteEvent : MonoBehaviour
{
    private Marionette m_marionette;
    private Command m_command;

    [SerializeField,Header("落下したとき")] private UnityEvent fallEvents;

    private void Start()
    {
        m_marionette = GetComponent<Marionette>();
        m_command = GetComponent<Command>();

        // イベント取得
        m_marionette.OnFallSubject
            .Subscribe(kind => { OnFall(); });
    }

    // 落ちた時のイベント発動
    private void OnFall()
    {
        fallEvents?.Invoke();
    }
}
