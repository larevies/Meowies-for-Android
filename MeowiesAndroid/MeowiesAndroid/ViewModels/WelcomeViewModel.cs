using System;

namespace MeowiesAndroid.ViewModels;

public class WelcomeViewModel : ProfileViewModelBase
{
    public override bool CanNavigateNext
    {
        get => true;
        protected set => throw new NotSupportedException();
    }
    public override bool CanNavigatePrevious => true;
}
