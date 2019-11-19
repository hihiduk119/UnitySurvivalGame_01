using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine.Events;

public class projectileActor : MonoBehaviour {

    public Transform spawnLocator;
    [Header("[총기 화염 부분]")]
    public Transform spawnLocatorMuzzleFlare;
    [Header("[탄피 배출부분]")]
    public Transform shellLocator;
    public Animator recoilAnimator;
    [Header("[파티클 스캐일 조정을 위해 추가]")]
    public Vector3 spawnScale = new Vector3(0.75f, 0.75f, 0.75f);

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

    //추가한 부분
    //생성된 탄환 루트
    public static Transform Projectile;
    public static Transform Muzzle;
    public static Transform Shell;

    //캐슁용
    private GameObject clone;
    Rigidbody rocketInstance;

    //발사 발사시 이벤트 발생
    public UnityEvent eventProjectileFired = new UnityEvent();

    

    private void Awake()
    {
        //자동으로 하이얼아키에서 해당 오브젝트 찾기
        //한번한번만 실행되게
        if (projectileActor.Projectile == null)
        {
            //테그에 ProjectileImpactRoot 있는거 찾기
            projectileActor.Projectile = GameObject.FindGameObjectWithTag("Projectile").transform;
        }

        //한번 실행되게
        if (projectileActor.Muzzle == null)
        {
            //테그에 ProjectileImpactRoot 있는거 찾기
            projectileActor.Muzzle = GameObject.FindGameObjectWithTag("Muzzle").transform;
        }

        //한번만 실행되게
        if (projectileActor.Shell == null)
        {
            //테그에 ProjectileImpactRoot 있는거 찾기
            projectileActor.Shell = GameObject.FindGameObjectWithTag("Shell").transform;
        }
    }
    //여기까지

    // Use this for initialization
    void Start ()
    {
        if (UImaster)
        {
            UiText.text = bombList[bombType].name.ToString();
        }
        if (swarmMissileLauncher)
        {
            projectileSimFire = 5;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //test code [사격중간에 느려지는 타이밍 발생해서]

        //Movement
        if (Input.GetButton("Horizontal"))
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

        //키버튼 오른쪽 눌려서 발사체 변경된 것을 느려짐 현상이 발생했다고 착각
        //if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.E))
        //{
        //    Switch(1);
        //}


        /*if (Input.GetButtonDown("Fire1"))
        {
            firing = true;
            Fire();

            Debug.Log("down");
        }
        if (Input.GetButtonUp("Fire1"))
        {
            firing = false;
            firingTimer = 0;
        }*/



    }

    //원래는 Update에 있었으나 timeScale싱크 문제로 슬로우 모드에서 타임 스케일을 나눈 값만큼 속도 추가 되어야
    //하는데 씹혀서 들어가는 부분 해결
    //고정 업데이트 부분으로 옮김
    private void FixedUpdate()
    {
        if (firing)
        {
            firingTimer += Time.deltaTime;
        }

        if (bombList[bombType].rapidFire && firing)
        {
            if (firingTimer > bombList[bombType].rapidFireCooldown + rapidFireDelay)
            {
                Fire();
                firingTimer = 0;
            }
        }
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
        if (UImaster)
        {
            UiText.text = bombList[bombType].name.ToString();
        }
    }

    public void Fire()
    {
        //사격 이벤트 발사
        eventProjectileFired.Invoke();

        //카메라 흔들기
        if (CameraShake)
        {
            CameraShakeCaller.ShakeCamera();
        }
        clone = Instantiate(bombList[bombType].muzzleflare, spawnLocatorMuzzleFlare.position, spawnLocatorMuzzleFlare.rotation);
        //스케일 조정
        clone.transform.localScale = spawnScale;
        //부모 추가부분 0
        if (projectileActor.Muzzle != null) { clone.transform.SetParent(projectileActor.Muzzle); }
        //   bombList[bombType].muzzleflare.Play();

        if (bombList[bombType].hasShells)
        {
            clone = Instantiate(bombList[bombType].shellPrefab, shellLocator.position, shellLocator.rotation);
            //스케일 조정
            clone.transform.localScale = spawnScale;
            //부모 추가부분 1
            if (projectileActor.Shell != null) { clone.transform.SetParent(projectileActor.Shell); }
            
        }
        recoilAnimator.SetTrigger("recoil_trigger");

        //캐슁용으로 전환 때문에 지역변수 disable
        //Rigidbody rocketInstance;

        //라이플,권총 탄환 부분
        rocketInstance = Instantiate(bombList[bombType].bombPrefab, spawnLocator.position,spawnLocator.rotation) as Rigidbody;
        //스케일 조정
        rocketInstance.transform.localScale = spawnScale;
        //부모 추가부분 2
        if (projectileActor.Projectile != null) { rocketInstance.transform.SetParent(projectileActor.Projectile);}

        // Quaternion.Euler(0,90,0)
        //Time.timeScale /하는 부분 추가 => 슬로우 적용시 문제가 생겨서 추가함.
        rocketInstance.AddForce(spawnLocator.forward * Random.Range(bombList[bombType].min, bombList[bombType].max) / Time.timeScale);
        //샷건 탄환 부분
        if (bombList[bombType].shotgunBehavior)
        {
            for(int i = 0; i < bombList[bombType].shotgunPellets ;i++ )
            {
                Rigidbody rocketInstanceShotgun;
                rocketInstanceShotgun = Instantiate(bombList[bombType].bombPrefab, shotgunLocator[i].position, shotgunLocator[i].rotation) as Rigidbody;
                //스케일 조정
                rocketInstanceShotgun.transform.localScale = spawnScale;
                //부모 추가부분 3
                if (projectileActor.Projectile != null) { rocketInstanceShotgun.transform.SetParent(projectileActor.Projectile); }

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
