using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FutebolTempoRealApp.Droid.Adapters;
using Newtonsoft.Json;

namespace FutebolTempoRealApp.Droid
{
	[Activity (Label = "FutebolTempoRealApp.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        ViewModel.CentralViewModel ViewModel;
        ListView ListViewCentral;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            SetContentView(Resource.Layout.Main);

            ViewModel = new ViewModel.CentralViewModel();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            ListViewCentral = FindViewById<ListView>(Resource.Id.ListViewCentral);
            ListViewCentral.ItemClick += ListViewCentral_ItemClick;
		}

        private void ListViewCentral_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = ViewModel.SectionJogos[e.Position];

            if (item.IsSection) return;

            var partida = item.Item;

            var partidaIntent = new Intent(this, typeof(PartidaActivity));
            partidaIntent.PutExtra("Partida", JsonConvert.SerializeObject(partida));

            StartActivity(partidaIntent);
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Jogos")
            {
                ListViewCentral.Adapter = new CentralAdapter(this,  ViewModel.SectionJogos);
                FindViewById<ProgressBar>(Resource.Id.CentralProgress).Visibility = ViewStates.Gone;
            }
        }
    }
    
}


