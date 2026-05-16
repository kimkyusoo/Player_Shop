using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRaycastController : MonoBehaviour
{
    [SerializeField] private LayerMask interactLayer;

    public float interactDistance = 3f;

    [SerializeField] private Camera m_cam;

    private PlayerInput playerInput;
    private InputAction shopping;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shopping = playerInput.actions.FindAction("Shopping", throwIfNotFound: true);
        if (m_cam == null) m_cam = Camera.main;
    }

    private void OnEnable()
    {
        shopping.performed += OperateStore;
        shopping.Enable();
    }

    private void OnDisable()
    {
        shopping.performed -= OperateStore;
        shopping.Disable();
    }

    private void OperateStore(InputAction.CallbackContext _)
    {
        Debug.Log("Shop »£√‚");
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, interactDistance, interactLayer))
        {
            if (hit.collider.gameObject.layer == 3)
            {
                Shop shop = hit.collider.GetComponent<Shop>();
                if(shop != null)
                {
                    ShopUI.Instance.OpenShop(shop, GetComponent<PlayerInventory>());
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blueViolet;

        Vector3 startPos = transform.position + Vector3.up * 1.5f;
        Vector3 endPos = startPos + (transform.forward * interactDistance);

        Gizmos.DrawLine(startPos, endPos);
    }
}
