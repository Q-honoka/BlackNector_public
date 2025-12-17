using UnityEngine;

[RequireComponent(typeof(Command))]
public class ConfigBtn : MonoBehaviour
{
    private Command command;

    private void Start()
    {
        command = GetComponent<Command>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            command.CallPanelChange_SendMenu();
        }
    }
}
