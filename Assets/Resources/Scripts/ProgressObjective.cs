using UnityEngine;
using UnityEngine.UI;

public abstract class ProgressObjective : MonoBehaviour, GameObjective {

    public GameObject canvas;
    public Image progressBar;
    public Text progressText;

    public Sprite[] sprites;
    private int progress;

    internal abstract string GetHint();
    internal abstract string GetCompletedText();
    internal abstract int GetProgressStep();
    internal abstract KeyCode GetKeyCode();
    public abstract string GetDescription();
    public abstract float GetWeight();

    void Start()
    {
        Init();
    }

    public void Init()
    {
        progress = 0;
        canvas.SetActive(false);
        progressText.text = GetHint();

        GetComponent<SpriteRenderer>().sprite = sprites[0];
        progressBar.fillAmount = progress / 100f;
    }

    public void Cleanup()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[1];
        canvas.SetActive(false);
    }

    public void Update()
    {
        if (canvas.activeSelf && Input.GetKeyUp(GetKeyCode()))
        {
            progress += GetProgressStep();
            if (progress >= 100)
            {
                progress = 100;
                progressBar.fillAmount = 1f;
                GetComponent<SpriteRenderer>().sprite = sprites[1];
                progressText.text = GetCompletedText();
            }
            else
            {
                progressText.text = progress + "%";
                progressBar.fillAmount = progress / 100f;
            }
        }
    }

    public void ActivateObj()
    {
        canvas.SetActive(true);
    }

    public void DeactivateObj()
    {
        canvas.SetActive(false);
    }

    public bool IsCompleted()
    {
        return progress == 100;
    }
}
