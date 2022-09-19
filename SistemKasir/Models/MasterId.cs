using System;
using System.Collections.Generic;

namespace SistemKasir.Models
{
    public partial class MasterId
    {
        public int IdMaster { get; set; }
        public string? PrefixId { get; set; }
        public int? Counter { get; set; }
    }
}
