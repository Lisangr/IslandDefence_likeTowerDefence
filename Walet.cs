using UnityEngine;
using UnityEngine.UI;
using YG;
public class Walet : MonoBehaviour
{
    private int _totalGold;
    private const string GoldKey = "GoldKey"; // Ключ для сохранения в PlayerPrefs
    public Text goldText; // Текст для отображения золота в UI
    private int currentGold;
    private void OnEnable()
    {
        Enemy.OnEnemyDeath += AddGold;
        YandexGame.GetDataEvent += LoadSaveFromCloud;
        LoadGold(); // Загружаем золото при включении
        UpdateGoldUI(); // Обновляем UI после загрузки
    }
    public void LoadSaveFromCloud()
    {
        currentGold = YandexGame.savesData.gold;
        Debug.Log("Всего монет загружено" + currentGold);
    }
    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= AddGold;
        YandexGame.GetDataEvent -= LoadSaveFromCloud;
        SaveGold(); // Сохраняем золото при отключении
    }

    private void AddGold(int gold)
    {
        _totalGold += gold;
        SaveGold(); // Сохраняем золото каждый раз при добавлении
        UpdateGoldUI(); // Обновляем UI каждый раз при добавлении
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt(GoldKey, _totalGold);
        PlayerPrefs.Save(); // Принудительно сохраняем изменения
        YandexGame.savesData.gold = _totalGold;
        YandexGame.SaveProgress();

        YandexGame.NewLeaderboardScores("Gold", _totalGold);
    }

    private void LoadGold()
    {
        if (PlayerPrefs.HasKey(GoldKey))
        {
            _totalGold = PlayerPrefs.GetInt(GoldKey);
        }
        else
        {
            _totalGold = 0; // Если нет сохранённых данных, начнем с нуля
        }
    }

    private void UpdateGoldUI()
    {
        if (_totalGold >= currentGold) 
        {
            goldText.text = _totalGold.ToString(); // Обновляем текст UI
        }
        else
        {
            _totalGold = currentGold;            
            goldText.text = _totalGold.ToString();
        }
    }

    public int GetTotalGold()
    {
        return _totalGold;
    }

    public void ReduceGold(int amount)
    {
        if (_totalGold >= amount)
        {
            _totalGold -= amount;
            SaveGold(); // Сохраняем новое значение золота
            UpdateGoldUI(); // Обновляем UI
        }
        else
        {
            Debug.LogError("Недостаточно золота для этой операции.");
        }
    }

    public void ResetGold()
    {
        _totalGold = 0;
        PlayerPrefs.DeleteKey(GoldKey); // Удаляет сохраненное значение золота
        UpdateGoldUI(); // Обновляем UI при сбросе
    }
}
