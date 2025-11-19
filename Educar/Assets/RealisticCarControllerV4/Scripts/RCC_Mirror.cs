//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------


using UnityEngine;
using System.Collections;

/// <summary>
/// Must be attached to an external camera. This camera will be used as a mirror.
/// Improved for robustness, flexibility, and performance.
/// Now supports Unity's physical camera properties.
/// </summary>
public class RCC_Mirror : RCC_Core {
    [Header("Mirror Settings")]
    [Tooltip("Depth value for the mirror camera.")]
    [SerializeField] private float mirrorDepth = 1f;
    [Tooltip("Invert camera for mirror effect.")]
    [SerializeField] private bool invertMirror = true;

    [Header("Physical Camera (Optional)")]
    [Tooltip("Enable Unity's physical camera properties for more realistic mirrors.")]
    [SerializeField] private bool usePhysicalCamera = true;
    [Tooltip("Focal length in millimeters when using physical camera.")]
    [SerializeField] private float focalLength = 50f;
    [Tooltip("Sensor size in millimeters (width, height) when using physical camera.")]
    [SerializeField] private Vector2 sensorSize = new Vector2(36f, 24f);
    [Tooltip("Lens shift to adjust the optical center when using physical camera.")]
    [SerializeField] private Vector2 lensShift = Vector2.zero;
    [Tooltip("Gate fit mode used by the physical camera.")]
    [SerializeField] private Camera.GateFitMode gateFit = Camera.GateFitMode.Horizontal;

    private Camera cam; // Reference to the camera component.
    private bool isInverted = false;

    private void Awake() {
        // Try to get the Camera component.
        if (!TryGetComponent(out cam)) {
            Debug.LogError($"RCC_Mirror: No Camera component found on {gameObject.name}. Disabling script.");
            enabled = false;
            return;
        }

        // Apply physical camera settings if requested.
        ApplyPhysicalCameraSettings();

        // Invert camera for mirror effect if enabled.
        if (invertMirror)
            InvertCamera();
    }

    private void OnEnable() {
        StartCoroutine(FixDepth());
    }

    /// <summary>
    /// Applies configured physical camera properties.
    /// Safe to call even if physical camera is disabled.
    /// </summary>
    private void ApplyPhysicalCameraSettings() {
        if (cam == null)
            return;

        cam.usePhysicalProperties = usePhysicalCamera;

        if (usePhysicalCamera) {
            cam.focalLength = Mathf.Max(0.01f, focalLength);
            cam.sensorSize = new Vector2(Mathf.Max(0.01f, sensorSize.x), Mathf.Max(0.01f, sensorSize.y));
            cam.lensShift = lensShift;
            cam.gateFit = gateFit;
        }
    }

    /// <summary>
    /// Sets the depth of the camera after frame ends.
    /// </summary>
    private IEnumerator FixDepth() {
        yield return new WaitForEndOfFrame();
        if (cam != null)
            cam.depth = mirrorDepth;
    }

    /// <summary>
    /// Inverts the camera for mirror effect. Only applies once.
    /// </summary>
    private void InvertCamera() {
        if (cam == null || isInverted)
            return;
        cam.ResetWorldToCameraMatrix();
        cam.ResetProjectionMatrix();
        cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
        isInverted = true;
    }

    private void OnPreRender() {
        GL.invertCulling = invertMirror;
    }

    private void OnPostRender() {
        GL.invertCulling = false;
    }

    private void Update() {

        //  Enable or disable with controllable state of the vehicle.
        cam.enabled = CarController.canControl;

    }
}
