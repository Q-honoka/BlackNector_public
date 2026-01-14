using UnityEngine;

[RequireComponent(typeof(Command))]
public class NovelViewNext : MonoBehaviour
{
    [SerializeField,Range(0,2),Header("左クリックかホイールか右クリック")] private int clickTrigger;
    private Command _command;

    private void Start()
    {
        _command = transform.GetComponent<Command>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(clickTrigger))
        {
            _command.CallNovelMessageNext();
        }
    }
}
