using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] protected float speed;
    [SerializeField] protected float lifeTime = 5f;
    [SerializeField] protected int damage = 1;

    protected Vector3 velocity;
    private float timer;

    public virtual void Initialize(Vector3 direction, float duration)
    {
        velocity = direction.normalized * speed;
        lifeTime = duration;
        timer = 0f;
    }

    protected virtual void Update()
    {
        transform.position += velocity * Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            Deactivate();
        }
    }

    protected virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    protected abstract void OnTriggerEnter(Collider other);
}
