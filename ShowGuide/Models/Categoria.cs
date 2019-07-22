using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowGuide.Models
{
    public class Categoria
    {

        public Categoria()
        {
            Filmes = new HashSet<Filme>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        // especifica que uma categoria tem vários Filmes
        public virtual ICollection<Filme> Filmes { get; set; }
    }
}