using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class TerrainController : MonoBehaviour
{
     public GameObject roadPrefab;
     public GameObject castlePrefab;
     public GameObject portalPrefab;
     private Grid terrain;
     public List<Tile> path;
     void Awake()
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
          path = getFullPath(5);

          //Remove repeated elements from the path
          path = path.Distinct().ToList();

          Tile castle = path[0];
          Tile portal = path[path.Count - 1];

          //Assign the corresponding prefabs to the base and the portal
          castle.gameObject = castlePrefab;
          portal.gameObject = portalPrefab;

          //Draw the grid (it uses this.gameobject as a parameter to assign the terrain GameObject as the father of all the tiles)
          terrain.drawPath(path, this.gameObject);

          castle.gameObject.transform.LookAt(path[1].gameObject.transform);
          portal.gameObject.transform.LookAt(portal.previous.gameObject.transform);
     }
     //This function returns random tiles from the terrain grid
     Tile getRandomTile()
     {
          int x = Random.Range(0, terrain.width);
          int y = Random.Range(0, terrain.height);
          return terrain.tiles[x, y];
     }

     //Function used to optain the path beetween all the points
     List<Tile> getFullPath(int numberOfPoints)
     {
          List<Tile> path = new List<Tile>();
          Tile start = getRandomTile();
          int i = 0;
          while (i < numberOfPoints)
          {
               Tile goal = getRandomTile();
               if (goal.gameObject == null)
               {
                    List<Tile> semiPath = semiPath = terrain.AStar(start, goal);

                    if (semiPath != null)
                    {
                         assignatePrefab(semiPath);
                         path.AddRange(semiPath);
                         start = goal;
                         i++;
                    }
                    else
                    {
                         i++;
                    }
               }
               else
               {
                    goal = getRandomTile();
               }
          }
          return path;
     }

     // Function used to assignate the respective prefab when the semipaths are being generated
     void assignatePrefab(List<Tile> semiPath)
     {
          foreach (Tile tile in semiPath)
          {
               tile.gameObject = roadPrefab;
          }
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
     private int determineH(Tile current, Tile goal)
     {
          //We don't use movement cost cause for all tile the movement cost is 1.
          return Mathf.Abs(current.x - goal.x) + Mathf.Abs(current.y - goal.y);
     }

     //Implementation of A* algorithm to find a path beetween 2 tiles of the grid
     public List<Tile> AStar(Tile start, Tile goal)
     {
          List<Tile> searchedTiles = new List<Tile>();
          List<Tile> unsearchedTiles = new List<Tile>();

          //Add start node to the search Queue;
          unsearchedTiles.Add(start);

          //Continue while there are still tiles in the queue
          while (unsearchedTiles.Count > 0)
          {
               //Get the first element of the list (which is the one with lowest f since the list orders itself at the end of each iteration )
               Tile currentTile = unsearchedTiles[0];
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
                    returnList.Add(start);
                    //Reverse the list to get the path from start to goal 
                    returnList.Reverse();
                    return returnList;
               }
               else
               {
                    //Check all 4 neighbours (since we are only allowing 4 movement directions) and see if is an already visited tile
                    //This ckecks the neighbours clockwise
                    //Sets the previous node of the neighbour to the current tile
                    //Update their f values and add them into the list
                    //This is inside a try-catch block for possible index out of range exeption in the edges of the grid
                    try
                    {
                         //Top Neighbour
                         Tile neighbour = tiles[currentTile.x, currentTile.y + 1];
                         if (!searchedTiles.Contains(neighbour) && neighbour.gameObject == null)
                         {
                              if (!unsearchedTiles.Contains(neighbour))
                              {
                                   neighbour.g = currentTile.g + 1;
                                   neighbour.f = determineH(neighbour, goal) + neighbour.g;
                                   neighbour.previous = currentTile;
                                   unsearchedTiles.Add(neighbour);
                              }
                              else
                              {
                                   int tentativeG = currentTile.g + 1;
                                   if (tentativeG < neighbour.g)
                                   {
                                        neighbour.previous = currentTile;
                                        neighbour.g = tentativeG;
                                        neighbour.f = determineH(neighbour, goal) + neighbour.g;
                                   }
                              }
                         }
                    }
                    catch { }
                    try
                    {
                         //Right Neighbour
                         Tile neighbour = tiles[currentTile.x + 1, currentTile.y];
                         if (!searchedTiles.Contains(neighbour) && neighbour.gameObject == null)
                         {
                              if (!unsearchedTiles.Contains(neighbour))
                              {
                                   neighbour.g = currentTile.g + 1;
                                   neighbour.f = determineH(neighbour, goal) + neighbour.g;
                                   neighbour.previous = currentTile;
                                   unsearchedTiles.Add(neighbour);
                              }
                              else
                              {
                                   int tentativeG = currentTile.g + 1;
                                   if (tentativeG < neighbour.g)
                                   {
                                        neighbour.previous = currentTile;
                                        neighbour.g = tentativeG;
                                        neighbour.f = determineH(neighbour, goal) + neighbour.g;
                                   }
                              }
                         }
                    }
                    catch { }
                    try
                    {
                         //Bottom Neighbour
                         Tile neighbour = tiles[currentTile.x, currentTile.y - 1];
                         if (!searchedTiles.Contains(neighbour) && neighbour.gameObject == null)
                         {
                              if (!unsearchedTiles.Contains(neighbour))
                              {
                                   neighbour.g = currentTile.g + 1;
                                   neighbour.f = determineH(neighbour, goal) + neighbour.g;
                                   neighbour.previous = currentTile;
                                   unsearchedTiles.Add(neighbour);
                              }
                              else
                              {
                                   int tentativeG = currentTile.g + 1;
                                   if (tentativeG < neighbour.g)
                                   {
                                        neighbour.previous = currentTile;
                                        neighbour.g = tentativeG;
                                        neighbour.f = determineH(neighbour, goal) + neighbour.g;
                                   }
                              }
                         }
                    }
                    catch { }
                    try
                    {
                         //Left  Neighbour
                         Tile neighbour = tiles[currentTile.x - 1, currentTile.y];
                         if (!searchedTiles.Contains(neighbour) && neighbour.gameObject == null)
                         {
                              if (!unsearchedTiles.Contains(neighbour))
                              {
                                   neighbour.g = currentTile.g + 1;
                                   neighbour.f = determineH(neighbour, goal) + neighbour.g;
                                   neighbour.previous = currentTile;
                                   unsearchedTiles.Add(neighbour);
                              }
                              else
                              {
                                   int tentativeG = currentTile.g + 1;
                                   if (tentativeG < neighbour.g)
                                   {
                                        neighbour.previous = currentTile;
                                        neighbour.g = tentativeG;
                                        neighbour.f = determineH(neighbour, goal) + neighbour.g;
                                   }
                              }
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
     public void drawPath(List<Tile> path, GameObject parent)
     {
          foreach (Tile tile in path)
          {
               try
               {
                    tile.gameObject = Object.Instantiate(tile.gameObject, new Vector3(tile.x, 0, tile.y) * tile.size, tile.gameObject.transform.rotation, parent.transform);
                    tile.gameObject.name = tile.x + " " + tile.y;
                    tile.gameObject.transform.LookAt(tile.previous.gameObject.transform);
               }
               catch { }
          }
     }
}

public class Tile
{
     public GameObject gameObject;
     public int x;
     public int y;
     public float size;
     public int f;
     public int g;
     public Tile previous;
     public Tile(int x, int y, float size = 10)
     {
          this.x = x;
          this.y = y;
          this.size = size;
     }

}