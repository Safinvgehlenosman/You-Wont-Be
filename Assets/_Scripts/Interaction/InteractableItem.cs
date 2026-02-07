using UnityEngine;

public class InteractableBox : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private bool isBeingHeld = false;
    private Transform holdPoint;

    [Header("Settings")]
    public float throwForce = 10f; // You can now change this in the Inspector!
    public float pickUpRange = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact(IItemHoldPointProvider interactor)
    {
        if (!isBeingHeld)
        {
            PickUp(interactor);
        }
        else
        {
            Drop();
        }
    }

    void PickUp(IItemHoldPointProvider interactor)
    {
        if (interactor == null || interactor.HoldPoint == null)
        {
            Debug.LogWarning("InteractableBox: No hold point available on interactor.");
            return;
        }

        holdPoint = interactor.HoldPoint;
        isBeingHeld = true;
        rb.useGravity = false;
        rb.isKinematic = true; 
        
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    void Drop()
    {
        isBeingHeld = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        
        transform.SetParent(null);

        // Uses the public throwForce value
        rb.AddForce(holdPoint.forward * throwForce, ForceMode.Impulse);
    }

    public string GetInteractText()
    {
        return isBeingHeld ? "Press E to Throw" : "Press E to Pick Up";
    }
}