using System.ComponentModel.DataAnnotations;

namespace Voicecord.Models
{
    public class VoiceChat
    {
        [Key]
        public int Id { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Candidate> OfferCandidates { get; set; }
        public List<Candidate> AnswerCandidates { get; set; }
        public List<OffersAnswers> OffersAns { get; set; }// key=offer,value=answer (колво пар человек в собрании)
    }
}
