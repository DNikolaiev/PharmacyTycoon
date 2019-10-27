using UnityEngine;
 using System.Collections;

public class Builder : MonoBehaviour
{
    public ParticleSystem onObjectBuild;
    public ParticleSystem onRoomBuild;
    public AudioClip[] onBuildSounds;
    public Constructible Build(int id, Constructible[] objectsArray, Cell cell, bool playEffects=true)
    {
        Constructible selectedObject = objectsArray[id];
        if(selectedObject.gameObject.tag=="Room")
        {
          
            EventManager.TriggerEvent("OnBuildRoom",1);
            if (onRoomBuild != null)
            {
                if (playEffects)
                {
                    ParticleSystem particle = Instantiate(onRoomBuild, cell.transform.position, Quaternion.identity);
                    particle.gameObject.SetActive(true);
                    particle.Play();
                }
                PlayGameScript.UnlockAchievement(GPGSIds.achievement_builder);
            }
        }
        else
        {
        
            EventManager.TriggerEvent("OnBuildObject",1);
            PlayGameScript.IncrementAchievement(GPGSIds.achievement_a_new_beginning, 1);
            if (onObjectBuild != null)
            {
                if (playEffects)
                {
                    ParticleSystem particle = Instantiate(onObjectBuild, cell.transform.position, Quaternion.identity);
                    particle.gameObject.SetActive(true);
                    particle.Play();
                }
            }
        }
        if (playEffects)
        {
            GameController.instance.player.GainExperience(100 * GameController.instance.roomOverseer.rooms.Count);
            GameController.instance.player.finances.AddToActiveExpences(selectedObject.description.buyPrice);
            GameController.instance.audio.MakeSound(onBuildSounds[Random.Range(0, onBuildSounds.Length)]);
        }
        Constructible toReturn = Instantiate(selectedObject, new Vector3(cell.transform.position.x, cell.transform.position.y, cell.transform.position.z - 0.5f), cell.transform.rotation);
        StartCoroutine(Camera.main.gameObject.GetComponent<CameraController>().FocusCamera(toReturn.transform.position));
        
        return toReturn;
        
    }
}