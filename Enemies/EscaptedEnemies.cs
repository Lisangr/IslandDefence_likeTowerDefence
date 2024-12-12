using UnityEngine;
using UnityEngine.UI;

public class EscaptedEnemies : MonoBehaviour
{
    private Text text;
    private int escaptedEnemies = 0;
    private void OnEnable()
    {
        text = GetComponent<Text>();
        Enemy.OnEnemyFinished += Enemy_OnEnemyFinished;
    }

    private void Enemy_OnEnemyFinished()
    {
        escaptedEnemies++;
        text.text = escaptedEnemies.ToString() + "/15";

        if (escaptedEnemies >= 15)
        {
            CanvasButtons canvasButtons = FindObjectOfType<CanvasButtons>();
            canvasButtons.ShowDefeatPanel();
        }
    }
    private void OnDisable()
    {
        Enemy.OnEnemyFinished -= Enemy_OnEnemyFinished;
    }
}


