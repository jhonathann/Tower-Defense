using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainController : MonoBehaviour
{
     public GameObject roadPrefab;
     public GameObject castlePrefab;
     public GameObject portalPrefab;
     private Grid terrain;
     void Start()
     {
          //Function that creates the terrain 
          createTerrain();
     }
     void Update()
     {

     }
     void createTerrain()
     {
          //Create the terrain grid
          terrain = new Grid(20, 20);

          //Getting the full path using diferent points (min 2 points start and base)(else it will throw an indexOutOfRange exeption)
          List<Tile> path = getFullPath(getPoints(5));

          //Assign the road prefab to all the tiles in the path
          foreach (Tile tile in path)
          {
               tile.prefab = roadPrefab;
          }

          Tile castle = path[0];
          Tile portal = path[path.Count - 1];

          //Assign the corresponding prefabs to the base and the portal
          castle.prefab = castlePrefab;
          portal.prefab = portalPrefab;

          //Draw the grid (it uses this.gameobject as a parameter to assign the terrain GameObject as the father of all the tiles)
          terrain.drawGrid(this.gameObject);

          castle.prefab.transform.LookAt(path[1].prefab.transform);
          portal.prefab.transform.LookAt(portal.previous.prefab.transform);
     }

     //This list keeps track of the already assigned tiles (used in getRandomTile())
     List<Tile> assignedTiles = new List<Tile>();

     //This function returns random tiles from the terrain grid
     Tile getRandomTile()
     {
     inicio:
          int x = Random.Range(0, terrain.width);
          int y = Random.Range(0, terrain.height);
          if (!assignedTiles.Contains(terrain.tiles[x, y]))
          {
               assignedTiles.Add(terrain.tiles[x, y]);
               return terrain.tiles[x, y];
          }
          else
          {
               goto inicio; //If the random gives an already assigned tile then tries again
          }
     }

     //function that generates a list of the points used to create the full path
     List<Tile> getPoints(int numberOfPoints)
     {
          List<Tile> points = new List<Tile>();
          for (int i = 0; i < numberOfPoints; i++)
          {
               points.Add(getRandomTile());
          }
          return points;
     }

     //Function used to optain the path beetween all the points
     List<Tile> getFullPath(List<Tile> points)
     {
          List<Tile> path = new List<Tile>();
          for (int i = 0; i < points.Count - 1; i++)
          {
               path.AddRange(terrain.AStar(points[i], points[i + 1]));
          }
          return path;
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

                    //Add the start node (to keep the start node as the first path node and the goal node as the last)
                    returnList.Add(start);
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
          return null;
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
                         tiles[i, j].prefab = Object.Instantiate(tiles[i, j].prefab, new Vector3(i, 0, j) * tiles[i, j].size, tiles[i, j].prefab.transform.rotation, parent.transform);
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