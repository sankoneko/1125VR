using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.VR;
public class ShootingGunController : MonoBehaviour
{
    public AudioSource audioSource; 
    public VRInput vrInput;
    public ParticleSystem flareParticle;
    public LineRenderer gunFlare;
    public Transform gunEnd;
    //----------------------------------------
    public Transform cameraTransform;
    public Reticle reticle;
    public Transform gunContainer;
    public float damping = 0.5f;
    public float dampingCoef = -20f;
    public float gunContainerSmoothe = 10f;
    //----------------------------------------
    public float defaultLineLength = 70f;
    public float gunFlareVisibleDeconds = 0.07f;
    private void OnEnable()
    {
        vrInput.OnDown += HandleDown;
    }

    private void OnDisable()
    {
        vrInput.OnDown -= HandleDown;
    }
    private void HandleDown()
    {
        
        StartCoroutine(Fire());
        
    }

    private IEnumerator Fire()
    {
        audioSource.Play();
        float lineLength = defaultLineLength;
        //TODO 判斷有無射到東西
        flareParticle.Play();
        gunFlare.enabled = true;
        yield return StartCoroutine(MoveLineRenderer(lineLength));
        gunFlare.enabled = false;
    }

    private IEnumerator MoveLineRenderer(float lineLength)
    {
        float timer = 0f;
        while(timer <gunFlareVisibleDeconds)
        {
            gunFlare.SetPosition(0, gunEnd.position);
            gunFlare.SetPosition(1, gunEnd.position + gunEnd.forward * lineLength);
            yield return null;
            timer += Time.deltaTime;
        } 
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, InputTracking.GetLocalRotation(VRNode.Head), damping * (1 - Mathf.Exp(dampingCoef * Time.deltaTime)));
        transform.position = cameraTransform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(reticle.ReticleTransform.position - gunContainer.position);
        gunContainer.rotation = Quaternion.Slerp(gunContainer.rotation, lookAtRotation, gunContainerSmoothe * Time.deltaTime);
    }


}


