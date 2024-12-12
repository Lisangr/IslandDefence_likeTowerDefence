using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using YG;

public class CannonsBuilder : MonoBehaviour
{
    public AcceptedData acceptedData; // Ссылка на скрипт AcceptedData
    private Button placeCannonButton; // Кнопка для размещения префабов
    private GameObject cannonInstance; // Префаб, следующий за курсором
    private bool isPlacingCannon = false; // Флаг, указывающий на процесс размещения пушки
    public float placementRadius = 2.0f; // Минимальное расстояние между пушками
    public float heightOffset = 2.0f; // Смещение по высоте при размещении
    public Walet wallet; // Ссылка на компонент кошелька

    private List<Vector3> cannonPositions = new List<Vector3>(); // Список для хранения позиций всех установленных пушек

    private void Start()
    {
        placeCannonButton = GetComponent<Button>();
        placeCannonButton.onClick.AddListener(StartPlacingCannon);
    }

    private void Update()
    {
        // Проверка на нажатие Escape для отмены размещения пушки
        if (isPlacingCannon && Input.GetKeyDown(KeyCode.Escape))
        {
            CancelCannonPlacement();
            return;
        }

        if (isPlacingCannon && cannonInstance != null)
        {
            FollowCursor(); // Обновление позиции префаба под курсором

            if (Input.GetMouseButtonDown(0)) // ЛКМ для установки префаба
            {
                PlaceCannonOnTower();
            }
        }
    }

    void StartPlacingCannon()
    {
        CannonData selectedCannonData = acceptedData.GetSelectedCannon();
        if (selectedCannonData == null || selectedCannonData.prefab == null)
        {
            Debug.LogError("Selected CannonData or its prefab is not set.");
            return;
        }

        // Проверка наличия достаточного количества золота
        int totalGold = wallet.GetTotalGold();
        //int totalGoldYG = YandexGame.savesData.gold; 
        if (totalGold < selectedCannonData.price)//|| totalGoldYG < selectedCannonData.price)
        {
            Debug.Log("Недостаточно золота для размещения пушки.");
            return;
        }

        cannonInstance = Instantiate(selectedCannonData.prefab);
        isPlacingCannon = true;
    }

    void FollowCursor()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera is not found. Please ensure that a camera is tagged as MainCamera.");
            return;
        }

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            targetPosition.y += heightOffset;

            cannonInstance.transform.position = targetPosition;
        }
    }

    void PlaceCannonOnTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Tower"))
            {
                Vector3 placementPosition = hit.point;
                placementPosition.y += heightOffset;

                Collider[] colliders = Physics.OverlapSphere(placementPosition, placementRadius);
                bool canPlace = true;

                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Cannon"))
                    {
                        canPlace = false;
                        Debug.Log("Пушка не может быть установлена: слишком близко к другой пушке.");
                        break;
                    }
                }

                if (canPlace)
                {
                    CannonData selectedCannonData = acceptedData.GetSelectedCannon();
                    int totalGold = wallet.GetTotalGold();

                    if (totalGold >= selectedCannonData.price)
                    {
                        wallet.ReduceGold(selectedCannonData.price); // Уменьшаем золото на стоимость пушки
                        cannonInstance.transform.SetParent(hitObject.transform);
                        cannonInstance.transform.position = placementPosition;

                        isPlacingCannon = false;
                        cannonInstance = null;
                    }
                    else
                    {
                        Debug.Log("Недостаточно золота для завершения покупки.");
                    }
                }
            }
        }
    }
    void CancelCannonPlacement()
    {
        if (cannonInstance != null)
        {
            Destroy(cannonInstance); // Убираем префаб пушки с экрана
            cannonInstance = null;
        }
        isPlacingCannon = false;
        Debug.Log("Размещение пушки отменено.");
    }
}
