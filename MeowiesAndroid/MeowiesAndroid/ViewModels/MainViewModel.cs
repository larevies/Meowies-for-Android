﻿using System.Windows.Input;
using ReactiveUI;

namespace MeowiesAndroid.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        CurrentPage = _pages[0];
        
        CatCommand = ReactiveCommand.Create(Cat);
        SearchCommand = ReactiveCommand.Create(Search);
        BookmarksCommand = ReactiveCommand.Create(Bookmarks);
    }

    private readonly ViewModelBase[] _pages = 
    { 
        new ProfileViewModel(),
        new SearchViewModel(),
        new BookmarksViewModel()
    };
    private ViewModelBase _currentPage = null!;
    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }
    
    public ICommand CatCommand { get; }
    private void Cat() { CurrentPage = _pages[0]; }
    
    public ICommand SearchCommand { get; }
    private void Search() { CurrentPage = _pages[1]; }
    
    public ICommand BookmarksCommand { get; }
    private void Bookmarks() { CurrentPage = _pages[2]; }

}
