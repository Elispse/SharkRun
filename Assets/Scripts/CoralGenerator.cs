using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEditor.VersionControl;

public class CoralGenerator : MonoBehaviour
{
    [SerializeField] int LoadSecondsMinimum = 0;
    [SerializeField] int LoadSecondsMaximum = 5;

    private int weightLost = 0;
    private GameObject lastAsset;

    private int TotalWeight = 0;
    private string path = "Assets/Prefabs/Coral";
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
            WeightScript wScript = asset.GetComponent<WeightScript>();
            TotalWeight += wScript.Weight;
            Obstacles.Add(asset);
        }
    }

    private GameObject GetRandomObject()
    {
        int rnd = Random.Range(1, TotalWeight + 1);
        foreach (var item in Obstacles)
        {
            rnd -= item.GetComponent<WeightScript>().Weight;
            if (rnd > 0) continue;
            return item;
        }
        return null;
    }

    IEnumerator GenerateRandomObject(int seconds)
    {
        yield return new WaitForSeconds(seconds);

        GameObject obj = GetRandomObject();
        if (obj == null)
        {
            Debug.Log("Major issue in random algo");
            yield break;
        }

        GameObject newObstacle = Instantiate(obj);

        if (lastAsset != null)
        {
            lastAsset.GetComponent<WeightScript>().Weight += weightLost;
        }

        foreach (var obstacle in Obstacles)
        {
            if (newObstacle == obstacle)
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
