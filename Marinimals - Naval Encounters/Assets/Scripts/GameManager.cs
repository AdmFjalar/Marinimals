using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool addedPoint = false; //Used to check if the game has already added a point to the winner.

    private float[] damageTimer = new float[4];

    [SerializeField] private Slider volume; //Volume slider used to alter the ingame volume.

    [SerializeField] private Image[] joinGame = new Image[4]; //The images/animations telling the player to join the lobby by pressing A.
    [SerializeField] private Image[] ready = new Image[4]; //The images shown when the respective player is ready.

    [SerializeField] private Sprite[] crosshair = new Sprite[4]; //The sprites for the crosshairs. The color of the crosshair indicates which controller it belongs to, in order to help the player identify their sight.

    [SerializeField] private GameObject[] shipVariants = new GameObject[6]; //The different variants of ships the players can choose from.
    [SerializeField] private GameObject[] chosenShip = new GameObject[4]; //The chosen ship of the respective player.
    [SerializeField] private GameObject[] playerInformation = new GameObject[4]; //The information panels ingame that indicate health, points, etc.

    [SerializeField] private GameObject background; //The background gameobject used in the main menu.
    [SerializeField] private GameObject countdownTimer; //The countdown timer used when starting a match.
    [SerializeField] private GameObject victory; //The image/slide shown when a player has won a round.
    [SerializeField] private GameObject mapHazards; //Hazards on the map; Seamines, AI turrets, etc.
    [SerializeField] private GameObject stormPrefab;
    [SerializeField] private GameObject laserPrefab;

    [SerializeField] private TextMeshProUGUI[] playerIDText = new TextMeshProUGUI[4]; //The UI elements shown in the lobby to indicate the ID (order of gameobjects, i.e. Player 1, Player 2, etc) of the controller.
    [SerializeField] private TextMeshProUGUI gainPointText;

    [SerializeField] private PlayerControls[] controls = new PlayerControls[4]; //The different control layouts for each controller.

    [SerializeField] private bool[] hasJoined = new bool[4]; //Contains all the bools indicating whether a controllers has joined or not. Example: Controller 3 has joined.
    [SerializeField] private bool[] isReady = new bool[4]; //Contains all the bools indicating whether the controllers are ready or not. Example: Controller 3 has joined and chosen a character/ship.

    [SerializeField] private int stormRisk = 10000;
    [SerializeField] private int laserRisk = 100000;

    public Material[] materials = new Material[4];

    public int[] damageModifier = new int[4];

    public float maxBoundary = 20f; //The furthest distance a player can travel from the centre of the map without taking passive damage.

    public List<GameObject> players; //The players who are alive/on the map.

    public GameObject[] playerContainers = new GameObject[4]; //The gameobjects containing everything for each respective player.

    public GameObject lobby; //The lobby container.
    public GameObject singleplayerLobby; //The singleplayer lobby.
    public GameObject playermode; //The menu for selecting multi/singleplayer.
    public GameObject startMenu; //The startmenu container.
    public GameObject settings; //The settings container.

    public List<int> playerIDs; //The id's of the players who has joined, used to assign the correct controls to the correct player.

    public PlayerControls anyPlayer; //The control layout used to allow all players to navigate menus, exit a match, etc.

    public Country[] countries = new Country[6]; //The country profiles containing the name of the country and admiral, the portrait of the admiral, etc.

    public ShipSelectionWheelControl[] shipSelection = new ShipSelectionWheelControl[4]; //The selecion wheel holding the different ship options.

    public bool[] startedTimer = new bool[4];

    public bool hasStarted = false; //Indicates whether a match/round is ongoing.
    public bool inDialogue = false;
    public bool isPaused = false;

    public Camera mainCamera;

    public static GameManager instance;

    void Start()
    {
        instance = this;

        Application.targetFrameRate = 60;

        Cursor.visible = false;
    }

    void Update()
    {
        if (!hasStarted && lobby.activeSelf == true && !inDialogue) //Checks if there is not an ongoing match and if the lobby is open.
        {
            if (playerIDs.Count <= 0 && Input.GetButtonDown(anyPlayer.bButton)) //Checks if there are no players and if anyone is pressing B.
            {
                lobby.SetActive(false);
                playermode.SetActive(false);
                startMenu.SetActive(true);
                settings.SetActive(false);
            }

            for (int i = 0; i < 4; i++)
            {
                Join(i);

                Leave(i);
            }

            bool[] allReady = Array.FindAll(isReady, x => x == true); //Returns all players who are ready to start a match.

            if (allReady.Length == playerIDs.Count && playerIDs.Count > 1) //Checks if there are more than one player and if everyone is ready.
            {
                countdownTimer.SetActive(true); //Starts the countdown.
            }
            else if (allReady.Length < playerIDs.Count || playerIDs.Count <= 1) //Checks if there are fewer than or equal to one player in the lobby OR if everyone aren't ready.
            {
                countdownTimer.SetActive(false); //Stops the countdown.
            }
        }
        else if (hasStarted) //Checks if there is an ongoing match/round.
        {
            for (int i = 0; i < 4; i++)
            {
                DamageModifier(i);
            }

            countdownTimer.SetActive(false); //Deactivates the timer.
        }
        else if (!hasStarted && !lobby.activeSelf && settings.activeSelf && Input.GetButtonDown(anyPlayer.bButton) && !inDialogue) //Checks if the player/s are in the settings and are pressing B. 
        {
            lobby.SetActive(false);
            playermode.SetActive(false);
            startMenu.SetActive(true);
            settings.SetActive(false);
        }
        else if (!hasStarted && !lobby.activeSelf && !settings.activeSelf && singleplayerLobby.activeSelf && Input.GetButtonDown(anyPlayer.bButton) && !inDialogue)
        {
            lobby.SetActive(false);
            playermode.SetActive(false);
            singleplayerLobby.SetActive(false);
            startMenu.SetActive(true);
            settings.SetActive(false);
        }
        else if (!hasStarted && !lobby.activeSelf && !settings.activeSelf && !singleplayerLobby.activeSelf && playermode.activeSelf && Input.GetButtonDown(anyPlayer.bButton) && !inDialogue)
        {
            lobby.SetActive(false);
            playermode.SetActive(false);
            singleplayerLobby.SetActive(false);
            startMenu.SetActive(true);
            settings.SetActive(false);
        }
        else if (!hasStarted && !lobby.activeSelf && settings.activeSelf) //Checks if the player/s are in the settings.
        {
            if (Input.GetAxis(anyPlayer.horizontal) > 0.5 && !inDialogue) //Checks if the player is moving the left thumbstick to the right.
            {
                volume.value += 0.8f; //Increases the volume.
            }
            else if (Input.GetAxis(anyPlayer.horizontal) < -0.5 && !inDialogue) //Checks if the player is moving the left thumbstick to the left.
            {
                volume.value -= 0.8f; //Decreases the volume.
            }
        }
    }

    void FixedUpdate()
    {
        if (UnityEngine.Random.Range(0, stormRisk) <= 0 && hasStarted)
        {
            if (UnityEngine.Random.Range(0, 2) <= 0)
            {
                Instantiate(stormPrefab, new Vector2(-maxBoundary, UnityEngine.Random.Range(-maxBoundary, maxBoundary)), Quaternion.identity); //Spawns a storm on the left side of the map.
            }
            else
            {
                Instantiate(stormPrefab, new Vector2(maxBoundary, UnityEngine.Random.Range(-maxBoundary, maxBoundary)), Quaternion.identity); //Spawns a storm on the right side of the map.
            }
        }

        if (UnityEngine.Random.Range(0, laserRisk) <= 0 && hasStarted)
        {
            if (UnityEngine.Random.Range(0, 2) <= 0)
            {
                Instantiate(laserPrefab, new Vector2(-maxBoundary, UnityEngine.Random.Range(-maxBoundary, maxBoundary)), Quaternion.identity);
            }
            else
            {
                Instantiate(laserPrefab, new Vector2(maxBoundary, UnityEngine.Random.Range(-maxBoundary, maxBoundary)), Quaternion.identity);
            }
        }
    }

    void LateUpdate()
    {
        if (hasStarted && !inDialogue) //Checks if there is an ongoing match/round.
        {
            lobby.SetActive(false); //Deactivates the lobby.
            playermode.SetActive(false); //Deactivates the playermode menu.
            background.SetActive(false); //Deactivates the main menu background.

            if (Input.GetButton(anyPlayer.xboxButton)) //Checks if anyone is pressing the xbox button.
            {
                EndMatch();
            }
        }
        else
        {
            background.SetActive(true); //Activates the background.
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < playerIDs.Count) //Checks if the value is within the amount of joined players.
            {
                int index = playerIDs[i]; //Stores the index of the ID (i.e. which controller it belongs to).
                playerIDText[index].text = "Player " + (i + 1); //Sets the text to indicate which player/id the controller is. (This is a value between 1-4).

                if (playerContainers[i].GetComponentInChildren<Stats>() != null)
                {
                    if (Vector2.Distance(playerContainers[i].GetComponentInChildren<Stats>().transform.position, transform.position) > maxBoundary) //Checks if the player is outside the max boundary of the game.
                    {
                        playerContainers[i].GetComponentInChildren<Stats>().TakeDamage(1); //Damages the player.
                    }
                }
            }

            if (hasJoined[i]) //Checks if the player has joined.
            {
                Color c = joinGame[i].color; //Stores the color of the image.
                c.a = 0; //Sets the alpha to 0%, thus making it completely transparent.
                joinGame[i].color = c; //Assings the color to the image.
            }
            else
            {
                Color c = joinGame[i].color; //Stores the color of the image.
                c.a = 1; //Sets the alpha to 100%, thus making it visible.
                joinGame[i].color = c; //Assigns the color to the image.
            }

            if (isReady[i]) //Checks if the player is ready.
            {
                Color c = ready[i].color; //Stores the color of the image.
                c.a = 1; //Sets the alpha to 100%, thus making it visible.
                ready[i].color = c; //Assigns the color to the image.
            }
            else
            {
                Color c = ready[i].color; //Stores the color of the image.
                c.a = 0; //Sets the alpha to 0%, thus making it completely transparent.
                ready[i].color = c; //Assigns the color to the image.
            }

            if (!hasJoined[i]) //Checks if the player hasn't joined.
            {
                shipSelection[i].gameObject.SetActive(false); //Deactivates the ship selection menu.
                shipSelection[i].transform.parent.gameObject.SetActive(false);
            }
            else if (hasJoined[i] == true && isReady[i] == false) //Checks if the player has joined and has not yet chosen a ship.
            {
                shipSelection[i].gameObject.SetActive(true); //Activates the ship selection menu.
                //shipSelection[i].transform.parent.gameObject.SetActive(true);
                shipSelection[i].controls = controls[i]; //Assigns the correct control scheme to the menu.
            }

            if (playerIDs.Contains(i) && hasStarted) //Checks if the value is within the amount of joined players and if there is an ongoing match/round.
            {
                playerInformation[playerIDs[playerIDs.FindIndex(x => x == i)]].SetActive(true); //Activates the information panels containing health, points, etc.
            }
            else
            {
                playerInformation[i].SetActive(false); //Deactivates the information panels containing health, points, etc.
            }
        }

        if (players.Count > 0) //Checks if there are any players alive.
        {
            FindObjectOfType<CameraZoomer>().CameraZooming(); //Calls the method for moving and zooming the camera.
        }
        else if (players.Count <= 0) //Checks if there aren't any players alive.
        {
            mainCamera.transform.position = new Vector2(0, 0); //Returns the camera to centre of the map.
            mainCamera.orthographicSize = 12f; //Returns the camera's zoom to default.
        }

        if (hasStarted && players.Count == 1) //Checks if there is an ongoing match/round and if there is one player alive.
        {
            if (players[0].GetComponent<Stats>().points >= 2 && !addedPoint) //Checks if the player has more or equal to 2 points and if the game has not already added a point.
            {
                StartCoroutine(Win()); //Starts the win cycle.

                players[0].gameObject.GetComponent<Stats>().points++; //Adds a point to the player.
                addedPoint = true; //Tells the game that a point has been added.
            }
            else if (players[0].GetComponent<Stats>().points < 2 && !addedPoint) //Checks if the player has less than 2 points and if the game has not already added a point.
            {
                StartCoroutine(AddPoint()); //Starts the 'add point cycle'/victory sequence.

                players[0].gameObject.GetComponent<Stats>().points++; //Adds a point to the player.
                addedPoint = true; //Tells the game that a point has been added.
            }
        }
        else if (hasStarted && players.Count == 0) //Checks if there is a match ongoing and if there are no players left.
        {
            StartCoroutine(AddPoint()); //Starts the victory sequence.
        }
    }

    /// <summary>
    /// Adds players when they press 'A' and does the correct adjustments.
    /// </summary>
    /// <param name="i">Iteration/Player ID to check</param>
    public void Join(int i)
    {
        if (Input.GetButtonDown(controls[i].aButton) && hasJoined[i] == false) //Checks if the player is pressing A and hasn't already joined.
        {
            hasJoined[i] = true; //Tells the game that the player has joined.
            playerIDs.Add(i); //Adds the player to list.
            //chosenShip[i] = null;

            shipSelection[i].transform.parent.gameObject.SetActive(true);
            shipSelection[i].gameObject.SetActive(true); //Activates the ship selection menu.
            shipSelection[i].controls = controls[i]; //Assigns the correct control scheme to the ship selection menu.
        }
        else if (Input.GetButtonDown(controls[i].aButton) && hasJoined[i] == true && isReady[i] == false) //Checks if the player is pressing A and has already joined but isn't ready.
        {
            isReady[i] = true; //Tells the game that the player is ready.
            shipSelection[i].gameObject.SetActive(false); //Deactivates the ship selection menu.
        }
    }

    /// <summary>
    /// Removes players when they press 'B' and does the correct adjustments.
    /// </summary>
    /// <param name="i">Iteration/Player ID to check.</param>
    public void Leave(int i)
    {
        if (Input.GetButtonDown(controls[i].bButton) && hasJoined[i] == true && isReady[i] == false && chosenShip[playerIDs.FindIndex(x => x == i)] == null) //Checks if the player is pressing B, has joined, is not ready and hasn't chosen a ship.
        {
            shipSelection[i].transform.parent.gameObject.SetActive(false); //Deactivates the ship selection menu.
            hasJoined[i] = false; //Tells the player has not joined.
            playerIDs.Remove(i); //Removes the controller/player from the ID list.
            shipSelection[i].transform.rotation = Quaternion.Euler(0, 0, shipSelection[i].wantedRotation); //Resets the ship selection wheel's rotation to the chosen rotation.
        }
        else if (Input.GetButtonDown(controls[i].bButton) && hasJoined[i] == true && isReady[i] == true) //Checks if the player is pressing B, has joined and is ready.
        {
            isReady[i] = false; //Tells the game that the player is not ready.
            chosenShip[playerIDs.FindIndex(x => x == i)] = null; //Resets the player's ship choise.
            shipSelection[i].transform.parent.gameObject.SetActive(true); //Activates the ship selection menu.
            shipSelection[i].gameObject.SetActive(true);
            shipSelection[i].transform.rotation = Quaternion.Euler(0, 0, shipSelection[i].wantedRotation); //Resets the ship selection wheel's rotation to the chosen rotation.
        }
    }

    /// <summary>
    /// Sets the controls of ships to the controls of the respective player.
    /// </summary>
    public void ShipSelectionControls()
    {
        for (int i = 0; i < playerIDs.Count; i++) //Goes through all the joined players.
        {
            shipSelection[i].controls = controls[playerIDs[i]]; //Assigns the control scheme of the controller to the ID.
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < playerIDs.Count && isReady[playerIDs[i]] == false) //Checks if the value is within the amount of players who has joined and if the player is not ready.
            {
                shipSelection[i].gameObject.SetActive(true); //Activates the ship selection menu.
            }
            else
            {
                shipSelection[i].gameObject.SetActive(false); //Deactivates the ship selection menu.
            }
        }
    }

    /// <summary>
    /// Starts the game and instantiates the ships.
    /// </summary>
    public void StartMultiGame()
    {
        countdownTimer.SetActive(false); //Deactivates the countdown timer.

        for (int i = 0; i < playerIDs.Count; i++) //Goes through the joined players.
        {
            int f = playerIDs[i]; //Stores the controller ID of the player.
            int n = shipSelection[f].x; //Stores the coordinate/index of the chosen ship in the ship selection menu.

            chosenShip[f] = shipVariants[n]; //Sets the chosen ship to the gameobject at the index in the ship variants.

            if (playerContainers[f].GetComponentInChildren<Stats>() == null) //Checks if the player doesn't have any stats.
            {
                GameObject ship = Instantiate(chosenShip[f], playerContainers[f].transform.position, Quaternion.identity); //Spawns a ship.

                playerContainers[f].SetActive(true);

                ship.transform.parent = playerContainers[f].transform; //Sets the parent of the ship to the respective player container.
                ship.layer = ship.transform.parent.gameObject.layer; //Sets the layer of the ship to the layer of the parent.

                for (int b = 0; b < ship.transform.childCount; b++) //Goes through the children of the ship.
                {
                    ship.transform.GetChild(b).gameObject.layer = ship.transform.parent.gameObject.layer; //Sets the layer of the childobject to the layer of the ship.
                }

                playerContainers[f].GetComponentInChildren<Rudder>().ship = ship.transform; //Sets the ship reference in the rudder to the ship.
                playerContainers[f].GetComponentInChildren<ShipControl>().rudder = playerContainers[f].GetComponentInChildren<Rudder>().transform.GetChild(0); //Sets the rudder reference in the ship control script to the rudder.
                playerContainers[f].GetComponentInChildren<Rudder>().transform.rotation = Quaternion.Euler(0, 0, playerContainers[f].transform.eulerAngles.z - 90); //Sets the z-rotation of the rudder to its rotation - 90.
                playerContainers[f].GetComponentInChildren<ShipControl>().transform.rotation = playerContainers[f].GetComponentInChildren<Rudder>().transform.rotation; //Sets the rotation of the ship to the rotation of the rudder.
            }

            if (!playerContainers[f].activeSelf && playerContainers[f].GetComponentInChildren<Stats>() != null) //Checks if the player container isn't active.
            {
                playerContainers[f].GetComponentInChildren<Stats>().health = playerContainers[f].GetComponentInChildren<Stats>().maxHealth; //Heals the player to max health.
            }

            //playerContainers[f].SetActive(true); //Activates the player container.
            playerContainers[f].GetComponent<Player>().controls = controls[f]; //Sets the controls to the respective controls.
            playerContainers[f].GetComponent<Player>().SetControls(); //Applies the changes.
            playerContainers[f].GetComponentInChildren<ShipControl>().velocity.x = 0; //Sets the velocity of the ship to 0.
            playerContainers[f].GetComponentInChildren<ShipControl>().velocity.y = 0; 
            playerContainers[f].GetComponentInChildren<ShipControl>().velocity.z = 0; 
            playerContainers[f].GetComponentInChildren<ShipControl>().gameObject.transform.position = playerContainers[f].transform.position;
            playerContainers[f].GetComponentInChildren<TrailRenderer>().Clear();

            if (playerContainers[f].GetComponentInChildren<Gun>().type != Gun.WeaponType.ram)
            {
                playerContainers[f].GetComponentInChildren<Sight>().gameObject.GetComponentInChildren<SpriteRenderer>().sprite = crosshair[f]; //Sets the sprite of the sight to the responding crosshair sprite for the controller.
            }
        }

        hasStarted = true; //Tells the game that there is an ongoing match/round.

        FindPlayers();
    }

    /// <summary>
    /// Finds all alive players.
    /// </summary>
    public void FindPlayers()
    {
        players.Clear();
        ShipControl[] _players = FindObjectsOfType<ShipControl>(); //Finds all the ships.
        foreach (ShipControl s in _players)
        {
            players.Add(s.gameObject); //Adds the ship to the list of alive players.
            //s.transform.parent.GetComponent<Player>().playerID = players.Count; //Sets the player ID of the player container to the current amount of players in the list.
        }
    }

    public void DamageModifier(int i)
    {
        if (startedTimer[i])
        {
            damageTimer[i] += Time.deltaTime;

            if (damageTimer[i] > 10)
            {
                startedTimer[i] = false;
                damageTimer[i] = 0;
                damageModifier[i] = 1;
            }
        }
    }

    public void EndMatch()
    {
        gainPointText.text = "";
        gainPointText.gameObject.SetActive(false);

        for (int i = 0; i < 4; i++) //Goes through all the players.
        {
            playerContainers[i].SetActive(false); //Deactivates the player container.
            playerContainers[i].GetComponent<Player>().controls = null; //Unassigns the controls for the player.
            playerContainers[i].GetComponent<Player>().SetControls(); //Finalizes the change.
            chosenShip[i] = null;
            if (playerContainers[i].transform.GetComponentInChildren<Stats>() != null)
            {
                playerContainers[i].transform.GetComponentInChildren<Stats>().points = 0; //Resets the player's points.
                Destroy(playerContainers[i].transform.GetComponentInChildren<Stats>().gameObject);
            }

            hasJoined[i] = false; //Tells the game that the player has not joined.
            isReady[i] = false; //Tells the game that the player is not ready.
        }

        players.Clear(); //Clears the list of alive players.
        playerIDs.Clear(); //Clears the list with the ID's for the controllers.

        hasStarted = false; //Tells the game that there is no ongoing match or round.

        lobby.SetActive(true); //Activates the lobby.

        addedPoint = false; //Resets the added point variable incase the player left during a victory sequence.

        Torpedo[] torpedoes = FindObjectsOfType<Torpedo>();

        foreach (Torpedo t in torpedoes) //Finds and destroys all torpedoes on the map.
        {
            Destroy(t.gameObject);
        }

        StopAllCoroutines(); //Stops all running coroutines.

        for (int i = 0; i < mapHazards.transform.childCount; i++)
        {
            mapHazards.transform.GetChild(i).gameObject.SetActive(true); //Reactivates all mines and other hazards on the map.
        }
    }

    /// <summary>
    /// Restarts lobby and outputs a "win" message.
    /// </summary>
    /// <returns></returns>
    IEnumerator Win()
    {
        gainPointText.text = players[0].transform.parent.name + " Won!";
        gainPointText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3);

        EndMatch();
    }

    /// <summary>
    /// Adds point to player and resets the map.
    /// </summary>
    /// <returns></returns>
    IEnumerator AddPoint()
    {
        foreach(GameObject g in players)
        {
            g.GetComponent<Stats>().isShielded = true;
        }

        if (players.Count == 1)
        {
            gainPointText.text = players[0].transform.parent.name + " Gains Point!";
            gainPointText.gameObject.SetActive(true); //Activates the victory slide/sprite.
        }
        else if (players.Count <= 0)
        {
            gainPointText.text = "Tie!";
            gainPointText.gameObject.SetActive(true); //Activates the victory slide/sprite.
        }

        yield return new WaitForSeconds(3);

        Torpedo[] torpedoes = FindObjectsOfType<Torpedo>(); //Finds all the torpedoes in the scene

        foreach (Torpedo t in torpedoes) //Goes through all the torpedoes.
        {
            Destroy(t.gameObject); //Destroys the torpedo
        }

        CarpetBomber[] carpetBombs = FindObjectsOfType<CarpetBomber>();

        foreach (CarpetBomber c in carpetBombs) //Finds and destroys all airplanes on the map.
        {
            Destroy(c.gameObject);
        }

        for (int i = 0; i < playerIDs.Count; i++) //Goes through the players.
        {
            playerContainers[playerIDs[i]].SetActive(true); //Activates the player container.
        }

        gainPointText.gameObject.SetActive(false); //Deactivates the victory slide/sprite.

        StartMultiGame();

        addedPoint = false; //Resets the added point bool.

        foreach (GameObject g in players) //Deactivates immortality for every player.
        {
            g.GetComponent<Stats>().isShielded = false; 
        }

        StopAllCoroutines();
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}