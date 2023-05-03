using UnityEngine;

public sealed class InputHandler : MonoBehaviour
{
    public Actions _actions { get; private set; }
    private void Awake()
    {
        _actions = new Actions();
    }

    void OnEnable() => _actions.Enable();
    void OnDisable() => _actions.Disable();
}
