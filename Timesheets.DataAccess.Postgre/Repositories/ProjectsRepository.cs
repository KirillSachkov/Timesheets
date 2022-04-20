﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Timesheets.DataAccess.Postgre.Entities;
using Timesheets.Domain.Interfaces;

namespace Timesheets.DataAccess.Postgre.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly TimesheetsDbContext _context;
        private readonly IMapper _mapper;

        public ProjectsRepository(TimesheetsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Domain.Project?> Get(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.WorkTimes)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == projectId);

            if (project == null)
            {
                return null;
            }

            return _mapper.Map<Project, Domain.Project>(project);
        }

        public async Task<Domain.Project[]> Get()
        {
            var projectEntities = await _context.Projects
                .Include(p => p.WorkTimes)
                .AsNoTracking()
                .ToArrayAsync();

            var projects = _mapper.Map<Domain.Project[]>(projectEntities);

            return projects;
        }

        public async Task<int> Add(Domain.Project newProject)
        {
            var project = _mapper.Map<Project>(newProject);

            _context.Projects.Add(project);

            await _context.SaveChangesAsync();

            return project.Id;
        }

        public async Task<bool> Delete(int projectId)
        {
            _context.Projects.Remove(new Project { Id = projectId });

            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<(Domain.Project[], string[])> Get()
        //{
        //    var projectEntities = await _context.Projects
        //        .Include(p => p.WorkTimes)
        //        .AsNoTracking()
        //        .ToArrayAsync();

        //    var projects = projectEntities
        //        .Select(x =>
        //        {
        //            var (workTimes, errors) = x.WorkTimes
        //                .Select(y => Domain.WorkTime.Create(y.ProjectId, y.WorkingHours, y.Date))
        //                .Aggregate((Result: new List<Domain.WorkTime>(), Errors: Array.Empty<string>()), (a, b) =>
        //                {
        //                    if (b.Result == null || b.Errors.Any())
        //                    {
        //                        return (a.Result, a.Errors.Union(b.Errors).ToArray());
        //                    }
        //                    else
        //                    {
        //                        a.Result.Add(b.Result);
        //                        return (a.Result, a.Errors);
        //                    }
        //                });

        //            if (errors.Any())
        //            {
        //                return (null, errors);
        //            }
        //            else
        //            {
        //                return Domain.Project.Create(x.Title, x.Id, workTimes.ToArray());
        //            }
        //        })
        //        .Aggregate((Result: new List<Domain.Project>(), Errors: Array.Empty<string>()), (a, b) =>
        //        {
        //            if (b.Result == null || b.Errors.Any())
        //            {
        //                return (a.Result, a.Errors.Union(b.Errors).ToArray());
        //            }
        //            else
        //            {
        //                a.Result.Add(b.Result);
        //                return (a.Result, a.Errors);
        //            }
        //        });

        //    return (projects.Result.ToArray(), projects.Errors);
        //}
    }
}