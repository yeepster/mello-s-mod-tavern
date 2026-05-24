using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRModMenu : MonoBehaviour
{
    // === PUBLIC FIELDS ===
    public GameObject uiPanel;              
    public InputActionReference bButtonAction; 
    public XRController leftController;      
    public XRController rightController;     
    public GameObject playerObject;          

    // === PRIVATE MODS ===
    private bool isVisible = false;
    private bool isFlyModeEnabled = false;
    private float speedMultiplier = 1.0f;

    // === COMPONENTS ===
    private PlayerMovement playerMovementComponent;

    void OnEnable()
    {
        if (bButtonAction != null)
        {
            bButtonAction.action.Enable();
            bButtonAction.action.performed += OnBButtonPressed;
        }

        // Try to get PlayerMovement component from playerObject
        if (playerObject != null)
        {
            playerMovementComponent = playerObject.GetComponent<PlayerMovement>();
        }
    }

    void OnDisable()
    {
        if (bButtonAction != null)
        {
            bButtonAction.action.performed -= OnBButtonPressed;
            bButtonAction.action.Disable();
        }
    }

    void OnBButtonPressed(InputAction.CallbackContext context)
    {
        // Toggle visibility
        isVisible = !isVisible;
        uiPanel.SetActive(isVisible);

        // Disable/enable player movement
        if (playerMovementComponent != null)
        {
            playerMovementComponent.enabled = !isVisible;
        }

        // Optional: Send haptic feedback
        if (leftController != null)
            leftController.SendHapticImpulse(0.5f, 0.1f);
        if (rightController != null)
            rightController.SendHapticImpulse(0.5f, 0.1f);

        // Log
        Debug.Log($"[VRModMenu] Menu {(isVisible ? "OPENED" : "CLOSED")}");
        Debug.Log($"[VRModMenu] Fly Mode: {isFlyModeEnabled}, Speed: {speedMultiplier}");

    }

    // === OTHER SCRIPTS TO CALL ===

    public void ToggleFlyMode()
    {
        isFlyModeEnabled = !isFlyModeEnabled;
        Debug.Log($"[VRModMenu] Fly Mode toggled: {isFlyModeEnabled}");
    }

    public void SetSpeedMultiplier(float value)
    {
        speedMultiplier = Mathf.Clamp(value, 0.5f, 5.0f);
        Debug.Log($"[VRModMenu] Speed Multiplier set to: {speedMultiplier}");
    }

    public void CloseMenu()
    {
        isVisible = false;
        uiPanel.SetActive(false);

        if (playerMovementComponent != null)
            playerMovementComponent.enabled = true;

        Debug.Log("[VRModMenu] Menu CLOSED via API");
    }

    // === GETTERS FOR OTHER SCRIPTS ===

    public bool IsMenuOpen => isVisible;
    public bool IsFlyModeEnabled => isFlyModeEnabled;
    public float SpeedMultiplier => speedMultiplier;
}
