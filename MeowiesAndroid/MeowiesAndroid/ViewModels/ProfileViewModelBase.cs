using System;
using Avalonia.Controls;
using MeowiesAndroid.Models;

namespace MeowiesAndroid.ViewModels;

public abstract class ProfileViewModelBase : ViewModelBase
{
    public abstract bool CanNavigateNext { get; protected set; }
    public abstract bool CanNavigatePrevious { get; }
}