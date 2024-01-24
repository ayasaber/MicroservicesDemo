﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class Lookup<T> : FullAuditableEntity<T>
	{
		public string Code { get; set; }
		public string NameAr { get; set; }
		public string NameEn { get; set; }
	}
}
