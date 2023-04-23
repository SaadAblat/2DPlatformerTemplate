using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Shootgun : MonoBehaviour, IWeaponInterface
{
	[SerializeField] Animator animator;
	[SerializeField] Transform firePoint;
	[SerializeField] GameObject bulletPrefab;
	[SerializeField] float Camera_shake_intensity;
	[SerializeField] float Camera_shake_time;
	[SerializeField] float Camera_shake_frequency;
	[Header("accuracy from 0 to 90")]
	[SerializeField] float accuracy;
	[SerializeField] float drawBack;
	[SerializeField] float timeBeforeFireAgain;
	float timeElapsedAfterLastBullet;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] Aiming AimingScript;
	internal bool ShootRequested;

	[SerializeField] Player player;

	float timeElapsedAfterLastBulletBeforDisable;


	[SerializeField] string shootgunShootSound;
	private void Start()
    {
		timeElapsedAfterLastBullet = timeBeforeFireAgain;

	}
    private void OnEnable()
    {
		animator.Play("Idle");
		timeElapsedAfterLastBullet = timeElapsedAfterLastBulletBeforDisable;	
	}
    private void OnDisable()
    {
		timeElapsedAfterLastBulletBeforDisable = timeElapsedAfterLastBullet;

	}


    // Update is called once per frame
    void Update()
	{
		timeElapsedAfterLastBullet += Time.deltaTime;

		if (ShootRequested)
		{
			if (player.ShotgunAmmoCount > 0)
			{
				Shoot();
			}
			ShootRequested = false;
		}

	}

	public void Shoot()
	{


		if (timeElapsedAfterLastBullet > timeBeforeFireAgain)
		{
			player.ShotgunAmmoCount -= 1;
			animator.Play("Shoot", -1, 0f);
			timeElapsedAfterLastBullet = 0;
			CinemachineShake.CameraInstance.ShakeCamera(Camera_shake_intensity, Camera_shake_time, Camera_shake_frequency);
			GameObject tmp = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
			rb.AddForce(-AimingScript.direction * drawBack, ForceMode2D.Impulse);
			tmp.transform.Rotate(0, 0f, Random.Range(-90 + accuracy, 90 - accuracy));

			AudioManager.instance.Play(shootgunShootSound);

		}

	}
	public void ShootRequestTotrue()
	{
		ShootRequested = true;
	}
}
