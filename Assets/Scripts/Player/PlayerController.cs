﻿using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    // these variables are used for interactions with interactable objects
    public int interactionRadius;
    private GameObject lineEndWest, lineEndEast, lineEndSouth, lineEndNorth;
    private Transform lineEnd, lineStart;
    private GameObject gameManagerObject;
    private GameManager gameManager;
    private List<string> gadgetList;
    RaycastHit2D hitObject;
    bool interact = false;

    // speed of the player
    public float speed;

    Animator animator;
    // where the last movemement is
    Vector2 lastMovement;
    // checks if player is in idle
    bool isRunning;
    // used for diagonals
    float currentSpeed;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        gameManagerObject = GameObject.FindWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        gadgetList = new List<string>();
        gadgetList.Add("Theca");
        gadgetList.Add("Painting");
        gadgetList.Add("Safe");
        gadgetList.Add("Door");

        lineEndWest = new GameObject();
        lineEndEast = new GameObject();
        lineEndSouth = new GameObject();
        lineEndNorth = new GameObject();
        lineEndWest.transform.position = new Vector2(gameObject.transform.position.x - interactionRadius, gameObject.transform.position.y) ;
        lineEndEast.transform.position = new Vector2(gameObject.transform.position.x + interactionRadius, gameObject.transform.position.y);
        lineEndSouth.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - interactionRadius);
        lineEndNorth.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + interactionRadius);
        lineEnd = lineEndSouth.transform;
        lineStart = gameObject.transform;
    }

    // Update is called once per frame
    void Update() {
        Move();
        Raycast();
    }

    // move the player
    void Move() {
        // get the stress value of the axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // each frame, at the start player not run
        isRunning = false;
        // move horizontally
        if (horizontal > 0f || horizontal < 0f) {
            transform.Translate(horizontal * currentSpeed * Time.deltaTime, 0f, 0f);
            isRunning = true;
            lastMovement = new Vector2(horizontal, 0f);
            if (horizontal > 0f)
            {
                lineEndEast.transform.position = new Vector2(gameObject.transform.position.x + interactionRadius, gameObject.transform.position.y);
                lineEnd = lineEndEast.transform;
            }
            else
            {
                lineEndWest.transform.position = new Vector2(gameObject.transform.position.x - interactionRadius, gameObject.transform.position.y);
                lineEnd = lineEndWest.transform;
            }
        }
        // move vertically
        if (vertical > 0f || vertical < 0f) {
            transform.Translate(0f, vertical * currentSpeed * Time.deltaTime, 0f);
            isRunning = true;
            lastMovement = new Vector2(0f, vertical);
            if (vertical > 0f)
            {
                lineEndNorth.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + interactionRadius);
                lineEnd = lineEndNorth.transform;
            }
            else
            {
                lineEndSouth.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - interactionRadius);
                lineEnd = lineEndSouth.transform;
            }
        }
        // check if we move in diagonal on the map and it reduces the speed
        if (Mathf.Abs(horizontal) > 0.5f && Mathf.Abs(vertical) > 0.5f) {
            currentSpeed = speed / Mathf.Sqrt(2f);
        }
        else {
            currentSpeed = speed;
        }
        // set values of the parameters
        animator.SetFloat("RunX", horizontal);
        animator.SetFloat("RunY", vertical);
        animator.SetFloat("LastRunX", lastMovement.x);
        animator.SetFloat("LastRunY", lastMovement.y);
        animator.SetBool("Running", isRunning);
    }

    //cast a ray from the player to see if he can activate an interaction with an object
    void Raycast() {
        Debug.DrawLine(lineStart.position, lineEnd.position, Color.black);

        if (Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Interactable")))
        {
            hitObject = Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Interactable"));
            if (checkGadgetPresence(hitObject.collider.gameObject.GetComponent<InteractableObject>().getObjectType()))
            {
                gameManager.ActivateInteractionText(true);
                interact = true;
            }
            else
                gameManager.ActivateNoGadgetText(true);
        }
        else
        {
            interact = false;
            gameManager.ActivateInteractionText(false);
            gameManager.ActivateNoGadgetText(false);
        }

        //if the user can interact with the object AND he presses E, he interacts
        if (Input.GetKeyDown(KeyCode.E) && interact == true)
        {
             gameManager.AddMoney(hitObject.collider.gameObject.GetComponent<InteractableObject>().Interact());
        }

    }

    private bool checkGadgetPresence(string objectType)
    {
        foreach(string gadget in gadgetList)
        {
            if (objectType == gadget)
                return true;
        }
        return false;

    }

}