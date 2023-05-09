using UnityEngine;

namespace RTS
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private bool cursorLocked = true;
        public bool CursorLocked
        {
            get => cursorLocked;

            set
            {
                cursorLocked = value;
                SetupCursorMode();
            }
        }

        [SerializeField]
        [Min(1f)]
        private float locateSpeed = 5f;

        [SerializeField]
        [Min(1f)]
        private float mouseDetectBreadth = 16f;

        [SerializeField]
        private Rect controlArea = new Rect
        {
            size = new Vector2(32f, 32f),
            center = new Vector2(0f, 0f),
        };

        private Rect viewport;
        private Rect locatableArea;
        private Rect mouseDetectArea;

        private void Reset()
        {
            mainCamera = GetComponent<Camera>();
        }

        private void OnValidate()
        {
            Setup();
        }

        private void Awake()
        {
            Setup();
        }

        private void FixedUpdate()
        {
            if (!CursorLocked)
                return;

            var mousePosition = Input.mousePosition;
            LocateCameraX(mousePosition.x);
            LocateCameraY(mousePosition.y);
            LimitCameraPosition();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(controlArea.center, controlArea.size);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(locatableArea.center, locatableArea.size);

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, viewport.size);

            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(transform.position, mouseDetectArea.size);
        }

        private void Setup()
        {
            SetupCursorMode();
            SetupView();
        }

        private void SetupCursorMode()
        {
            Cursor.lockState = cursorLocked ? CursorLockMode.Confined : CursorLockMode.None;
        }

        private void SetupView()
        {
            var cameraHeight = mainCamera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * mainCamera.aspect;

            viewport = new Rect
            {
                size = new Vector2(cameraWidth, cameraHeight),
                center = new Vector2(0, 0),
            };

            locatableArea = new Rect
            {
                xMin = controlArea.xMin + Mathf.Abs(viewport.xMin),
                xMax = controlArea.xMax - Mathf.Abs(viewport.xMax),
                yMin = controlArea.yMin + Mathf.Abs(viewport.yMin),
                yMax = controlArea.yMax - Mathf.Abs(viewport.yMax),
            };

            mouseDetectArea = new Rect
            {
                size = new Vector2
                {
                    x = viewport.width * (1f - mouseDetectBreadth * 2f / mainCamera.scaledPixelWidth),
                    y = viewport.height * (1f - mouseDetectBreadth * 2f / mainCamera.scaledPixelHeight),
                },
                center = new Vector2(0, 0),
            };
        }

        private void LocateCameraX(float mouseX)
        {
            if (mouseX < mouseDetectBreadth)
            {
                transform.position += new Vector3(-locateSpeed * Time.deltaTime, 0f);
                return;
            }

            if (mouseX > Screen.width - mouseDetectBreadth)
            {
                transform.position += new Vector3(locateSpeed * Time.deltaTime, 0f);
            }
        }

        private void LocateCameraY(float mouseY)
        {
            if (mouseY < mouseDetectBreadth)
            {
                transform.position += new Vector3(0f, -locateSpeed * Time.deltaTime);
                return;
            }

            if (mouseY > Screen.height - mouseDetectBreadth)
            {
                transform.position += new Vector3(0f, locateSpeed * Time.deltaTime);
            }
        }

        private void LimitCameraPosition()
        {
            transform.position = new Vector2
            {
                x = Mathf.Clamp(transform.position.x, locatableArea.xMin, locatableArea.xMax),
                y = Mathf.Clamp(transform.position.y, locatableArea.yMin, locatableArea.yMax),
            };
        }
    }
}
