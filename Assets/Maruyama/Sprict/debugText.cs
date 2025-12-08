using TMPro;
using UnityEngine;
namespace Maruyama
{
    public class debugText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DebugText;
    [SerializeField] ChildController child;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //DebugText.text = child.player.ToString();
    }
}
}