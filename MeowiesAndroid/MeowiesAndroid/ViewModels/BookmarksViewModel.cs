using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MeowiesAndroid.Models;
using ReactiveUI;

namespace MeowiesAndroid.ViewModels;


public class BookmarksViewModel : ViewModelBase
{
    public static ObservableCollection<MovieItemDoc> Bookmarks { get; set; }

    private string _refresh = "";
    public string Refresh
    {
        get => _refresh;
        set
        {
            _refresh = value;
            OnPropertyChanged(nameof(Refresh));
        }
    }
    
    public async void Delete(MovieItemDoc a)
    {
        int intId = Convert.ToInt32(SignInViewModel.CurrentUser.Id.ToString());
        var newBookmark = new Bookmark()
        {
            UserId = intId,
            MovieId = a.id
        };
        var idString = await MeowiesApiRequests.FindBookmark(newBookmark);
        var id = Convert.ToInt32(idString);
        await MeowiesApiRequests.RemoveFromBookmarks(id);
        
        var itemToDelete = Bookmarks.Single
            (x => x.id == a.id);
        a.IsButtonVisible = false;
        
        ObservableCollection<MovieItemDoc> newBookmarks = Bookmarks;
        newBookmarks.Remove(itemToDelete);
        
        Bookmarks = newBookmarks;
        
    }
}