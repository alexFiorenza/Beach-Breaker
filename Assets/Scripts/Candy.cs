using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{

    private static Color selectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    private static Candy previousSelected = null;
    private SpriteRenderer spriteRenderer;
    private bool isSelected=false;
    public int id;
    private Vector2[] adjacentDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void SelectCandy()
    {
        isSelected = true;
        spriteRenderer.color = selectedColor;
        previousSelected = gameObject.GetComponent<Candy>(); //El anterior seleccionado es el actual
    }
    private void DeselectCandy()
    {
        isSelected = false;
        spriteRenderer.color = Color.white;
        previousSelected = null;
    }
    private void OnMouseDown()
    {
        if (spriteRenderer.sprite == null || BoardManager.sharedInstance.isShifting)
        {
            return;
        }
        if (isSelected)
        {
            DeselectCandy();
        }
        else
        {
            if (previousSelected == null&&GUIManager.sharedInstance.currentGameState==GameState.inGame)
            {
                SelectCandy();
            }
            else
            {
                if (CanSwipe())
                {
                    SwapSprite(previousSelected);
                    previousSelected.FindAllMatches();
                    previousSelected.DeselectCandy();
                    FindAllMatches();
                    GUIManager.sharedInstance.MoveCounter--;
                }
                else
                {
                    if (GUIManager.sharedInstance.currentGameState == GameState.inGame)
                    {
                        previousSelected.DeselectCandy();
                        SelectCandy();
                    }
                    
                }
               
                //Accedo al script del objeto seleccionado con anterioridad y a ese lo deselecciono 
               
            }
        }
    }

    public void SwapSprite(Candy newCandy)
    {
        if (spriteRenderer.sprite== newCandy.GetComponent<SpriteRenderer>().sprite)
        {
            return;
        }
        Sprite oldCandy = newCandy.spriteRenderer.sprite; //aux
        newCandy.spriteRenderer.sprite = this.spriteRenderer.sprite;
        this.spriteRenderer.sprite = oldCandy;
        int tempId = newCandy.id; //aux
        newCandy.id = this.id;
        this.id = tempId;
    }
    private GameObject GetNeighbor(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);

        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }
    private List<GameObject> GetAllNeighbors()
    {
        List<GameObject> neighbors = new List<GameObject>();

        foreach(Vector2 direction in adjacentDirections)
        {
            neighbors.Add(GetNeighbor(direction));
        }
        return neighbors;
    }

    private bool CanSwipe()
    {
        if( GUIManager.sharedInstance.currentGameState == GameState.inGame)
        {
            return GetAllNeighbors().Contains(previousSelected.gameObject);
        }
        else
        {
            return false;
        }
       
    }
    private List<GameObject> FindMatch(Vector2 direction)
    {
        List<GameObject> matchingCandies = new List<GameObject>();                  
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);        //Trazar un raycast desde mi transform a la dirección indicada
        while(hit.collider !=null && hit.collider.GetComponent<SpriteRenderer>().sprite==spriteRenderer.sprite)          //Si el raycast hit detecta un collider y ese collider tiene la misma id que mi transform entonces se agrega al array de matching candies y se traza otro rayo
        {
            matchingCandies.Add(hit.collider.gameObject);
            hit= Physics2D.Raycast(hit.collider.transform.position, direction);         //Desde la direccion del collider que se detecto hacia la dirección que se indico
        }
        return matchingCandies;
    }

    private bool ClearMatch(Vector2[] directions)
    {
        List<GameObject> matchingCandies = new List<GameObject>();
        foreach(Vector2 direction in directions)    //Recorro las direccion que me llegaron por parametro
        {
            matchingCandies.AddRange(FindMatch(direction));             //Y se las paso al findMatch para que busque concidencias 
        }
        if (matchingCandies.Count >= BoardManager.MinCandiesToMatch)   //Si todas las concidencias que busco supera 2 entonces lo limpiamos 
        {
            foreach(GameObject candy in matchingCandies)
            {
                candy.GetComponent<SpriteRenderer>().sprite = null;

            }
            return true;    
        }
        else
        {
            return false;
        }
        
    }

    public void FindAllMatches()
    {
        if (spriteRenderer.sprite == null)
        {
            return;
        }
        bool hMatch = ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
        bool vMatch = ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
        if (hMatch || vMatch)
        {
            spriteRenderer.sprite = null;
            StopCoroutine(BoardManager.sharedInstance.FindNullCandies());
            StartCoroutine(BoardManager.sharedInstance.FindNullCandies());
            BoardManager.sharedInstance.sfxSource.Play();
        }
        
    }
    
}
