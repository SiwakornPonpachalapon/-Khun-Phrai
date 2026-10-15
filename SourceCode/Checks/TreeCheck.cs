using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TreeCheck : MonoBehaviour
{
    private bool ReadyCut = true;
    public TMP_Text stattext;
    private float TimeLeft = 60f;
    private bool Playerinrage = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ReadyCut == false)
        {
            TimeLeft -= Time.deltaTime;
        }
        if(TimeLeft <= 0)
        {
            ReadyCut = true;
            TimeLeft = 60;
        }
        if (Keyboard.current.fKey.wasPressedThisFrame&&Playerinrage == true)
        {
            if (ReadyCut)
            {
                PlayerCombat.Instance.PlayAnimation("Min");
                PlayerManager.Instance.AddLogWood(1);
                PlayerManager.Instance.ChangeState(PlayerStats.CutTree);
                if (QuestNavigationTrail.Instance != null)
                {
                    QuestNavigationTrail.Instance.UpdateNavigation();
                }
                ReadyCut = false;
            }
            else
            {
                StartCoroutine(TreeFallSleep());
            }
            
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Playerinrage = true;
            
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Playerinrage = false;
        }
    }
    IEnumerator TreeFallSleep()
    {    
        stattext.text = $"Tree Sleep zzZ {Mathf.CeilToInt(TimeLeft)}s";
        yield return new WaitForSeconds(3f);
        stattext.text = "";
    }
}
