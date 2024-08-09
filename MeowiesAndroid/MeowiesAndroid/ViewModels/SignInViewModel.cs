using System.ComponentModel.DataAnnotations;
using MeowiesAndroid.Models;

namespace MeowiesAndroid.ViewModels;

public class SignInViewModel : ProfileViewModelBase
{
    public static string Message { get; set; } = "";
    [Required] [EmailAddress] public static string MailAddress { get; set; } = null!;
    [Required] public static string Password { get; set; } = null!;

    public static User CurrentUser { get; set; } = null!;
}
