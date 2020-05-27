using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode
{
    SemiAuto,
    FullAuto
}

public class WeaponBase : MonoBehaviour
{
    [Header("Sound References")]

    [SerializeField]
    protected AudioClip fireSound;
    [SerializeField]
    protected AudioClip dryFireSound;
    [SerializeField]
    protected AudioClip magOutSound;
    [SerializeField]
    protected AudioClip magInSound;
    [SerializeField]
    protected AudioClip boltSound;
    [SerializeField]
    protected AudioClip drawSound;

    [Header("Weapon Effects")]

    public ParticleSystem muzzleFlash;
    [SerializeField]
    GameObject sparkEffect;

    [Header("Weapon Attributes")]

    [SerializeField]
    protected Weapon weapon;
    [SerializeField]
    protected DamageMap[] damageMap;
    [Space]
    [SerializeField]
    protected float fireRate = 1.0f;
    [SerializeField]
    protected int clipSize = 12;
    [SerializeField]
    protected int maxAmmo = 100;
    [SerializeField]
    protected int pellets = 1;
    [SerializeField]
    protected FireMode fireMode;
    [SerializeField]
    protected KeyCode reloadKey;
    [SerializeField]
    protected Transform shootPoint;
    [SerializeField]
    protected float spreadRate = 0.7f;
    [SerializeField]
    protected float recoilRate = 1f;
    [SerializeField]
    protected float recoilRecoverAmount = 2f;
    [SerializeField]
    private bool isActive = false;
    [SerializeField]
    WeaponShopInfo weaponShopInfo;

    protected int bulletsInClip;
    protected int bulletsLeft;
    protected WeaponAnimationController weaponAnimation;
    protected bool isRealoding;
    protected bool fireLock = true;
    protected AudioSource audioSource;

    public bool IsActive { get => isActive; set => isActive = value; }
    public bool IsSelected { get; set; }
    public Weapon Weapon => weapon;
    public WeaponShopInfo WeaponShopInfo => weaponShopInfo;

    public int BulletsToBuy
    {
        get
        {
            if (!isActive) return 0;
            else
            {
                if (maxAmmo - bulletsLeft == maxAmmo && bulletsLeft != 0) return 0;

                return maxAmmo - bulletsLeft;
            }
        }
    }


    WaitForSeconds resetFirelockSeconds;
    Coroutine resetFireLockCoroutine;
    PlayerLook playerLook;

    Dictionary<BodyPartType, float> damageMapDic = new Dictionary<BodyPartType, float>();

    protected void Awake()
    {
        InitializeDamageMapDictionary();

        bulletsInClip = clipSize;
        bulletsLeft = maxAmmo;

        muzzleFlash.Stop();
        resetFirelockSeconds = new WaitForSeconds(fireRate);
        playerLook = GetComponentInParent<PlayerLook>();
        weaponAnimation = GetComponent<WeaponAnimationController>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void UpdateUIStats()
    {
        UIEvents.OnPlayerStatChangedFunc(bulletsInClip, bulletsLeft, transform.name);
    }

    protected void InitializeDamageMapDictionary()
    {
        for (int i = 0; i < damageMap.Length; i++)
        {
            if (!damageMapDic.ContainsKey(damageMap[i].bodyPartType))
                damageMapDic.Add(damageMap[i].bodyPartType, damageMap[i].givenDamage);
            else
                Debug.LogError($"This Key Already's been Added : {damageMap[i].bodyPartType}");
        }
    }

    protected void OnEnable()
    {
        UIEvents.OnBuyMenuActivenessChanged += UIEvents_OnBuyMenuActivenessChanged;
        muzzleFlash.Stop();
    }

    private void UIEvents_OnBuyMenuActivenessChanged(bool activeness)
    {
        fireLock = activeness;
    }

    private void OnDisable()
    {
        UIEvents.OnBuyMenuActivenessChanged -= UIEvents_OnBuyMenuActivenessChanged;
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(0) && fireMode == FireMode.SemiAuto)
        {
            Shoot();
        }

        if (Input.GetMouseButton(0) && fireMode == FireMode.FullAuto)
        {
            Shoot();
        }

        if (Input.GetKeyDown(reloadKey) || bulletsInClip == 0)
        {
            Reload();
        }
    }

