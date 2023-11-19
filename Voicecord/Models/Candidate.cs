using System.ComponentModel.DataAnnotations;

namespace Voicecord.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CandidateString { get; set; }
        [Required]
        public int SdpMLineIndex {  get; set; }
        [Required]
        public string SdpMLine { get; set; }
        [Required]
        public string UsernameFragment {  get; set; }
    }
}
