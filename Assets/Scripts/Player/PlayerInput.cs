using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }

    PlayerControls inputActions;
    private InputAction move , jump , jetpack , grenade;   

    private void Awake()
    {
        inputActions = new PlayerControls();

        move = inputActions.Player.Move;
        jump = inputActions.Player.Jump;
        jetpack = inputActions.Player.JetPack;
        grenade = inputActions.Player.Grenade;
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
            Jump = jump.WasPressedThisFrame(),
            JetPack = jetpack.WasPressedThisFrame(),
            Grenade = grenade.WasPressedThisFrame()
        };
    }

}

public struct FrameInput
{
    public Vector2 Move;
    public bool Jump;
    public bool JetPack;
    public bool Grenade;    
}