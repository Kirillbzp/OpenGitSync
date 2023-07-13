namespace OpenGitSync.Shared.ViewModels
{
    public class LoginResult
    {
        public bool Succeeded { get; }
        public string Token { get; }
        public string ErrorMessage { get; }

        private LoginResult(bool succeeded, string token, string errorMessage)
        {
            Succeeded = succeeded;
            Token = token;
            ErrorMessage = errorMessage;
        }

        public static LoginResult Success(string token)
        {
            return new LoginResult(true, token, null);
        }

        public static LoginResult Failed(string errorMessage)
        {
            return new LoginResult(false, null, errorMessage);
        }
    }

}
