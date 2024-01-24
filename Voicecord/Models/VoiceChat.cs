using System.ComponentModel.DataAnnotations;

namespace Voicecord.Models
{
    public class VoiceChat : Interfaces.IModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Candidate> OfferCandidates { get; set; }
        public List<Candidate> AnswerCandidates { get; set; }
        public List<OffersAnswers> OffersAns { get; set; }
    }
}
