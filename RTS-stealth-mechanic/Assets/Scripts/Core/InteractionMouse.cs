using UnityEngine;

public sealed class InteractionMouse : MonoBehaviour
{
  Actions _actions;
  [SerializeField] Camera _mainCamera;
    public Transform target;
    RaycastHit hit;
    [SerializeField] InputHandler _inputHandler;
    private void Start()
    {
        _actions = _inputHandler._actions;
        InvokeRepeating(nameof(CheckOutline), 0f, 0.5f); 
    }
    void CheckOutline()
    {
        if (target != null)
        {
            target.GetComponent<Common.Interfaces.IOutlineOnMouseOver>().DisableOutline();
            target = null;
        }

        Vector2 mousePosition = _actions.GameMode.MousePosition.ReadValue<Vector2>();
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray,out hit, Mathf.Infinity))
        {
        
            if (hit.transform.gameObject.GetComponent<Common.Interfaces.IOutlineOnMouseOver>() != null)
            {
                target = hit.transform;
                target.gameObject.GetComponent<Common.Interfaces.IOutlineOnMouseOver>().EnableOutline();
            }   
        }
    }
}
