using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
     *  Targets:
     *  1) Static targets (blue) are just spawned on one position and not moving
     *  2) Movable targets (red) are spawned and moving within arena
     *
     *  Target spawning:
     *  I've decided to use Object pooling to avoid performance issues (in case of 30+ targets)
     *  Approach: Create multiple targets, set them inactive after destroy and activate them after respawn (and prevent overload instantiating).
     *   
     */
    #region Singleton Instance
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject g = new GameObject("GameManager");
                g.AddComponent<GameManager>();
            }
            return instance;
        }
    }


    #endregion

    [Header("Game settings")]
    [SerializeField]
    [Tooltip("Number of targets in arena")]
    private int numberOfTargets;

    [SerializeField]
    [Tooltip("Time between destroy and respawn")]
    private float newTargetSpawnDelay = 2f;

    private List<GameObject> targets;           // List of all targets (active and unactive)

    [Header("Prefabs & Objects")]
    public GameObject targetPrefab;             
    public GameObject movableTargetPrefab;           
    public GameObject arena;
    public GameObject tutorialImage;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        arena = GameObject.FindGameObjectWithTag("Arena");

        StartCoroutine(FadeImage());

        // This creates N targets, sets them inactive (prevent overloading during runtime) - Object Pooling

        targets = new List<GameObject>();

        for (int i = 0; i < numberOfTargets; i++)
        {
            // Every second target is movable (AI)

            GameObject target = i % 2 == 0 ? Instantiate(targetPrefab) : Instantiate(movableTargetPrefab);

            targets.Add(target);
            target.SetActive(true);
        }
    }
    public void Update()
    {
        if (Input.GetKey("escape"))
        {
            ExitGame();
        }
    }

    // Secures a spawn after N miliseconds (triggered by Target)
    public void TargetDestroyed()
    {
        StartCoroutine(SpawnTarget());
    }

    IEnumerator SpawnTarget()
    {
        yield return new WaitForSeconds(newTargetSpawnDelay);

        GameObject target = GetFirstInactiveTarget();
        target.SetActive(true);
    }

    private GameObject GetFirstInactiveTarget()
    {
        foreach (var t in targets)
        {
            if (!t.activeInHierarchy)
            {
                return t;
            }
        }

        return null;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Fade out tutorial image
    IEnumerator FadeImage()
    {
        yield return new WaitForSeconds(4.5f);

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            tutorialImage.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
            yield return null;
        }
        Destroy(tutorialImage);
    }
}
