using System.Collections;
using UnityEngine;
using TMPro;

public class Player_Shoot : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private UIController uiController;
    [SerializeField] private Player_CamaraChanger playerCamaraChanger;

    [Header("Raycast y Daño")]
    [SerializeField] private LayerMask destroyableLayer;
    [SerializeField] private float shootRange = 10f;
    [SerializeField] private EnemyHealth enemyHealth;
    public GameObject bulletImpact;

    [Header("Disparo Normal")]
    public float remainingBullets;
    public float maxBullets;
    public float fireRateBullet = 1f;
    private float nextTimeToFireBullet = 0f;
    [SerializeField] private TMP_Text bulletCounterText;

    [Header("Light Probe")]
    public float remainingLightProbes;
    public float maxLightProbes;
    public float fireRateLightProbe = 1f;
    [SerializeField] private TMP_Text lightProbeCounterText;
    [SerializeField] private float lightProbeDuration;
    private float nextTimeToFireLightProbe = 0f;

    private void Start()
    {
        if (lightProbeCounterText != null)
        {
            lightProbeCounterText.gameObject.SetActive(false);
        }

        if (cam == null)
        {
            cam = Camera.main;
        }

        remainingBullets = maxBullets;
        remainingLightProbes = maxLightProbes;
    }

    void Update()
    {

        Shoot();

        if (lightProbeCounterText != null && lightProbeCounterText.gameObject.activeSelf)
        {
            float remaining = nextTimeToFireLightProbe - Time.time;
            if (remaining > 0f)
            {
                lightProbeCounterText.text = $"Lightprobes remaining: {remainingLightProbes} / {maxLightProbes}";
            }
            else
            {
                lightProbeCounterText.gameObject.SetActive(false);
            }
        }

        bulletCounterText.text = $"Discharges remaining: {remainingBullets} / {maxBullets}";
    }

    void Shoot()
    {
        // Disparo normal (clic izquierdo)
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFireBullet && remainingBullets > 0 )
        {
            Ray ray;
            RaycastHit hit;

            if (playerCamaraChanger.isFirstPerson)
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);
            }
            else
            {
                ray = new Ray(shootOrigin.position, shootOrigin.forward);
            }

            uiController.FlashRedCrosshair();

            if (Physics.Raycast(ray, out hit, shootRange))
            {
                if (((1 << hit.collider.gameObject.layer) & destroyableLayer) != 0)
                {
                    EnemyHealth hitEnemy = hit.transform.GetComponent<EnemyHealth>();
                    hitEnemy.TakeDamage(1);
                }

                Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
            }

            remainingBullets--;

            nextTimeToFireBullet = Time.time + 1f / fireRateBullet;
        }

        // Disparo Light Probe (clic derecho)
        if (Input.GetMouseButtonDown(1) && Time.time >= nextTimeToFireLightProbe && remainingLightProbes > 0)
        {

            GameObject lightProbe = ObjectPool_PlayerShoot.instance.GetPooledObject();


            if (lightProbe != null)
            {
                lightProbe.transform.position = shootOrigin.position;
                lightProbe.SetActive(true);

                remainingLightProbes--;
                nextTimeToFireLightProbe = Time.time + 1f / fireRateLightProbe;

                if (lightProbeCounterText != null)
                {
                    lightProbeCounterText.gameObject.SetActive(true);
                }

                StartCoroutine(DeactivateAfterTime(lightProbe, lightProbeDuration));
            }
        }

    }

    private IEnumerator DeactivateAfterTime(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
    }

}
