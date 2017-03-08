using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FutebolTempoRealApp.Model
{
    public interface ISaveAndLoad
    {
        void SaveText(string filename, string text);
        string LoadText(string filename);
        Task SaveTextAsync(string filename, string text);
        Task<string> LoadTextAsync(string filename);
    }
}
