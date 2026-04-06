using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.Utils
{
    [ExecuteAlways]
    [RequireComponent(typeof(UIDocument))]
    public class WorldSpaceUIScaler : MonoBehaviour
    {
        [Tooltip("Name of the VisualElement from uxml. If empty, the root element will be used")]
        public string targetElementName;
        public bool updateEveryFrame = false;

        [Tooltip("Camera reference. If null, the Camera.main will be used. Camera must be orthographic")]
        public Camera orthographicCamera;

        private UIDocument _uiDocument;
        private VisualElement targetElement;
        private PanelSettings panelSettings;
        private float pixelsPerUnit;

        private void OnEnable()
        {
            _uiDocument = GetComponent<UIDocument>();

            if (orthographicCamera == null)
                orthographicCamera = Camera.main;

            if (_uiDocument == null)
            {
                Debug.LogError("UIDocument is null!");
                enabled = false;
                return;
            }

            if (orthographicCamera == null || !orthographicCamera.orthographic)
            {
                Debug.LogError("Orthographic Camera not found!");
                enabled = false;
                return;
            }

            panelSettings = _uiDocument.panelSettings;
            if (panelSettings == null)
            {
                Debug.LogError("PanelSettings not found in UIDocument!");
                enabled = false;
                return;
            }

            pixelsPerUnit = panelSettings.referenceSpritePixelsPerUnit;

            FindTargetElement();
            UpdateLayout();
        }

        private void FindTargetElement()
        {
            targetElement = string.IsNullOrEmpty(targetElementName)
                ? _uiDocument.rootVisualElement
                : _uiDocument.rootVisualElement.Q<VisualElement>(targetElementName);

            if (targetElement == null)
            {
                Debug.LogError($"VisualElement '{targetElementName}' not found!");
                enabled = false;
            }
        }

        private void Update()
        {
            if (updateEveryFrame)
                UpdateLayout();
        }

        private void UpdateLayout()
        {
            if (targetElement == null || orthographicCamera == null)
                return;

            float targetHeight = orthographicCamera.orthographicSize * 2;
            float targetWidth = targetHeight * orthographicCamera.aspect;

            targetElement.style.width = targetWidth * pixelsPerUnit;
            targetElement.style.height = targetHeight * pixelsPerUnit;

            float offsetX = targetWidth * 0.5f;
            float offsetY = targetHeight * 0.5f;
            var center = new Vector3(
                -offsetX,
                offsetY,
                transform.localPosition.z
            );

            transform.localPosition = center;
        }

        // #if UNITY_EDITOR
        // private void OnValidate()
        // {
        //     if (Application.isPlaying && enabled && uiDocument != null)
        //     {
        //         UpdateLayout();
        //     }
        // }
        // #endif
    }
}