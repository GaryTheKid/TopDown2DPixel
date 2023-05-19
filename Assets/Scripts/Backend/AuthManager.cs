using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    [Header("UI References")]
    public InputField emailInputField;
    public InputField passwordInputField;
    public Button loginButton;
    public Button signupButton;
    public Button signoutButton;

    private FirebaseAuth auth;

    private void Start()
    {
        // Initialize Firebase Auth
        auth = FirebaseAuth.DefaultInstance;

        // Add event listeners to buttons
        loginButton.onClick.AddListener(Login);
        signupButton.onClick.AddListener(Signup);
        signoutButton.onClick.AddListener(SignOut);
    }

    private void Login()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Failed to sign in with email and password: " + task.Exception);
                return;
            }

            Debug.Log("User signed in successfully!");
        });
    }

    private void Signup()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Failed to create user: " + task.Exception);
                return;
            }

            Debug.Log("User created and signed in successfully!");
        });
    }

    private void SignOut()
    {
        auth.SignOut();
        Debug.Log("User signed out.");
    }
}