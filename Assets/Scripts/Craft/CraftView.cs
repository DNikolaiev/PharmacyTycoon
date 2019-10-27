using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraftView : MonoBehaviour {
    
    public List<TalentHolder> primaryHolders;
    public List<TalentHolder> secondaryHolders;
    public CraftHolder holderSelected;
    public RecipeHolder recipeHolderSelected;
    public Slider liquidSlider;
    public Button craftButton;
    #region particles
    public ParticleSystem badMatch;
    public ParticleSystem okMatch;
    public ParticleSystem goodMatch;
    public ParticleSystem afterCraft;
    [SerializeField] private ParticleSystem lastPlayed;
    [SerializeField] private int firstUnlockLvl;
    [SerializeField] private int secondUnlockLvl;
    #endregion
    #region prefabs
    public ResourcePanel rPanel;
    public GameObject elementInList; // prefab for displaying talent as in-list element
    public GameObject recipeInList; // prefab for displaying recipe as in-list element
    public Transform talentsListView; // list of talents
    public Transform recipesListView; //list of recipes
    public DescriptionPanel craftDescriptionPanel;
    #endregion
    public Image craftPanel; // main panel
    public Text capacity;
    private Vector2 startSize;
    private bool isCoroutineRunning;
    private void Start()
    {
        Invoke("UpdateView", .5f);
        EventManager.StartListening(Player.OnLevelEvent, UpdateView);
    }
    private IEnumerator BounceCraftButton(float frequency, float amplitude)
    {
        
        startSize = craftButton.GetComponent<RectTransform>().sizeDelta;
        Vector2 tempSize = Vector2.zero;

        while(true)
        {
            isCoroutineRunning = true;
            tempSize = startSize;
            tempSize.x += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency)*amplitude;
            tempSize.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            craftButton.GetComponent<RectTransform>().sizeDelta = tempSize;
           
            yield return null;
        }
    }
    public void ResetCraftButton()
    {
        Debug.Log("resetted");

        StopAllCoroutines();
       
            craftButton.GetComponent<RectTransform>().sizeDelta = startSize;
           
        
    }
    public void ReflectMatch(int matchCounter)
    {
        
        StartCoroutine(BounceCraftButton(1.5f,5.5f));
        switch (matchCounter)
        {
                
            case 2:
                {
                    DisableAllParticles();
                    okMatch.gameObject.SetActive(true);
                    okMatch.Play();
                    lastPlayed = okMatch;
                }
                break;
            case 3:
                {
                    DisableAllParticles();
                    okMatch.gameObject.SetActive(true);
                    okMatch.Play();
                    lastPlayed = okMatch;
                }
                break;
            case 4:
                {
                    DisableAllParticles();
                    goodMatch.gameObject.SetActive(true);
                    goodMatch.Play();
                    lastPlayed = goodMatch;
                }
                break;
            case 5:
                {
                    DisableAllParticles();
                    goodMatch.gameObject.SetActive(true);
                    goodMatch.Play();
                    lastPlayed = goodMatch;
                }
                break;
            case 6:
                {
                    DisableAllParticles();
                    goodMatch.gameObject.SetActive(true);
                    goodMatch.Play();
                    lastPlayed = goodMatch;
                }
                break;
            case 7:
                {
                    DisableAllParticles();
                    goodMatch.gameObject.SetActive(true);
                    goodMatch.Play();
                    lastPlayed = goodMatch;
                }
                break;
            case 8:
                {
                    DisableAllParticles();
                    goodMatch.gameObject.SetActive(true);
                    goodMatch.Play();
                    lastPlayed = goodMatch;
                }
                break;
            default:
                {
                    Debug.Log("def");
                    DisableAllParticles();
                    badMatch.gameObject.SetActive(true);
                    badMatch.Play();
                    lastPlayed = badMatch;
                }
                break;
        }
    }
    public void EmitLastParticles()
    {
        if (lastPlayed != null)
        {
            lastPlayed.gameObject.SetActive(true);
            lastPlayed.Play();
        }
    }
	public void DisableAllParticles()
    {
        ParticleSystem[] particles = { badMatch, okMatch, goodMatch };
        foreach(ParticleSystem p in particles)
        {
            p.Stop();
            p.gameObject.SetActive(false);
        }
    }
    public void UnlockNewHolders(int primaryUnlock, int secondaryUnlock)
    {
        primaryUnlock++;
        secondaryUnlock++;
        int lastIndex = 0;
        for (int i = 0; i < primaryHolders.Count; i++)
        {
            if (primaryHolders[i].isUnlocked)
                lastIndex = i;
        }
        int finalUnlock = Mathf.Clamp(lastIndex + primaryUnlock, 0, primaryHolders.Count);
        for (int n = lastIndex + 1; n < finalUnlock; n++)
        {
            primaryHolders[n].Unlock(true);
        }
        lastIndex = 0;
        for (int i = 0; i < secondaryHolders.Count; i++)
        {
            if (secondaryHolders[i].isUnlocked)
                lastIndex = i;
        }
        finalUnlock = Mathf.Clamp(lastIndex + secondaryUnlock, 0, secondaryHolders.Count);
        for (int n = lastIndex + 1; n < finalUnlock; n++)
        {
           secondaryHolders[n].Unlock(true);
        }

    }
    public void HighlightHolders(bool isPrimary = true)
    {

        if (isPrimary)
        {
            primaryHolders.ForEach(x =>
            {
                if (x.Talent == null && x.isUnlocked)
                { x.glowImg.gameObject.SetActive(true); }
                else { x.glowImg.gameObject.SetActive(false); }
            });
            secondaryHolders.ForEach(x => x.glowImg.gameObject.SetActive(false));
        }
        else
        {
            secondaryHolders.ForEach(x => {
                if (x.Talent == null && x.isUnlocked)
                { x.glowImg.gameObject.SetActive(true); }
                else { x.glowImg.gameObject.SetActive(false); }
            });
            primaryHolders.ForEach(x => x.glowImg.gameObject.SetActive(false));
        }
    }
   public void UpdateView()
    {
        Player player = GameController.instance.player;
        Debug.Log("PLAYER LEVEL IS " + player.level);
        if (player.level >= firstUnlockLvl)
            UnlockNewHolders(1, 1);
       if (player.level >= secondUnlockLvl)
            UnlockNewHolders(1, 1);
       
    }
    public void PlayOnCraftParticles()
    {
        if (afterCraft != null)
        {
            if (afterCraft.isPlaying)
                afterCraft.Stop();
            afterCraft.gameObject.SetActive(true);
            afterCraft.Play();

        }
    }
}
