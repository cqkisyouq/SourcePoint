using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PenAPI.Domain
{    
	[Table("MP_User")]
	public class MP_Use : BaseEntity
	{  
		/// <summary>
        /// 
        /// </summary>        
        public string Name { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string LoginName { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public DateTime? LoginTime { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public DateTime? CreateTime { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public DateTime? UpdateTime { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public int? Status { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public int? Gender { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string Phone { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string LoginIp { get; set; }		
    }
}
