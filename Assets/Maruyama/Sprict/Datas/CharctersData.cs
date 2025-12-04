using UnityEngine;

[System.Serializable]
public class CharctersData
{
    public Vector2 pos;        //キャラクターの初期位置
    public float speed;        //キャラクターの速度
    public ICharcters charctersInterface;
    public GameObject obje;    //キャラクターのオブジェクト
}

