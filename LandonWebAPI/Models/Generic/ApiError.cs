using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LandonWebAPI.Models.Generic;

public class ApiError
{
    public ApiError()
    {

    }

    public ApiError(string message)
    {
        Message = message;
    }

    public ApiError(ModelStateDictionary modelState)
    {
        Message = "Invalid parameters.";
        Detail = modelState
            .FirstOrDefault(model => model.Value.Errors.Any()).Value.Errors
            .FirstOrDefault().ErrorMessage;
    }
    public string Message { get; set; }

    public string Detail { get; set; }
}