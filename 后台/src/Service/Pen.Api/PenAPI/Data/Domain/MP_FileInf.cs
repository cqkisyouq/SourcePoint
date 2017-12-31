using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PenAPI.Domain
{    
	[Table("MP_FileInfo")]
	public class MP_FileInf : BaseEntity
	{  
		/// <summary>
        /// 
        /// </summary>        
        public string Name { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string Extension { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string Path { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public long? Size { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public Guid CreatedUser { get; set; }		
    }
}
