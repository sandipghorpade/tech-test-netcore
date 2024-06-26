﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class TodoItemController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public TodoItemController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create(int todoListId)
        {
            var fields = GetViewModelForCreate(todoListId);
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> Create(TodoItemCreateFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var item = new TodoItem(fields.TodoListId, fields.ResponsiblePartyId, fields.Title, fields.Importance);

            await dbContext.AddAsync(item);
            await dbContext.SaveChangesAsync();

            return RedirectToListDetail(fields.TodoListId);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Edit([FromQuery] int todoItemId)
        {
            var todoItem = dbContext.SingleTodoItem(todoItemId);
            var fields = TodoItemEditFieldsFactory.Create(todoItem);
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> Edit(TodoItemEditFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var todoItem = dbContext.SingleTodoItem(fields.TodoItemId);

            TodoItemEditFieldsFactory.Update(fields, todoItem);

            dbContext.Update(todoItem);
            await dbContext.SaveChangesAsync();

            return RedirectToListDetail(todoItem.TodoListId);
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult GetTodoItemFormPartialView(int todoListId)
        {
            var fields = GetViewModelForCreate(todoListId);
            return PartialView("_TodoItemFormPartial", fields);
        }

        [HttpPatch]
        [Route("[action]/{todoListId}")]
        public async Task<IActionResult> Patch([FromRoute] int todoListId,
                                               [FromBody] JsonPatchDocument<TodoItemPatchFields> patchDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var todoItem = dbContext.SingleTodoItem(todoListId);
            var patchToDoFields = TodoItemPatchFieldsFactory.Create(todoItem);
            patchDocument.ApplyTo(patchToDoFields, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            TodoItemPatchFieldsFactory.Update(patchToDoFields, todoItem);

            dbContext.Update(todoItem);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        private RedirectToActionResult RedirectToListDetail(int fieldsTodoListId)
        {
            return RedirectToAction("Detail", "TodoList", new { todoListId = fieldsTodoListId });
        }

        private TodoItemCreateFields GetViewModelForCreate(int todoListId)
        {
            var todoList = dbContext.SingleTodoList(todoListId);
            return TodoItemCreateFieldsFactory.Create(todoList, User.Id());
        }
    }
}