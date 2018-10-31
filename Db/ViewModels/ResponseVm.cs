namespace Db.ViewModels
{
    public class ResponseVm
    {
        public string UserId;

        public int Status { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        public string Message { get; set; }
        public string AccessToken { get; set; }
        public string ProviderName { get; set; }
        public string Rule { get; set; }

        public static ResponseVm SuccessfulLogin(string token)
        {
            return new ResponseVm
            {
                Status = 1,
                Message = "Successful Login..",
                AccessToken = token,
                ProviderName = token
            };
        }

        public static ResponseVm FailearLogin(string errorMessage = "")
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                return new ResponseVm
                {
                    Status = 0,
                    Message = "Operation Failed!!"
                };
            }

            return new ResponseVm
            {
                Status = 0,
                Message = errorMessage
            };
        }
    }
}