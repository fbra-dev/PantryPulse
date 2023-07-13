namespace Backend.WebApi.Domain.Models;

public record PushNotificationAction : AutomatedAction
{
    public string CustomText { get; set; }
}