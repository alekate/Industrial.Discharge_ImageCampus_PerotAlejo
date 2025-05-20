using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    private Vector3 velocity;
    public float lifeTime = 5f;
    public int damage = 1;
    private float timer;

    public void Init(Vector3 vel)
    {
        velocity = vel;
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Health player = other.GetComponent<Player_Health>();

            if (player != null)
            {
                player.TakeDamage(1);
            }

            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }

    public void OnActivate()
    {
        timer = 0f;
    }

    public void OnDeactivate()
    {
        velocity = Vector3.zero;
    }

    private void OnEnable()
    {
        OnActivate();
    }

    private void OnDisable()
    {
        OnDeactivate();
    }
}
