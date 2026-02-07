using UnityEngine;

public interface IInteractable
{
    // Every interactable object must have an Interact function
    void Interact(IItemHoldPointProvider interactor);
    
    // Optional: Returns a string to show on the UI (e.g., "Open Door")
    string GetInteractText();
}
