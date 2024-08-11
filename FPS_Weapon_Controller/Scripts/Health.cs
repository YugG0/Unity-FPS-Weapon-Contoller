using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;

    public void Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
