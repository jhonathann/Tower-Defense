using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainController : MonoBehaviour
{
     public GameObject roadPrefab;
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
          terrain = new Grid(20, 20);
          //Define the base and the portal locations given by randomizers
          int baseX = Random.Range(0, terrain.width);
          int baseY = Random.Range(0, terrain.height);
          int portalX = Random.Range(0, terrain.width);
          int portalY = Random.Range(0, terrain.height);

          //Optain the path beetween the 2 tiles 
          List<Tile> Path = terrain.AStar(terrain.tiles[baseX, baseY], terrain.tiles[portalX, portalY]);

          //Assign the tile prefab to all the tiles in the path
          foreach (Tile tile in Path)
          {
               tile.prefab = roadPrefab;
          }

          //Assign the corresponding prefabs to the base and the portal
          terrain.tiles[baseX, baseY].prefab = basePrefab;
          terrain.tiles[portalX, portalY].prefab = portalPrefab;

          terrain.drawGrid(this.gameObject);
     }
}

//Class that describes a grid that will be used to implement the terrain
public class Grid
{
     public int width;
     public int height;
     public Tile[,] tiles;
     public Grid(int width, int height)
     {
          this.width = width;
          this.height = height;
          tiles = new Tile[width, height];
          for (int i = 0; i < tiles.GetLength(0); i++)
          {
               for (int j = 0; j < tiles.GetLength(1); j++)
               {
                    tiles[i, j] = new Tile(i, j);
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

     //Determines the total estimated cost beetween two tiles.
     private float determineF(Tile current, Tile goal)
     {
          //We don't use movement cost cause for all tile the movement cost is 1.
          return Mathf.Abs(current.x - goal.x) + Mathf.Abs(current.y - goal.y);
     }

     //Implementation of A* algorithm to find a path beetween 2 tiles of the grid
     public List<Tile> AStar(Tile start, Tile goal)
     {
          List<Tile> searchedTiles = new List<Tile>();
          List<Tile> unsearchedTiles = new List<Tile>();
          Tile previous = start;

          //Add start node to the search Queue;
          unsearchedTiles.Add(start);

          //Continue while there are still tiles in the queue
          while (unsearchedTiles.Count > 0)
          {
               //Get the first element of the list (which is the one with lowest f since the list orders itself at the end of each iteration )
               Tile currentTile = unsearchedTiles[0];
               //Set the previous tile to be able to track the path back
               currentTile.previous = previous;
               previous = currentTile;
               //Remove the current tile since is already going to be searched.
               unsearchedTiles.Remove(unsearchedTiles[0]);
               //Add the searched tile to the list of searchedTiles to avoid searching the same tile twice twice
               searchedTiles.Add(currentTile);
               //Check if the goal has been reached
               if (currentTile == goal)
               {
                    //Create list with the path that will be returned
                    List<Tile> returnList = new List<Tile>();

                    //Retrace back the path finded
                    while (currentTile != start)
                    {
                         returnList.Add(currentTile);
                         currentTile = currentTile.previous;
                    }

                    //Reverse the list to get the path from start to goal 
                    returnList.Reverse();
                    return returnList;
               }
               else
               {
                    //Check all 4 neighthbours (since we are only allowing 4 movement directions) and see if is an already visited tile
                    //This ckecks the neightbours clockwise
                    //Update their f values and add them into the list
                    //This is inside a try-catch block for possible index out of range exeption in the edges of the grid
                    try
                    {
                         Tile top = tiles[currentTile.x, currentTile.y + 1];
                         if (!searchedTiles.Contains(top))
                         {
                              top.f = determineF(top, goal);
                              unsearchedTiles.Add(top);
                         }
                    }
                    catch { }
                    try
                    {
                         Tile right = tiles[currentTile.x + 1, currentTile.y];
                         if (!searchedTiles.Contains(right))
                         {
                              right.f = determineF(right, goal);
                              unsearchedTiles.Add(right);
                         }
                    }
                    catch { }
                    try
                    {
                         Tile bottom = tiles[currentTile.x, currentTile.y - 1];
                         if (!searchedTiles.Contains(bottom))
                         {
                              bottom.f = determineF(bottom, goal);
                              unsearchedTiles.Add(bottom);
                         }
                    }
                    catch { }
                    try
                    {
                         Tile left = tiles[currentTile.x - 1, currentTile.y];
                         if (!searchedTiles.Contains(left))
                         {
                              left.f = determineF(left, goal);
                              unsearchedTiles.Add(left);
                         }
                    }
                    catch { }

                    //Sort the list to keep the tile with lowest f as the element 0 of the list
                    unsearchedTiles.Sort(compareFs);
               }
          }
          //This is only reachable if there is no possible path
          Debug.Log("Path not found");
          return new List<Tile>();
     }

     //Function that draws the whole grid instantiating the prefab that every tile has
     public void drawGrid(GameObject parent)
     {
          for (int i = 0; i < tiles.GetLength(0); i++)
          {
               for (int j = 0; j < tiles.GetLength(1); j++)
               {
                    //This is inside a try-catch block in case the tile doesn't have an associated prefab
                    try
                    {
                         Object.Instantiate(tiles[i, j].prefab, new Vector3(i, 0, j) * tiles[i, j].size, tiles[i, j].prefab.transform.rotation, parent.transform);
                    }
                    catch { }

               }
          }
     }
}

public class Tile
{
     public GameObject prefab;
     public int x;
     public int y;
     public float size;
     public float f;
     public Tile previous;
     public Tile(int x, int y, float size = 10)
     {
          this.x = x;
          this.y = y;
          this.size = size;
     }

}