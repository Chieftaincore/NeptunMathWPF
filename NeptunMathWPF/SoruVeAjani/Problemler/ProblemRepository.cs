using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NeptunMathWPF.SoruVeAjani.Problemler
{
    public class ProblemRepository
    {
        [JsonPropertyName("problem")]
        public string problem { get; set; }
        [JsonPropertyName("dogru_cevap")]
        public string correctAnswer { get; set; }
        [JsonPropertyName("yanlis_cevaplar")]
        public List<string> incorrectAnswers { get; set; }
    }
}
