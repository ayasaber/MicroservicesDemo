using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Notifications.Entities
{
    public  class User //: IdentityUser<long>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column("Id")]
        public  long Id { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string? SecondName { get; set; }
        [MaxLength(50)]
        public string? ThirdName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PreferredLanguage { get; set; }
        public string UserName { get; set; }

    }
}
