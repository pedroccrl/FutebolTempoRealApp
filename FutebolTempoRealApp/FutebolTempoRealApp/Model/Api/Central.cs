using System;
using System.Collections.Generic;
using System.Text;

namespace FutebolTempoRealApp.Model.Api
{
    public class Central
    {
        public string data_hoje { get; set; }
        public List<Jogo> jogos { get; set; }
    }

    public class TimeCasa
    {
        public object penalti { get; set; }
        public int placar { get; set; }
        public string sigla { get; set; }
        public string escudo { get; set; }
        public string esquema { get; set; }
        public string nome { get; set; }
        public string escudo_pequeno { get; set; }
        public string tecnico { get; set; }
        public int id { get; set; }
        public int gols { get; set; }
        public string escudo_grande { get; set; }
        public string slug { get; set; }
    }

    public class TimeVisitante
    {
        public object penalti { get; set; }
        public int placar { get; set; }
        public string sigla { get; set; }
        public string escudo { get; set; }
        public string esquema { get; set; }
        public string nome { get; set; }
        public string escudo_pequeno { get; set; }
        public string tecnico { get; set; }
        public int id { get; set; }
        public int gols { get; set; }
        public string escudo_grande { get; set; }
        public string slug { get; set; }
    }

    public class Jogo
    {
        public int transmissao_id { get; set; }
        public string status { get; set; }
        public string fase_rodada { get; set; }
        public string dia_semana { get; set; }
        public TimeCasa time_casa { get; set; }
        public string video_ao_vivo { get; set; }
        public string tipo_transmissao { get; set; }
        public string hora { get; set; }
        public string nome_campeonato { get; set; }
        public TimeVisitante time_visitante { get; set; }
        public object localizacao { get; set; }
        public string data { get; set; }
        public int id { get; set; }
        public EquipeVisitante equipe_visitante { get; set; }
        public EquipeMandante equipe_mandante { get; set; }
        public string url { get; set; }
        public int placar_visitante { get; set; }
        public int placar_mandante { get; set; }
        public int penaltis_mandante { get; set; }
        public int penaltis_visitante { get; set; }

        public string Placar
        {
            get { return $"{time_casa.placar} x {time_visitante.placar}"; }
        }

        public string Info
        {
            get { return $"{status.ToUpper()} | {hora} | {fase_rodada}"; }
        }

        public string Status
        {
            get
            {
                switch (status)
                {
                    case "Em Andamento": return "AO VIVO! Veja lance a lance";
                    case "Criada": return "Sem lances ainda";
                    case "Encerrada": return "Confira como foi";
                    default: return string.Empty;
                }
            }
        }

        public string LancesUrl
        {
            get { return $"{url}/mensagens.json"; }
        }
    }

    public class EquipeVisitante
    {
        public int id { get; set; }
        public string nome { get; set; }
    }

    public class EquipeMandante
    {
        public int id { get; set; }
        public string nome { get; set; }
    }
}
