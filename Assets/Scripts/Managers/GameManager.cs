using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Animals")]
    public Sprite[] animalSprites;
    public GameObject animalContainer;
    public GameObject animalPrefab;
    public GameObject animalToMatch;

    [Header("Animal Icon Positions")]
    public float minimumX;
    public float maximumX;

    [Space]
    public float minimumY;
    public float maximumY;

    [Header("Interface")]
    public Text message;

    [Header("Audio")]
    public AudioSource soundEffect;
    public AudioClip goodSound;
    public AudioClip badSound;

	// Use this for initialization
	void Start () {
        SetAnimalToMatch();
        RenderAnimals();
	}
	
	// Update is called once per frame
	void Update () {
        HandleTouch();
    }

    private void SetAnimalToMatch()
    {
        Sprite currentSprite = animalToMatch.GetComponent<SpriteRenderer>().sprite;
        Sprite newSprite = currentSprite;

        while (newSprite.name == currentSprite.name)
        {
            newSprite = animalSprites[Random.Range(0, animalSprites.Length - 1)];
        }

        animalToMatch.GetComponent<SpriteRenderer>().sprite = newSprite;
        animalToMatch.name = newSprite.name;
    }

    private void RenderAnimals()
    {
        float currentX = minimumX;
        float currentY = minimumY;

        foreach (Sprite sprite in animalSprites)
        {
            GameObject animal = Instantiate(animalPrefab, new Vector3(currentX, currentY, 0), Quaternion.identity);
            animal.GetComponent<SpriteRenderer>().sprite = sprite;
            animal.name = sprite.name;
            animal.transform.parent = animalContainer.transform;

            currentX++;

            if (currentX > maximumX)
            {
                currentX = minimumX;
                currentY++;
            }
        }
    }

    private void HandleTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                soundEffect.Stop();

                if (hit.collider.gameObject.name == animalToMatch.name)
                {
                    GoodMatch(); 
                }
                else
                {
                    BadMatch();
                }
            }
        }
    }

    private void GoodMatch()
    {
        string messageString = "You found the " + animalToMatch.name + "!";
        message.text = messageString.ToUpper();
        soundEffect.clip = goodSound;
        soundEffect.Play();
        SetAnimalToMatch();
    }

    private void BadMatch()
    {
        string messageString = "Try again!";
        message.text = messageString.ToUpper();
        soundEffect.clip = badSound;
        soundEffect.Play();
    }
}
