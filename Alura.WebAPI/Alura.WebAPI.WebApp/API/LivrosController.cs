﻿using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.WebAPI.WebApp.API
{
    [ApiController]
    [Route("[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public LivrosController(IRepository<Livro> repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public IActionResult ListaDeLivros()
        {
            var lista = _repo.All.Select(l => l.ToModel()).ToList();    
            return Ok(lista);
        }


        [HttpGet("{id}")]
        public IActionResult Recuperar(int id)
        {
            var model = _repo.Find(id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model.ToModel());
        }

        [HttpPost]
        public IActionResult Incluir([FromBody] LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                _repo.Incluir(livro);
                var url = Url.Action("Recuperar", new { id = livro.Id });
                return Created(url, livro); //201
            }
            return BadRequest();
        }


        [HttpPut]
        public IActionResult Alterar([FromBody] LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                if (model.Capa == null)
                {
                    livro.ImagemCapa = _repo.All
                        .Where(l => l.Id == livro.Id)
                        .Select(l => l.ImagemCapa)
                        .FirstOrDefault();
                }
                _repo.Alterar(livro);
                return Ok(); // 200
            }
            return BadRequest(); //400
        }


        [HttpDelete("{id}")]
        public IActionResult Remover(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            _repo.Excluir(model);

            return NoContent(); //204
        }



    }
}
