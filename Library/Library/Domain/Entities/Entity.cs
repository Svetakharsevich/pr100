﻿using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entities
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
