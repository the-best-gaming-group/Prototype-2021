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
            StartCoroutine(SwitchSceneWithDelay(collision.gameObject.GetComponent<Enemy>().enemyType));
        }
    }

    private IEnumerator SwitchSceneWithDelay(string enemyType)
    {
        // Wait for the GameManager to be initialized
        yield return new WaitForEndOfFrame();

        // Store enemy type and player health in the GameManager script
        GameManager.Instance.enemyType = enemyType;
        GameManager.Instance.playerHealth = GetComponent<PlayerHealthBar>().currentHealth;
        Debug.Log("EnemyType = " + GameManager.Instance.enemyType);
        Debug.Log("PlayerHealth = " + GameManager.Instance.playerHealth);

        // Load the combat scene. Make sure you have this scene created in your Unity project.
        SceneManager.LoadScene("CombatScene");
    }
}
