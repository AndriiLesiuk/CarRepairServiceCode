namespace CarRepairServiceCode.Helper
{
    public struct Messages
    {
        public const string SuccessfullyAdded = "{0} with {1} successfully added.";
        public const string NotFoundInTheSystem = "{0} with {1} not found in the system.";
        public const string AuthorizationUnsuccessful = "Authorization failed. The user credentials for {0} are invalid or user does not exist.";
        public const string RemoveOrUpdateServicePosition = "You cannot delete or update service positions. The \"{0}\" with id = {1} is a service position.";
        public const string SwaggerUnauthorizedMessage = "Unauthorized. Sign in first!";
        public const string SwaggerAccessDenied = "Access denied!";
        public const string PasswordCustomValidatorMessage =
            "Please provide valid password: Should have at least one lower case; Should have at least one number; Minimum 6 characters.";
        public const string TokenSuccessfullyGenerated = "Token successfully generated for Username: {0} {1}, with Id: {2}.";
        public const string TokenSuccessfullyRefreshed = "Token successfully refreshed for Username: {0} {1}, with Id: {2}.";
        public const string NotEnoughPermissions = "You cannot {0} {1}, you do not have enough permissions.";
        public const string DoNotHaveAccessRightsConfigured = "You do not have access rights configured, contact your administrator to resolve this issue.";
    }
}
