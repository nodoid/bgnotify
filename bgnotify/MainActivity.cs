using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Threading.Tasks;
using Android.Content;

using NotificationCompat = Android.Support.V4.App.NotificationCompat;

namespace bgnotify
{
    [Activity(Label = "notifications", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        // static button reference
        static readonly int ButtonClickNotificationId = 1000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            var button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += Start_ButtonTimer; // we could do this as an anon delegate...
        }

        void Start_ButtonTimer(object s, EventArgs e)
        {
            // let's create a background thread
            var timer = 0;
            new TaskFactory().StartNew(() =>
            {
                var t = new System.Timers.Timer
                {
                    Interval = 5 * 1000 // 5 seconds more or less
                };
                t.Elapsed += (sender, er) =>
                {
                    var builder = new NotificationCompat.Builder(this)
                    .SetAutoCancel(true)
                    .SetContentTitle("5 seconds")      // Set the title
                    .SetNumber(timer)                       // Display the count in the Content Info
                    .SetSmallIcon(Android.Resource.Drawable.IcButtonSpeakNow) // This is the icon to display
                    .SetContentText(string.Format("The timer is now at {0} seconds", timer / 1000)); // the message to display.

                    // Finally, publish the notification:
                    var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
                    notificationManager.Notify(ButtonClickNotificationId, builder.Build());
                    timer += 5000;
                };
                t.Start();
            });
        }
    }
}

