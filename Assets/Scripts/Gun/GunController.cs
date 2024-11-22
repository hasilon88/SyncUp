using System;
using UnityEngine;

public class GunController : MonoBehaviour
{

    //should be gun transform
    private Camera mainCamera; 
    public KeyCode TriggerKey;
    public int MaxAmmunitionCount = 5;
    public int CurrentAmmunitionCount;
    public float ReloadDurationInSeconds = 3f;
    public float ShootingCooldownInSeconds = 3f;
    public event EventHandler BeforeFiring;
    public event EventHandler AfterFiring;  
    public bool CanFire = true;

    private Vector3 bulletDirection;
    public GameObject BaseBulletModelPrefab;
    public GameObject MultiBulletsPrefab;

    void Start()
    {
        if (TriggerKey == KeyCode.None) TriggerKey = KeyCode.Mouse0;  
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        CurrentAmmunitionCount = MaxAmmunitionCount;
    }

    private void UpdateBulletDirection()
    {
        bulletDirection = mainCamera.ScreenPointToRay(Input.mousePosition).direction;
    }

    private void Fire()
    {
        BeforeFiring?.Invoke(this, EventArgs.Empty);
        InstantiateSoundBullet();
        AfterFiring?.Invoke(this, EventArgs.Empty);
    }

    private void InstantiateSoundBullet()
    {
        GameObject _gameObject = Instantiate(MultiBulletsPrefab, mainCamera.transform.position, Quaternion.Euler(Vector3.zero));
        SoundBullet soundBullet = _gameObject.GetComponent<SoundBullet>();
        soundBullet.SetBaseBulletModel(BaseBulletModelPrefab);
        soundBullet.SetDirection(bulletDirection);
        soundBullet.InstantiateElements();
        soundBullet.StartTravelling();
    }

    public void UpdateShootingState()
    {
        UpdateBulletDirection();
        if (Input.GetKeyUp(TriggerKey) && CanFire)
            Fire();
    }
}
