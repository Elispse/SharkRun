using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] int LoadSecondsMinimum = 0;
    [SerializeField] int LoadSecondsMaximum = 5;
    [SerializeField] GameObject Player;

    private int weightLost = 0;
    private GameObject lastAsset;

    private string path = "Assets/Prefabs/Obstacles";
    private List<GameObject> Obstacles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GetAllPrefabs();
        Debug.Log("Script Loaded");

        StartCoroutine(GenerateRandomObject(1));
    }

    private void GetAllPrefabs()
    {
        string[] prefabFiles = Directory.GetFiles(path, "*.prefab");
        foreach (var file in prefabFiles)
        {
            GameObject asset = (GameObject) AssetDatabase.LoadAssetAtPath(file, typeof(GameObject));
            Obstacles.Add(asset);
        }
    }

    private GameObject GetObjectFromListByWeight(List<GameObject> values, int weight)
    {
        int rnd = Random.Range(1, weight + 1);
        foreach (var item in values)
        {
            rnd -= item.GetComponent<WeightScript>().Weight;
            if (rnd > 0) continue;

            return item;
        }
        return null;
    }

    private GameObject GetRandomObjectInArea(string Area)
    {
        var filtered = Obstacles.Where(item => item.GetComponent<WeightScript>().Area.ToString() == Area).ToList();
        int filteredTotalWeight = 0;

        foreach (var item in filtered)
        {
            filteredTotalWeight += item.GetComponent<WeightScript>().Weight;
        }

        return GetObjectFromListByWeight((List<GameObject>)filtered, filteredTotalWeight);
    }

    private string GetPlayerArea()
    {
        Vector3 CurrentVector = Player.transform.position;
        switch (CurrentVector.y)
        {
            case > 2:
                return "TOP";
            case > -1:
                return "MIDDLE";
            case > -5:
                return "BOTTOM";
        }
        return null;
    }

    IEnumerator GenerateRandomObject(int seconds)
    {
        yield return new WaitForSeconds(seconds);

        string Area = GetPlayerArea();
        Area = Area != null ? Area : "BOTTOM";
        GameObject obj = GetRandomObjectInArea(Area);

        if (obj == null)
        {
            Debug.Log("Major issue in random algo");
            yield break;
        }

        GameObject newObstacle = Instantiate(obj);
        newObstacle.name = obj.name;

        if (lastAsset != null)
        {
            lastAsset.GetComponent<WeightScript>().Weight += weightLost;
        }

        foreach (var obstacle in Obstacles)
        {
            if (newObstacle.name == obstacle.name)
            {
                lastAsset = obj;
                int weight = obstacle.GetComponent<WeightScript>().Weight;
                int newWeight = weight / 2;
                weightLost = weight - newWeight;
                obstacle.GetComponent<WeightScript>().Weight = newWeight;
            }
        }

        int rnd = Random.Range(LoadSecondsMinimum, LoadSecondsMaximum);
        StartCoroutine("GenerateRandomObject", rnd);
        yield break;
    }
}
