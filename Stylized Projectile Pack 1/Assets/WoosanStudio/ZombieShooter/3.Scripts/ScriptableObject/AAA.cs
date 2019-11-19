using UnityEngine;

[CreateAssetMenu(fileName = "New AAA", menuName = "AAA/AAA Data", order = 52)]
public class AAA : ScriptableObject
{
    [SerializeField]
    private string swordName;
    [SerializeField]
    private string description;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private int goldCost;
    [SerializeField]
    private int attackDamage;
}