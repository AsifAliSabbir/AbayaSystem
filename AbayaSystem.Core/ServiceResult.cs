namespace AbayaSystem.Core
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static ServiceResult Success() => new ServiceResult { IsSuccess = true };

        public static ServiceResult Failure(string errorMessage) => new ServiceResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}