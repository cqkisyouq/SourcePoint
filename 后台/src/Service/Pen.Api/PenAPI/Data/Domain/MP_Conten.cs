using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PenAPI.Domain
{    
	[Table("MP_Content")]
	public class MP_Conten : BaseEntity
	{  
		/// <summary>
        /// 
        /// </summary>        
        public string Content { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public bool? IsShield { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public DateTime? ShieldTime { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string ShieldReason { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public Guid CreatedUser { get; set; }		
    }
}
