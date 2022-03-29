using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace nat.Storage.Entity
{
    public class Center
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; }

        [Required]
        [Column("City")]
        public string AdressCity { get; set; }

        [Required]
        [Column("Street")]
        public string AdressStreet { get; set; }

        [Required]
        [Column("NumberHouse")]
        public string AdressNumberHouse { get; set; }

        public List<Customer> Customers { get; set; }
        public List<Trainer> Trainers { get; set; }

    }
}
