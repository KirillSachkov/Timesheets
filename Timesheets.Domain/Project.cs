﻿namespace Timesheets.Domain
{
    public record Project
    {
        private readonly List<WorkTime> _workingHours;

        private Project(int id, string title, WorkTime[] workingHours)
        {
            Id = id;
            Title = title;
            _workingHours = new List<WorkTime>(workingHours);
        }

        public int Id { get; init; }

        public string Title { get; init; }

        public WorkTime[] WorkingHours => _workingHours.ToArray();

        public static (Project? Result, string[] Errors) Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return (null, new string[] { "Title cannot be null or empty." });
            }

            if (title.Length > 200)
            {
                return (null, new string[] { "Title cannot contains more then 200 symbols." });
            }

            return Create(title, 0, Array.Empty<WorkTime>());
        }

        public static (Project? Result, string[] Errors) Create(
            string title,
            int id,
            WorkTime[] workTimes)
        {
            return (
                new Project(id, title, workTimes),
                Array.Empty<string>());
        }

        public string[] AddWorkTime(WorkTime workTime)
        {
            if (_workingHours.Any(x => x.Date.Day == workTime.Date.Day))
            {
                return new string[] { "Can not add hours on the same date." };
            }

            _workingHours.Add(workTime);

            return Array.Empty<string>();
        }
    }
}