using UnityEngine;
using System.Collections;

/* THIS CODE IS JUST FOR PREVIEW AND TESTING */
// Feel free to use any code and picking on it, I cannot guaratnee it will fit into your project
public class ExplodingProjectile : MonoBehaviour
{
    //첫 생성시는 projectileActor.cs에서 프리팹으로 사용 됨.
    //인스턴스화 됨면 projectileActor에 의해서 풀링된 오브젝트의 포인터로 사용됨.
    public GameObject impactPrefab;
    public GameObject explosionPrefab;
    public float thrust;

    public Rigidbody thisRigidbody;

    public GameObject particleKillGroup;
    private Collider thisCollider;

    public bool LookRotation = true;
    public bool Missile = false;
    public Transform missileTarget;
    public float projectileSpeed;
    public float projectileSpeedMultiplier;

    public bool ignorePrevRotation = false;

    public bool explodeOnTimer = false;
    public float explosionTimer;
    float timer;

    public Vector3 previousPosition;

    // Use this for initialization
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        if (Missile)
        {
            missileTarget = GameObject.FindWithTag("Target").transform;
        }
        thisCollider = GetComponent<Collider>();
        previousPosition = transform.position;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    /*     if(Input.GetButtonUp("Fire2"))
    //         {
    //             Explode();
    //         }*/
    //    timer += Time.deltaTime;
    //    if (timer >= explosionTimer && explodeOnTimer == true)
    //    {
    //        Explode();
    //    }

    //}

    void FixedUpdate()
    {
        if (Missile)
        {
            projectileSpeed += projectileSpeed * projectileSpeedMultiplier;
            //   transform.position = Vector3.MoveTowards(transform.position, missileTarget.transform.position, 0);

            transform.LookAt(missileTarget);

            thisRigidbody.AddForce(transform.forward * projectileSpeed);
        }

        if (LookRotation && timer >= 0.05f)
        {
            transform.rotation = Quaternion.LookRotation(thisRigidbody.velocity);
        }

        CheckCollision(previousPosition);

        previousPosition = transform.position;
    }

    void CheckCollision(Vector3 prevPos)
    {
        RaycastHit hit;
        Vector3 direction = transform.position - prevPos;
        Ray ray = new Ray(prevPos, direction);
        float dist = Vector3.Distance(transform.position, prevPos);

        if (Physics.Raycast(ray, out hit, dist))
        {
            transform.position = hit.point;
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            Vector3 pos = hit.point;
            //edit ma start
            //old code end
            //Instantiate(impactPrefab, pos, rot);
            //old code end
            impactPrefab.SetActive(true);//활성화
            impactPrefab.transform.position = pos;//포지션 재조정
            impactPrefab.transform.rotation = rot;//로테이션 재조정
            impactPrefab.GetComponent<ParticleSystem>().Simulate(0f, true, true);  //에니메이션 Reset
            impactPrefab.GetComponent<ParticleSystem>().Play();                    //에니메이션 Start
            //edit ma end

            if (!explodeOnTimer && Missile == false)
            {
                //Destroy(gameObject);
            }
            else if (Missile == true)
            {
                thisCollider.enabled = false;
                particleKillGroup.SetActive(false);
                thisRigidbody.velocity = Vector3.zero;
                //Destroy(gameObject, 5);
            }

            //edit ma zombie hit check
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("zombie")) {
                hit.transform.GetComponent<WoosanStudio.ZombieShooter.Zombie>().Hit();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "FX")
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
            if (ignorePrevRotation)
            {
                rot = Quaternion.Euler(0, 0, 0);
            }
            Vector3 pos = contact.point;
            //edit ma start
            //old code end
            //Instantiate(impactPrefab, pos, rot);
            //old code end
            impactPrefab.SetActive(true);//활성화
            impactPrefab.transform.position = pos;//포지션 재조정
            impactPrefab.transform.rotation = rot;//로테이션 재조정
            impactPrefab.GetComponent<ParticleSystem>().Simulate(0f, true, true);  //에니메이션 Reset
            impactPrefab.GetComponent<ParticleSystem>().Play();                    //에니메이션 Start
            //edit ma end

            if (!explodeOnTimer && Missile == false)
            {
                //Destroy(gameObject);
            }
            else if (Missile == true)
            {

                thisCollider.enabled = false;
                particleKillGroup.SetActive(false);
                thisRigidbody.velocity = Vector3.zero;

                //Destroy(gameObject, 5);

            }
        }
    }

    void Explode()
    {
        Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }

}