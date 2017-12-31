using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PenAPI.Domain
{    
	[Table("MP_ContentFile")]
	public class MP_ContentFil : BaseEntity
	{  
		/// <summary>
        /// 
        /// </summary>        
        public Guid ContentId { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public Guid FileInfoId { get; set; }  
		/// <summary>
        /// 1：图片 2：文档  3：压缩文件 4：其它
        /// </summary>        
        public string FileType { get; set; }  
		/// <summary>
        /// 
        /// </summary>        
        public Guid CreatedUser { get; set; }		
    }
}
