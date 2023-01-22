using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Script that controls the behaviour of the enemies
/// </summary>
public class EnemyController : MonoBehaviour, IDamagable
{
     /// <summary>
     /// Reference to the gameData Scriptable object
     /// </summary>
     public GameData gameData;
     /// <summary>
     /// type of the enemy
     /// </summary>
     [HideInInspector]
     public EnemyType type;
     /// <summary>
     /// Actions to be executed on the update
     /// </summary>
     public Action OnUpdate;
     /// <summary>
     /// Health of the enemy
     /// </summary>
     public float health;
     /// /// <summary>
     /// Speed of the enemy
     /// </summary>
     public float speed;
     /// <summary>
     /// Variable that defines the next checkpoint for parth traveling
     /// </summary>
     [HideInInspector]
     public int goalCheckPoint;
     /// <summary>
     /// Used for the direction of the movement
     /// </summary>
     Vector3 direction;
     /// <summary>
     /// Variable that checks if the enemy hasnt been damaged
     /// </summary>
     private bool undamaged;

     [SerializeField]
     private GameObject healthBar;
     [SerializeField]
     private GameObject damageTextPrefab;
     /// <summary>
     /// constant of the enemy height used to avoid clipping with the terrain
     /// </summary>
     private const float ENEMY_HEIGHT = 1f;
     void Start()
     {
          InitializeEnemy();
          OnUpdate += TravelPath;
     }

     void Update()
     {
          OnUpdate?.Invoke();
          LookTowardsGoal();
     }
     /// <summary>
     /// Sets the rotation towards the current goal
     /// </summary>
     private void LookTowardsGoal()
     {
          this.transform.LookAt(new Vector3(gameData.path[goalCheckPoint].gameObject.transform.position.x, this.transform.position.y, gameData.path[goalCheckPoint].gameObject.transform.position.z));
     }

     void OnTriggerEnter(Collider enteringObjectCollider)
     {
          if (enteringObjectCollider.GetComponent<CastleController>() is not null)
          {
               enteringObjectCollider.GetComponent<IDamagable>().TakeDamage(this.gameObject);
               Destroy(this.gameObject);
          }
     }
     /// <summary>
     /// Sets the starting conditions of the enemy
     /// </summary>
     void InitializeEnemy()
     {
          this.undamaged = true;
          //Set the stats of the enemy
          this.health = EnemyStats.GetHealth(type);
          this.speed = EnemyStats.GetSpeed(type);
          //Setting the correct name (used in the collider interactions)
          this.gameObject.name = "Enemy";
          //Setting the correct Hight so the enemy does not clip with the ground
          this.transform.position += Vector3.up * ENEMY_HEIGHT;
          //Set the starting position to one tile before the end of the path
          goalCheckPoint = gameData.path.Count - 2;
          //Set the starting mov ement direction
          direction = GetXZDirection();
     }

     /// <summary>
     /// Updates the logic that enables the travel of the path
     /// </summary>
     public void TravelPath()
     {
          //check for edge case at the start of the path
          if (goalCheckPoint >= 0)
          {
               direction = GetXZDirection();
               //Travels the path
               this.transform.Translate(direction * Time.deltaTime * speed, Space.World);
               if (CalculateXZDistance() < 1f)
               {
                    goalCheckPoint--;
               }
          }
          else
          {
               goalCheckPoint++;
          }

     }
     /// <summary>
     /// Travels the path backwards(used for the thunder effect)
     /// </summary>
     public void TravelPathBackwards()
     {
          //check for end case at the start of the path
          if (goalCheckPoint <= gameData.path.Count - 2)
          {
               direction = GetXZDirection();
               //Travels the path
               this.transform.Translate(direction * Time.deltaTime * speed, Space.World);
               if (CalculateXZDistance() < 1f)
               {
                    goalCheckPoint++;
               }
          }
          else
          {
               goalCheckPoint--;
          }
     }
     /// <summary>
     /// Gets the direction in the XZ plane
     /// </summary>
     /// <returns>The direction that the enemi should follow/returns>
     Vector3 GetXZDirection()
     {
          Vector3 direction = Vector3.Normalize(gameData.path[goalCheckPoint].gameObject.transform.position - this.transform.position);
          direction.y = 0;
          return direction;
     }
     /// <summary>
     /// Calculate the distance to the checkpoint ignoring the y component (height)
     /// </summary>
     /// <returns>The distance beetween the 2 points</returns>
     float CalculateXZDistance()
     {
          Vector3 pointA = this.transform.position;
          Vector3 pointB = gameData.path[goalCheckPoint].gameObject.transform.position;
          pointA.y = 0;
          pointB.y = 0;
          return Vector3.Distance(pointA, pointB);
     }
     /// <summary>
     /// Function that destribes how the object takes damage
     /// </summary>
     /// <param name="damageAmount">The amount of damage taken</param>
     void IDamagable.TakeDamage(GameObject damager, float damageAmount, Func<EnemyController, IEnumerator> Effect)
     {
          if (damager.GetComponent<ShotController>() is not null)
          {
               this.health = this.health - damageAmount;
               ///creates the healthbar and the text damage
               CreateHealthBar();
               CreateDamageText();
               if (health <= 0)
               {
                    Destroy(this.gameObject);
               }
               if (Effect != null)
               {
                    StartCoroutine(Effect(this));
               }
          }
          /// <summary>
          /// Function that creates the damage text and sets the damage amount
          /// </summary>
          void CreateDamageText()
          {
               DamageTextController damageText = Instantiate(damageTextPrefab, this.transform.position, Quaternion.identity, this.transform).GetComponent<DamageTextController>();
               damageText.SetUp(damageAmount);
          }
          void CreateHealthBar()
          {
               if (undamaged)
               {
                    Instantiate(healthBar, this.transform.position, this.transform.rotation, this.transform);
                    undamaged = false;
               }
          }
     }
}
