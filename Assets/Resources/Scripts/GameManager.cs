using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public GameObject[] availableObjectives;
    public GameObject[] availableObstacles;

    private HashSet<GameObjective> objectives;
    private Dictionary<GameObject, Vector2> obstacles;
    private Clock gameClock;
    private GameObject report;

    private int currentLevel;
    private int maxLevel = 2;

    private bool ldfinished;
    private bool starting;

    private Vector2 startPos = new Vector2(-2.42f, 18.6f);
    public GameObject playerObj;

    void Start () {
        gameClock = GameObject.FindGameObjectWithTag("Clock").GetComponent<Clock>();
        report = GameObject.FindGameObjectWithTag("Report");

        this.objectives = new HashSet<GameObjective>();
        this.obstacles = new Dictionary<GameObject, Vector2>();
        starting = false;
        ldfinished = false;

        SetupGame(1);
        ShowStartReport(currentLevel);
        GameGlobalState.Get().IsPlaying = false;
    }

    void Update () {
        if (!GameGlobalState.Get().IsPlaying && starting && report.activeSelf && Input.GetKeyUp(KeyCode.Space))
        {
            report.SetActive(false);
            GameGlobalState.Get().IsPlaying = true;
        }

        if (!GameGlobalState.Get().IsPlaying && !starting && report.activeSelf && Input.GetKeyUp(KeyCode.Space))
        {
            if (ldfinished)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            string evaluation = CalculateEvaluation(currentLevel);
            if (evaluation.Equals("Failed"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            } else
            {
                // Load the next level
                if (currentLevel == maxLevel)
                {
                    report.transform.FindChild("BodyText").GetComponent<Text>().text = 
                        "Game finished. Thanks for playing! Sorry didn't have time to implement a full game :(";
                    ldfinished = true;
                } else
                {
                    SetupGame(currentLevel + 1);
                    ShowStartReport(currentLevel);
                }
            }
        }

        if (GameGlobalState.Get().IsPlaying)
        {
            gameClock.Tick(Time.deltaTime);

            if (objectives.All(o => o.IsCompleted()))
            {
                GameGlobalState.Get().IsPlaying = false;
                ShowEndReport(currentLevel);
            }

            if (currentLevel == 1 && gameClock.Hour == 12)
            {
                GameGlobalState.Get().IsPlaying = false;
                ShowEndReport(currentLevel);
            }
        }
    }

    public bool isActiveObjective(GameObjective obj)
    {
        return objectives.Contains(obj);
    }

    void ShowStartReport(int level)
    {
        report.SetActive(true);
        starting = true;
        Text text = report.transform.FindChild("BodyText").GetComponent<Text>();
        if (level == 1)
        {
            return;
        } else if (level == 2)
        {
            text.text = "Good Day!\n\nMr Jones is checked in and he requested room service. You have half an hour, please make sure everything is clean."
                + " As you are cleaning, please be careful and not move any customer items. I don't want to get in trouble.\n\n"
                + "<i>By the way, I've ordered two more plants in the room because Mr Jones complained there was not enough green."
                + "Make sure you water those as well.</i>\n\nJack,\nHotel Manager";
        }
    }

    void ShowEndReport(int level)
    {
        report.SetActive(true);
        starting = false;
        GameGlobalState.Get().IsPlaying = false;

        string evaluation = CalculateEvaluation(level);
        Text text = report.transform.FindChild("BodyText").GetComponent<Text>();
        text.text = "";

        if (level == 1)
        {
            text.text = "Your first day is finished!";
        } else if (level == 2)
        {
            text.text = "Mr Jones has checked out. Thanks for the hard work!";
        }
        
        // Add the task list report
        text.text += "\n\nHere is your result: \n" 
            + string.Join("\n", objectives.Select(o => "-    " + o.GetDescription() + " (" + (o.IsCompleted() ? "Done" : "Not Done") + ")").ToArray())
            + "\n"
            + string.Join("\n", obstacles.Keys.Where(key => Vector2.Distance(key.transform.position, obstacles[key]) > 0).Select(key => "-    " + key.name + " is moved.").ToArray());

        // Add evaluation
        text.text += "\n\nEvaluation:   <b>" + evaluation + "</b>";
    }

    string CalculateEvaluation(int level)
    {
        float score = calculateScore();
        float passScore = 0;
        float satisfactoryScore = 0;
        float perfectScore = 1;
        
        if (level == 1)
        {
            passScore = 1f;
        } else if (level == 2)
        {
            satisfactoryScore = 0.8f;
            passScore = 0.5f;
        }

        if (score < passScore)
        {
            return "Failed";
        } else if (score >= passScore && score < satisfactoryScore)
        {
            return "Passed";
        } else if (score >= satisfactoryScore && score < perfectScore)
        {
            return "Satisfactory";
        } else
        {
            return "Perfect";
        }
    }

    float calculateScore()
    {
        float total = objectives.Aggregate<GameObjective, float>(0, (acc, o) => acc + o.GetWeight()) + obstacles.Keys.Count;

        float score = objectives.Aggregate<GameObjective, float>(0, (acc, o) => acc + (o.IsCompleted() ? o.GetWeight() : 0))
            + obstacles.Keys.Aggregate<GameObject, float>(0, (acc, go) => acc + (Vector2.Distance(go.transform.position, obstacles[go]) > 0 ? 0 : 1));
        return score / total;
    }

    void SetupGame(int level)
    {
        CleanObjectives();

        playerObj.transform.position = startPos;
        if (level == 1)
        {
            // Level 1
            // Set the timer
            // 11:30am to 12am
            currentLevel = 1;
            gameClock.Hour = 11;
            gameClock.Minute = 30;

            Enumerable.Range(0, 3).ToList().ForEach(i => AddToObjective(i));
            foreach (GameObjective o in objectives)
            {
                o.Init();
            }
        } else if (level == 2)
        {
            // Level 1
            // Set the timer
            // 11:30am to 12am
            currentLevel = 2;
            gameClock.Hour = 11;
            gameClock.Minute = 30;

            Enumerable.Range(0, 5).ToList().ForEach(i => AddToObjective(i));
            foreach (GameObjective o in objectives)
            {
                o.Init();
            }

            AddToObstacles(0, new Vector2(3.715403f, 18.77188f));
            AddToObstacles(1, new Vector2(14.2f, 15.82f));
        }
    }

    private void CleanObjectives()
    {
        if (objectives.Count > 0)
        {
            foreach (GameObjective o in objectives)
            {
                o.Cleanup();
            }
            objectives.Clear();
        } else
        {
            foreach (GameObject go in availableObjectives)
            {
                go.GetComponent<GameObjective>().Cleanup();
            }
            objectives.Clear();
        }
    }

    private void CleanObstacles()
    {
        obstacles.Keys.ToList().ForEach(k => k.SetActive(false));
        obstacles.Clear();
    }

    private void AddToObstacles(int index, Vector2 pos)
    {
        GameObject ob = availableObstacles[index];
        ob.SetActive(true);
        ob.transform.position = pos;
        obstacles.Add(ob, pos);
    }

    private void AddToObjective(int index)
    {
        GameObject gobj = availableObjectives[index];
        gobj.SetActive(true);
        objectives.Add(gobj.GetComponent<GameObjective>());
    }
}

public interface Interactable
{
    void ActivateObj();
    void DeactivateObj();
}

public interface GameObjective : Interactable
{
    bool IsCompleted();
    void Init();
    void Cleanup();
    string GetDescription();
    float GetWeight();
}

public class GameGlobalState
{
    private static GameGlobalState instance = null;
    public static GameGlobalState Get()
    {
        if (instance == null)
        {
            instance = new GameGlobalState();
        }
        return instance;
    }

    private bool isPlaying;
    public bool IsPlaying
    {
        get { return isPlaying; }
        set { isPlaying = value; }
    }

    private GameGlobalState()
    {
        this.isPlaying = false;
    }
}