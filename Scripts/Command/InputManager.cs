using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    #region Variavbles
    public LayerMask groundLayer;
    FSMPlayer player;
    PlayerParam myParam;


    GameManager gameManager;
    public GameObject scanObj;

    
    private bool pickupActivated = false;
    [SerializeField]
    private Text actionText;
    private GameObject actiond;
    [SerializeField]
    private Inventory theInventory;

    #endregion Variables

    ParticleSystem targetPosPart;
    

    // Start is called before the first frame update
    void Start()
    {
        theInventory = FindObjectOfType<Inventory>();
        actiond = GameObject.Find("Show");
        actionText = actiond.transform.GetChild(0).GetComponent<Text>();
        player = GetComponent<FSMPlayer>();
        myParam = GetComponent<PlayerParam>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void PlayerCommand()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 30.0f, Color.green);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit, 100, groundLayer) && !EventSystem.current.IsPointerOverGameObject())
            {
                player.MoveCharactor(hit.point);
                
                if(targetPosPart == null)
                {
                    GameObject par = Resources.Load<GameObject>("MovePrefabs/move");
                    targetPosPart = Instantiate(par).GetComponent<ParticleSystem>();

                }
                targetPosPart.GetComponent<Transform>().position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
                targetPosPart.GetComponent<ParticleSystem>().Play();

            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100, groundLayer) && !EventSystem.current.IsPointerOverGameObject())
            {
                player.ClickAttack(hit.point);

            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (myParam.CheckMp(SkillSlash.skillMp) == true)
            {
                if (Physics.Raycast(ray, out hit, 100, groundLayer))
                {
                    player.ComboSkill(hit.point, 0);

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (myParam.CheckMp(SkillCombo.skillMp) == true)
            {
                if (Physics.Raycast(ray, out hit, 100, groundLayer))
                {
                    player.ComboSkill(hit.point, 1);

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Physics.Raycast(ray, out hit, 100, groundLayer))
            {
                player.Dash(hit.point);

            }
        }
        if (Input.GetKeyDown(KeyCode.F) && scanObj != null)
        {
            if(scanObj.CompareTag("Npc"))
                gameManager.Action(scanObj);

            if(scanObj.CompareTag("Item"))
                CanPickUp();
        }
    }
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = scanObj.transform.GetComponent<ItemPickUp>().item.itemName + " È¹µæ "  + "<color=yellow>"+ "(FÅ°)" + "</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);

    }

    private void CanPickUp()
    {
        if(pickupActivated)
        {
            if(scanObj != null)
            {
                Debug.Log(scanObj.transform.GetComponent<ItemPickUp>().item.itemName + "È¹µæ");
                theInventory.AcquireItem(scanObj.transform.GetComponent<ItemPickUp>().item);
                Destroy(scanObj.transform.gameObject);
                InfoDisappear();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        PlayerCommand();
    }

    private void OnTriggerStay(Collider other)
    {
    
        if (other.CompareTag("Npc"))
        {
            scanObj = other.gameObject;
        }
       if (other.CompareTag("Item"))
        {
            scanObj = other.gameObject;
            ItemInfoAppear();
        }

        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            scanObj = null;
            InfoDisappear();
        }
        if (other.CompareTag("Npc"))
        {
            scanObj = null;
            gameManager.Action(scanObj);
        }
    }
}
