using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public  class TalentHolder : Holder, IPointerDownHandler, IDescription {
    [SerializeField] private Talent talent;
    [SerializeField] private bool generateDescription;
    public Talent Talent { get { return talent; } set { talent = value; UpdateView(); } }
    public Image glowImg;
   
 
    protected bool ClickedInsideHolder()
    {
                RectTransform rectTransform = GetComponent<RectTransform>();

            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main))
                return false;
            else return true; 
    }
    protected void UpdateView()
    {

        if (talent == null)
        {
            picture.sprite = defaultSprite;
        }

        else
        {
            picture.sprite = talent.description.sprite;
        }
        
    }
   
    public virtual void OnPointerDown(PointerEventData eventData)
    {
            SetDescription();
    }
    public virtual void SetDescription() { }

    public override void SetPanel()
    {
        if (Talent != null)
        {
            picture.sprite = Talent.description.sprite;
            Start();
        }
    }
    private void Start()
    {
        if (generateDescription && talent!=null)
        {
            Debug.Log("123");
            GetComponent<Button>().onClick.AddListener(delegate { GameController.instance.buttons.GetHint(Talent.description.Name + "\n" + "Cures: " + Talent.cures); });
        }
    }




}
