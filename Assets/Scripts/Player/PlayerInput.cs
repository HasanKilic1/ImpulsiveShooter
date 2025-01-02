using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }

    PlayerControls inputActions;
    private InputAction move;
    private InputAction jump;

    private void Awake()
    {
        inputActions = new PlayerControls();

        move = inputActions.Player.Move;
        jump = inputActions.Player.Jump;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        FrameInput = GatherInput();
    }

    private FrameInput GatherInput()
    {
        return new FrameInput
        {
            Move = move.ReadValue<Vector2>(),
            Jump = jump.WasPressedThisFrame()
        };
    }

}

public struct FrameInput
{
    public Vector2 Move;
    public bool Jump;
}