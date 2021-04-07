using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class FirebaseManager : MonoBehaviour
{
    //[SerializeField] QuestionManager questionManager;
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public static FirebaseUser User;
    public DatabaseReference DBreference;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;
    public TMP_Text confirmRegisterText;

    //User Data variables
    [Header("UserData")]
    public TMP_InputField usernameField;
    public TMP_InputField xpField;
    public TMP_InputField killsField;
    public TMP_InputField masteryField;
    public GameObject scoreElement;
    public Transform scoreboardContent;

    public static string username;

    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void ClearLoginFeilds()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }
    public void ClearRegisterFeilds()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }

    //Function for the login button
    public void LoginButton()
    {
        //questionManager.Awake();
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }
    //Function for the sign out button
    public void SignOutButton()
    {
        auth.SignOut();
        UIManager.instance.LoginScreen();
        ClearRegisterFeilds();
        ClearLoginFeilds();
    }

    public void EnterGameButton() {
        ClearRegisterFeilds();
        ClearLoginFeilds();
        SceneManager.LoadScene("Character Selection");
    }
    //Function for the save button
    public void SaveDataButton()
    {   StartCoroutine(UpdateStars(1,1,1,0));
        StartCoroutine(UpdateUsernameAuth(usernameField.text));
        StartCoroutine(UpdateUsernameDatabase(usernameField.text));
        StartCoroutine(UpdateKills(int.Parse(killsField.text)));
    }
    //Function for the scoreboard button
    // public void ScoreboardButton()
    // {        
    //     StartCoroutine(LoadScoreboardData());
    // }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            username = User.DisplayName;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
            // StartCoroutine(LoadUserData());

            yield return new WaitForSeconds(1);

            usernameField.text = User.DisplayName;
            //UIManager.instance.UserDataScreen(); // Change to user data UI
            confirmLoginText.text = "";
            initialisePlayerPrefStars();
            ClearLoginFeilds();
            ClearRegisterFeilds();
            // SceneManager.LoadScene("Character Selection");
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if(passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else 
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile{DisplayName = _username};

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);
                    // StartCoroutine(UpdateInnerStars(0));
                    InitialisePlayerProfile();
                    StartCoroutine(UpdateUsernameDatabase(_username));
                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        confirmRegisterText.text = "Account created successfully";
                        yield return new WaitForSeconds(1f);
                        confirmRegisterText.text = "";
                        UIManager.instance.LoginScreen();                        
                        warningRegisterText.text = "";
                        ClearRegisterFeilds();
                        ClearLoginFeilds();
                    }
                }
            }
        }
    }

    private IEnumerator UpdateStars(int starsworld, int starssection, int starslevel, int input)
    {   
        string starworld = starsworld.ToString();
        string starsection = starssection.ToString();
        string starlevel = starslevel.ToString();
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("stars").Child(starworld).Child(starsection).Child(starlevel).SetValueAsync(input);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Kills are now updated
        }
    }

    public void UniversalUpdateStars(int starsworld, int starssection, int starlevel, int input){
        StartCoroutine(UpdateStars(starsworld,starssection, starlevel, input));
    }

    private IEnumerator UpdateBattleStats(int starsworld,int section, int input)
    {   
        string starsection = section.ToString();
        string starworld = starsworld.ToString();
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("BattleStats").Child(starworld).Child(starsection).Child("Points").SetValueAsync(input);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Kills are now updated
        }
    }

    private void initialisePlayerPrefStars() 
    {
        StartCoroutine(loadPlayerPrefStars());
    }

    private IEnumerator loadPlayerPrefStars() 
    {
        Debug.Log("reached here at load before");
        Debug.Log($"{User.UserId}");
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("stars").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        Debug.Log("reached here at load");

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
            int index = 1;
            for (int world = 1; world < 4; world++) {
                for (int section = 1; section < 4; section++) {
                    int totalStarsSection = 0;
                    for (int level = 1; level < 5;  level++) {
                        if (world == 1) {
                            if (section == 1) {
                                int value = int.Parse(snapshot.Child($"{world}").Child($"{section}").Child($"{level}").Value.ToString());
                                Debug.Log($"{value}");
                                totalStarsSection += value;
                                PlayerPrefs.SetInt($"{level}", value);
                            }
                            else if (section == 2) {
                                int value = int.Parse(snapshot.Child($"{world}").Child($"{section}").Child($"{level+4}").Value.ToString());
                                Debug.Log($"{value}");
                                totalStarsSection += value;
                                PlayerPrefs.SetInt($"{level+4}", value);
                            }
                            else if (section == 3) {
                                int value = int.Parse(snapshot.Child($"{world}").Child($"{section}").Child($"{level+8}").Value.ToString());
                                Debug.Log($"{value}");
                                totalStarsSection += value;
                                PlayerPrefs.SetInt($"{level+8}", value);
                            }
                            
                        }
                        if (world == 2) {
                            if (section == 1) {
                                int value = int.Parse(snapshot.Child($"{world}").Child($"{section}").Child($"{level+12}").Value.ToString());
                                Debug.Log($"{value}");
                                totalStarsSection += value;
                                PlayerPrefs.SetInt($"{level+12}", value);
                                
                            }
                            else if (section == 2) {
                                int value = int.Parse(snapshot.Child($"{world}").Child($"{section}").Child($"{level+16}").Value.ToString());
                                Debug.Log($"{value}");
                                totalStarsSection += value;
                                PlayerPrefs.SetInt($"{level+16}", value);
                            }
                            else if (section == 3) {
                                int value = int.Parse(snapshot.Child($"{world}").Child($"{section}").Child($"{level+20}").Value.ToString());
                                Debug.Log($"{value}");
                                totalStarsSection += value;
                                PlayerPrefs.SetInt($"{level+20}", value);
                            }
                            //PlayerPrefs.SetInt($"{world}.{section}", totalStarsSection);
                        }
                        if (world == 3) {
                            if (section == 1) {
                                int value = int.Parse(snapshot.Child($"{world}").Child($"{section}").Child($"{level+24}").Value.ToString());
                                Debug.Log($"{value}");
                                totalStarsSection += value;
                                PlayerPrefs.SetInt($"{level+24}", value);
                            }
                            else if (section == 2) {
                                int value = int.Parse(snapshot.Child($"{world}").Child($"{section}").Child($"{level+28}").Value.ToString());
                                Debug.Log($"{value}");
                                totalStarsSection += value;
                                PlayerPrefs.SetInt($"{level+28}", value);
                            }
                            else if (section == 3) {
                                int value = int.Parse(snapshot.Child($"{world}").Child($"{section}").Child($"{level+32}").Value.ToString());
                                Debug.Log($"{value}");
                                totalStarsSection += value;
                                PlayerPrefs.SetInt($"{level+32}", value);
                            }
                            
                        }
                    } 
                    string ind = index.ToString();
                    PlayerPrefs.SetInt("Lv" + ind, totalStarsSection);
                    Debug.Log($"{totalStarsSection}");
                    Debug.Log("Lv" + ind);
                    index++;
                }
            }
        }
        SceneManager.LoadScene("Character Selection");
    }
    // for (int i = 0; i<snapshot.ChildrenCount; i++) {
    //             int value = int.Parse(snapshot.Child($"{i+1}").Value.ToString());
    //             Debug.Log($"{value}");
    //         }

    private void InitialisePlayerProfile()
    {   
        StartCoroutine(UpdateBattleStats(1,1,0));
        StartCoroutine(UpdateBattleStats(1,2,0));
        StartCoroutine(UpdateBattleStats(1,3,0));
        StartCoroutine(UpdateBattleStats(2,1,0));
        StartCoroutine(UpdateBattleStats(2,2,0));
        StartCoroutine(UpdateBattleStats(2,3,0));
        StartCoroutine(UpdateBattleStats(3,1,0));
        StartCoroutine(UpdateBattleStats(3,2,0));
        StartCoroutine(UpdateBattleStats(3,3,0));

        //register stars
        StartCoroutine(UpdateStars(1,1,1,0));
        StartCoroutine(UpdateStars(1,1,2,0));
        StartCoroutine(UpdateStars(1,1,3,0));
        StartCoroutine(UpdateStars(1,1,4,0));

        StartCoroutine(UpdateStars(1,2,5,0));
        StartCoroutine(UpdateStars(1,2,6,0));
        StartCoroutine(UpdateStars(1,2,7,0));
        StartCoroutine(UpdateStars(1,2,8,0));

        StartCoroutine(UpdateStars(1,3,9,0));
        StartCoroutine(UpdateStars(1,3,10,0));
        StartCoroutine(UpdateStars(1,3,11,0));
        StartCoroutine(UpdateStars(1,3,12,0));

        StartCoroutine(UpdateStars(2,1,13,0));
        StartCoroutine(UpdateStars(2,1,14,0));
        StartCoroutine(UpdateStars(2,1,15,0));
        StartCoroutine(UpdateStars(2,1,16,0));

        StartCoroutine(UpdateStars(2,2,17,0));
        StartCoroutine(UpdateStars(2,2,18,0));
        StartCoroutine(UpdateStars(2,2,19,0));
        StartCoroutine(UpdateStars(2,2,20,0));

        StartCoroutine(UpdateStars(2,3,21,0));
        StartCoroutine(UpdateStars(2,3,22,0));
        StartCoroutine(UpdateStars(2,3,23,0));
        StartCoroutine(UpdateStars(2,3,24,0));

        StartCoroutine(UpdateStars(3,1,25,0));
        StartCoroutine(UpdateStars(3,1,26,0));
        StartCoroutine(UpdateStars(3,1,27,0));
        StartCoroutine(UpdateStars(3,1,28,0));

        StartCoroutine(UpdateStars(3,2,29,0));
        StartCoroutine(UpdateStars(3,2,30,0));
        StartCoroutine(UpdateStars(3,2,31,0));
        StartCoroutine(UpdateStars(3,2,32,0));

        StartCoroutine(UpdateStars(3,3,33,0));
        StartCoroutine(UpdateStars(3,3,34,0));
        StartCoroutine(UpdateStars(3,3,35,0));
        StartCoroutine(UpdateStars(3,3,36,0));
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }        
    }

    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }
    
    public void UpdateStarsToDb() 
    {
        StartCoroutine(UpdateStars(1,1,1,PlayerPrefs.GetInt("1")));
        StartCoroutine(UpdateStars(1,1,2,PlayerPrefs.GetInt("2")));
        StartCoroutine(UpdateStars(1,1,3,PlayerPrefs.GetInt("3")));
        StartCoroutine(UpdateStars(1,1,4,PlayerPrefs.GetInt("4")));

        StartCoroutine(UpdateStars(1,2,5,PlayerPrefs.GetInt("5")));
        StartCoroutine(UpdateStars(1,2,6,PlayerPrefs.GetInt("6")));
        StartCoroutine(UpdateStars(1,2,7,PlayerPrefs.GetInt("7")));
        StartCoroutine(UpdateStars(1,2,8,PlayerPrefs.GetInt("8")));

        StartCoroutine(UpdateStars(1,3,9,PlayerPrefs.GetInt("9")));
        StartCoroutine(UpdateStars(1,3,10,PlayerPrefs.GetInt("10")));
        StartCoroutine(UpdateStars(1,3,11,PlayerPrefs.GetInt("11")));
        StartCoroutine(UpdateStars(1,3,12,PlayerPrefs.GetInt("12")));

        StartCoroutine(UpdateStars(2,1,13,PlayerPrefs.GetInt("13")));
        StartCoroutine(UpdateStars(2,1,14,PlayerPrefs.GetInt("14")));
        StartCoroutine(UpdateStars(2,1,15,PlayerPrefs.GetInt("15")));
        StartCoroutine(UpdateStars(2,1,16,PlayerPrefs.GetInt("16")));

        StartCoroutine(UpdateStars(2,2,17,PlayerPrefs.GetInt("17")));
        StartCoroutine(UpdateStars(2,2,18,PlayerPrefs.GetInt("18")));
        StartCoroutine(UpdateStars(2,2,19,PlayerPrefs.GetInt("19")));
        StartCoroutine(UpdateStars(2,2,20,PlayerPrefs.GetInt("20")));

        StartCoroutine(UpdateStars(2,3,21,PlayerPrefs.GetInt("21")));
        StartCoroutine(UpdateStars(2,3,22,PlayerPrefs.GetInt("22")));
        StartCoroutine(UpdateStars(2,3,23,PlayerPrefs.GetInt("23")));
        StartCoroutine(UpdateStars(2,3,24,PlayerPrefs.GetInt("24")));

        StartCoroutine(UpdateStars(3,1,25,PlayerPrefs.GetInt("25")));
        StartCoroutine(UpdateStars(3,1,26,PlayerPrefs.GetInt("26")));
        StartCoroutine(UpdateStars(3,1,27,PlayerPrefs.GetInt("27")));
        StartCoroutine(UpdateStars(3,1,28,PlayerPrefs.GetInt("28")));

        StartCoroutine(UpdateStars(3,2,29,PlayerPrefs.GetInt("29")));
        StartCoroutine(UpdateStars(3,2,30,PlayerPrefs.GetInt("30")));
        StartCoroutine(UpdateStars(3,2,31,PlayerPrefs.GetInt("31")));
        StartCoroutine(UpdateStars(3,2,32,PlayerPrefs.GetInt("32")));

        StartCoroutine(UpdateStars(3,3,33,PlayerPrefs.GetInt("33")));
        StartCoroutine(UpdateStars(3,3,34,PlayerPrefs.GetInt("34")));
        StartCoroutine(UpdateStars(3,3,35,PlayerPrefs.GetInt("35")));
        StartCoroutine(UpdateStars(3,3,36,PlayerPrefs.GetInt("36")));
    }


    private IEnumerator UpdateKills(int _kills)
    {
        //Set the currently logged in user kills
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("kills").SetValueAsync(_kills);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            // Kills are now updated
        }
    }


    // private IEnumerator LoadUserData()
    // {
    //     //Get the currently logged in user data
    //     var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

    //     yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //     if (DBTask.Exception != null)
    //     {
    //         Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //     }
    //     else if (DBTask.Result.Value == null)
    //     {
    //         //No data exists yet
    //         xpField.text = "0";
    //         killsField.text = "0";
    //         masteryField.text = "0";
    //     }
    //     else
    //     {
    //         //Data has been retrieved
    //         DataSnapshot snapshot = DBTask.Result;

    //         killsField.text = snapshot.Child("kills").Value.ToString();
    //     }
    // }

    // private IEnumerator LoadScoreboardData()
    // {
    //     //Get all the users data ordered by kills amount
    //     var DBTask = DBreference.Child("users").OrderByChild("mastery").GetValueAsync();

    //     yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //     if (DBTask.Exception != null)
    //     {
    //         Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //     }
    //     else
    //     {
    //         //Data has been retrieved
    //         DataSnapshot snapshot = DBTask.Result;

    //         //Destroy any existing scoreboard elements
    //         foreach (Transform child in scoreboardContent.transform)
    //         {
    //             Destroy(child.gameObject);
    //         }

    //         //Loop through every users UID
    //         foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
    //         {
    //             string username = childSnapshot.Child("username").Value.ToString();
    //             int kills = int.Parse(childSnapshot.Child("kills").Value.ToString());
    //             int mastery = int.Parse(childSnapshot.Child("mastery").Value.ToString());
    //             int xp = int.Parse(childSnapshot.Child("xp").Value.ToString());

    //             //Instantiate new scoreboard elements
    //             GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
    //             scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, kills, mastery, xp);
    //         }

    //         //Go to scoareboard screen
    //         UIManager.instance.ScoreboardScreen();
    //     }
    // }
}