using System.Collections;
using UnityEngine;

public abstract class ClickObject : MonoBehaviour {

    [SerializeField] protected ParticleSystem onClick;
    [SerializeField] protected int clicksToDestroy;
    [SerializeField] protected int clicks;
    [SerializeField] protected int lifeTime;
    [SerializeField] protected AudioClip[] clickSound;
    protected abstract void StartAction();
    protected abstract void ClickAction();
    private RaycastHit hit;
    private Ray ray;
    protected void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
             ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("# "+hit.transform.gameObject.name);
                if (hit.transform.gameObject != this.transform.parent.gameObject) return;
                ParticleSystem inst = Instantiate(onClick, transform.parent.position, Quaternion.identity);
                inst.gameObject.SetActive(true);
                inst.Play();
                ClickAction();
                if (clickSound.Length > 0)
                {
                    GameController.instance.audio.MakeSound(clickSound[Random.Range(0, clickSound.Length)]);
                }
                clicks++;
                if (clicks >= clicksToDestroy)
                {
                    Destroy(gameObject);
                }
            }
           
        }

    }
    protected void Start()
    {
        StartCoroutine(StartCountDownToExtinction());
        StartAction();
    }
    private IEnumerator StartCountDownToExtinction()
    {
        int timeElapsed = 0;
        while(true)
        {
            if (timeElapsed >= lifeTime)
                Destroy(gameObject);
            yield return new WaitForSeconds(1);
        }
    }
}
