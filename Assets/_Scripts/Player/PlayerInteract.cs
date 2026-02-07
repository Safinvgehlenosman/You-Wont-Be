using UnityEngine;

public class PlayerInteract : MonoBehaviour, IItemHoldPointProvider
{
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform holdPoint;
    private Camera cam;
    private InputManager inputManager;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void OnEnable()
    {
        if (inputManager == null)
        {
            inputManager = GetComponentInParent<InputManager>();
        }

        if (inputManager != null)
        {
            inputManager.InteractPressed += DoInteract;
        }
    }

    void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.InteractPressed -= DoInteract;
        }
    }

    public Transform HoldPoint
    {
        get
        {
            if (holdPoint == null)
            {
                holdPoint = CreateDefaultHoldPoint();
            }

            return holdPoint;
        }
    }

    // This is called by the InputManager when you press 'E'
    public void DoInteract()
    {
        // Make sure we actually have a camera reference
        if (cam == null) cam = GetComponent<Camera>();

        // Log the camera position so we can see if it's at (0,0,0) or elsewhere
        Debug.Log($"<color=cyan>Raycast Info:</color> Origin: {cam.transform.position}, Forward: {cam.transform.forward}");

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        
        // Draw the ray in the scene view. 
        // We use Color.magenta so it stands out against the gray grid.
        Debug.DrawLine(ray.origin, ray.origin + (ray.direction * interactRange), Color.magenta, 5f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this);
                Debug.Log("<color=green>Interact:</color> Success with " + hit.collider.name);
            }
        }
        else
        {
            Debug.Log("<color=yellow>Interact:</color> Raycast hit nothing.");
        }
    }

    private Transform CreateDefaultHoldPoint()
    {
        GameObject holdPointObject = new GameObject("HoldPoint");
        holdPointObject.transform.SetParent(transform, false);
        holdPointObject.transform.localPosition = new Vector3(0f, 0f, 1f);
        return holdPointObject.transform;
    }
}