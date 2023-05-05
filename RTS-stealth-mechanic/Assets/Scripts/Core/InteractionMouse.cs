using UnityEngine;
using Common.Interfaces;

public sealed class InteractionMouse : MonoBehaviour
{
    Actions _actions;
    [SerializeField] Camera _mainCamera;
    Transform target;
    RaycastHit hit;
    [SerializeField] InputActionsHolder _inputHandler;
    private void Start()
    {
        _actions = _inputHandler._actions;
        InvokeRepeating(nameof(CheckOutline), 0f, 0.15f);

        if (_mainCamera == null) _mainCamera = Camera.main; 
    }
    void CheckOutline()
    {
        if (target != null)
        {
            target.GetComponent<IOutlineOnMouseOver>().ToggleOutline(false);
            target = null;
        }

        Vector2 mousePosition = _actions.GameMode.MousePosition.ReadValue<Vector2>();
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray,out hit, Mathf.Infinity))
        {
        
            if (hit.transform.gameObject.GetComponent<IOutlineOnMouseOver>() != null)
            {
                target = hit.transform;
                target.gameObject.GetComponent<IOutlineOnMouseOver>().ToggleOutline(true);
            }   

          
        }
    }
}
