using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The current state the game is in (View, Place, Remove, etc.)
// View is default state (player is viewing environment), Place is when the player is placing a species, etc.
public enum GameState
{
    View,
    Place,
    Delete
    // Add other states here
}

public class GameManagerScript : MonoBehaviour
{
    public Camera mainCamera;
    public ScoreHandler scoreScript;

    private GameState state = GameState.View;
    // a prefab of an object to be placed, if in Place GameState
    private GameObject placingObject;
    // a preview instance of the object to be placed
    private GameObject placingObjectPreview;
<<<<<<< Updated upstream
=======
    
    private CoinHandler coinHandler;


    private void Awake()
    {
        coinHandler = FindObjectOfType<CoinHandler>(); 
    }
    
>>>>>>> Stashed changes
    void Start()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (state == GameState.Place)
        {
            Placing();
        }

        scoreScript = GameObject.FindWithTag("UpperRightUI").GetComponent<ScoreHandler>();
    }

    public void SetState(GameState newState, GameObject objectToPlace = null)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Place:
                // Set the object to be placed to the desired prefab
                placingObject = objectToPlace;
                // Create a preview version of the desired prefab
                CreateObjectPreview(objectToPlace);

                break;
        }
    }

    // To be run when GameState is in Place state
    private void Placing()
    {
        // Have a preview of the species to be placed follow the mouse, showing the player where it would be placed
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            
            // Calculate the true bottom of the model with all renderers accounted for. 
            
            float adjustmentHeight = 0; 
            Queue<Renderer> renderers = new Queue<Renderer>(); 
            
            if (placingObjectPreview.GetComponent<Renderer>())
            {
                renderers.Enqueue(placingObjectPreview.GetComponent<Renderer>()); 
            }

            foreach (var rend in placingObjectPreview.GetComponentsInChildren<Renderer>())
            {
                renderers.Enqueue(rend);
            }

            if (renderers.Count > 0)
            {
                Bounds combinedBounds = renderers.Dequeue().bounds;
                while (renderers.Count > 0)
                {
                    combinedBounds.Encapsulate(renderers.Dequeue().bounds);   
                }
                adjustmentHeight = combinedBounds.size.y / 2f;
            }
            
            // place the object on top of the collider, not halfway through it
            Vector3 placePoint = new Vector3(hit.point.x, adjustmentHeight + 1f, hit.point.z);
            placingObjectPreview.transform.position = placePoint;

            // The object should be spawned when the mouse is clicked, reverting to view mode
            if (Input.GetMouseButton(0))
            {
                Destroy(placingObjectPreview);
                Instantiate<GameObject>(placingObject, placePoint, Quaternion.Euler(0, 0, 0));
                SetState(GameState.View);
                if (scoreScript.animalScoreValues.ContainsKey(placingObject.tag)) {
                    scoreScript.changeScore((int)scoreScript.animalScoreValues[placingObject.tag]);
                }
            }
        }

        // if user presses escape, revert to view mode
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(placingObjectPreview);
            SetState(GameState.View);
        }
    }

    private void CreateObjectPreview(GameObject objectToPreview)
    {
        // Destroy any existing placement preview
        if (placingObjectPreview)
        {
            Destroy(placingObjectPreview);
        }

        // New instance of the object, placed in the center of the screen 
        placingObjectPreview = Instantiate<GameObject>(objectToPreview,
            mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 5)), Quaternion.Euler(0, 0, 0));

        // remove behavior from this species so it is essentially just a model
        Destroy(placingObjectPreview.GetComponent<MonoBehaviour>());
        Destroy(placingObjectPreview.GetComponent<Collider>());
        Destroy(placingObjectPreview.GetComponent<Rigidbody>());

        // TODO: The current models don't go transparent, even in the editor.  Figure out why this is. 
        if (placingObjectPreview.GetComponent<Renderer>())
        {
            Renderer r = placingObjectPreview.GetComponent<Renderer>(); 
            // now make a new color that is slightly transparent and apply it to each of the renderer's materials
            foreach (Material m in r.materials)
            {
                Color newColor = m.color;
                newColor.a = .3f;
                m.color = newColor;
            }
            
            // remove shadows from preview
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        foreach (Renderer r in placingObjectPreview.GetComponentsInChildren<Renderer>())
        {
            // now make a new color that is slightly transparent and apply it to each of the renderer's materials
            foreach (Material m in r.materials)
            {
                Color newColor = m.color;
                newColor.a = .3f;
                m.color = newColor;
            }

            // remove shadows from preview
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
}
