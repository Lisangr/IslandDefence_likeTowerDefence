using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using YG;

public class CannonsBuilder : MonoBehaviour
{
    public AcceptedData acceptedData; // ������ �� ������ AcceptedData
    private Button placeCannonButton; // ������ ��� ���������� ��������
    private GameObject cannonInstance; // ������, ��������� �� ��������
    private bool isPlacingCannon = false; // ����, ����������� �� ������� ���������� �����
    public float placementRadius = 2.0f; // ����������� ���������� ����� �������
    public float heightOffset = 2.0f; // �������� �� ������ ��� ����������
    public Walet wallet; // ������ �� ��������� ��������

    private List<Vector3> cannonPositions = new List<Vector3>(); // ������ ��� �������� ������� ���� ������������� �����

    private void Start()
    {
        placeCannonButton = GetComponent<Button>();
        placeCannonButton.onClick.AddListener(StartPlacingCannon);
    }

    private void Update()
    {
        // �������� �� ������� Escape ��� ������ ���������� �����
        if (isPlacingCannon && Input.GetKeyDown(KeyCode.Escape))
        {
            CancelCannonPlacement();
            return;
        }

        if (isPlacingCannon && cannonInstance != null)
        {
            FollowCursor(); // ���������� ������� ������� ��� ��������

            if (Input.GetMouseButtonDown(0)) // ��� ��� ��������� �������
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

        // �������� ������� ������������ ���������� ������
        int totalGold = wallet.GetTotalGold();
        //int totalGoldYG = YandexGame.savesData.gold; 
        if (totalGold < selectedCannonData.price)//|| totalGoldYG < selectedCannonData.price)
        {
            Debug.Log("������������ ������ ��� ���������� �����.");
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
                        Debug.Log("����� �� ����� ���� �����������: ������� ������ � ������ �����.");
                        break;
                    }
                }

                if (canPlace)
                {
                    CannonData selectedCannonData = acceptedData.GetSelectedCannon();
                    int totalGold = wallet.GetTotalGold();

                    if (totalGold >= selectedCannonData.price)
                    {
                        wallet.ReduceGold(selectedCannonData.price); // ��������� ������ �� ��������� �����
                        cannonInstance.transform.SetParent(hitObject.transform);
                        cannonInstance.transform.position = placementPosition;

                        isPlacingCannon = false;
                        cannonInstance = null;
                    }
                    else
                    {
                        Debug.Log("������������ ������ ��� ���������� �������.");
                    }
                }
            }
        }
    }
    void CancelCannonPlacement()
    {
        if (cannonInstance != null)
        {
            Destroy(cannonInstance); // ������� ������ ����� � ������
            cannonInstance = null;
        }
        isPlacingCannon = false;
        Debug.Log("���������� ����� ��������.");
    }
}
