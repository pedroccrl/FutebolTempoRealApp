using FutebolTempoRealApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FutebolTempoRealApp.ViewModel
{
    public class CentralViewModel : INotifyPropertyChanged
    {
        private const string URL_CENTRAL = "http://globoesporte.globo.com/temporeal/futebol/central.json";
        private const string URL_CENTRAL_BRASIL_SERIE_A = "http://globoesporte.globo.com/temporeal/futebol/central_4815_38.json";
        public event PropertyChangedEventHandler PropertyChanged;

        private List<Model.Api.Jogo> _jogos;

        public List<Model.Api.Jogo> Jogos
        {
            get { return _jogos; }
            set { _jogos = value; NotifyPropertyChanged("Jogos"); }
        }

        private SectionList<Model.Api.Jogo> _sectionJogos;

        public SectionList<Model.Api.Jogo> SectionJogos
        {
            get { return _sectionJogos; }
            set { _sectionJogos = value; }
        }


        public CentralViewModel()
        {
            GetJogosCentral();
        }

        public async void GetJogosCentral()
        {
            var http = new HttpClient();
            var json = await http.GetStringAsync(URL_CENTRAL);
            if (json == string.Empty) return;
            var central = JsonConvert.DeserializeObject<Model.Api.Central>(json);

            central.jogos.Reverse();

            var jogosGroup = from jogo in central.jogos
                             group jogo by jogo.nome_campeonato into jogoGroup
                             select jogoGroup;

            SectionJogos = new SectionList<Model.Api.Jogo>(jogosGroup);

            Jogos = central.jogos;
        }

        void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
