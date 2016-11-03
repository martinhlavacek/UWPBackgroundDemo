using System;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace BackgroundStuff
{
    public sealed class MyBackgroundTask : IBackgroundTask
    {

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["MH"] = Guid.NewGuid();
            deferral.Complete();
        }
    }
}