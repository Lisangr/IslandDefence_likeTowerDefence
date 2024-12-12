using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    private int counter;
    private void OnEnable()
    {
        Enemy.OnEnemyDestroy += Enemy_OnEnemyDestroy;
    }

    private void Enemy_OnEnemyDestroy()
    {
        counter++;
        if (counter >= 200)
        {
            CanvasButtons canvasButtons = FindObjectOfType<CanvasButtons>();
            canvasButtons.ShowWinPanel();
        }
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDestroy -= Enemy_OnEnemyDestroy;
    }
}
