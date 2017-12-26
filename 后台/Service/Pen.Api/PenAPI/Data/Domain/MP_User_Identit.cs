using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PenAPI.Domain
{    
	[Table("MP_User_Identity")]
	public class MP_User_Identit : BaseEntity
	{  
		/// <summary>
        /// 
        /// </summary>        
        public Guid? UserId { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string Password { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string HashAlgorithm { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string PasswordSalt { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public DateTime? LastPasswordChangedTime { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public bool? IsLocked { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public DateTime? LastLockedTime { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public int? FailePasswordAttemptCount { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public DateTime? UpdateTime { get; set; }		
    }
}
