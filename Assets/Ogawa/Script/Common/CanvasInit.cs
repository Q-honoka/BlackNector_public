using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasInit : MonoBehaviour
{
    private Canvas _canvas;

    [SerializeField] private bool renderIsWorldCamera; 

    private void Start()
    {
        _canvas = GetComponent<Canvas>();

        if(renderIsWorldCamera)
        {
            _canvas.worldCamera = Camera.main;
        }
    }
}
