using System;
using System.ComponentModel;
using MeowiesAndroid.Models;

namespace MeowiesAndroid.ViewModels;

public class TrendingViewModel : ViewModelBase
{
    public static MovieItemDoc Bookmark { get; set; } = null!;
}