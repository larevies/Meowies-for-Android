using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using DynamicData;
using MeowiesAndroid.Models;
using ReactiveUI;

namespace MeowiesAndroid.ViewModels;

public class ProfileViewModel : ViewModelBase
{
    public ProfileViewModel()
    {
        CurrentProfile = Welcome;
        
        NavigateNextCommand = ReactiveCommand.Create(NavigateNext);
        NavigatePreviousCommand = ReactiveCommand.Create(NavigatePrevious);
    }
    
    private string _next = "Sign up";
    public string Next
    {
        get => _next;
        set
        {
            _next = value;
            OnPropertyChanged(nameof(Next));
        } 
    }
    
    private string _previous = "Sign in";
    public string Previous
    {
        get => _previous;
        set
        {
            _previous = value;
            OnPropertyChanged(nameof(Previous));
        } 
    }

    private bool _areButtonsVisible = true;

    public bool AreButtonsVisible
    {
        get => _areButtonsVisible;
        set
        {
            _areButtonsVisible = value;
            OnPropertyChanged(nameof(AreButtonsVisible));
        }
    }
    
    #region Profile ViewModels

    private static readonly ProfileViewModelBase Welcome = new WelcomeViewModel();
    private static readonly ProfileViewModelBase SignUp = new SignUpViewModel();
    private static readonly ProfileViewModelBase SignIn = new SignInViewModel();
    private static readonly ChangeProfileViewModel ChangeProfile = new();

    private readonly ProfileViewModelBase[] _profilePages =
    {
        Welcome,
        SignUp,
        SignIn,
        ChangeProfile
    };

    #endregion

    private ProfileViewModelBase _currentProfile = null!;
    public ProfileViewModelBase CurrentProfile
    {
        get => _currentProfile;
        set
        {
            _currentProfile = value;
            OnPropertyChanged(nameof(CurrentProfile));
        }
    }
    
    
    public ICommand NavigateNextCommand { get; }

    private async void NavigateNext()
    {
        var index = _profilePages.IndexOf(CurrentProfile) + 1;
        CurrentProfile = _profilePages[index];

        if (CurrentProfile == SignIn)
        {
            Next = "Sign in";
            Previous = "Go back";
            try
            {
                Regex rgx = new Regex(@"^[-\w.]+@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,4}$");
                if (!rgx.IsMatch(SignUpViewModel.NewUser.Email))
                {
                    throw new InvalidExpressionException("Invalid email address");
                }
                await MeowiesApiRequests.PostUserToDb(SignUpViewModel.NewUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                SignUpViewModel.Message = "This email is taken or isn't valid.";
                CurrentProfile = SignUp;
            }
        }
        else if (CurrentProfile == Welcome)
        {
            Next = "Sign up";
            Previous = "Sign in";
        }
        else if (CurrentProfile == SignUp)
        {
            Next = "Sign up";
            Previous = "Go Back";
        }
        else if (CurrentProfile == ChangeProfile)
        {
            try
            {
                var task = MeowiesApiRequests.Authorize(SignInViewModel.MailAddress, SignInViewModel.Password);
                var user = await task!;
                ChangeProfile.CurrentUser = user!;
                SignInViewModel.CurrentUser = user!;
                ChangeProfile.StartSwitchPicture(user!.ProfilePicture);
                
                var intId = Convert.ToInt32(user.Id.ToString());
                var taskB = MeowiesApiRequests.GetBookmarksForUser(intId);
                var bookmarks = await taskB!;
                
                List<MovieItemDoc> userBookmarks = new(bookmarks!.Count);
                
                foreach (var taskM in bookmarks.Select(bookmark => JsonDeserializers.GetBmAsync(
                             Getters.GetMovieUrlById(
                                 bookmark.MovieId.ToString()))))
                {
                    var movie = await taskM!;
                    var doc = movie!.docs[0];
                    userBookmarks.Add(doc);
                }
                
                BookmarksViewModel.Bookmarks = new ObservableCollection<MovieItemDoc>(userBookmarks);
                AreButtonsVisible = false;
                CurrentProfile = ChangeProfile;
            }
            catch (Exception e)
            {
                AreButtonsVisible = true;
                Console.WriteLine(e.Message);
                SignInViewModel.Message = "E-mail address or password do not match. Try again";
                CurrentProfile = SignIn;
            }
        }
    }

    public ICommand NavigatePreviousCommand { get; }
    private void NavigatePrevious()
    {
        int index;
        if (CurrentProfile == Welcome)
        {
            index = _profilePages.IndexOf(CurrentProfile) + 2;
        }
        else
        {
            index = 0;
        }

        CurrentProfile = _profilePages[index];
        if (CurrentProfile == SignIn)
        {
            Next = "Sign in";
            Previous = "Go back";
        }
        else if (CurrentProfile == Welcome)
        {
            Next = "Sign up";
            Previous = "Sign in";
        }
        else if (CurrentProfile == SignUp)
        {
            Next = "Sign up";
            Previous = "Go Back";
        }
    }
}