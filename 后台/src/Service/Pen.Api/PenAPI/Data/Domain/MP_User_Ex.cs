using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PenAPI.Domain
{    
	[Table("MP_User_Ext")]
	public class MP_User_Ex : BaseEntity
	{  
		/// <summary>
        /// 
        /// </summary>        
        public string UsualAddress { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public string usualCoord { get; set; }		
    }
}
