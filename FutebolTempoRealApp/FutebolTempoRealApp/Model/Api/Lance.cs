using FutebolTempoRealApp.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FutebolTempoRealApp.Model.Api
{

    public class Lance
    {
        public string operacao { get; set; }
        public string url_thumb_grande { get; set; }
        public int total_mensagens { get; set; }
        public string tipo { get; set; }
        public string url_thumb_pequeno { get; set; }
        public string periodo_sigla { get; set; }
        public string periodo { get; set; }
        public string url_thumb_medio { get; set; }
        public object video_id { get; set; }
        public int periodo_id { get; set; }
        private string _texto;
        public string texto
        {
            get
            {
                return _texto;
            }
            set
            {
                _texto = value;
                if (_texto.Contains("blockquote"))
                {
                    _tipo = LanceType.TwitterEmbed;
                    return;
                }
                _texto = HtmlRemoval.RemoveHTML(_texto);
                _texto = HtmlRemoval.StripTagsRegex(_texto);
                _texto = HtmlRemoval.StripTagsRegexCompiled(_texto);
                _texto = HtmlRemoval.FiltraHtml(_texto);
            }
        }
        public int id { get; set; }
        public string nome_time { get; set; }
        public string momento { get; set; }
        public string foto_credito { get; set; }
        public Jogo jogo { get; set; }
        public string titulo { get; set; }
        public string foto_url { private get; set; }
        public int? time { get; set; }
        public string foto_titulo { get; set; }
        public bool video_multicamera { get; set; }
        public Cartao cartao { get; set; }
        public Gol gol { get; set; }
        public Substituicao substituicao { get; set; }
        public Penalti penalti { get; set; }
        public Jogador Jogador
        {
            get
            {
                var jogador = new Jogador();
                if (tipo == "LANCE_GOL")
                {
                    jogador.FotoUrl = gol?.foto;
                    jogador.PosicaoDescricao = gol?.posicao_descricao;
                    jogador.Nome = gol?.autor;
                    jogador.NomeTime = nome_time;
                }
                else if (tipo == "LANCE_CARTAO")
                {
                    jogador.FotoUrl = cartao?.foto;
                    jogador.PosicaoDescricao = cartao?.posicao_descricao;
                    jogador.Nome = cartao?.nome_jogador;
                    jogador.NomeTime = nome_time;
                }
                else if (tipo == "LANCE_PENALTIS")
                {
                    jogador.FotoUrl = penalti?.foto;
                    jogador.PosicaoDescricao = penalti?.posicao_descricao;
                    jogador.Nome = penalti?.nome_jogador;
                    jogador.NomeTime = nome_time;
                }
                else if (tipo == "LANCE_SUBSTITUICAO") jogador.NomeTime = nome_time;

                //jogador.EscudoClubeUrl = PartidaViewModel.TimeCasa.TimeId == jogador.IdTimeGlobo ? PartidaViewModel.TimeCasa.EscudoUrl : PartidaViewModel.TimeVisitante.EscudoUrl;
                if (jogador.NomeTime == jogo.equipe_mandante.nome)
                {
                    jogador.EscudoClubeUrl = PartidaViewModel.TimeCasa.EscudoUrl;
                }
                else if (jogador.NomeTime == jogo.equipe_visitante.nome)
                {
                    jogador.EscudoClubeUrl = PartidaViewModel.TimeVisitante.EscudoUrl;
                }

                return jogador;
            }
        }
        public string Tempo
        {
            get
            {
                if (periodo != "Pré-Jogo" && periodo != "Pós Jogo" && periodo != "Penalidades" && periodo!="Intervalo" && periodo != "Fim de jogo") return $"{Periodo} {momento}'";
                else return string.Empty;
            }
        }
        public string Periodo
        {
            get
            {
                if (periodo == "1º tempo") return "1º";
                else if (periodo == "2º tempo") return "2º";
                else return periodo;
            }
        }
        private LanceTwitter _lanceTwitter;

        public LanceTwitter LanceTwitter
        {
            get
            {
                if (tipo != "LANCE_TWITTER") return null;
                if (_lanceTwitter == null)
                {
                    //var json = texto.Replace("\\","");
                    var json = texto;
                    var twitter = JsonConvert.DeserializeObject<LanceTwitter.Twitter>(json);
                    _lanceTwitter = new LanceTwitter { Lance = twitter };
                }
                return _lanceTwitter;
            }
            private set { _lanceTwitter = value; }
        }

        private LanceType _tipo = LanceType.SemLance;
        public LanceType Tipo
        {
            get
            {
                if (_tipo != LanceType.SemLance) return _tipo;
                if (tipo == "LANCE_GOL")
                {
                    _tipo = LanceType.Gol;
                }
                else if (tipo == "LANCE_CARTAO")
                {
                    _tipo = LanceType.Cartao;
                }
                else if (tipo == "LANCE_TWITTER")
                {
                    _tipo = LanceType.Twitter;
                }
                else if (tipo == "LANCE" || tipo == "LANCE_IMPORTANTE" || tipo == "POS_LANCE_DESTAQUE" || tipo == "POS_LANCE_OJOGO")
                {
                    if (texto.Contains("blockquote")) _tipo = LanceType.TwitterEmbed;
                    else if (string.IsNullOrWhiteSpace(titulo)) _tipo = LanceType.SimplesSemTitulo;
                    else _tipo = LanceType.Simples;
                }
                else if (tipo == "LANCE_SUBSTITUICAO") 
                {
                    _tipo = LanceType.Substituicao;
                }

                return _tipo;
            }
        }

        public string FotoUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(foto_url))
                {
                    if (!string.IsNullOrWhiteSpace(url_thumb_grande)) return url_thumb_grande;
                    else return null;
                }
                return foto_url;
            }
        }
    }
    
    /// <summary>
    /// Precisa adicionar a foto do escudo manualmente
    /// </summary>
    public class Jogador
    {
        public string FotoUrl { get; set; }
        public string EscudoClubeUrl { get; set; }
        public string PosicaoDescricao { get; set; }
        public string Nome { get; set; }
        public int? IdTimeGlobo { get; set; }
        public string NomeTime { get; set; }
    }

    public class Cartao
    {
        public string foto { get; set; }
        public string tipo { get; set; }
        public string nome_jogador { get; set; }
        public int jogador_id { get; set; }
        public string posicao_descricao { get; set; }
        public int atuacao_id { get; set; }
        public int cartao_id { get; set; }
    }

    public class Gol
    {
        public string foto { get; set; }
        public bool contra { get; set; }
        public string autor { get; set; }
        public string posicao_descricao { get; set; }
        public int? id_time { get; set; }
        public int? autor_id { get; set; }
        public int? atuacao { get; set; }
        public int? id { get; set; }
    }

    public class Substituicao
    {
        public int pessoatr_id { get; set; }
        public string foto { get; set; }
        public int jogotr_id { get; set; }
        public bool titular { get; set; }
        public string nome { get; set; }
        public Substituido_Por substituido_por { get; set; }
        public int minuto_entrou { get; set; }
        public int ordem { get; set; }
        public int time_id { get; set; }
        public string posicao_descricao { get; set; }
        public int jogo_id { get; set; }
        public string camisa { get; set; }
        public string posicao { get; set; }
        public int timetr_id { get; set; }
        public Periodo_Entrou periodo_entrou { get; set; }
        public int atuacao_id { get; set; }
        public string lance_substituto { get; set; }
    }

    public class Substituido_Por
    {
        public int pessoatr_id { get; set; }
        public string foto { get; set; }
        public bool titular { get; set; }
        public string nome { get; set; }
        public string substituido_por { get; set; }
        public string posicao_descricao { get; set; }
        public string camisa { get; set; }
        public string posicao { get; set; }
        public int atuacao_id { get; set; }
    }

    public class Periodo_Entrou
    {
        public int bola_rolando { get; set; }
        public int ordem { get; set; }
        public string id_tipo_cobertura { get; set; }
        public int periodo_id { get; set; }
        public string nome_exibicao { get; set; }
        public string sigla { get; set; }
    }

    public class Penalti
    {
        public int ordem { get; set; }
        public bool convertido { get; set; }
        public string nome_jogador { get; set; }
        public int penalti_id { get; set; }
        public string foto { get; set; }
        public string posicao_descricao { get; set; }
        public int id_time { get; set; }
        public int atuacao_id { get; set; }
    }

    public class LanceTwitter
    {
        public Twitter Lance { get; set; }

        public class User
        {
            public string name { get; set; }
            public string profile_image_url { get; set; }
            public string screen_name { get; set; }
            public string Perfil
            {
                get { return $"@{screen_name}"; }
            }
        }

        public class Twitter
        {
            public string text { get; set; }
            public User user { get; set; }
            public List<string> photo { get; set; }
            public string FotoUrl
            {
                get { if (photo != null && photo.Count > 0) return photo[0]; else return null; }
            }

        }
    }

    public enum LanceType
    {
        Titulo,
        Simples,
        SimplesSemTitulo,
        Twitter,
        TwitterEmbed,
        Gol,
        Cartao,
        Substituicao,
        Video,
        SemLance = -1,
    }
}
