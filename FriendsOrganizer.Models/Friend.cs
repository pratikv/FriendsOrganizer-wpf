using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FriendsOrganizer.Model
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public class Friend
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class LookupItem
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}
