using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.ML;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Hubs;
using StudentCommunity.UI.Models.AccountModel;
using StudentCommunity.UI.Models.Competition;
using StudentCommunity.UI.ViewModels;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace StudentCommunity.UI.Controllers
{
    [Authorize]
    public class CompetitionController : Controller
    {
        #region Constructor
        private readonly UserManager<ApplicationUser> userManager;
        private readonly CommunityContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IHubContext<ChatHub> hubContext;
        private readonly IHttpClientFactory httpClientFactory;
        public CompetitionController(UserManager<ApplicationUser> _userManager, CommunityContext context, IHttpClientFactory _httpClientFactory,
            IConfiguration configuration, RoleManager<IdentityRole> roleManager, IHubContext<ChatHub> _hubContext)
        {
            userManager = _userManager;
            this.context = context;
            this.roleManager = roleManager;
            this.context = context;
            this.roleManager = roleManager;
            hubContext = _hubContext;
            httpClientFactory = _httpClientFactory;
        }
		#endregion
		// Retrieves a list of announced competitions and renders the corresponding view.
		public IActionResult Index()
        {
            List<AnnouncedCompetition> competitionList = context.AnnouncedCompetition.Where(com => !(com.IsDeleted)).ToList();
            List<CompetitionViewModel> modelLst = new List<CompetitionViewModel>();

            foreach (var item in competitionList)
            {
                CompetitionViewModel competition = new CompetitionViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    CompetitionTime = item.CompetitionTime,
                    InitialCompetitionId = item.InitialCompetitionId,
                    IsDelete = item.IsDeleted,
                    ShowResult = item.ShowResult,
                    IsFinshed = (DateTime.Now > item.CompetitionTime.Value.AddHours(1))
                };
                modelLst.Add(competition);
            }
            return View(modelLst);
        }

		// Retrieves teams for a given competition ID and renders the corresponding view.
		public async Task<IActionResult> Teams(int Id)
        {
            if (Id == 0)
            {
                Id = int.Parse(HttpContext.Session.GetString("Id"));
            }
            ViewBag.competitionId = Id;
            AnnouncedCompetition competition = GetAnnouncedCompetition(Id);
            ApplicationUser user = await userManager.GetUserAsync(User);
            List<TeamsViewModel> models = new List<TeamsViewModel>();
            if (competition != null)
            {
                foreach (var item in context.CompetitionTeams.Where(t => t.competationId == Id).ToList())
                {
                    TeamsViewModel team = new TeamsViewModel();
                    team.Id = item.Id;
                    team.Name = item.Name;
                    team.IsUserInThisTeam = false;
                    team.students = new List<ApplicationUser>();
                    foreach (var member in context.CompetitionMembers.Where(m => m.TeamId == item.Id).ToList())
                    {
                        ApplicationUser Member = await userManager.FindByIdAsync(member.MemberId);
                        if (user.Id == Member.Id)
                        {
                            team.IsUserInThisTeam = true;
                        }
                        team.students.Add(Member);
                    }
                    models.Add(team);
                }
            }
            return View(models);
        }

		// Creates a team for a competition.
		public async Task<IActionResult> CreateTeam(string teamName, string competitionId)
        {
            if (!IsThereTeamWithSameName(teamName, competitionId))
            {
                ApplicationUser user = await userManager.GetUserAsync(User);

                if (await TeamRoomCreatedAsync(teamName, competitionId))
                {
                    CompetitionTeam team = new CompetitionTeam
                    {
                        Name = teamName,
                        competationId = int.Parse(competitionId),
                        students = new List<CompetitionMember>()
                    };
                    team.RoomId = context.Rooms.FirstOrDefault(room => room.Name == "Team" + competitionId + teamName).Id;
                    context.CompetitionTeams.Add(team);
                    context.SaveChanges();

                    CompetitionMember member = new CompetitionMember
                    {
                        MemberId = user.Id,
                        TeamId = team.Id,
                        CompetitionId = int.Parse(competitionId),
                        IsLeader = true,
                    };
                    try
                    {
                        team.students.Add(member);
                        context.CompetitionMembers.Add(member);
                        context.SaveChanges();

                        context.CompetitionTeams.Update(team);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, errorMessage = "An error occurred while processing your request." });
                    }

                    HttpContext.Session.SetString("Id", competitionId);
                    return Json(new { success = true });

                }
            }

            return Json(new { success = false, errorMessage = "The Team Name is exists" });
        }

		// Allows a user to join a team for a competition.
		public async Task<ActionResult> JoinTeamAsync(int teamId, string competitionId)
        {
            ApplicationUser user = await userManager.GetUserAsync(User);

            int _compeitionId = Int32.Parse(competitionId);

            CompetitionTeam Team = GetTeam(teamId);

            if (!IsFullTeam(Team.Id))
            {
                if (!IsInTeam(user.Id, Team.Id))
                {
                    if (IsInAnotherTeam(user.Id, _compeitionId))
                    {
                        CompetitionMember userMember = GetMember(user.Id, _compeitionId);
                        CompetitionTeam userTeam = context.CompetitionTeams.Include(t => t.students).FirstOrDefault(tem => tem.Id == userMember.TeamId);
                        if (userTeam != null)
                        {
                            userTeam.students.Remove(userMember);
                            userMember.IsLeader = false;
                            if (userTeam.students.Count == 0)
                            {
                                context.CompetitionTeams.Remove(userTeam);
                                context.SaveChanges();
                            }

                        }
                    }

                    if (JoinTheMember(user, Team, _compeitionId))
                    {
                        return Json(new { success = true, competitionId = competitionId });
                        var roomName = "Team" + Team.Name;

                        await hubContext.Clients.All.SendAsync("addUserToRoom", new { userName = user.UserName, roomName });

                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "An error occurred while processing your request." });
                    }
                }
                return Json(new { success = false, errorMessage = "You Already in this Team" });
            }

            return Json(new { success = false, errorMessage = "This Team Is full" });
        }

		// Allows a user to leave a team for a competition.
		public async Task<IActionResult> LeaveTeamAsync(int teamId, string competitionId)
        {
            ApplicationUser user = await userManager.GetUserAsync(User);

            int _compeitionId = Int32.Parse(competitionId);

            CompetitionMember userMember = GetMember(user.Id, _compeitionId);

            CompetitionTeam userTeam = context.CompetitionTeams.Include(t => t.students).FirstOrDefault(tem => tem.Id == userMember.TeamId);
            if (userTeam != null)
            {
                userTeam.students.Remove(userMember);
                context.SaveChanges();
            }
            return Json(new { success = true, competitionId = competitionId });
        }
        [Authorize(Roles = "Admin")]

		// Renders the view for creating a competition.
		[HttpGet]
        public IActionResult Create()
        {
            return View();
        }
		// Handles the creation of a competition.
		[HttpPost]
        public IActionResult Create(CreateCompetitionModel model)
        {
            if (ModelState.IsValid)
            {
                Competition competition = new Competition
                {
                    Name = model.Name,
                    Description = model.Description
                };

                context.Competitions.Add(competition);
                context.SaveChanges();
                return RedirectToAction("CompetitionsList");
            }

            return View(model);
        }
        [Authorize(Roles = "Admin")]
		// Retrieves a list of competitions and renders the corresponding view.
		public IActionResult CompetitionsList()
        {
            List<CompetitionViewModel> models = new List<CompetitionViewModel>();
            List<Competition> comp = context.Competitions.ToList();
            foreach (var item in comp)
            {
                CompetitionViewModel model = new CompetitionViewModel
                {
                    Name = item.Name,
                    Description = item.Description,
                    InitialCompetitionId = item.Id
                };
                models.Add(model);
            }
            return View(models);
        }
        [Authorize(Roles = "Admin")]
		// Renders the view for announcing a competition.
		public IActionResult AnnouncingCompetition(int Id)
        {
            Competition competition = context.Competitions.Where(com => com.Id == Id).FirstOrDefault();
            if (competition != null)
            {
                CompetitionViewModel model = new CompetitionViewModel
                {
                    InitialCompetitionId = competition.Id,
                    Name = competition.Name,
                    Description = competition.Description,
                };
                return View(model);
            }
            return View();
        }
		// Handles the announcement of a competition.
		[HttpPost]
        public IActionResult AnnouncingCompetition(CompetitionViewModel model)
        {
            AnnouncedCompetition competition = new AnnouncedCompetition
            {
                Name = model.Name,
                CompetitionTime = model.CompetitionTime,
                InitialCompetitionId = model.InitialCompetitionId
            };
            context.AnnouncedCompetition.Add(competition);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
		// Renders the view for creating questions for a competition.
		[Authorize(Roles = "Admin")]
        public IActionResult CreateQuestion(int Id)
        {
            CreateQuestionModel model = new CreateQuestionModel();
            model.InitialCompetitionId = Id;
            return View(model);
        }
		// Handles the creation of questions for a competition.
		[HttpPost]
        public IActionResult CreateQuestion(CreateQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                Questions questions = new Questions
                {
                    QuestionTitle = model.QuestionTitle,
                    QuestionText = model.QuestionText,
                    QuestionLevel = (Data.LevelEnum)model.QuestionLevel,
                    InitialCompetitionId = model.InitialCompetitionId
                };
                context.Questions.Add(questions);
                context.SaveChanges();
            }
            return RedirectToAction("CompetitionsList");
        }
		// Manages questions for a competition.
		public IActionResult QuestionsManagement(int competitionId)
        {
            AnnouncedCompetition competition = context.AnnouncedCompetition.Where(c => c.Id == competitionId).FirstOrDefault();
            QuestionsCompetitionModel model = new QuestionsCompetitionModel();
            List<QuestionsViewModel> list = new List<QuestionsViewModel>();
            List<Questions> questions = context.Questions.Where(ques => ques.InitialCompetitionId == competition.InitialCompetitionId).ToList();

            foreach (Questions question in questions)
            {
                QuestionsViewModel _model = new QuestionsViewModel
                {
                    Id = question.Id,
                    QuestionTitle = question.QuestionTitle,
                    QuestionText = question.QuestionText,
                    QuestionLevel = (Models.Competition.LevelEnum)question.QuestionLevel,
                };
                if (context.CompetitionQuestions.Where(qu => qu.QuestionId == _model.Id && qu.CompetitionId == competition.Id).Any())
                {
                    _model.IsSelected = true;
                }
                list.Add(_model);
            }
            model.Questions = list;
            model.competitionId = competitionId;
            return View(model);
        }
		// Adds questions to a competition.
		[Authorize(Roles = "Admin")]

        [HttpPost]
        public IActionResult AddQuestions(QuestionsCompetitionModel competition)
        {
            foreach (QuestionsViewModel item in competition.Questions)
            {
                CompetitionQuestion obj = context.CompetitionQuestions.Where(co => co.QuestionId == item.Id && co.CompetitionId == competition.competitionId).FirstOrDefault();
                if (obj != null)
                {
                    if (!item.IsSelected)
                    {
                        context.CompetitionQuestions.Remove(obj);
                    }
                }
                else
                {

                    if (item.IsSelected)
                    {
                        CompetitionQuestion _obj = new CompetitionQuestion
                        {
                            QuestionId = item.Id,
                            CompetitionId = competition.competitionId
                        };
                        context.CompetitionQuestions.Add(_obj);
                    }
                }
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
		// Manages the exam process for a competition.
		public async Task<IActionResult> Exam(int CompetitionId)
        {
            ExamViewModel model = new ExamViewModel();

            ApplicationUser user = await userManager.GetUserAsync(User);

            CompetitionMember member = GetMember(user.Id, CompetitionId);

            if (member != null)
            {
                model.team = GetTeam(member.TeamId);

                model.competition = GetAnnouncedCompetition(CompetitionId);

                if (model.team != null && !IsFinshed(CompetitionId) && IsInThisCompetition(member.MemberId, CompetitionId))
                {
                    foreach (var item in context.CompetitionQuestions.Where(com => com.CompetitionId == CompetitionId).ToList())
                    {
                        Questions questions = context.Questions.Where(x => x.Id == item.QuestionId).FirstOrDefault();
                        QuestionAnswerTeamModel questionAnswer = new QuestionAnswerTeamModel
                        {
                            Id = questions.Id,
                            QuestionLevel = (Models.Competition.LevelEnum)questions.QuestionLevel,
                            QuestionText = questions.QuestionText,
                            QuestionTitle = questions.QuestionTitle,
                            answer = GetAnswer(member.TeamId, questions.Id),
                        };
                        model.questions.Add(questionAnswer);
                    }
                    foreach (CompetitionMember student in model.team.students)
                    {
                        if (student.IsLeader == true)
                        {
                            model.TeamLeaderId = student.MemberId;
                        }
                        model.team.students.Add(student);
                    }
                    model.team.competationId = CompetitionId;
                    return View(model);
                }
            }
            return RedirectToAction("AccessDenied", "Account");
        }
		// Saves answers submitted during the exam.
		[HttpPost]
        public IActionResult SaveAnswer(int questionId, int teamId, string answer)
        {
            QuestionAnswer questionAnswer = context.QuestionsAnswers.Where(qu => qu.QuestionId == questionId && qu.TeamId == teamId).FirstOrDefault();
            if (questionAnswer == null)
            {
                var newAnswer = new QuestionAnswer
                {
                    QuestionId = questionId,
                    TeamId = teamId,
                    Answer = answer
                };

                context.QuestionsAnswers.Add(newAnswer);
                context.SaveChanges();
            }
            else
            {
                questionAnswer.Answer = answer;
                context.Update(questionAnswer);
                context.SaveChanges();
            }

            return Json(new { success = true });
        }
		// Evaluates answers submitted during the exam.
		[Authorize(Roles = "Admin")]
        public IActionResult EvaluateTheAnswers(int competitionId)
        {
            List<CompetitionTeam> teams = context.CompetitionTeams.Where(team => team.competationId == competitionId).ToList();

            return View(teams);
        }
		// Retrieves answers submitted by teams.
		public IActionResult GetTeamsAnswers(int TeamId)
        {
            List<EvaluateViewMode> models = new List<EvaluateViewMode>();

            CompetitionTeam team = context.CompetitionTeams.Where(x => x.Id == TeamId).FirstOrDefault();
            List<CompetitionQuestion> questions = context.CompetitionQuestions.Where(qa => qa.CompetitionId == team.competationId).ToList();

            foreach (var question in questions)
            {

                QuestionAnswer answer = context.QuestionsAnswers.Where(ans => ans.QuestionId == question.QuestionId && ans.TeamId == TeamId).FirstOrDefault();

                EvaluateViewMode obj = new EvaluateViewMode();
                if (answer != null)
                {
                    obj.Id = answer.Id;
                    obj.TeamId = answer.TeamId;
                    obj.answer = answer.Answer;
                    obj.mark = answer.Mark;
                }
                obj.question = context.Questions.Where(qa => qa.Id == question.QuestionId).FirstOrDefault();

                models.Add(obj);
            }

            models = models.OrderBy(x => x.question.QuestionLevel).ToList();

            return View(models);
        }
		// Updates marks for evaluated answers.
		[HttpPost]
        public IActionResult UpdateMark(List<EvaluateViewMode> models)
        {
            if (models != null && models.Any())
            {
                foreach (var model in models)
                {
                    var existingQuestionAnswer = context.QuestionsAnswers
                        .FirstOrDefault(an => an.Id == model.Id);

                    if (existingQuestionAnswer != null)
                    {
                        existingQuestionAnswer.Mark = model.mark;
                    }
                }

                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        // Renders the view for editing a competition.
        public IActionResult EditCompetition(int Id)
        {
            AnnouncedCompetition _competition = context.AnnouncedCompetition.Where(com => com.Id == Id).FirstOrDefault();
            if (_competition != null)
            {
                CompetitionViewModel model = new CompetitionViewModel
                {
                    Id = _competition.Id,
                    InitialCompetitionId = _competition.Id,
                    Name = _competition.Name,
                    CompetitionTime = _competition.CompetitionTime,
                };
                return View(model);
            }
            return NotFound("Your requiest Not Found");

        }
		// Handles the editing of a competition.
		[HttpPost]
        public IActionResult EditCompetition(AnnouncedCompetition competition)
        {
            AnnouncedCompetition _competition = context.AnnouncedCompetition.Where(com => com.Id == competition.Id).FirstOrDefault();
            if (_competition != null)
            {
                _competition.CompetitionTime = competition.CompetitionTime;
                context.AnnouncedCompetition.Update(_competition);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(competition);
        }
		// Deletes a competition.
		public IActionResult DeleteCompetition(int competitionId)
        {
            AnnouncedCompetition competition = context.AnnouncedCompetition.Where(com => com.Id == competitionId).FirstOrDefault();
            if (competition != null)
            {
                competition.IsDeleted = true;
                context.AnnouncedCompetition.Update(competition);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound("Not Found");
        }
		// Retrieves and displays results for a competition.
		public IActionResult Results(int CompetitionId)
        {
            List<CompetitionTeam> TeamsInCompetetion = context.CompetitionTeams.Where(ta => ta.competationId == CompetitionId).ToList();
            List<ResultsViewModel> models = new List<ResultsViewModel>();
            foreach (var team in TeamsInCompetetion)
            {
                ResultsViewModel result = new ResultsViewModel()
                {
                    TeamId = team.Id,
                    TeamName = team.Name,
                    mark = 0
                };
                foreach (var answer in context.QuestionsAnswers.Where(ans => ans.TeamId == team.Id).ToList())
                {
                    if (answer.Mark != null)
                    {
                        result.mark += answer.Mark;
                    }
                }
                models.Add(result);
            }
            return View(models);
        }
		// Displays results for a competition.
		public IActionResult ShowResults(int CompetitionId)
        {
            if (CompetitionId != 0)
            {
                AnnouncedCompetition competition = context.AnnouncedCompetition.Where(com => com.Id == CompetitionId).FirstOrDefault();
                if (competition != null)
                {
                    competition.ShowResult = true;
                    context.AnnouncedCompetition.Update(competition);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        #region Function

        public async Task<bool> TeamRoomCreatedAsync(string teamName, string competitionId)
        {
            string roomCreationUrl = "http://localhost:38187/api/Rooms";
            RoomViewModel roomViewModel = new RoomViewModel { Name = "Team" + competitionId + teamName };

            using (HttpClient client = new HttpClient())
            {
                var roomContent = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(roomViewModel),
                    Encoding.UTF8,
                    "application/json");

                var roomResponse = await client.PostAsync(roomCreationUrl, roomContent);
                var responseContent = await roomResponse.Content.ReadAsStringAsync();
                if (roomResponse.IsSuccessStatusCode)
                {
                    return true;
                }

            }
            return false;
        }


        public CompetitionTeam GetTeam(int teamId)
        {
            CompetitionTeam team = context.CompetitionTeams.Include(t => t.students).FirstOrDefault(team => team.Id == teamId);
            if (team != null)
            {
                return team;
            }
            return null;
        }

        public AnnouncedCompetition GetAnnouncedCompetition(int competitionId)
        {
            AnnouncedCompetition competition = context.AnnouncedCompetition.FirstOrDefault(com => com.Id == competitionId);
            if (competition != null)
            {
                return competition;
            }
            return null;
        }

        public CompetitionMember GetMember(string userId, int CompetitionId)
        {
            CompetitionMember member = context.CompetitionMembers.Where(c => c.MemberId == userId && c.CompetitionId == CompetitionId).FirstOrDefault();

            if (member != null)
            {
                return member;
            }
            return null;
        }

        public bool JoinTheMember(ApplicationUser user, CompetitionTeam Team, int competitionId)
        {
            CompetitionMember member = new CompetitionMember
            {
                TeamId = Team.Id,
                MemberId = user.Id,
                CompetitionId = competitionId,
            };
            try
            {
                Team.students.Add(member);
                context.CompetitionMembers.Add(member);
                context.CompetitionTeams.Update(Team);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsInThisCompetition(string userId, int compitionId)
        {
            if (userId != null && compitionId != 0)
            {
                CompetitionMember member = context.CompetitionMembers.FirstOrDefault(member => member.MemberId == userId && member.CompetitionId == compitionId);
                if (member != null)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInTeam(string userId, int TeamId)
        {
            CompetitionTeam Team = GetTeam(TeamId);
            if (Team != null)
            {
                bool InTeam = Team.students.Any(member => member.MemberId == userId);
                if (InTeam)
                    return true;
                return false;
            }
            return false;
        }

        public bool IsFinshed(int CompetitionId)
        {
            AnnouncedCompetition competition = context.AnnouncedCompetition.FirstOrDefault(com => com.Id == CompetitionId);
            if (DateTime.Now < (competition.CompetitionTime.Value.AddHours(1)))
            {
                return false;
            }
            return true;
        }

        public bool IsFullTeam(int TeamId)
        {
            var team = context.CompetitionTeams.Include(t => t.students).FirstOrDefault(t => t.Id == TeamId);
            if (team != null)
            {
                return team.students.Count() >= 3;
            }
            return false;
        }

        public bool IsInAnotherTeam(string memberId, int competitionId)
        {
            return context.CompetitionMembers.Any(t => t.MemberId == memberId && t.CompetitionId == competitionId);
        }

        public bool IsThereTeamWithSameName(string teamName, string compeitionId)
        {
            return context.CompetitionTeams.Any(team => team.Name == teamName && team.competationId.ToString() == compeitionId);
        }
        public string GetAnswer(int teamId, int QuestionId)
        {
            QuestionAnswer questionAnswer = context.QuestionsAnswers.FirstOrDefault(qa => qa.TeamId == teamId && qa.QuestionId == QuestionId);
            if (questionAnswer != null)
            {
                return questionAnswer.Answer;
            }
            return null;
        }
        #endregion
    }
}
