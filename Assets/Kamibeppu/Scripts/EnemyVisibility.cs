using UnityEngine;
using UnityEngine.Rendering.Universal;

/*
 * このスクリプトは
 * 視界（ライト）の中に「子どもがいるか いないか」
 * を判定するスクリプトです。
 * 
 * 視界（ライト）に直接アタッチして
 * 使用してください。
 * 
 * 視界は判定をとる都合上
 * 【 Freeform Light 2D 】で作ってください。
 */

public class EnemyVisibility : MonoBehaviour
{
    GameObject child;

    private void Start()
    {
        child = GameObject.FindGameObjectWithTag("Child");
        if(child == null)
        {
            Debug.Log("こどもが見つからない");
        }
    }

    // 視界の中に子どもがいるか調べる
    public bool IsWithinChildInVisibility()
    {/*
        if(child == null)
            return false;

        Vector3 childPos = child.transform.position;
        Vector3[] lightPath = this.gameObject.GetComponent<Light2D>().shapePath;
        
        for(int i = 0; i < lightPath.Length; i++)
        {
            // 現在のライト頂点 から 次のライト頂点 への方向ベクトルを求める
            int nextLightPath = (i + 1) % lightPath.Length;
            Vector3 lightLocalPos = new Vector3(lightPath[i].x, lightPath[i].y, 0);
            Vector3 lightWorldPos = transform.TransformPoint(lightLocalPos);
            Vector3 nextLightLocalPos = new Vector3(lightPath[nextLightPath].x, lightPath[nextLightPath].y, 0);
            Vector3 nextLighWorldtPos =  transform.TransformPoint(nextLightLocalPos);

            // 外積を使って方向ベクトルの左右どちらにいるか調べる
            Vector3 nextLightDir = (nextLighWorldtPos - lightWorldPos).normalized;
            Vector3 toChildDir = (childPos - lightWorldPos).normalized;
            Vector3 cross = Vector3.Cross(nextLightDir, toChildDir);

            // 外積の値が負の値なら範囲外にいるため false を返す
            if(cross.z < 0)
            {
                Debug.Log("子どもいない");
                return false;
            }
        }

        // ループを抜けた = 範囲内に子どもがいるため true を返す
        Debug.Log("子どもがいる！");
        return true;
        */
        return false;   // 小川デバッグ用
    }
}
