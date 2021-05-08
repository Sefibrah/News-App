namespace api.Errors {
    public class ApiException {
        public ApiException(int statusCode, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Details = details;
            Message = message;
        }

        public int StatusCode { get; set; }
        public string Details { get; set; }
        public string Message { get; set; }
    }
}