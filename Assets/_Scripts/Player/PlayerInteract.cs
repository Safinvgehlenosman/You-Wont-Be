using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 3f;
    public LayerMask interactableLayer;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
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
            interactable.Interact();
            Debug.Log("<color=green>Interact:</color> Success with " + hit.collider.name);
        }
    }
    else
    {
        Debug.Log("<color=yellow>Interact:</color> Raycast hit nothing.");
    }
}
}