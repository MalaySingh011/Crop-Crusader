using UnityEngine;

public class Food : MonoBehaviour
{
    public static int totalFoodCount = 0;
    public static int foodCount = 0;

    void Start()
    {
        foodCount = FindObjectsByType<Food>(FindObjectsSortMode.None).Length;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            foodCount--;
            Destroy(gameObject);
        }
    }
}
