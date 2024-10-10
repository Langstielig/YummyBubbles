using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubblesGenerator : MonoBehaviour
{
    [SerializeField] private Sprite[] bubblesSprites = new Sprite[5];
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private GameObject images;

    private GameObject xyinya;

    private Button[,] bubbles = new Button[10, 6];
    private int[,] bubblesIndex = new int[10, 6];

    private bool isTutorial;
    private int tutorialLine = 4, tutorialColumn = 2;
    private bool isBubblesActive = true;

    private void Awake()
    {
        GetButtons();
    }

    private void Start()
    {
        GenerateSprites();
        MakeTutorialBubbles();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {

            Vector3 coordinates = bubbles[0, 0].transform.position;
            //Debug.Log("coordinates " + coordinates);
            Vector3 localCoordinates = bubbles[0, 0].transform.localPosition;
            //Debug.Log("local coordinates " + localCoordinates);
            //Vector3 newCoordinates = Camera.main.ScreenToWorldPoint(coordinates);
            float height = bubbles[0, 0].gameObject.GetComponent<RectTransform>().rect.height;
            float width = bubbles[0, 0].gameObject.GetComponent<RectTransform>().rect.width;
            GameObject newImage = Instantiate(imagePrefab);
            newImage.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            newImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(10f, 10f);
            newImage.transform.SetParent(images.transform);

            //GameObject clone = Instantiate(bubbles[0, 0].gameObject);
            //clone.transform.SetParent(images.transform);

            //xyinya = newImage;

            //AnimationClip clip = new AnimationClip();
            //clip.legacy = true;
            //Keyframe[] keys = new Keyframe[3]; 
            //keys[0] = new Keyframe(0.0f, 0.0f); 
            //keys[1] = new Keyframe(1.0f, 1.5f); 
            //keys[2] = new Keyframe(2.0f, 0.0f); 
            //AnimationCurve curve = new AnimationCurve(keys);
            //clip.SetCurve("", typeof(Transform), "localPosition.x", curve);
            //curve = AnimationCurve.Linear(0.0f, 1.0f, 2.0f, 0.0f);
            //clip.SetCurve("", typeof(Renderer), "material._Color.r", curve);

            //Animation anim = newImage.GetComponent<Animation>();
            //anim.AddClip(clip, "Drop");
            //anim.Play();
        }
        //if (xyinya != null)
        //{
        //    xyinya.transform.position = Vector3.MoveTowards(xyinya.transform.position, bubbles[1, 0].transform.position, 0.1f);
        //    Debug.Log("distance is " + (xyinya.transform.position.y - bubbles[1, 0].transform.position.y));
        //    if(xyinya.transform.position.y - bubbles[1, 0].transform.position.y <= 0)
        //    {
        //        Destroy(xyinya);
        //    }
        //}
    }

    public void ChangeGameStatus(bool status)
    {
        isBubblesActive = status;
    }

    public void OnBubbleClick(GameObject currentGameObject)
    {
        int line, column;
        (line, column) = FindButton(currentGameObject);

        if((isBubblesActive && isTutorial && line == tutorialLine && column >= tutorialColumn && column < tutorialColumn + 3) ||
            (isBubblesActive && !isTutorial && line != -1 && column != -1))
        {
            if(isTutorial)
            {
                MessagesController messagesController = FindObjectOfType<MessagesController>();
                if(messagesController != null) 
                {
                    messagesController.CloseTutorial();
                }
            }

            int countOfPoints = RemoveSimilarBubbles(line, column);

            PanelsController panelsController = FindObjectOfType<PanelsController>();
            if (panelsController != null)
            {
                panelsController.ChangeScore(countOfPoints);
                if (countOfPoints > 2)
                {
                    panelsController.ChangeCountOfTurns(countOfPoints - 1);
                }
                else
                {
                    panelsController.ChangeCountOfTurns(-1);
                }
            }
        }
    }

    private void FillBubble(int line, int column, int indexOfSprite)
    {
        bubblesIndex[line, column] = indexOfSprite;
        bubbles[line, column].GetComponent<Image>().sprite = bubblesSprites[indexOfSprite];
    }

    private int RemoveSimilarBubbles(int line, int column)
    {
        if(isTutorial)
        {
            isTutorial = false;
        }

        List<Tuple<int, int>> indices = new List<Tuple<int, int>>();
        indices.Add(new Tuple<int, int>(line, column));
        FindSimilarBubbles(line, column, ref indices);

        int countOfPoints = 1;

        if (indices.Count > 2)
        {
            for (int k = 0; k < indices.Count; k++)
            {
                int i, j;
                (i, j) = indices[k];
                StartCoroutine(ChangeToDisappearanceAnimation(i, j));
                DropBubblesInColumn(i, j);
            }

            countOfPoints = indices.Count;
        }
        else
        {
            StartCoroutine(ChangeToDisappearanceAnimation(line, column));
            DropBubblesInColumn(line, column);
        }

        return countOfPoints;
    }

    private void FindSimilarBubbles(int line, int column, ref List<Tuple<int, int>> indices)
    {
        for(int i = 0; i < 4; i++)
        {
            int lineTurm = 0;
            int columnTurm = 0;

            if(i % 2 == 0)
            {
                columnTurm = 1;
            }
            else
            {
                lineTurm = 1;
            }

            if(i > 1)
            {
                columnTurm = columnTurm * -1;
                lineTurm = lineTurm * -1;
            }

            Tuple<int, int> tuple = new Tuple<int, int>(line + lineTurm, column + columnTurm);
            if (line + lineTurm < bubblesIndex.GetLength(0) && line + lineTurm >= 0 &&
               column + columnTurm < bubblesIndex.GetLength(1) && column + columnTurm >= 0)
            {
                if (bubblesIndex[line, column] == bubblesIndex[line + lineTurm, column + columnTurm] &&
                !indices.Contains(tuple))
                {
                    indices.Add(tuple);
                    FindSimilarBubbles(line + lineTurm, column + columnTurm, ref indices);
                }
            }
        }
    }

    private void GetButtons()
    {
        for(int i = 0; i < bubbles.GetLength(0); i++)
        {
            GameObject currentLine = field.transform.GetChild(i).gameObject;
            for(int j = 0; j < bubbles.GetLength(1); j++)
            {
                Button currentButton = currentLine.transform.GetChild(j).gameObject.GetComponent<Button>();
                bubbles[i, j] = currentButton;
            }
        }
    }

    private void GenerateSprites()
    {
        for(int i = 0; i < bubbles.GetLength(0); i++)
        {
            for(int j = 0; j < bubbles.GetLength(1); j++)
            {
                int index = UnityEngine.Random.Range(0, bubblesSprites.Length);
                FillBubble(i, j, index);
            }
        }
    }

    private void MakeTutorialBubbles()
    {
        int indexOfSprite = UnityEngine.Random.Range(0, bubblesSprites.Length);
        
        for(int j = -1; j < 4; j++)
        {
            if(j == -1 || j == 3)
            {
                if (bubblesIndex[tutorialLine, tutorialColumn + j] == indexOfSprite)
                {
                    int index = RandomSpriteIndexWithExcludition(indexOfSprite);
                    FillBubble(tutorialLine, tutorialColumn + j, index);
                }
            }
            else
            {
                if (bubblesIndex[tutorialLine + 1, tutorialColumn + j] == indexOfSprite)
                {
                    int index = RandomSpriteIndexWithExcludition(indexOfSprite);
                    FillBubble(tutorialLine + 1, tutorialColumn + j, index);
                }
                if (bubblesIndex[tutorialLine - 1, tutorialColumn + j] == indexOfSprite)
                {
                    int index = RandomSpriteIndexWithExcludition(indexOfSprite);
                    FillBubble(tutorialLine - 1, tutorialColumn + j, index);
                }
            }
        }

        for (int j = 0; j < 3; j++)
        {
            FillBubble(tutorialLine, tutorialColumn + j, indexOfSprite);
            ChangeToPulsationAnimation(tutorialLine, tutorialColumn + j);
        }

        isTutorial = true;
    }

    private (int, int) FindButton(GameObject gameObject)
    {
        Button currentButton = gameObject.GetComponent<Button>();
        for (int i = 0; i < bubbles.GetLength(0); i++)
        {
            for (int j = 0; j < bubbles.GetLength(1); j++)
            {
                if (currentButton == bubbles[i, j])
                {
                    return (i, j);
                }
            }
        }
        return (-1, -1);
    }

    private void DropBubblesInColumn(int line, int column)
    {
        for (int i = line; i >= 1; i--)
        {
            StartCoroutine(ChangeToDisappearanceAnimation(i - 1, column, true));
        }

        StartCoroutine(GenerateSprite(0, column));
    }

    private int RandomSpriteIndexWithExcludition(int excludition)
    {
        int index = UnityEngine.Random.Range(0, bubblesSprites.Length);
        while(index == excludition)
        {
            index = UnityEngine.Random.Range(0, bubblesSprites.Length);
        }
        return index;
    }

    private void ChangeToPulsationAnimation(int i, int j)
    {
        Animator animator = bubbles[i, j].gameObject.GetComponent<Animator>();
        animator.SetBool("isPulsating", true);
    }

    private void TurnOffAllAnimations(int i, int j)
    {
        Animator animator = bubbles[i, j].gameObject.GetComponent<Animator>();

        animator.SetBool("isDisappearing", false);
        animator.SetBool("isAppearing", false);
        animator.SetBool("isPulsating", false);
    }

    IEnumerator ChangeToDisappearanceAnimation(int i, int j, bool next = false)
    {
        Animator animator = bubbles[i, j].gameObject.GetComponent<Animator>();
        animator.SetBool("isDisappearing", true);

        yield return new WaitForSeconds(1);

        if (next)
        {
            StartCoroutine(DropBubble(i + 1, j));
        }
    }

    IEnumerator ChangeToAppearanceAnimation(int i, int j)
    {
        Animator animator = bubbles[i, j].gameObject.GetComponent<Animator>();
        animator.SetBool("isAppearing", true);

        yield return new WaitForSeconds(1);

        TurnOffAllAnimations(i, j);
    }

    IEnumerator DropBubble(int line, int column)
    {
        bubblesIndex[line, column] = bubblesIndex[line - 1, column];
        bubbles[line, column].GetComponent<Image>().sprite = bubblesSprites[bubblesIndex[line, column]];

        yield return new WaitForSeconds(0);

        StartCoroutine(ChangeToAppearanceAnimation(line, column));
    }

    IEnumerator GenerateSprite(int i, int j)
    {
        yield return new WaitForSeconds(1);

        int index = UnityEngine.Random.Range(0, bubblesSprites.Length);
        FillBubble(i, j, index);

        StartCoroutine(ChangeToAppearanceAnimation(i, j));
    }
}
