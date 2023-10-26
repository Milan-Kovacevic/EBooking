using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.Entities
{
    [Table("Administrator")]
    internal class AdministratorEntity : UserEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
