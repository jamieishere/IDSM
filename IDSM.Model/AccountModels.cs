using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;

namespace IDSM.Model
{
    /// <summary>
    /// UsersContext
    /// Removed class in remarks as per this link (http://stackoverflow.com/questions/12449126/asp-net-mvc-membership-code-first-existing-database)
    /// This was to enable the merging of my existing models & DBContext with WebSecurity, which is what the AccountController uses
    /// Needed to add a n intiialise websecurity call in the global.asax to replace this.
    /// Also see
    ///     http://odetocode.com/blogs/scott/archive/2012/09/23/perils-of-the-mvc4-accountcontroller.aspx
    ///     http://stackoverflow.com/questions/13207893/net-mvc-simple-membership-authentication-with-database (initialise websecurity)
    /// </summary>
    /// <remarks>
    ///public class UsersContext : DbContext
    ///{
    ///    public UsersContext()
    ///        //: base("DefaultConnection")
    ///        : base("IDSMContext")
    ///    {
    ///    }
    ///    public DbSet<UserProfile> UserProfiles { get; set; }
    ///}
    ///</remarks>
    [Table("UserProfile")]
    public class UserProfile
    {
        // are these attributes necessary?
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }

        //constructor that populates the navigation properties
        public UserProfile()
        {
            UserTeams = new HashSet<UserTeam>();
            Games = new HashSet<Game>();
        }

        //navigation properties
       // [ForeignKey("UserId")]
        public virtual ICollection<UserTeam> UserTeams { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
