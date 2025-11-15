using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.Utils
{
    [ExecuteAlways]
    [RequireComponent(typeof(UIDocument))]
    public class WorldSpaceUIScaler : MonoBehaviour
    {
        [Header("Основные настройки")]
        [Tooltip("Имя целевого VisualElement (оставьте пустым для корневого)")]
        public string targetElementName;


        [Tooltip("Обновлять каждый кадр (для динамических разрешений)")]
        public bool updateEveryFrame = false;

        [Header("Настройки камеры")]
        [Tooltip("Ссылка на камеру (если null - будет использоваться Camera.main)")]
        public Camera targetCamera;

        private UIDocument uiDocument;
        private VisualElement targetElement;
        private PanelSettings panelSettings;
        private float pixelsPerUnit;

        private void OnEnable()
        {
            uiDocument = GetComponent<UIDocument>();
            targetCamera = targetCamera ?? Camera.main;

            if (uiDocument == null || targetCamera == null || !targetCamera.orthographic)
            {
                Debug.LogError("UIDocument или ортографическая камера не найдены!");
                enabled = false;
                return;
            }

            panelSettings = uiDocument.panelSettings;
            if (panelSettings == null)
            {
                Debug.LogError("PanelSettings не назначены в UIDocument!");
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
                ? uiDocument.rootVisualElement
                : uiDocument.rootVisualElement.Q<VisualElement>(targetElementName);

            if (targetElement == null)
            {
                Debug.LogError($"VisualElement '{targetElementName}' не найден!");
                enabled = false;
            }
        }

        private void Update()
        {
            if (updateEveryFrame)
            {
                UpdateLayout();
            }
        }

        private void UpdateLayout()
        {
            if (targetElement == null || targetCamera == null) return;

            // Размер видимой области в мировых координатах
            float targetHeight = targetCamera.orthographicSize * 2;
            float targetWidth = targetHeight * targetCamera.aspect;

            // Устанавливаем размер в пикселях
            targetElement.style.width = targetWidth * pixelsPerUnit;
            targetElement.style.height = targetHeight * pixelsPerUnit;

            // Смещение для компенсации отступов
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