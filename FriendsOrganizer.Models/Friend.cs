using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public Friend()
        {
            PhoneNumbers = new Collection<FriendPhoneNumber>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        public int? FavouriteLanguageId { get; set; }
        public ProgrammingLanguage FavouriteLanguage { get; set; }

        public ICollection<FriendPhoneNumber> PhoneNumbers { get; set; }
    }

    public class LookupItem
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }

    public class NullLookupItem : LookupItem
    {
        public new int? Id { get; set; }
    }

    public class ProgrammingLanguage
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }

    public class FriendPhoneNumber
    {
        public int Id { get; set; }
        [Required]
        [Phone]
        public string Number { get; set; }
        public int FriendId { get; set; }
        public Friend Friend { get; set; }
    }
}
