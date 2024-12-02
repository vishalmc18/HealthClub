using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.Models
{
    public class Address
    {

        [Key]
        public int Id { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }

    }
}
