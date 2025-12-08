using UnityEngine;

public interface IEnemy
{
    void FoundPlayer();
    void CatchByNector();
    bool GetFoundPlayer();
}

//作成するときはべつのファイルに移してください
//
//・一応CharcterDataというクラスにキャラクター全体に共通するデータ構造体をまとめてあります。
//使っているのはPlayerDataで使用しています
//
//◇interface作成
//public interface IEnemy 
//{
//enemy全体に共通する行動
//}
//
//◇クラス作成
//public class オブジェクト名 : ICharcters,IEnemy
//{
//   void ICharcters.State(){}          
//   void ICharcters.Idle(){}           
//   void ICharcters.Move(){}           
//   void ICharcters.End(){}             
//   void ICharcters.SetMyState(int newState){}
//   bool ICharcters.Collision(){}
//   
//  IEnemyのメンバー関数を定義
//
//  enum 構造体名
//  {
//      Idle,
//      Move,
//      End,
//      必要に応じて固有の行動を足してください
//  }
//
//  void Start()
//  {
//      変数名.myData.myInterface = this 　←　この処理またはこの処理と同じ内容のものを必ず使用してください
//
//  }
//}