    protected void Shoot()
    {
        if (fireLock || isRealoding)
            return;

        if (bulletsInClip > 0)
        {
            Fire();
        }
        else
        {
            DryFire();
        }

        void Fire()
        {
            audioSource.PlayOneShot(fireSound);
            muzzleFlash.Stop();
            muzzleFlash.Play();

            PlayFireAnimation();

            for (int i = 0; i < pellets; i++)
            {
                DetectHit();
            }

            playerLook.RecoilAmount += recoilRate;
            playerLook.RecoilRecoverAmount = recoilRecoverAmount;

            bulletsInClip--;
            UpdateUIStats();

            fireLock = true;

            resetFireLockCoroutine = StartCoroutine(ResetFirelockCoroutine());
        }

        void DryFire()
        {
            audioSource.PlayOneShot(dryFireSound);

            fireLock = true;

            resetFireLockCoroutine = StartCoroutine(ResetFirelockCoroutine());
        }

        void DetectHit()
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(shootPoint.position, CalculateSpread(spreadRate, shootPoint), out hitInfo))
            {
                var bodyPart = hitInfo.transform.GetComponent<BodyPart>();

                if (bodyPart != null)
                {
                    bodyPart.TakeDamage(damageMapDic[bodyPart.BodyPartType]);
                    bodyPart.BloodEffect(hitInfo.point, Quaternion.identity);
                }
                else
                {
                    GameObject tmpSpark = Instantiate(sparkEffect, hitInfo.point, hitInfo.transform.rotation);
                    Destroy(tmpSpark, 1.5f);
                }
            }
        }
    }

    protected Vector3 CalculateSpread(float spread, Transform point)
    {
        return Vector3.Lerp(shootPoint.TransformDirection(Vector3.forward * 100f), Random.onUnitSphere, spread);
    }

    public void HandleWeaponBuy()
    {
        bulletsLeft = maxAmmo;

        isActive = true;
    }

    public void HandleAmmoBuy()
    {
        bulletsLeft = maxAmmo;

        if (PlayerController.Instance.WeaponManager.CurrentWeapon == Weapon)
            UpdateUIStats();
    }

    protected virtual void PlayFireAnimation()
    {
        weaponAnimation.CrossFadeShoot();
    }

    protected virtual void Reload()
    {
        if (isRealoding) return;

        if (bulletsLeft > 0 && bulletsInClip < clipSize)
        {
            HandleReload();
        }

        void HandleReload()
        {
            isRealoding = true;
            weaponAnimation.CrossFadeReload();
        }
    }

    public void OnWeaponSwitched()
    {
        isRealoding = false;
    }

    protected virtual void FillAmmo()
    {
        int bulletsToLoad = clipSize - bulletsInClip;
        int bulletsToSub = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToSub;
        bulletsInClip += bulletsToLoad;

        UpdateUIStats();
    }

    protected IEnumerator ResetFirelockCoroutine()
    {
        yield return resetFirelockSeconds;
        fireLock = false;
    }

    protected void StopResetFirelockCoroutine()
    {
        if (resetFireLockCoroutine != null)
        {
            StopCoroutine(resetFireLockCoroutine);
            resetFireLockCoroutine = null;
        }
    }

    protected void ResetIsReloading() => isRealoding = false;

    #region AnimationEvents

    public virtual void OnDraw()
    {
        fireLock = false;
        Invoke("ResetIsReloading", 0.8f);
        audioSource.PlayOneShot(drawSound);
    }

    public virtual void OnMagOut()
    {
        audioSource.PlayOneShot(magOutSound);
    }

    public virtual void OnMagIn()
    {
        FillAmmo();
        audioSource.PlayOneShot(magInSound);
    }

    public virtual void OnBoltForwarded()
    {
        Invoke("ResetIsReloading", 1f);
        audioSource.PlayOneShot(boltSound);
    }

    #endregion
}

[System.Serializable]
public class DamageMap
{
    public BodyPartType bodyPartType;
    public float givenDamage;
}
