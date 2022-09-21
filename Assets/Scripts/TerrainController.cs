using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
     public GameObject tilePrefab;
     public GameObject basePrefab;
     public GameObject portalPrefab;
     private Vector2 size;
     private GameObject[][] terrain;
     void Start()
     {
          initialization();
     }
     void Update()
     {

     }
     private void initialization()
     {
          size = new Vector3(10, 10);
          terrain = new GameObject[(int)size.x][];

          int baseLocation = Random.Range(0, (int)size.x - 1);
          int portalLocation = Random.Range(0, (int)size.x - 1);

          for (int i = 0; i < size.x; i++)
          {
               terrain[i] = new GameObject[(int)size.y];
               for (int j = 0; j < size.y; j++)
               {
                    terrain[i][j] = Instantiate(tilePrefab, new Vector3(5 + 10 * i, 0, 5 + 10 * j), tilePrefab.transform.rotation);
                    if (i == 0 && j == baseLocation)
                    {
                         terrain[i][j] = Instantiate(basePrefab, new Vector3(5 + 10 * i, 0, 5 + 10 * j), basePrefab.transform.rotation);
                    }
                    if (i == 9 && j == portalLocation)
                    {
                         terrain[i][j] = Instantiate(portalPrefab, new Vector3(5 + 10 * i, 0, 5 + 10 * j), portalPrefab.transform.rotation);
                         //terrain[i][j].GetComponent<Renderer>().material.color = Color.red;
                    }
               }
          }
     }
}
