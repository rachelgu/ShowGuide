using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShowGuide.Models
{
    public class Filme
    {
        public Filme()
        {
            Categorias = new HashSet<Categoria>();
            Comentraios = new HashSet<Comentario>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Título")]
        public string Titulo { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Lançamento")]
        public DateTime DataLancamento { get; set; }
        [Required]
        public string Elenco { get; set; }
        [Display(Name = "Trailler")]
        public string TraillerLink { get; set; }
        [Display(Name = "Imagem")]
        public string ImageExtension { get; set; }

        // especifica que um filme tem vários Comentarios
        public virtual ICollection<Comentario> Comentraios { get; set; }
        // especifica que um filme tem várias Categorias
        [Required]
        public virtual ICollection<Categoria> Categorias { get; set; }
    }
}