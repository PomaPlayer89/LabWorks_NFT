using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace nat.Storage.Entity
{
    public class Customer
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
        [Column("Birthday", TypeName ="date")]
        public DateTime Birthday { get; set; }

        [Required]
        [Column("TrainerId")]
        public Guid TrainerId { get; set; }
    }
}
