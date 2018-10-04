﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;
using Vivid.GraphQL;
using Vivid.GraphQL.Models;

namespace Vivid.Web.GraphQL
{
    public class QueryResolver : IQueryResolver
    {
        private readonly IUserRegistrationRepository _userRegistrationRepo;

        private readonly ITaskListRepository _taskListRepo;

        private readonly ITaskItemRepository _taskItemRepo;

        public QueryResolver(IUserRegistrationRepository userRegistrationRepo, ITaskListRepository taskListRepo,
            ITaskItemRepository taskItemRepo)
        {
            _userRegistrationRepo = userRegistrationRepo;
            _taskListRepo = taskListRepo;
            _taskItemRepo = taskItemRepo;
        }

        public async Task<UserDto> CreateUserAsync(ResolveFieldContext<object> context)
        {
            var dto = context.GetArgument<UserCreationDto>("user");

            var entity = (User) dto;
            try
            {
                await _userRegistrationRepo.AddAsync(entity, context.CancellationToken)
                    .ConfigureAwait(false);

                string token = GenerateAlphaNumericString(100);
                await _userRegistrationRepo.SetTokenForUserAsync(entity.Id, token, context.CancellationToken)
                    .ConfigureAwait(false);
                string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                entity.Token = encodedToken;
            }
            catch (DuplicateKeyException)
            {
                var err = new Error("duplicate key")
                {
                    Path = new[] {"user"}
                };
                context.Errors.Add(err);
                return default;
            }

            return (UserDto) entity;
        }

        public async Task<UserDto> GetUserAsync(ResolveFieldContext<object> context)
        {
            string username = context.GetArgument<string>("userId");
            User entity;
            try
            {
                entity = await _userRegistrationRepo.GetByNameAsync(username, cancellationToken: context.CancellationToken)
                    .ConfigureAwait(false);
            }
            catch (EntityNotFoundException)
            {
                var err = new Error("not found")
                {
                    Path = new[] {"user"}
                };
                context.Errors.Add(err);
                return null;
            }

            return (UserDto) entity;
        }

        public async Task<TaskList> CreateTaskListAsync(ResolveFieldContext<object> context)
        {
            string username = context.GetArgument<string>("owner");
            var dto = context.GetArgument<TaskListCreationDto>("list");

            var entity = (TaskList) dto;
            await _taskListRepo.SetUsernameAsync(username)
                .ConfigureAwait(false);
            try
            {
                await _taskListRepo.AddAsync(entity, context.CancellationToken)
                    .ConfigureAwait(false);
            }
            catch (DuplicateKeyException)
            {
                var err = new Error("duplicate key")
                {
                    Path = new[] {"list"}
                };
                context.Errors.Add(err);
                return default;
            }

            return entity;
        }

        public async Task<TaskList[]> GetTaskListsForUserAsync(ResolveFieldContext<UserDto> context)
        {
            string username = context.Source.Id;
            await _taskListRepo.SetUsernameAsync(username, context.CancellationToken)
                .ConfigureAwait(false);

            var taskLists = await _taskListRepo.GetUserTaskListsAsync(context.CancellationToken)
                .ConfigureAwait(false);

            var taskListDtos = taskLists
                .Select(tl => tl)
                .ToArray();

            return taskListDtos;
        }

        public async Task<TaskItemDto> CreateTaskItemAsync(ResolveFieldContext<object> context)
        {
            string userName = context.GetArgument<string>("owner");
            string listName = context.GetArgument<string>("list");
            var dto = context.GetArgument<TaskItemCreationDto>("task");

            var entity = (TaskItem) dto;
            await _taskItemRepo.SetTaskListAsync(userName, listName, context.CancellationToken)
                .ConfigureAwait(false);
            try
            {
                await _taskItemRepo.AddAsync(entity, context.CancellationToken)
                    .ConfigureAwait(false);
            }
            catch (DuplicateKeyException)
            {
                var err = new Error("duplicate key")
                {
                    Path = new[] {"task"}
                };
                context.Errors.Add(err);
                return default;
            }

            return (TaskItemDto) entity;
        }

        public async Task<TaskItemDto[]> GetTaskItemsForListAsync(ResolveFieldContext<TaskList> context)
        {
            string tasklistName = context.Source.DisplayId;
            string userName = (await _userRegistrationRepo
                    .GetByIdAsync(context.Source.OwnerId, cancellationToken: context.CancellationToken)
                    .ConfigureAwait(false)
                ).DisplayId;

            await _taskItemRepo.SetTaskListAsync(userName, tasklistName, context.CancellationToken)
                .ConfigureAwait(false);

            var tasks = await _taskItemRepo.GetTaskItemsAsync(cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            var taskDtos = tasks
                .Select(tl => (TaskItemDto) tl)
                .ToArray();

            return taskDtos;
        }

        private string GenerateAlphaNumericString(int charCount)
        {
            var rnd = new Random(DateTime.UtcNow.Millisecond);
            var chars = Enumerable.Range(0, charCount)
                .Select(_ =>
                {
                    char c = default;
                    int charType = rnd.Next() % 3;
                    switch (charType)
                    {
                        case 0: // Number
                            c = (char) rnd.Next(48, 50);
                            break;
                        case 1: // Upper-Case Letter
                            c = (char) rnd.Next(65, 91);
                            break;
                        case 2: // Lower-Case Letter
                            c = (char) rnd.Next(97, 123);
                            break;
                    }

                    return c;
                });
            return string.Join(string.Empty, chars);
        }
    }
}