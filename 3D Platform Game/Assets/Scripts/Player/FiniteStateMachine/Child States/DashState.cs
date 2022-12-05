using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : AbilityState
{
    Vector3 dashDirection;
    float dashStartTime;
    Coroutine ResetDash;

    //Dash Effect
    public float activeTime = 0.2f;
    public float meshRefreshRate = 0.01f;
    bool isTrailActive;
    SkinnedMeshRenderer[] skinnedMeshRenderers;
    public DashState(Player player, StateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
       
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Enter Dash");

        player.amountOfDashLeft--;

        player.inputHandler.UsedDash();

        player.playerAnimator.SetBool(animationName, true);

        dashStartTime = Time.time;

        if (ResetDash != null)
        {
            player.StopCoroutine(ResetDash);
        }

        if (!isTrailActive)
        {
            isTrailActive = true;
            player.StartCoroutine(ActivateTrail(activeTime));
        }
        
    }

    public override void Exit()
    {
        base.Exit();

        player.playerAnimator.SetBool(animationName, false);

        

        ResetDash = player.StartCoroutine(DashResetTimer());
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        dashDirection = player.slopeMovementDirection;
        //dashDirection = player.transform.forward;
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();

        Dash();

        if (Time.time > dashStartTime + 0.1f)
        {
            isAbilityDone = true;

            player.playerRB.AddForce(-player.playerRB.velocity, ForceMode.VelocityChange);
        }
        
    }

    void Dash()
    {

        dashDirection = player.slopeMovementDirection;
        Vector3 finalMove = player.dashSpeed * Time.deltaTime * dashDirection ;

        player.playerRB.AddForce(finalMove, ForceMode.VelocityChange);
    }

    IEnumerator DashResetTimer()
    {
        if(player.amountOfDashLeft == 0)
        {
            while (player.amountOfDashLeft < 2)
            {
                yield return new WaitForSeconds(1f);

                player.amountOfDashLeft = 2;
            }
        } else if(player.amountOfDashLeft == 1)
        {
            while (player.amountOfDashLeft < 2)
            {
                yield return new WaitForSeconds(0.1f);

                player.amountOfDashLeft = 2;
            }
        }
        
    }

    IEnumerator ActivateTrail(float activeTime)
    {
        while (activeTime > 0)
        {
            activeTime -= meshRefreshRate;

            if(skinnedMeshRenderers == null)
            {
                skinnedMeshRenderers = player.GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            for(int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();

                gObj.transform.SetPositionAndRotation(player.transform.position, player.transform.rotation);

                MeshRenderer mR =  gObj.AddComponent<MeshRenderer>();

                MeshFilter mF = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();

                mF.mesh = mesh;
                mR.material = player.mat;

                skinnedMeshRenderers[i].BakeMesh(mesh);

                player.StartCoroutine(TrailFading(mR, gObj));
 
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }

    IEnumerator TrailFading(MeshRenderer mR, GameObject objectToDestroy)
    {
        for (float f = 0; f < activeTime + 0.1f; f += meshRefreshRate)
        {
            Color c = mR.material.color;

            c.a = f;

            mR.material.color = c;

            yield return new WaitForEndOfFrame();
        }

        Object.Destroy(objectToDestroy);
    }
}
   

