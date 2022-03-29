using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace nat.Storage.Entity
{
    public class Trainer
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("SurName")]
        public string SurName { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; }

        [Required]
        [Column("LastName")]
        public string LastName { get; set; }

        [Required]
        [Column("Specialization")]
        public string Specialization { get; set; }

        [Required]
        [Column("CenterId")]
        public Guid CenterId { get; set; }
        [ForeignKey(nameof(CenterId))]
        public Center Center { get; set; }

        public List<Customer> Customers { get; set; }
    }

}
