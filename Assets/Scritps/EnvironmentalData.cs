using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentalDatabase", menuName = "EkoSkan/Environmental Database")]
public class EnvironmentalData : ScriptableObject
{
    [System.Serializable]
    public class EnvironmentalItem
    {
        public string name;
        public string description;
        public float environmentalScore; // 0-100
        public Sprite icon;
        public GameObject prefab;
    }

    public List<EnvironmentalItem> items = new List<EnvironmentalItem>();

    public EnvironmentalItem GetItemByName(string name)
    {
        return items.Find(item => item.name == name);
    }

    public EnvironmentalItem GetRandomItem()
    {
        if (items.Count > 0)
        {
            int randomIndex = Random.Range(0, items.Count);
            return items[randomIndex];
        }
        return null;
    }
}