using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Text.RegularExpressions;

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
    public InputField usernameInputField_SignUp;

    // alert text
    [Header("Alert Text")]
    public Text emailInputAlertText_Login;
    public Text passwordInputAlertText_Login;
    public Text emailInputAlertText_SignUp;
    public Text passwordInputAlertText_SignUp;
    public Text passwordDoubleCheckAlertText_SignUp;
    public Text usernameAlertText_SignUp;

    // notification text
    [Header("Notification Text")]
    public Text notificationText_Login;
    public Text notificationText_SignUp;

    // buttons
    [Header("Buttons")]
    public Button loginButton;
    public Button createNewAccountButton;
    public Button signupButton;
    public Button backToLoginButton;
    public Button signoutButton;
    public Button resetPassword;

    // login and signup pages
    [Header("Pages")]
    public GameObject loginPage;
    public GameObject signUpPage;

    private FirebaseAuth auth;
    private FirebaseUser user;
    private bool emailValidationPass;
    private bool userNameValidationPass;
    private bool passwordValidationPass;
    private bool passwordDoubleCheckValidationPass;
    private IEnumerator Login_Co;
    private IEnumerator SignUp_Co;

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
        resetPassword.onClick.AddListener(ResetPassword);
    }

    private void Update()
    {
        // Check email validation each frame
        ValidateEmail();
        ValidateUserName();
        ValidateSignUpPassword();
        ValidatePasswordDoubleCheck();
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

        // sign up pass
        emailValidationPass = isValid;
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

    private void ValidateUserName()
    {
        InputField activeInputField;
        Text activeAlertText;

        // Check the active page and assign the corresponding input field and alert text
        if (signUpPage.activeSelf)
        {
            activeInputField = usernameInputField_SignUp;
            activeAlertText = usernameAlertText_SignUp;
        }
        else
        {
            return; // No active page, no need for validation
        }

        string userName = activeInputField.text;

        // Check if email input is empty
        if (string.IsNullOrEmpty(userName))
        {
            // Reset the input field's image color to its default
            Color initColor = Color.white;
            initColor.a = activeInputField.image.color.a; // Preserve input field's transparency
            activeInputField.image.color = initColor;
            activeAlertText.text = string.Empty;
            return;
        }

        bool isValid = ValidateUserNameFormat(userName);

        // Provide feedback to the user (you can modify this based on your UI requirements)
        Color color = isValid ? Color.white : Color.red;
        color.a = activeInputField.image.color.a; // Preserve input field's transparency
        activeInputField.image.color = color;

        activeAlertText.text = isValid ? string.Empty : "Invalid user name format";

        // sign up pass
        userNameValidationPass = isValid;
    }

    
    private bool ValidateUserNameFormat(string userName)
    {
        if (4 > userName.Length && userName.Length > 12)
            return false;




        //TODO: username format







        return true;
    }

    private void ValidateSignUpPassword()
    {
        InputField activeInputField;
        Text activeAlertText;

        // Check the active page and assign the corresponding input field and alert text
        if (signUpPage.activeSelf)
        {
            activeInputField = passwordInputField_SignUp;
            activeAlertText = passwordInputAlertText_SignUp;
        }
        else
        {
            return; // No active page, no need for validation
        }
        string pw = activeInputField.text;

        if (string.IsNullOrEmpty(pw))
        {
            // Reset the input field's image color to its default
            Color initColor = Color.white;
            initColor.a = activeInputField.image.color.a; // Preserve input field's transparency
            activeInputField.image.color = initColor;
            activeAlertText.text = string.Empty;
            return;
        }

        bool isValid;
        string alert;
        (isValid, alert) = ValidatePassWordFormat(pw);

        // Provide feedback to the user (you can modify this based on your UI requirements)
        Color color = isValid ? Color.white : Color.red;
        color.a = activeInputField.image.color.a; // Preserve input field's transparency
        activeInputField.image.color = color;

        activeAlertText.text = isValid ? string.Empty : alert;

        // sign up pass
        passwordValidationPass = isValid;
    }

    private (bool, string) ValidatePassWordFormat(string pw)
    {
        string text;

        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMiniMaxChars = new Regex(@".{8,15}");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        if (!hasLowerChar.IsMatch(pw))
        {
            text = "Password should contain at least one lower case letter.";
            return (false, text);
        }
        else if (!hasUpperChar.IsMatch(pw))
        {
            text = "Password should contain at least one upper case letter.";
            return (false, text);
        }
        else if (!hasMiniMaxChars.IsMatch(pw))
        {
            text = "Password should not be lesser than 8 or greater than 15 characters.";
            return (false, text);
        }
        else if (!hasNumber.IsMatch(pw))
        {
            text = "Password should contain at least one numeric value.";
            return (false, text);
        }

        else if (!hasSymbols.IsMatch(pw))
        {
            text = "Password should contain at least one special case character.";
            return (false, text);
        }
        else
        {
            text = string.Empty;
            return (true, text);
        }
    }

    private void ValidatePasswordDoubleCheck()
    {
        InputField activeInputField;
        InputField activeInputField2;
        Text activeAlertText;

        // Check the active page and assign the corresponding input field and alert text
        if (signUpPage.activeSelf)
        {
            activeInputField = passwordInputField_SignUp;
            activeInputField2 = passwordDoubleCheckField;
            activeAlertText = passwordDoubleCheckAlertText_SignUp;
        }
        else
        {
            return; // No active page, no need for validation
        }
        string pw = activeInputField.text;
        string pw2 = activeInputField2.text;

        if (string.IsNullOrEmpty(pw) || string.IsNullOrEmpty(pw2))
        {
            // Reset the input field's image color to its default
            Color initColor = Color.white;
            initColor.a = activeInputField2.image.color.a; // Preserve input field's transparency
            activeInputField2.image.color = initColor;
            activeAlertText.text = string.Empty;
            return;
        }

        bool isValid = pw.Equals(pw2);

        // Provide feedback to the user (you can modify this based on your UI requirements)
        Color color = isValid ? Color.white : Color.red;
        color.a = activeInputField2.image.color.a; // Preserve input field's transparency
        activeInputField2.image.color = color;

        activeAlertText.text = pw.Equals(pw2) ? string.Empty : "Password not match!";

        // sign up pass
        passwordDoubleCheckValidationPass = isValid;
    }

    private void Login()
    {
        if (Login_Co == null)
        {
            Login_Co = LoggingIn();
            StartCoroutine(Login_Co);
        }
    }

    private IEnumerator LoggingIn()
    {
        string email = emailInputField_Login.text;
        string password = passwordInputField_Login.text;

        string outText = string.Empty;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(taskResult =>
        {
            if (taskResult.IsCanceled || taskResult.IsFaulted)
            {
                FirebaseException firebaseException = taskResult.Exception.GetBaseException() as FirebaseException;
                outText = "Failed to sign in with email and password: " + firebaseException;
                Debug.LogError(outText);
                return;
            }

            outText = "Login successfully.";
            Debug.Log(outText);
        });

        notificationText_Login.text = "Logging in...";

        yield return new WaitUntil(() => loginTask.IsCompleted);

        notificationText_Login.text = outText;
        
        Login_Co = null;

        SceneManager.LoadScene(1);
    }

    private void Signup()
    {
        if (SignUp_Co == null)
        {
            SignUp_Co = Register(emailInputField_SignUp.text, usernameInputField_SignUp.text,
                passwordInputField_SignUp.text, passwordDoubleCheckField.text);
            StartCoroutine(SignUp_Co);
        }
    }

    private IEnumerator Register(string email, string userName, string password, string confirmPassword)
    {

        if (!emailValidationPass)
        {
            notificationText_SignUp.text = "Email field is empty";
            Debug.LogError("Email field is empty");
        }
        else if (!userNameValidationPass)
        {
            notificationText_SignUp.text = "User name format invalid";
            Debug.LogError("User name format invalid");
        }
        else if (!passwordValidationPass)
        {
            notificationText_SignUp.text = "Password format invalid";
            Debug.LogError("Password format invalid");
        }
        else if (! passwordDoubleCheckValidationPass)
        {
            notificationText_SignUp.text = "Password does not match";
            Debug.LogError("Password does not match");
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            notificationText_SignUp.text = "Creating account...";

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Becuase ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Registration Failed";
                        break;
                }

                notificationText_SignUp.text = failedMessage;
                Debug.Log(failedMessage);
            }
            else
            {
                // Get The User After Registration Success
                user = registerTask.Result.User;

                UserProfile userProfile = new UserProfile { DisplayName = userName };
                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;


                    string failedMessage = "Profile update Failed! Becuase ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password is missing";
                            break;
                        default:
                            failedMessage = "Profile update Failed";
                            break;
                    }

                    notificationText_SignUp.text = failedMessage;
                    Debug.Log(failedMessage);
                }
                else
                {
                    notificationText_SignUp.text = "Registration Sucessful Welcome " + user.DisplayName;
                    Debug.Log("Registration Sucessful Welcome " + user.DisplayName);
                }
            }
        }

        SignUp_Co = null;

        SceneManager.LoadScene(1);
    }

    private void SignOut()
    {
        auth.SignOut();
        Debug.Log("User signed out.");

        Application.Quit();
    }

    private void ResetPassword()
    {
        InputField activeInputField;

        // Check the active page and assign the corresponding input field and alert text
        if (loginPage.activeSelf)
        {
            activeInputField = emailInputField_Login;
        }
        else if (signUpPage.activeSelf)
        {
            activeInputField = emailInputField_SignUp;
        }
        else
        {
            return; // No active page, no need for validation
        }

        // validate email
        string email = activeInputField.text;
        if (ValidateEmailFormat(email))
        {
            auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Password reset email sent successfully!");
                // Provide appropriate feedback to the user, such as displaying a success message or navigating to a confirmation screen.
            });
        }
        else
        {
            Debug.Log("Wrong email format.");
        }

        
    }
}