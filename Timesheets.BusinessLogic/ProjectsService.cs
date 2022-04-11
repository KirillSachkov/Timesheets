﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Domain;
using Timesheets.Domain.Interfaces;

namespace Timesheets.BusinessLogic
{
    public class ProjectsService : IProjectsService
    {
        public async Task<Project?> Get(int projectId)
        {
            return Projects.Get(projectId);
        }

        public async Task<Project[]> Get()
        {
            return Projects.Get();
        }

        public async Task<int> Create(Project newProject)
        {
            return Projects.Add(newProject);
        }

        public async Task<bool> Delete(int projectId)
        {
            Projects.Delete(projectId);

            return true;
        }

        public async Task<string[]> AddWorkTime(WorkTime workTime)
        {
            var project = Projects.Get(workTime.ProjectId);

            if (project == null)
            {
                return new string[] { "Project is null" };
            }

            return project.AddWorkTime(workTime);
        }
    }

    public static class Projects
    {
        private static List<Project> _projectList = new List<Project>();

        public static int Add(Project project)
        {
            var id = _projectList.Count + 1;

            _projectList.Add(project with { Id = id });

            return id;
        }

        public static Project[] Get()
        {
            return _projectList.ToArray();
        }

        public static Project? Get(int projectId)
        {
            return _projectList.FirstOrDefault(x => x.Id == projectId);
        }

        public static void Delete(int projectId)
        {
            var project = _projectList.FirstOrDefault(x => x.Id == projectId);

            _projectList.Remove(project);
        }
    }
}