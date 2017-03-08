using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FutebolTempoRealApp.Model;
using System.IO;
using System.Threading.Tasks;
using static Android.InputMethodServices.InputMethodService;
using FutebolTempoRealApp.ViewModel;
using Newtonsoft.Json;

namespace FutebolTempoRealApp.Droid
{
    
    public class SaveAndLoad : ISaveAndLoad
    {
        Context Context;
        
        public SaveAndLoad(Context context)
        {
            this.Context = context;
        }
        public void SaveText(string filename, string text)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            System.IO.File.WriteAllText(filePath, text);
        }
        public string LoadText(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return System.IO.File.ReadAllText(filePath);
        }

        public async Task SaveTextAsync(string filename, string text)
        {
            await Task.Run(() => { SaveText(filename, text); });
        }

        public async Task<string> LoadTextAsync(string filename)
        {
            var texto = string.Empty;
            await Task.Run(() =>
            {
                texto = LoadText(filename);
            });
            return texto;
        }
    }
}