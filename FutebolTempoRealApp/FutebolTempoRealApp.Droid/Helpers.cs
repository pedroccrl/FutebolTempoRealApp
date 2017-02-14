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
using System.Net;
using Android.Graphics;
using System.Threading.Tasks;
using System.Threading;

namespace FutebolTempoRealApp.Droid
{
   [Obsolete("Usar a biblioteca Picasso para carregar imagens.")]
   public static class Helpers
    {
        static List<KeyValuePair<string, Bitmap>> _listUrlBitmaps;
        private static List<KeyValuePair<string, Bitmap>> ListUrlBitmaps
        {
            get
            {
                if (_listUrlBitmaps == null) _listUrlBitmaps = new List<KeyValuePair<string, Bitmap>>();
                return _listUrlBitmaps;
            }
        }
        public static async void ShowImageFromUrl(ImageView imageView, string url)
        {
            if (ListUrlBitmaps.Exists(urlb => urlb.Key == url)) imageView.SetImageBitmap(ListUrlBitmaps.Find(urlb => urlb.Key == url).Value);
            else imageView.SetImageBitmap(await DownloadBitmap(url));
        }

        static Mutex mutexHtpp = new Mutex();
        static async Task<Bitmap> DownloadBitmap(string url)
        {
            Bitmap imageBitmap = null;
            mutexHtpp.WaitOne();
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            
            ListUrlBitmaps.Add(new KeyValuePair<string, Bitmap>(url, imageBitmap));

            mutexHtpp.ReleaseMutex();
            return imageBitmap;
        }
    }
}