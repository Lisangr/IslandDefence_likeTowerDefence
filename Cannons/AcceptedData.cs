using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AcceptedData : MonoBehaviour
{
    private Text text;
    public CannonData[] data;
    public Image image;
    public CannonData selectedCannon; // Выбранный экземпляр CannonData
    private List<int> usedIndices = new List<int>(); // Список использованных индексов

    void Start()
    {
        text = GetComponent<Text>();

        int i = GetUniqueRandomIndex();
        selectedCannon = data[i]; // Сохраняем выбранную пушку
        text.text = selectedCannon.price.ToString();
        image.sprite = selectedCannon.image;
    }

    int GetUniqueRandomIndex()
    {
        if (usedIndices.Count >= data.Length)
        {
            // Если все индексы уже использованы, очищаем список для повторного использования
            usedIndices.Clear();
        }

        int i;
        do
        {
            i = Random.Range(0, data.Length);
        } while (usedIndices.Contains(i));

        usedIndices.Add(i); // Добавляем выбранный индекс в список использованных
        return i;
    }

    public CannonData GetSelectedCannon()
    {
        return selectedCannon;
    }
}
