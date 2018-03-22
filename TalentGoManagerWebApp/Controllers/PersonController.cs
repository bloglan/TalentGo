using System;
using System.Linq;
using System.Web.Mvc;
using TalentGo.Identity;
using TalentGo.Linq;
using TalentGo;
using TalentGo.Web;
using TalentGoManagerWebApp.Models;
using System.Threading.Tasks;

namespace TalentGoManagerWebApp.Controllers
{
    public class PersonController : Controller
    {
        PersonManager manager;

        public PersonController(PersonManager manager)
        {
            this.manager = manager;
        }

        // GET: Mgmt/Users
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult PeopleSummary()
        {
            var people = this.manager.People;
            var model = new PeopleSummaryViewModel
            {
                AllUserCount = people.Count(),
                PendingRealIdValidationCount = people.PendingRealIdValid().Count(),
                RealdIdAcceptedCount = people.Count(p => p.RealIdValid.Value),
            };
            return PartialView("_PeopleSummary", model);
        }

        public ActionResult Search(string q)
        {
            if (string.IsNullOrEmpty(q))
                return View();

            //Search user via condition.
            var result = this.manager.People.Where(p => p.IDCardNumber.StartsWith(q)
            || p.Mobile.StartsWith(q)
            || p.Email.StartsWith(q)
            || p.DisplayName.StartsWith(q));
            if (result.Count() == 1)
            {
                var person = result.First();
                return RedirectToAction("Detail", new { id = person.Id });
            }
            return View(result.OrderByDescending(p => p.WhenCreated).Take(100));
        }

        public async Task<ActionResult> Detail(Guid id)
        {
            var person = await this.manager.FindByIdAsync(id);
            return View(person);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PendingRealIdValid()
        {
            var result = this.manager.People.PendingRealIdValid();
            return View(result.OrderBy(p => p.WhenRealIdCommited));
        }

        public ActionResult ValidateRealId(Guid? id)
        {
            var pendingList = this.manager.People.PendingRealIdValid().OrderBy(p => p.WhenRealIdCommited);
            Person forValid;

            if (id.HasValue)
            {
                forValid = pendingList.FirstOrDefault(p => p.Id == id.Value);
                if (forValid == null)
                    return HttpNotFound();
            }
            else
            {
                var count = pendingList.Count();
                if (count == 0)
                    return View("PendingValidNotFound");
                if (count > 10)
                    count = 10;
                forValid = pendingList.Skip(new Random().Next(count)).FirstOrDefault();
            }

            if (forValid == null)
                return View("PendingValidNotFound");
            var model = new ValidateRealIdViewModel
            {
                PersonId = forValid.Id,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ValidateRealId(Guid? id, ValidateRealIdViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            var person = await this.manager.FindByIdAsync(model.PersonId);
            if (person == null)
                return HttpNotFound();

            try
            {
                await this.manager.ValidateRealId(person, model.Accepted, this.DomainUser().DisplayName, model.ValidationMessage);

                //如果审核未通过，退回给用户。
                if (!model.Accepted)
                {
                    if (model.ReturnBackIfRefused)
                        await this.manager.ReturnBackAsync(person);
                }

                if (model.Next)
                    return RedirectToAction("ValidateRealId", routeValues: null);
                return View("ValidateRealIdComplete");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [ChildActionOnly]
        public async Task<ActionResult> RealIdViewPart(Guid id)
        {
            var person = await this.manager.FindByIdAsync(id);
            if (person == null)
                return HttpNotFound();
            return PartialView("_RealIdViewPart", person);
        }

        public ActionResult UserList(UserListViewModel model)
        {
            if (model == null)
            {
                model = new UserListViewModel()
                {
                    PageIndex = 0
                };
            }
            model.AppUserList = this.GetSelectedUsers(model.Keywords, model.OrderColumn, model.DownDirection, model.PageIndex, model.PageSize, out int allCount);

            model.AllCount = allCount;

            return View(model);
        }


        public IQueryable<Person> GetSelectedUsers(string Keywords, string OrderColumn, bool DownDirection, int PageIndex, int PageSize, out int ItemCount)
        {
            ///带分页
            ///
            //先获得符合初始条件的集合
            var userSet = this.manager.People;

            //if (userSet.Count() <= 0)
            //{
            //    ItemCount = 0;
            //    return userSet;
            //}

            //模糊查询
            if (!string.IsNullOrEmpty(Keywords))
                userSet = userSet.Where(e =>
                    e.DisplayName.StartsWith(Keywords) ||
                    e.IDCardNumber.StartsWith(Keywords) ||
                    e.Mobile.StartsWith(Keywords) ||
                    e.Email.StartsWith(Keywords)
                );


            ItemCount = userSet.Count();
            if (ItemCount == 0)
            {
                return userSet;
            }
            //按字段排序
            if (string.IsNullOrEmpty(OrderColumn))
                OrderColumn = "WhenCreated";


            if (DownDirection)
                userSet = userSet.OrderByDescending(OrderColumn);
            else
                userSet = userSet.OrderBy(OrderColumn);


            //检查PageIndex和PageSize是否符合要求
            if (PageSize <= -1)
                PageSize = int.MaxValue;
            if (PageSize >= 0 && PageSize < 5)
                PageSize = 5;

            int PageCount = (int)Math.Ceiling((double)ItemCount / (double)PageSize);

            if (PageIndex < 0)
                PageIndex = 0;
            if (PageIndex >= PageCount)
                PageIndex = PageCount - 1;

            //返回指定分页的条目
            return userSet.Skip(PageIndex * PageSize).Take(PageSize);


            //ItemCount = 0;
            //return new List<Enrollment>().AsEnumerable();

        }
    }
}