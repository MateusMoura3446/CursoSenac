using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class EmprestimoService 
    {
        public void Inserir(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Emprestimos.Add(e);
                bc.SaveChanges();
            }
        }

        public void Atualizar(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                Emprestimo emprestimo = bc.Emprestimos.Find(e.Id);
                emprestimo.NomeUsuario = e.NomeUsuario;
                emprestimo.Telefone = e.Telefone;
                emprestimo.LivroId = e.LivroId;
                emprestimo.DataEmprestimo = e.DataEmprestimo;
                emprestimo.DataDevolucao = e.DataDevolucao;
                emprestimo.Devolvido= e.Devolvido;

                bc.SaveChanges();
            }
        }

        public ICollection<Emprestimo> ListarTodos(FiltrosEmprestimos filtro = null)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Emprestimo> query;
                
                if(filtro != null)
                {
                    //definindo dinamicamente a filtragem
                    switch(filtro.TipoFiltro)
                    {
                        case "NomeUsuario":
                            query = bc.Emprestimos.Where(e => e.NomeUsuario.Contains(filtro.Filtro));
                        break;

                        case "Livro":
                   
                          List<Livro> LivrosFiltrados = bc.Livros.Where(l => l.Titulo.Contains(filtro.Filtro)).ToList();             
                           List<int>LivrosId = new List<int>();
                          for (int i = 0; i < LivrosFiltrados.Count; i++)
                            {
                             LivrosId.Add(LivrosFiltrados[i].Id);
                            }
                          query = bc.Emprestimos.Where(e => LivrosId.Contains(e.LivroId));
                          var debug = query.ToList();
                        break;

                        
                       
                    }
                }
                else
                {
                    // caso filtro não tenha sido informado
                    query = bc.Emprestimos;
                }
                
                return bc.Emprestimos.Include(e => e.Livro).ToList();
            }
        }

        public ICollection<Emprestimo> ListarDisponiveis()
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                //busca os livros onde o id não está entre os ids de livro em empréstimo
                // utiliza uma subconsulta
                return
                    bc.Emprestimos
                    .Where(l =>  !(bc.Emprestimos.Where(e => e.Devolvido == false).Select(e => e.LivroId).Contains(l.Id)) )
                    .ToList();
            }
        }

        public Emprestimo ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Emprestimos.Find(id);
            }
        }
    }
}