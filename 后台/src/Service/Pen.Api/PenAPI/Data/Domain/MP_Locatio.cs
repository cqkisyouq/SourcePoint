using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PenAPI.Domain
{    
	[Table("MP_Location")]
	public class MP_Locatio : BaseEntity
	{  
		/// <summary>
        /// 
        /// </summary>        
        public Guid InformationId { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public Guid ContentId { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string Address { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public double Longitude { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public double Latitude { get; set; }  
		/// <summary>
        /// 代码某个地区范围
        /// </summary>        
        public string AreaCode { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public Guid CreatedUser { get; set; }		
    }
}
