using UnityEngine;

namespace Maruyama
{
    public interface ICharcters
{
    void State();           //状態管理総括 
    void Idle();            //待機状態
    void Move();            //移動、行動状態
    void End();              //削除　→　アニメーションなどの追加の際に使用
    void SetMyState(int newState);
}



}