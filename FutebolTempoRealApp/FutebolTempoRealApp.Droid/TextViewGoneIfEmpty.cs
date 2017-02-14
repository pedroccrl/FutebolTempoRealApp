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
    public class TextViewGoneIfEmpty : TextView
    {
        public TextViewGoneIfEmpty(Context context, TextView textView) : base(context)
        {
            
        }

        private ViewStates _visibility;
        public override ViewStates Visibility
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Text)) return ViewStates.Gone;
                else return _visibility;
            }

            set
            {
                _visibility = value;
            }
        }
    }
}