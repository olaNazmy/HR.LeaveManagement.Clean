using FluentValidation.Results;

namespace HR.LeaveManagement.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {

        }
        // another ctor to send the result of validation
        public BadRequestException(string message, ValidationResult validationResult) : base(message)
        {
            ValidationErrors = validationResult.ToDictionary();
            
        }

        //define list of validation errors
        public IDictionary<string, string[]> ValidationErrors { get; set; }
    }


}
