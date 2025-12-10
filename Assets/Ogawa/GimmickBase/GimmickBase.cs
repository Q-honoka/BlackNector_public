using UnityEngine;

public class GimmickBase : MonoBehaviour
{
    private bool _isState;
    [SerializeField] protected GameObject stateTrueObj;
    [SerializeField] protected GameObject stateFalseObj;

    protected bool isState
    {
        set
        {
            if (value != _isState)
            {
                _isState = value;

                if (value)
                {
                    OnStateTrue();
                }
                else
                {
                    OnStateFalse();
                }
            }
        }
    }

    /// <summary>
    /// ステートがTrueに変化したとき
    /// </summary>
    protected virtual void OnStateTrue() { }

    /// <summary>
    /// ステートがFalseに変化したとき
    /// </summary>
    protected virtual void OnStateFalse() { }
}
