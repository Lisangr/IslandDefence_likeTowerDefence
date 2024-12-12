using UnityEngine;
using UnityEngine.UI;
using YG;

public class MyGames : MonoBehaviour
{
    // ������ �� ������ � ���������
    public Button button;

    // URL ��� ��������
    private string urlRU = "https://yandex.ru/games/developer/87388";
    private string urlEN = "https://yandex.com/games/developer/87388";

    void Start()
    {
        // �������� ������ � ������� ������� ������
        if (button != null)
        {
            button.onClick.AddListener(OpenURL);
        }
    }

    // ����� ��� �������� URL
    void OpenURL()
    {
        if (YandexGame.EnvironmentData.domain == "ru")
        {
            Application.OpenURL("https://yandex." + YandexGame.EnvironmentData.domain + "/games/developer/87388");
        }
        else
        {
            Application.OpenURL("https://yandex." + YandexGame.EnvironmentData.domain + "/games/developer/87388");
        }
    }
}
