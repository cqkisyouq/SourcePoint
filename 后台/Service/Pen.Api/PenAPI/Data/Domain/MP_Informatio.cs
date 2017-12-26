using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PenAPI.Domain
{    
	[Table("MP_Information")]
	public class MP_Informatio : BaseEntity
	{  
		/// <summary>
        /// 
        /// </summary>        
        public string Title { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public Guid LocationId { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public Guid ContentId { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public Guid UserId { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public bool? IsPublish { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public DateTime PublishTime { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public bool? IsShield { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public DateTime ShieldTime { get; set; }  
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
