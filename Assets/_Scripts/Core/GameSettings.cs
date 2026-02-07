using UnityEngine;

/// <summary>
/// Root configuration asset for high-level, non-gameplay settings.
/// Safe to reference, extend, or ignore; it is not a manager or loader.
/// </summary>
[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Input")]
    public float mouseSensitivity = 30f;

    [Header("Interaction")]
    public float interactDistance = 3f;

    [Header("Runtime Behavior")]
    public bool lockCursorOnStart = true;
    public bool showDebugGizmos = false;
}
