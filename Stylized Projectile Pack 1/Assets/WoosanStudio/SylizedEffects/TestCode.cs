using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public projectileActor projectileActor;
    public projectileActorExplosion1 projectileActorExplosion1;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void Auto() {
        if (projectileActor != null) 
        {
            //projectileActor.Fire();

            StartCoroutine(CorAutoShot());
        }

        if(projectileActorExplosion1 != null)
        {
            projectileActorExplosion1.Fire();
        }
    }

    IEnumerator CorAutoShot() 
    {
        while(true) {
            yield return new WaitForSeconds(0.2f);
            projectileActor.Fire();
        }
    }

    public void Next() {
        if (projectileActor != null)
        {
            projectileActor.Switch(1);
        }

        if (projectileActorExplosion1 != null)
        {
            projectileActorExplosion1.Switch(1);
        }
    }
}
