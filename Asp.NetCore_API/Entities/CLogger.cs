using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.NetCore_API.Entities
{
	[Table("Logger")]
	public class CLogger
	{
		[Key]
		[Column("Id")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Column("SourceProject")]
		[MaxLength(150)]
		public string SourceProject { get; set; }

		[Column("MachineName")]
		[MaxLength(50)]
		public string MachineName { get; set; }

		[Column("Logged")]
		public DateTime Logged { get; set; }

		[Column("Level")]
		[MaxLength(50)]
		public string Level { get; set; }

		[Column("Message")]
		public string Message { get; set; }

		[Column("Logger")]
		[MaxLength(250)]
		public string Logger { get; set; }

		[Column("CallSite")]
		public string CallSite { get; set; }

		[Column("Exception")]
		public string Exception { get; set; }

		[Column("Guid")]
		[Required]
		public Guid Guid { get; set; }

		[Column("CreatedAt")]
		[Required]
		public DateTime CreatedAt { get; set; }
	}
}