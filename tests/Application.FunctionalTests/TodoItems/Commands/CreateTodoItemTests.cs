using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.FunctionalTests.TodoItems.Commands;

public class CreateTodoItemTests : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateTodoItemCommand();

        await Should.ThrowAsync<ValidationException>(() => TestApp.SendAsync(command));
    }

    [Test]
    public async Task ShouldCreateTodoItem()
    {
        var userId = await TestApp.RunAsDefaultUserAsync();

        var listId = await TestApp.SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var command = new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "Tasks"
        };

        var itemId = await TestApp.SendAsync(command);

        var item = await TestApp.FindAsync<TodoItem>(itemId);

        item.ShouldNotBeNull();
        item!.ListId.ShouldBe(command.ListId);
        item.Title.ShouldBe(command.Title);
        item.CreatedBy.ShouldBe(userId);
        item.Created.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.ShouldBe(userId);
        item.LastModified.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }

    [Test]
    public async Task ShouldCreateTodoItemWithDueDate()
    {
        await TestApp.RunAsDefaultUserAsync();

        var listId = await TestApp.SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var dueDate = DateTime.Today.AddDays(7);

        var itemId = await TestApp.SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "Task with due date",
            DueDate = dueDate
        });

        var item = await TestApp.FindAsync<TodoItem>(itemId);

        item.ShouldNotBeNull();
        item!.DueDate.ShouldNotBeNull();
        item.DueDate!.Value.Date.ShouldBe(dueDate.Date);
    }
}
