using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShowGuide.Models
{
    public class Comentario
    {
        [Key]
        public int Id { get; set; }

        //texto do comentário
        [Required]
        [DataType(DataType.MultilineText)]
        public string Texto { get; set; }

        //data do comentario
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Data { get; set; }

        //filme a que o comentário está associado
        [ForeignKey("Filme")]
        [Display(Name = "Filme")]
        public int FilmeId { get; set; }
        public virtual Filme Filme { get; set; }

        //utilizador a que o comentário está associado
        [ForeignKey("Utilizador")]
        [Display(Name = "Utilizador")]
        public string UserId { get; set; }
        public virtual ApplicationUser Utilizador { get; set; }
    }
}