using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PenAPI.Domain
{    
    /// <summary>
    /// 
    /// </summary>
	[Table("MP_AreaCity")]
	public class MP_AreaCit : BaseEntity
	{  
		/// <summary>
        /// 
        /// </summary>        
        public string Code { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string Name { get; set; }  
		/// <summary>
        /// 区别国家
        /// </summary>        
        public string AreaType { get; set; }		
    }
}
