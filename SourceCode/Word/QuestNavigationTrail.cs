using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class QuestNavigationTrail : MonoBehaviour
{
    public static QuestNavigationTrail Instance;

    [Header("Player")]
    public Transform player;

    [Header("Navigation Point")]
    public GameObject pointPrefab;

    [Header("Trail Setting")]
    public float pointSpacing = 2f;

    // ยก Effect จากพื้นนิดหนึ่ง
    public float pointHeight = 0.1f;

    // ทุกกี่วินาทีให้สร้าง Trail ใหม่
    public float refreshTime = 2f;

    [Header("NavMesh")]
    public float sampleDistance = 3f;

    private Transform currentTarget;

    private readonly List<GameObject> spawnedPoints =
        new List<GameObject>();


    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        //Debug.Log("Scene = " + SceneManager.GetActiveScene().name +" | Quest = " + QuestManager.Instance.currentQuest);
        UpdateNavigation();

        StartCoroutine(NavigationLoop());
    }


    IEnumerator NavigationLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshTime);

            if (currentTarget != null)
            {
                CreateTrail();
            }
        }
    }


    // =====================================================
    // UPDATE QUEST TARGET
    // =====================================================

    public void UpdateNavigation()
    {
        QuestUI.Instance.UpdateQuestUI();
        
        if (QuestManager.Instance == null)
        {
            HideNavigation();
            return;
        }

        int quest =
            QuestManager.Instance.currentQuest;

        bool accepted =
            QuestManager.Instance.questAccepted;

        string scene =
            SceneManager.GetActiveScene().name;


        // =================================================
        // SCENE 1 - หมู่บ้าน
        // =================================================

        if (scene == "Scene1")
        {
            // QUEST 1
            if (quest == 1)
            {
                if (!accepted)
                {
                    SetTarget("NPC");
                }
                else if (QuestManager.Instance.IsQuest1Complete())
                {
                    SetTarget("NPC");
                }
                else
                {
                    SetTarget("Teleport1");
                }

                return;
            }


            // QUEST 2
            if (quest == 2)
            {
                if (!accepted)
                {
                    SetTarget("NPC");
                }
                else if (!QuestManager.Instance.storageCompleted)
                {
                    SetTarget("Storage");
                }
                else
                {
                    SetTarget("NPC");
                }

                return;
            }


            // QUEST 3
            if (quest == 3)
            {
                if (!accepted)
                {
                    SetTarget("NPC");
                }
                else if (!QuestManager.Instance.houseUpgraded)
                {
                    SetTarget("UpgradeHouse");
                }
                else
                {
                    SetTarget("NPC");
                }

                return;
            }


            // QUEST 4
            if (quest == 4)
            {
                if (!accepted || QuestManager.Instance.IsQuest4Complete())
                {
                    SetTarget("NPC");
                }
                else
                {
                    SetTarget("Teleport1");
                }

                return;
            }


            // QUEST 5
            if (quest == 5)
            {
                if (!accepted)
                {
                    SetTarget("NPC");
                }
                else
                {
                    SetTarget("Teleport1");
                }

                return;
            }
        }


        // =================================================
        // SCENE 2 - Resource / Enemy
        // =================================================

        if (scene == "Scene2")
        {
            // QUEST 1
            if (quest == 1)
            {
                QuestManager.Instance.IsQuest1Complete();
                // ยังเก็บของไม่ครบ
                // ให้ผู้เล่นหาเอง
                if (!QuestManager.Instance.IsQuest1Complete())
                {
                    HideNavigation();
                }
                else
                {
                    // ครบแล้ว กลับหมู่บ้าน
                    SetTarget("Teleport2");
                }

                return;
            }


            // QUEST 4
            if (quest == 4)
            {
                // ยังฆ่าไม่ครบ
                if (!QuestManager.Instance.IsQuest4Complete())
                {
                    HideNavigation();
                }
                else
                {
                    // ครบแล้ว กลับหมู่บ้าน
                    SetTarget("Teleport2");
                }

                return;
            }


            // QUEST 5
            if (quest == 5)
            {
                // ไป Boss
                SetTarget("Teleport3");

                return;
            }
        }


        // =================================================
        // SCENE 3 - BOSS
        // =================================================

        if (scene == "Scene3")
        {
            HideNavigation();
            return;
        }


        HideNavigation();
    }


    // =====================================================
    // TARGET
    // =====================================================

    void SetTarget(string targetTag)
    {
        GameObject target = null;

        try
        {
            target =
                GameObject.FindGameObjectWithTag(targetTag);
        }
        catch
        {
            /*Debug.LogWarning(
                "ไม่มี Tag : " + targetTag
            );*/

            HideNavigation();
            return;
        }


        if (target == null)
        {
            /*Debug.LogWarning(
                "หา Navigation Target ไม่เจอ : " +
                targetTag
            );*/

            HideNavigation();
            return;
        }


        currentTarget = target.transform;

        /*Debug.Log(
            "Navigation Target → " +
            currentTarget.name
        );*/

        CreateTrail();
    }


    // =====================================================
    // CREATE NAVMESH TRAIL
    // =====================================================

    void CreateTrail()
    {
        ClearTrail();

        if (player == null ||
            currentTarget == null ||
            pointPrefab == null)
        {
            return;
        }


        // -------------------------
        // หา Player บน NavMesh
        // -------------------------

        NavMeshHit playerHit;

        if (!NavMesh.SamplePosition(
            player.position,
            out playerHit,
            sampleDistance,
            NavMesh.AllAreas))
        {
            /*Debug.LogWarning(
                "หา Player บน NavMesh ไม่เจอ"
            );*/

            return;
        }


        // -------------------------
        // หา Target บน NavMesh
        // -------------------------

        NavMeshHit targetHit;

        if (!NavMesh.SamplePosition(
            currentTarget.position,
            out targetHit,
            sampleDistance,
            NavMesh.AllAreas))
        {
            /*Debug.LogWarning(
                "หา Target บน NavMesh ไม่เจอ : " +
                currentTarget.name
            );*/

            return;
        }


        // -------------------------
        // คำนวณ Path
        // -------------------------

        NavMeshPath path =
            new NavMeshPath();

        bool foundPath =
            NavMesh.CalculatePath(
                playerHit.position,
                targetHit.position,
                NavMesh.AllAreas,
                path
            );


        if (!foundPath ||
            path.status != NavMeshPathStatus.PathComplete)
        {
            /*Debug.LogWarning(
                "หาเส้นทางไป " +
                currentTarget.name +
                " ไม่สำเร็จ"
            );*/

            return;
        }


        // -------------------------
        // สร้างจุดตาม Path
        // -------------------------

        Vector3[] corners = path.corners;

        for (int i = 0;
             i < corners.Length - 1;
             i++)
        {
            CreatePointsBetween(
                corners[i],
                corners[i + 1]
            );
        }
    }


    // =====================================================
    // CREATE POINTS BETWEEN CORNERS
    // =====================================================

    void CreatePointsBetween(
        Vector3 start,
        Vector3 end)
    {
        float distance =
            Vector3.Distance(start, end);

        int pointCount =
            Mathf.FloorToInt(
                distance / pointSpacing
            );


        for (int i = 1;
             i <= pointCount;
             i++)
        {
            float travelDistance =
                i * pointSpacing;

            float t =
                travelDistance / distance;

            Vector3 position =
                Vector3.Lerp(
                    start,
                    end,
                    t
                );


            // ให้จุดเกาะ NavMesh
            NavMeshHit hit;

            if (NavMesh.SamplePosition(
                position,
                out hit,
                1f,
                NavMesh.AllAreas))
            {
                position = hit.position;
            }


            position.y += pointHeight;


            GameObject point =
                Instantiate(
                    pointPrefab,
                    position,
                    Quaternion.identity
                );


            spawnedPoints.Add(point);
        }
    }


    // =====================================================
    // CLEAR
    // =====================================================

    void ClearTrail()
    {
        foreach (GameObject point in spawnedPoints)
        {
            if (point != null)
            {
                Destroy(point);
            }
        }

        spawnedPoints.Clear();
    }


    public void HideNavigation()
    {
        currentTarget = null;

        ClearTrail();
    }
}