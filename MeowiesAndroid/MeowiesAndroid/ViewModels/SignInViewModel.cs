using System.ComponentModel.DataAnnotations;
using MeowiesAndroid.Models;
using ReactiveUI;

namespace MeowiesAndroid.ViewModels;

public class SignInViewModel : ProfileViewModelBase
{
    /*public SignInViewModel()
    {
        using var context = new MeowiesContext();
        this.WhenAnyValue(x => x.MailAddress, x => x.Password)
            .Subscribe(_ => UpdateCanNavigateNext());
    }*/

    public static string Message { get; set; } = "";
    [Required] [EmailAddress] public static string MailAddress { get; set; } = null!;
    [Required] public static string Password { get; set; } = null!;

    private void UpdateCanNavigateNext()
    {
        CanNavigateNext = 
            !string.IsNullOrEmpty(MailAddress) 
            && MailAddress.Contains("@")
            && !string.IsNullOrEmpty(Password);
    }

    private bool _canNavigateNext = true;
    public override bool CanNavigateNext
    {
        get => _canNavigateNext;
        protected set
        {
            _canNavigateNext = value;
            OnPropertyChanged(nameof(CanNavigateNext));
        }
    }

    public override bool CanNavigatePrevious => true;
    public static User CurrentUser { get; set; } = null!;
}
