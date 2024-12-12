using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera[] cameras;
    private int currentCameraIndex = 0;

    void Start()
    {
        // Отключаем все камеры кроме первой
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);
        }
    }

    void Update()
    {
        // Проверяем нажатие клавиши C для переключения камеры
        if (Input.GetKeyDown(KeyCode.C))
        {
            CameraChanger();
        }
    }

    public void CameraChanger()
    {
            // Отключаем текущую камеру
            cameras[currentCameraIndex].gameObject.SetActive(false);

            // Переход к следующей камере с зацикливанием
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

            // Включаем следующую камеру
            cameras[currentCameraIndex].gameObject.SetActive(true);
        
    }
}
