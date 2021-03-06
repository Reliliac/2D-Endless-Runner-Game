using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorController : MonoBehaviour
{
    [Header("Templates")]
    public List<TerrainTemplateController> terrainTemplates;
    public float terrainTemplateWidth;

    [Header("Generator Area")]
    public Camera gameCamera;
    public float areaStartOffset;
    public float areaEndOffset;
    
    private List<GameObject> spawnedTerrain;

    private float lastGeneratedPositionX;

    private const float debugLineHeight = 10.0f;

    private float GetHorizontalPositionStart()
    {
       return gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + areaStartOffset;
    }

    private float GetHorizontalPositionEnd()
    {
     return gameCamera.ViewportToWorldPoint(new Vector2(1f, 0f)).x + areaEndOffset;
    }
    
    [Header("Force Early Template")]
    public List<TerrainTemplateController> earlyTerrainTemplates;

// debug
    private void OnDrawGizmos()
    {
    Vector3 areaStartPosition = transform.position;
    Vector3 areaEndPosition = transform.position;

    areaStartPosition.x = GetHorizontalPositionStart();
    areaEndPosition.x = GetHorizontalPositionEnd();

    Debug.DrawLine(areaStartPosition + Vector3.up * debugLineHeight / 2, areaStartPosition + Vector3.down * debugLineHeight / 2, Color.red);
    Debug.DrawLine(areaEndPosition + Vector3.up * debugLineHeight / 2, areaEndPosition + Vector3.down * debugLineHeight / 2, Color.red);
    }
    // Start is called before the first frame update
    private void Start()
    {
      spawnedTerrain = new List<GameObject>();

     lastGeneratedPositionX = GetHorizontalPositionStart();
       foreach (TerrainTemplateController terrain in earlyTerrainTemplates)
    {
        GenerateTerrain(lastGeneratedPositionX, terrain);
        lastGeneratedPositionX += terrainTemplateWidth;
    }

     while (lastGeneratedPositionX < GetHorizontalPositionEnd())
     {
            GenerateTerrain(lastGeneratedPositionX);
           lastGeneratedPositionX += terrainTemplateWidth;
     }
    }

    private void GenerateTerrain(float posX, TerrainTemplateController forceterrain = null)
    {
        GameObject newTerrain;
        if (forceterrain == null)
        {
            newTerrain = Instantiate(terrainTemplates[Random.Range(0, terrainTemplates.Count)].gameObject, transform);
        }
        else
        {
            newTerrain = Instantiate(forceterrain.gameObject, transform);
        }


        newTerrain.transform.position = new Vector2(posX, 0f);
        _spawnedTerrain.Add(newTerrain);
    }

    // Update is called once per frame
    private void Update()
    {
    while (lastGeneratedPositionX < GetHorizontalPositionEnd())
    {
        GenerateTerrain(lastGeneratedPositionX);
        lastGeneratedPositionX += terrainTemplateWidth;
    }
}
}
