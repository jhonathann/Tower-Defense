using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainController : MonoBehaviour
{
     public GameObject tilePrefab;
     public GameObject basePrefab;
     public GameObject portalPrefab;
     private Grid terrain;
     private void Start()
     {
          //Function that creates the terrain 
          createTerrain();
     }
     private void Update()
     {

     }
     private void createTerrain()
     {
          //Create the terrain grid
          terrain = new Grid(20, 20, 10, tilePrefab, this.gameObject);
          //Define the base and the portal locations given by randomizers
          int startx = Random.Range(0, terrain.width);
          int starty = Random.Range(0, terrain.height);
          int goalx = Random.Range(0, terrain.width);
          int goaly = Random.Range(0, terrain.height);

          //Optain the path beetween the 2 tiles 
          List<Tile> Path = terrain.AStar(terrain.tiles[startx, starty], terrain.tiles[goalx, goaly]);

          foreach (Tile tile in Path)
          {
               tile.prefab.GetComponent<Renderer>().material.color = Color.black;
          }

          terrain.tiles[startx, starty].prefab = basePrefab;
          terrain.tiles[goalx, goaly].prefab = portalPrefab;

          terrain.drawGrid(this.gameObject);
     }
}

public class Grid
{
     public int width;
     public int height;
     private float size; // tile size
     public Tile[,] tiles;
     public Grid(int width, int height, float size, GameObject prefab, GameObject parent)
     {
          this.width = width;
          this.height = height;
          this.size = size;
          tiles = new Tile[width, height];
          for (int i = 0; i < tiles.GetLength(0); i++)
          {
               for (int j = 0; j < tiles.GetLength(1); j++)
               {
                    tiles[i, j] = new Tile(size, i, j, prefab, parent);
               }
          }
     }

     //Function used to compare the f of 2 tiles. Used inside de A* algorithm
     private int compareFs(Tile a, Tile b)
     {
          if (a.f > b.f) return 1;

          if (a.f < b.f) return -1;

          if (a.f == b.f) return 0;

          return 0;
     }

     public List<Tile> AStar(Tile start, Tile goal)
     {
          List<Tile> closedList = new List<Tile>();
          List<Tile> openList = new List<Tile>();
          Tile previous = start;

          openList.Add(start);

          while (openList.Count > 0)
          {
               Tile currentTile = openList[0];
               currentTile.previous = previous;
               previous = currentTile;
               openList.Remove(openList[0]);
               if (currentTile == goal)
               {
                    while (currentTile != start)
                    {
                         closedList.Add(currentTile);
                         currentTile = currentTile.previous;
                    }
                    closedList.Reverse();
                    return closedList;
               }
               else
               {
                    try
                    {
                         Tile top = tiles[currentTile.x, currentTile.y + 1];
                         top.f = Mathf.Abs(top.x - goal.x) + Mathf.Abs(top.y - goal.y);
                         openList.Add(top);
                    }
                    catch { }
                    try
                    {
                         Tile right = tiles[currentTile.x + 1, currentTile.y];
                         right.f = Mathf.Abs(right.x - goal.x) + Mathf.Abs(right.y - goal.y);
                         openList.Add(right);
                    }
                    catch { }
                    try
                    {
                         Tile bottom = tiles[currentTile.x, currentTile.y - 1];
                         bottom.f = Mathf.Abs(bottom.x - goal.x) + Mathf.Abs(bottom.y - goal.y);
                         openList.Add(bottom);
                    }
                    catch { }
                    try
                    {
                         Tile left = tiles[currentTile.x - 1, currentTile.y];
                         left.f = Mathf.Abs(left.x - goal.x) + Mathf.Abs(left.y - goal.y);
                         openList.Add(left);
                    }
                    catch { }
                    openList.Sort(compareFs);
               }
          }
          Debug.Log("Path not found");
          return new List<Tile>();
     }

     public void drawGrid(GameObject parent)
     {
          for (int i = 0; i < tiles.GetLength(0); i++)
          {
               for (int j = 0; j < tiles.GetLength(1); j++)
               {
                    Object.Instantiate(tiles[i, j].prefab, new Vector3(i, 0, j) * size, Quaternion.Euler(90, 0, 0), parent.transform);
               }
          }
     }
}

public class Tile
{
     public GameObject prefab;
     public float f = 0f;
     public int x;
     public int y;
     public Tile previous = null;
     public Tile(float size, int x, int y, GameObject prefab, GameObject parent)
     {
          this.x = x;
          this.y = y;
          this.prefab = Object.Instantiate(prefab, new Vector3(x, 0, y) * size, Quaternion.Euler(90, 0, 0), parent.transform);
     }

}