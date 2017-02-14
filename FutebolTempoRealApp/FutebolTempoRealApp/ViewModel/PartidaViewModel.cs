using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using FutebolTempoRealApp.Model.Api;
using System.Net.Http;
using Newtonsoft.Json;
using FutebolTempoRealApp.Model;
using System.Linq;

namespace FutebolTempoRealApp.ViewModel
{
    public class PartidaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Jogo Jogo { get; set; }

        private SectionList<Lance> _lances;

        public SectionList<Lance> Lances
        {
            get { return _lances; }
            set { _lances = value; NotifyPropertyChanged("Lances"); }
        }

        public static EscudoTimeId TimeCasa;
        public static EscudoTimeId TimeVisitante;

        public PartidaViewModel(Jogo jogo)
        {
            Jogo = jogo;
            //Jogo.url = "http://globoesporte.globo.com/pr/futebol/libertadores/jogo/08-02-2017/millonarios-atletico-pr/mensagens.json";

            TimeCasa = new EscudoTimeId
            {
                EscudoUrl = jogo.time_casa.escudo,
                TimeId = jogo.time_casa.id
            };

            TimeVisitante = new EscudoTimeId
            {
                EscudoUrl = jogo.time_visitante.escudo,
                TimeId = jogo.time_visitante.id
            };

            GetPartida();
        }

        async void GetPartida()
        {
            var http = new HttpClient();
            var json = await http.GetStringAsync(Jogo.LancesUrl);

            var lances = JsonConvert.DeserializeObject<List<Lance>>(json);

            var lancesAlteracao = lances.FindAll(l => l.operacao == "ALTERACAO");
            foreach (var item in lancesAlteracao)
            {
                var lance = lances.Find(l => l.id == item.id);
                lances.Remove(item);
                lance = item;
            }
            lances.Reverse();
            var lancesGrupo = from lance in lances
                              group lance by lance.periodo into lanceGrupo
                              select lanceGrupo;

            Lances = new SectionList<Lance>(lancesGrupo);
        }

        void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }

    public class EscudoTimeId
    {
        public string EscudoUrl { get; set; }
        public int TimeId { get; set; }
    }
}
