using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AcceptedData : MonoBehaviour
{
    private Text text;
    public CannonData[] data;
    public Image image;
    public CannonData selectedCannon; // ��������� ��������� CannonData
    private List<int> usedIndices = new List<int>(); // ������ �������������� ��������

    void Start()
    {
        text = GetComponent<Text>();

        int i = GetUniqueRandomIndex();
        selectedCannon = data[i]; // ��������� ��������� �����
        text.text = selectedCannon.price.ToString();
        image.sprite = selectedCannon.image;
    }

    int GetUniqueRandomIndex()
    {
        if (usedIndices.Count >= data.Length)
        {
            // ���� ��� ������� ��� ������������, ������� ������ ��� ���������� �������������
            usedIndices.Clear();
        }

        int i;
        do
        {
            i = Random.Range(0, data.Length);
        } while (usedIndices.Contains(i));

        usedIndices.Add(i); // ��������� ��������� ������ � ������ ��������������
        return i;
    }

    public CannonData GetSelectedCannon()
    {
        return selectedCannon;
    }
}
