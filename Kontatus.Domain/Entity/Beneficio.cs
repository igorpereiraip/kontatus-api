﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Kontatus.Domain.Entity
{
    public class Beneficio
    {
        public string NumeroBeneficio { get; set; }
        public string Situacao { get; set; }
        public int? ErrorID { get; set; }

    }
}
