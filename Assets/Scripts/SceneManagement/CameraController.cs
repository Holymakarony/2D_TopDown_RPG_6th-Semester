using Unity.Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineCamera cinemachineVirtualCamera;
    
    public void SetPlayerCameraFollow()
    {
        cinemachineVirtualCamera = FindAnyObjectByType<CinemachineCamera>();
        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }
}
