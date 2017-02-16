using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using FutebolTempoRealApp.ViewModel;
using FutebolTempoRealApp.Droid;

namespace FutebolTempoRealApp.Droid.Adapters
{
    public class PartidaPageAdapter : FragmentPagerAdapter
    {
        string[] tabTitulos = { "Lances", "Mídias", "Resenha" };
        Context Context;
        PartidaViewModel ViewModel;

        public PartidaPageAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public PartidaPageAdapter(Context context, FragmentManager fm, PartidaViewModel viewModel) : base(fm)
        {
            Context = context;
            ViewModel = viewModel;
        }

        public override int Count
        {
            get { return 3; }
        }

        public override Fragment GetItem(int position)
        {
            return new PartidaPageFragment((PartidasPages)position, ViewModel, Context);
        }

        public View GetTabView(int position)
        {
            var textView = (TextView)LayoutInflater.From(Context).Inflate(Resource.Layout.CustomTab, null);
            textView.Text = tabTitulos[position];
            return textView;
        }
    }

    
}