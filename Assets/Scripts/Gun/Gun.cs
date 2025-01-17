using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float gunCD = 0.2f;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private float muzzleFlashTime;
    private Coroutine muzzleCoroutine;
    private Vector2 mousePos;
    private float lastFireTime;

    private CinemachineImpulseSource impulseSource;
    private Animator animator;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private ObjectPool<Bullet> bulletPool;

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
        OnShoot += FireAnimation;
        OnShoot += ShakeScreen;
        OnShoot += MuzzleFlash;
    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= FireAnimation;
        OnShoot -= ShakeScreen;
        OnShoot -= MuzzleFlash;
    }

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        animator = GetComponent<Animator>();
        CreateBulletPool();
    }

    private void CreateBulletPool()
    {
        bulletPool = new ObjectPool<Bullet>(CreateBullet,
                                            ActivateBullet,
                                            ReleaseBullet,
                                            (Bullet bullet) => Destroy(bullet.gameObject),
                                            collectionCheck: false, 20, 40);
    }

    private Bullet CreateBullet() => Instantiate(_bulletPrefab);
    private void ActivateBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void ReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    public void ReleaseBulletFromPool(Bullet bullet) => bulletPool.Release(bullet);

    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time > lastFireTime + gunCD)
        {
            OnShoot?.Invoke();
        }
    }

    private void ResetLastFireTime()
    {
        lastFireTime = Time.time;
    }

    private void ShootProjectile()
    {
        Bullet newBullet = bulletPool.Get();
        newBullet.ClearTrail();
        newBullet.Init(this, _bulletSpawnPoint.position, mousePos);
    }

    private void FireAnimation()
    {
        animator.Play(FIRE_HASH, 0, 0f);
    }

    private void ShakeScreen()
    {
        impulseSource.GenerateImpulse();
    }

    private void RotateGun()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(mousePos);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void MuzzleFlash()
    {
        if(muzzleCoroutine != null)
        {
            StopCoroutine(muzzleCoroutine);
        }
        muzzleCoroutine = StartCoroutine(MuzzleFlashRoutine());
    }

    private IEnumerator MuzzleFlashRoutine()
    {
        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(muzzleFlashTime);

        muzzleFlash.SetActive(false);
    }
}
