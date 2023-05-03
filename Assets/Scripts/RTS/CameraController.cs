using UnityEngine;

namespace RTS
{
    [RequireComponent(typeof(Camera))]
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
                ValidateCursorLockState();
            }
        }

        [SerializeField]
        [Min(1f)]
        private float movementSensitivity = 5f;

        [SerializeField]
        [Min(1f)]
        private float mouseDetectBreadth = 16f;

        private Rect cameraViewRect;

        private void Reset()
        {
            mainCamera = GetComponent<Camera>();
        }

        private void OnValidate()
        {
            ValidateCursorLockState();
        }

        private void Awake()
        {
            InitCameraViewRect();

            ValidateCursorLockState();
        }

        private void Update()
        {
            if (!CursorLocked)
                return;

            var mousePosition = Input.mousePosition;
            VerticalMouseMovement(mousePosition.x);
            HorizontalMouseMovement(mousePosition.y);
        }

        private void InitCameraViewRect()
        {
            var cameraHeight = mainCamera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * mainCamera.aspect;
            
            cameraViewRect.size = new Vector2(cameraWidth, cameraHeight);
            cameraViewRect.center = new Vector2(0, 0);
        }

        private void ValidateCursorLockState()
        {
            Cursor.lockState = cursorLocked ? CursorLockMode.Confined : CursorLockMode.None;
        }

        private void VerticalMouseMovement(float mouseX)
        {
            if (mouseX < mouseDetectBreadth)
            {
                transform.position += new Vector3(-movementSensitivity * Time.deltaTime, 0);
                return;
            }

            if (mouseX > Screen.width - mouseDetectBreadth)
            {
                transform.position += new Vector3(movementSensitivity * Time.deltaTime, 0);
                return;
            }
        }

        private void HorizontalMouseMovement(float mouseY)
        {
            if (mouseY < mouseDetectBreadth)
            {
                transform.position += new Vector3(0, -movementSensitivity * Time.deltaTime);
                return;
            }

            if (mouseY > Screen.height - mouseDetectBreadth)
            {
                transform.position += new Vector3(0, movementSensitivity * Time.deltaTime);
                return;
            }
        }
    }
}
