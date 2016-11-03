using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPBackgroundDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string TaskName = "MyTask";
        private const string Key = "MH";
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var access = await BackgroundExecutionManager.RequestAccessAsync();

            
            if (access != BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity &&
                access != BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity) return;

            if (BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == TaskName))
            {
                Debug.WriteLine("Cannot run");
                return;
            }


            var taskBuilder = new BackgroundTaskBuilder
            {
                Name = TaskName,
                TaskEntryPoint = typeof(BackgroundStuff.MyBackgroundTask).ToString()
            };

            var trigger = new ApplicationTrigger();
            taskBuilder.SetTrigger(trigger);

            var condition = new SystemCondition(SystemConditionType.InternetAvailable);
            var registerTask = taskBuilder.Register();
            registerTask.Completed += RegisterTask_Completed;

            await trigger.RequestAsync();
        }

        private void RegisterTask_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            object taskStatus;
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.TryGetValue(Key, out taskStatus))
            {
                Debug.WriteLine(taskStatus);
                
            }

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == TaskName)
                {
                    task.Value.Unregister(true);
                }
            }

        }
    }
}