using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    [Header("UI References")]

    // input fields
    [Header("Input Fields")]
    public InputField emailInputField_Login;
    public InputField passwordInputField_Login;
    public InputField emailInputField_SignUp;
    public InputField passwordInputField_SignUp;
    public InputField passwordDoubleCheckField;

    // alert text
    [Header("Alert Text")]
    public Text emailInputAlertText_Login;
    public Text passwordInputAlertText_Login;
    public Text emailInputAlertText_SignUp;
    public Text passwordInputAlertText_SignUp;

    // buttons
    [Header("Buttons")]
    public Button loginButton;
    public Button createNewAccountButton;
    public Button signupButton;
    public Button backToLoginButton;
    public Button signoutButton;

    // login and signup pages
    [Header("Pages")]
    public GameObject loginPage;
    public GameObject signUpPage;

    private FirebaseAuth auth;
    private bool isEmailValid = true;

    private void Start()
    {
        // Initialize Firebase Auth
        auth = FirebaseAuth.DefaultInstance;

        // Add event listeners to buttons
        loginButton.onClick.AddListener(Login);
        createNewAccountButton.onClick.AddListener(GotoSignUpPage);
        signupButton.onClick.AddListener(Signup);
        backToLoginButton.onClick.AddListener(GotoLoginPage);
        signoutButton.onClick.AddListener(SignOut);
    }

    private void Update()
    {
        // Check email validation each frame
        ValidateEmail();
    }

    private void GotoSignUpPage()
    {
        loginPage.SetActive(false);
        signUpPage.SetActive(true);
    }

    private void GotoLoginPage()
    {
        signUpPage.SetActive(false);
        loginPage.SetActive(true);
    }

    private void ValidateEmail()
    {
        InputField activeInputField;
        Text activeAlertText;

        // Check the active page and assign the corresponding input field and alert text
        if (loginPage.activeSelf)
        {
            activeInputField = emailInputField_Login;
            activeAlertText = emailInputAlertText_Login;
        }
        else if (signUpPage.activeSelf)
        {
            activeInputField = emailInputField_SignUp;
            activeAlertText = emailInputAlertText_SignUp;
        }
        else
        {
            return; // No active page, no need for validation
        }

        string email = activeInputField.text;

        // Check if email input is empty
        if (string.IsNullOrEmpty(email))
        {
            // Reset the input field's image color to its default
            Color initColor = Color.white;
            initColor.a = activeInputField.image.color.a; // Preserve input field's transparency
            activeInputField.image.color = initColor;
            activeAlertText.text = string.Empty;
            return;
        }

        bool isValid = ValidateEmailFormat(email);

        // Provide feedback to the user (you can modify this based on your UI requirements)
        Color color = isValid ? Color.white : Color.red;
        color.a = activeInputField.image.color.a; // Preserve input field's transparency
        activeInputField.image.color = color;

        activeAlertText.text = isValid ? string.Empty : "Invalid email format";
    }

    private bool ValidateEmailFormat(string email)
    {
        if (!email.Contains("@") || !email.Contains("."))
            return false;

        int atIndex = email.IndexOf("@");
        int dotIndex = email.LastIndexOf(".");

        if (atIndex >= dotIndex)
            return false;

        string username = email.Substring(0, atIndex);
        string domain = email.Substring(atIndex + 1, dotIndex - atIndex - 1);
        string extension = email.Substring(dotIndex + 1);

        return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(domain) && !string.IsNullOrEmpty(extension);
    }

    private void Login()
    {
        string email = emailInputField_Login.text;
        string password = passwordInputField_Login.text;

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
        string email = emailInputField_SignUp.text;
        string password = passwordInputField_SignUp.text;

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