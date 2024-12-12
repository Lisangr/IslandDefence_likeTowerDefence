using UnityEngine;
using UnityEngine.UI;
using YG;
public class Walet : MonoBehaviour
{
    private int _totalGold;
    private const string GoldKey = "GoldKey"; // ���� ��� ���������� � PlayerPrefs
    public Text goldText; // ����� ��� ����������� ������ � UI
    private int currentGold;
    private void OnEnable()
    {
        Enemy.OnEnemyDeath += AddGold;
        YandexGame.GetDataEvent += LoadSaveFromCloud;
        LoadGold(); // ��������� ������ ��� ���������
        UpdateGoldUI(); // ��������� UI ����� ��������
    }
    public void LoadSaveFromCloud()
    {
        currentGold = YandexGame.savesData.gold;
        Debug.Log("����� ����� ���������" + currentGold);
    }
    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= AddGold;
        YandexGame.GetDataEvent -= LoadSaveFromCloud;
        SaveGold(); // ��������� ������ ��� ����������
    }

    private void AddGold(int gold)
    {
        _totalGold += gold;
        SaveGold(); // ��������� ������ ������ ��� ��� ����������
        UpdateGoldUI(); // ��������� UI ������ ��� ��� ����������
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt(GoldKey, _totalGold);
        PlayerPrefs.Save(); // ������������� ��������� ���������
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
            _totalGold = 0; // ���� ��� ���������� ������, ������ � ����
        }
    }

    private void UpdateGoldUI()
    {
        if (_totalGold >= currentGold) 
        {
            goldText.text = _totalGold.ToString(); // ��������� ����� UI
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
            SaveGold(); // ��������� ����� �������� ������
            UpdateGoldUI(); // ��������� UI
        }
        else
        {
            Debug.LogError("������������ ������ ��� ���� ��������.");
        }
    }

    public void ResetGold()
    {
        _totalGold = 0;
        PlayerPrefs.DeleteKey(GoldKey); // ������� ����������� �������� ������
        UpdateGoldUI(); // ��������� UI ��� ������
    }
}
