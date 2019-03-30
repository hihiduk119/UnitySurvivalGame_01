using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class projectileActor : MonoBehaviour {

    public Transform spawnLocator; 
    public Transform spawnLocatorMuzzleFlare;
    public Transform shellLocator;
    public Animator recoilAnimator;

    public Transform[] shotgunLocator;

    [System.Serializable]
    public class projectile
    {
        public string name;
        public Rigidbody bombPrefab;
        public GameObject muzzleflare;
        public float min, max;
        public bool rapidFire;
        public float rapidFireCooldown;   

        public bool shotgunBehavior;
        public int shotgunPellets;
        public GameObject shellPrefab;
        public bool hasShells;
    }
    public projectile[] bombList;


    string FauxName;
    public Text UiText;

    public bool UImaster = true;
    public bool CameraShake = true;
    public float rapidFireDelay;
    public CameraShakeProjectile CameraShakeCaller;

    float firingTimer;
    public bool firing;
    public int bombType = 0;

   // public ParticleSystem muzzleflare;

    public bool swarmMissileLauncher = false;
    int projectileSimFire = 1;

    public bool Torque = false;
    public float Tor_min, Tor_max;

    public bool MinorRotate;
    public bool MajorRotate = false;
    int seq = 0;

    //풀의 최상위 루트
    public GameObject objectPool;
    //총구화염 담아두는 풀 만들기
    List<GameObject> rifleFlarePool = new List<GameObject>();
    int rifleFlarePoolCount = 0;
    readonly int rifleFlarePoolMaxCount = 1;

    //탄피 담아두는 풀 만들기
    List<GameObject> rifleShellPool = new List<GameObject>();
    int rifleShellPoolCount = 0;
    readonly int rifleShellPoolMaxCount = 15;

    //탄두 담아두는 풀 만들기
    List<GameObject> rifleProjectilePool = new List<GameObject>();
    int rifleProjectilePoolCount = 0;
    readonly int rifleProjectilePoolMaxCount = 3;


    private void Awake()
    {
        if (objectPool == null) { return; }

        //재활용 총구 화염 생성해서 풀에 넣기
        for (int index = 0; index < rifleFlarePoolMaxCount; index++)
        {
            GameObject clone = Instantiate(bombList[bombType].muzzleflare, spawnLocatorMuzzleFlare.position, spawnLocatorMuzzleFlare.rotation) as GameObject;
            rifleFlarePool.Add(clone);
            clone.transform.parent = objectPool.transform;
            clone.SetActive(false);
        }

        //재활용 탄피 생성해서 풀에 넣기
        for (int index = 0; index < rifleShellPoolMaxCount; index++)
        {
            GameObject clone = Instantiate(bombList[bombType].shellPrefab, shellLocator.position, shellLocator.rotation) as GameObject;
            rifleShellPool.Add(clone);
            clone.transform.parent = objectPool.transform;
            clone.SetActive(false);
        }
    }

    // Use this for initialization
    void Start ()
    {
        //if (UImaster)
        //{
        //    UiText.text = bombList[bombType].name.ToString();
        //}

        if (swarmMissileLauncher)
        {
            projectileSimFire = 5;
        }
    }
	
	// Update is called once per frame
	/*void Update ()
    {
        //Movement
        if(Input.GetButton("Horizontal"))
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                gameObject.transform.Rotate(Vector3.up, -25 * Time.deltaTime);
            }
            else
            {
                gameObject.transform.Rotate(Vector3.up, 25 * Time.deltaTime);
            }
        }

        //BULLETS
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Switch(-1);
        }
        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.E))
        {
            Switch(1);
        }

	    if(Input.GetButtonDown("Fire1"))
        {
            firing = true;
            Fire();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            firing = false;
            firingTimer = 0;
        }

        if (bombList[bombType].rapidFire && firing)
        {
            if(firingTimer > bombList[bombType].rapidFireCooldown+rapidFireDelay)
            {
                Fire();
                firingTimer = 0;
            }
        }

        if(firing)
        {
            firingTimer += Time.deltaTime;
        }
	}*/

    public void Stop() 
    {
        firing = false;
    }

    public void Switch(int value)
    {
            bombType += value;
            if (bombType < 0)
            {
              bombType = bombList.Length;
              bombType--;
            }
            else if (bombType >= bombList.Length)
            {
                bombType = 0;
            }
        //if (UImaster)
        //{
        //    UiText.text = bombList[bombType].name.ToString();
        //}
    }

    public void Fire()
    {
        if(CameraShake)
        {
            CameraShakeCaller.ShakeCamera();
        }
        Debug.Log("bombType = " + bombType.ToString() + " index = " + (int)bombType);

        //edit start
        //old code start
        //Instantiate(bombList[bombType].muzzleflare, spawnLocatorMuzzleFlare.position, spawnLocatorMuzzleFlare.rotation);
        //old code end
        Transform flare = rifleFlarePool[rifleFlarePoolCount].transform;
        flare.gameObject.SetActive(true);//활성화
        flare.position = spawnLocatorMuzzleFlare.position;//포지션 재조정
        flare.rotation = spawnLocatorMuzzleFlare.rotation;//로테이션 재조정
        flare.GetComponent<ParticleSystem>().Simulate(0f, true, true);  //에니메이션 Reset
        flare.GetComponent<ParticleSystem>().Play();                    //에니메이션 Start
        rifleFlarePoolCount++;
        if(rifleFlarePoolMaxCount <= rifleFlarePoolCount) { rifleFlarePoolCount = 0; }
        //   bombList[bombType].muzzleflare.Play();
        if (bombList[bombType].hasShells)
        {
            //old code start
            //Instantiate(bombList[bombType].shellPrefab, shellLocator.position, shellLocator.rotation);
            //old code end
            Transform shell = rifleShellPool[rifleShellPoolCount].transform;
            shell.gameObject.SetActive(true);//활성화
            shell.position = shellLocator.position;//포지션 재조정
            shell.rotation = shellLocator.rotation;//로테이션 재조정
            shell.GetComponent<ParticleSystem>().Simulate(0f, true, true);  //에니메이션 Reset
            shell.GetComponent<ParticleSystem>().Play();                    //에니메이션 Start
            rifleShellPoolCount++;
            if (rifleShellPoolMaxCount <= rifleShellPoolCount) { rifleShellPoolCount = 0; }

        }
        //edit end

        recoilAnimator.SetTrigger("recoil_trigger");

        Rigidbody rocketInstance;
        rocketInstance = Instantiate(bombList[bombType].bombPrefab, spawnLocator.position,spawnLocator.rotation) as Rigidbody;
        // Quaternion.Euler(0,90,0)
        rocketInstance.AddForce(spawnLocator.forward * Random.Range(bombList[bombType].min, bombList[bombType].max));

        if (bombList[bombType].shotgunBehavior)
        {
            for(int i = 0; i < bombList[bombType].shotgunPellets ;i++ )
            {
                Rigidbody rocketInstanceShotgun;
                rocketInstanceShotgun = Instantiate(bombList[bombType].bombPrefab, shotgunLocator[i].position, shotgunLocator[i].rotation) as Rigidbody;
                // Quaternion.Euler(0,90,0)
                rocketInstanceShotgun.AddForce(shotgunLocator[i].forward * Random.Range(bombList[bombType].min, bombList[bombType].max));
            }
        }

        if (Torque)
        {
            rocketInstance.AddTorque(spawnLocator.up * Random.Range(Tor_min, Tor_max));
        }
        if (MinorRotate)
        {
            RandomizeRotation();
        }
        if (MajorRotate)
        {
            Major_RandomizeRotation();
        }
    }


    void RandomizeRotation()
    {
        if (seq == 0)
        {
            seq++;
            transform.Rotate(0, 1, 0);
        }
      else if (seq == 1)
        {
            seq++;
            transform.Rotate(1, 1, 0);
        }
      else if (seq == 2)
        {
            seq++;
            transform.Rotate(1, -3, 0);
        }
      else if (seq == 3)
        {
            seq++;
            transform.Rotate(-2, 1, 0);
        }
       else if (seq == 4)
        {
            seq++;
            transform.Rotate(1, 1, 1);
        }
       else if (seq == 5)
        {
            seq = 0;
            transform.Rotate(-1, -1, -1);
        }
    }

    void Major_RandomizeRotation()
    {
        if (seq == 0)
        {
            seq++;
            transform.Rotate(0, 25, 0);
        }
        else if (seq == 1)
        {
            seq++;
            transform.Rotate(0, -50, 0);
        }
        else if (seq == 2)
        {
            seq++;
            transform.Rotate(0, 25, 0);
        }
        else if (seq == 3)
        {
            seq++;
            transform.Rotate(25, 0, 0);
        }
        else if (seq == 4)
        {
            seq++;
            transform.Rotate(-50, 0, 0);
        }
        else if (seq == 5)
        {
            seq = 0;
            transform.Rotate(25, 0, 0);
        }
    }
}
