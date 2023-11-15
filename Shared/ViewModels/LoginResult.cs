namespace OpenGitSync.Shared.ViewModels
{
    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }

        private LoginResult(bool succeeded, string token, string errorMessage)
        {
            Succeeded = succeeded;
            Token = token;
            ErrorMessage = errorMessage;
        }

        public LoginResult()
        {

        }
    
        public static LoginResult Success(string token)
        {
            return new LoginResult(true, token, "");
        }

        public static LoginResult Failed(string errorMessage)
        {
            return new LoginResult(false, "", errorMessage);
        }
    }

}
