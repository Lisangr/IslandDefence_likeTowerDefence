using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewCannon", menuName = "ScriptableObjects/CannonData", order = 1)]
public class CannonData : ScriptableObject
{
    public int damage;
    public int price;
    public GameObject prefab;
    public Sprite image;
}