using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FutebolTempoRealApp.Droid
{
    public class Constants
    {
        public const string SenderID = "954618459869"; // Google API Project Number
        public const string ListenConnectionString = "Endpoint=sb://futeboltemporealappnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=PBg4+0d9N3TbIfdaP8Nem/RJE7syyLbXa+eaARrHNYM=";
        public const string NotificationHubName = "futeboltemporealappnotificationhub";
    }
}