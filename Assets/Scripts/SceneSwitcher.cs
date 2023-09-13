using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Store enemy type and player health in a GameManager script
            GameManager.Instance.enemyType = collision.gameObject.GetComponent<Enemy>().enemyType;
            GameManager.Instance.playerHealth = GetComponent<Player>().currentHealth;
            Debug.Log("EnemyType = " + GameManager.Instance.enemyType);
            Debug.Log("PlayerHealth = " + GameManager.Instance.playerHealth);
            // Load the combat scene. Make sure you have this scene created in your Unity project.
            SceneManager.LoadScene("CombatScene");
        }
    }
}
