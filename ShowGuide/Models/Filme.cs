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
            Utilizadores = new HashSet<ApplicationUser>();
        }
        [Key]
        public int Id { get; set; }
        //titulo do filme
        [Required]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        //descrição ou sinopse do filme
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        //data de lançamento ou data de estreia do filme
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Lançamento")]
        public DateTime DataLancamento { get; set; }

        //Elenco do filme
        [Required]
        public string Elenco { get; set; }

        //link para o trailler do filme
        [Display(Name = "Trailler")]
        public string TraillerLink { get; set; }

        //extensão da imagem do filme
        [Display(Name = "Imagem")]
        public string ImageExtension { get; set; }

        // especifica que um filme tem vários Comentarios
        public virtual ICollection<Comentario> Comentraios { get; set; }
        // especifica que um filme tem várias Categorias
        [Required]
        public virtual ICollection<Categoria> Categorias { get; set; }
        // utilizadores que viram este filme
        public virtual ICollection<ApplicationUser> Utilizadores { get; set; }
    }
}