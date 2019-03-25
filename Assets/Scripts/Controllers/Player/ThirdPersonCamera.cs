using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public LayerMask wallLayers;

    [SerializeField] private Transform cameraLookTarget;
    private PlayerController localPlayer;

    public enum Shoulder
    {
        Right, Left
    }

    public Shoulder shoulder;

    [System.Serializable]
    public class CameraSettings {
        [Header("-Positioning-")]
        public Vector3 camPositionOffsetLeft;
        public Vector3 camPositionOffsetRight;

        [Header("-Camera Options-")]
        public float mouseXSensitivity = 5.0f;
        public float mouseYSensitivity = 5.0f;

        public float minAngle = -30.0f;
        public float maxAngle = 70.0f;

        public float rotationSpeed = 5.0f;

        public float maxCheckDist = 0.1f;

        [Header("-Zoom-")]
        public float fieldOfView = 70.0f;
        public float zoomFieldOfView = 30.0f;
        public float zoomSpeed = 3.0f;

        [Header("-Visual Options-")]
        public float hidMeshWhenDistance = 0.5f;
    }

    [SerializeField] private CameraSettings cameraSettings;

    [System.Serializable]
    public class MovementSettings {
        public float movementLerpSpeed = 5.0f;
    }

    [SerializeField] private MovementSettings movement;

    [SerializeField] private Transform pivot;
    private Camera mainCamera;

    private float newX = 0.0f;
    private float newY = 0.0f;

    private InputController inputController;

    void Awake () {
        GameManager.Instance.OnLocalPlayerJoined += HandleOnLocalPlayerJoined;
        inputController = GameManager.Instance.InputController;
	}

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void HandleOnLocalPlayerJoined(PlayerController player)
    {
        localPlayer = player;
        cameraLookTarget = localPlayer.transform;
    }

    // Update is called once per frame
    void Update () {
        if (!localPlayer || !cameraLookTarget || !mainCamera || !pivot)
            return;
        RotateCamera();
        CheckWall();
        //CheckMeshRenderer();
        Zoom(inputController.Fire2);

        if (Input.GetKeyDown(KeyCode.V))
            SwitchShoulders();
	}

    void LateUpdate()
    {
        if (!cameraLookTarget)
            return;
        Vector3 targetPos = cameraLookTarget.position;
        Quaternion targetRot = cameraLookTarget.rotation;

        FollowTarget(targetPos, targetRot);
    }

    void FollowTarget(Vector3 targetPos, Quaternion targetRot)
    {
        Vector3 newPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * movement.movementLerpSpeed);
        transform.position = newPos;
    }

    void RotateCamera()
    {
        newX += cameraSettings.mouseXSensitivity * inputController.MouseInput.x;
        newY -= cameraSettings.mouseYSensitivity * inputController.MouseInput.y;

        Vector3 eulerAngleAxis = new Vector3();
        eulerAngleAxis.x = newY;
        eulerAngleAxis.y = newX;

        newX = Mathf.Repeat(newX, 360);
        newY = Mathf.Clamp(newY, cameraSettings.minAngle, cameraSettings.maxAngle);

        Quaternion newRotation = Quaternion.Slerp(pivot.localRotation, Quaternion.Euler(eulerAngleAxis), Time.deltaTime * cameraSettings.rotationSpeed);
        pivot.localRotation = newRotation;
    }

    void CheckWall()
    {
        RaycastHit hit;
        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 pivotPosition = pivot.position;

        Vector3 start = pivotPosition;
        Vector3 dir = mainCamPos - pivotPosition;

        float dist = Mathf.Abs(shoulder == Shoulder.Left ? cameraSettings.camPositionOffsetLeft.z : cameraSettings.camPositionOffsetRight.z);

        if (Physics.SphereCast(start, cameraSettings.maxCheckDist, dir, out hit, dist, wallLayers))
        {
            MoveCamUp(hit, pivotPosition, dir, mainCamT);
        }
        else {
            switch (shoulder)
            {
                case Shoulder.Left:
                    PositionCamera(cameraSettings.camPositionOffsetLeft);
                    break;
                case Shoulder.Right:
                    PositionCamera(cameraSettings.camPositionOffsetRight);
                    break;
            }
        }
    }

    void MoveCamUp(RaycastHit hit, Vector3 pivotPos, Vector3 dir, Transform cameraT)
    {
        float hitDist = hit.distance;
        Vector3 sphereCastCenter = pivotPos + (dir.normalized * hitDist);
        cameraT.position = sphereCastCenter;
    }

    void PositionCamera(Vector3 offset)
    {
        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.localPosition;
        Vector3 newCamPos = Vector3.Lerp(mainCamPos, offset, Time.deltaTime * movement.movementLerpSpeed);
        mainCamT.localPosition = newCamPos;
    }

    void CheckMeshRenderer()
    {
        SkinnedMeshRenderer[] meshes = cameraLookTarget.GetComponentsInChildren<SkinnedMeshRenderer>();
        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 targetPos = cameraLookTarget.position;
        float dist = Vector3.Distance(mainCamPos, (targetPos + cameraLookTarget.up));

        if (meshes.Length > 0)
        {
            foreach (var mesh in meshes)
            {
                if (dist <= cameraSettings.hidMeshWhenDistance) {
                    mesh.enabled = false;
                } else {
                    mesh.enabled = true;
                }
            }
        }
    }

    void Zoom(bool isZooming)
    {
        if (isZooming)
        {
            float newFov = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.zoomFieldOfView, Time.deltaTime * cameraSettings.zoomSpeed);
            mainCamera.fieldOfView = newFov;
        }
        else
        {
            float newFov = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.fieldOfView, Time.deltaTime * cameraSettings.zoomSpeed);
            mainCamera.fieldOfView = newFov;
        }
    }

    void SwitchShoulders()
    {
        switch (shoulder)
        {
            case Shoulder.Left:
                shoulder = Shoulder.Right;
                break;
            case Shoulder.Right:
                shoulder = Shoulder.Left;
                break;
        }
    }
}
