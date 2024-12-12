using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera[] cameras;
    private int currentCameraIndex = 0;

    void Start()
    {
        // ��������� ��� ������ ����� ������
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);
        }
    }

    void Update()
    {
        // ��������� ������� ������� C ��� ������������ ������
        if (Input.GetKeyDown(KeyCode.C))
        {
            CameraChanger();
        }
    }

    public void CameraChanger()
    {
            // ��������� ������� ������
            cameras[currentCameraIndex].gameObject.SetActive(false);

            // ������� � ��������� ������ � �������������
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

            // �������� ��������� ������
            cameras[currentCameraIndex].gameObject.SetActive(true);
        
    }
}
