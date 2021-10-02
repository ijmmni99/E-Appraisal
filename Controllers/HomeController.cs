using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using E_Appraisal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Exchange.WebServices.Autodiscover;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.VisualBasic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Web.Mvc;
using System.Data.Linq.SqlClient;
using System.IO;
using System.Web.UI;

namespace E_Appraisal.Controllers
{
    public class HomeController : Controller
    {
        EDIEntities dbm = new EDIEntities();
        iScalaC1Entities dbi = new iScalaC1Entities();

        private ExchangeService service;

        protected ExchangeService LoadEmailObject(string EmailId, string EmailPass)
        {
            service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            service.Credentials = new WebCredentials(EmailId, EmailPass);
            service.TraceEnabled = true;
            service.TraceFlags = TraceFlags.DebugMessage;
            service.Url = new Uri("https://outlook.office365.com/ews/exchange.asmx");

            return service;
        }

        public Microsoft.Exchange.WebServices.Data.Contact GetContactInfo(string sFullName)
        {
            Microsoft.Exchange.WebServices.Data.Contact contact = null;
            try
            {
                NameResolutionCollection allContacts = service.ResolveName(sFullName, ResolveNameSearchLocation.DirectoryOnly, true);

                if (allContacts.Count > 0)
                {
                    contact = allContacts[0].Contact;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetContactInfo(): ", ex);
            }
            return contact;
        }

        public string GetManEmail(string name)
        {
            string uname = "";
            string defaultNamingContext;

            DirectoryEntry rootDSE = new DirectoryEntry("LDAP://RootDSE");
            defaultNamingContext = rootDSE.Properties["defaultNamingContext"].Value.ToString();

            DirectoryEntry de = new DirectoryEntry("LDAP://" + defaultNamingContext);
            DirectorySearcher ds = new DirectorySearcher(de);
            ds.SearchScope = SearchScope.Subtree;
            ds.Filter = "(&(objectCategory=User) (displayName=" + name + "))";

            ds.PropertiesToLoad.Add("mail");       // given (or first) name

            try
            {
                SearchResult nresult = null;
                nresult = ds.FindOne();
                if (nresult != null)
                {
                    uname = nresult.Properties["mail"][0].ToString();

                }
            }
            catch (Exception f)
            {

            }
            return uname;
        }

        public string GetManDescip(string name)
        {
            string uname = "";
            string defaultNamingContext;

            DirectoryEntry rootDSE = new DirectoryEntry("LDAP://RootDSE");
            defaultNamingContext = rootDSE.Properties["defaultNamingContext"].Value.ToString();

            DirectoryEntry de = new DirectoryEntry("LDAP://" + defaultNamingContext);
            DirectorySearcher ds = new DirectorySearcher(de);
            ds.SearchScope = SearchScope.Subtree;
            ds.Filter = "(&(objectCategory=User) (displayName=" + name + "))";

            ds.PropertiesToLoad.Add("description");       // given (or first) name

            try
            {
                SearchResult nresult = null;
                nresult = ds.FindOne();
                if (nresult != null)
                {
                    uname = nresult.Properties["description"][0].ToString();

                }
            }
            catch (Exception f)
            {

            }
            return uname;
        }

        public ActionResult Index()
        {
            Session.Abandon();
            return View();
        }

        [HttpPost]
        public ActionResult Index(string usrname, string pass)
        {
            service = LoadEmailObject(usrname, pass);
            Microsoft.Exchange.WebServices.Data.Contact userProfile = GetContactInfo(usrname);
            if (userProfile != null)
            {

                //Get Email Userprofile
                //Microsoft.Exchange.WebServices.Data.Contact userProfile = GetContactInfo(usrname);

                string uname = "";
                string uDesc = "";
                string defaultNamingContext;

                //Get Active Directory Information
                DirectoryEntry rootDSE = new DirectoryEntry("LDAP://RootDSE");
                defaultNamingContext = rootDSE.Properties["defaultNamingContext"].Value.ToString();

                DirectoryEntry de = new DirectoryEntry("LDAP://" + defaultNamingContext);
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.SearchScope = SearchScope.Subtree;
                ds.Filter = "(&(objectCategory=User) (mail=" + usrname + "))";

                ds.PropertiesToLoad.Add("sn");              // surname = last name
                ds.PropertiesToLoad.Add("givenName");       // given (or first) name
                ds.PropertiesToLoad.Add("description");     // User Desc with Emp Num
                ds.PropertiesToLoad.Add("sAMAccountName");  // username
                var _man = userProfile.Manager;         //userProfile.Manager; 
                var _jbtitle = userProfile.JobTitle;
                var name = userProfile.DisplayName;
                var _dprt = userProfile.Department;

                try
                {
                    SearchResult nresult = null;
                    nresult = ds.FindOne();
                    if (nresult != null)
                    {
                        uname = nresult.Properties["givenName"][0].ToString();
                        uDesc = nresult.Properties["description"][0].ToString();

                        if (dbm.EA_Users.Any(u => u.Users_Email == usrname))
                        {
                            var curr_pos = dbm.EA_Users.Where(x => x.Users_Email == usrname).FirstOrDefault().Users_Position;
                            var _get = GetManEmail(_man);

                            var bb1 = dbm.EA_Users.Where(z => z.Users_Email == _get).Any();

                            if (bb1 == false)
                            {
                                Microsoft.Exchange.WebServices.Data.Contact mngProfile = GetContactInfo(_man);
                                var _man2 = mngProfile.Manager;
                                var _manemail2 = GetManEmail(_man2);
                                var name2 = mngProfile.DisplayName;
                                var _jbtitle2 = mngProfile.JobTitle;
                                var _decrip2 = GetManDescip(_man);
                                var _email = _get;
                                var _dprt2 = mngProfile.Department;

                                var ea_code2 = "";
                                EA_Users data2 = new EA_Users();
                                data2.Users_ID = "3";
                                foreach (char c in _decrip2)
                                {
                                    if (c != '(')
                                    {
                                        ea_code2 = ea_code2 + c;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                data2.Users_Code = ea_code2;
                                data2.Users_Fname = name2;
                                data2.Users_Position = _jbtitle2;
                                data2.Users_Email = _email;
                                data2.Manager_ID = _manemail2;
                                data2.Username = _email;
                                data2.Department_ID = _dprt2;
                                dbm.EA_Users.Add(data2);
                                dbm.SaveChanges();
                            }
                            if (curr_pos != _jbtitle.ToUpper())
                            {
                                EA_Users d = new EA_Users();
                                d = dbm.EA_Users.Single(x => x.Users_Email == usrname);
                                d.Users_Position = _jbtitle.ToUpper();
                                dbm.SaveChanges();
                            }
                            Session["data"] = dbm.EA_Users.Where(x => x.Users_Email == usrname).FirstOrDefault().Users_ID;
                            Session["Manager_Email"] = dbm.EA_Users.Where(x => x.Users_Email == usrname).FirstOrDefault().Users_Email;
                            return RedirectToAction("Manager_Dashboard", "Home");
                        }
                        else
                        {
                            var _manemail = GetManEmail(_man);
                            var ea_code = "";
                            EA_Users data = new EA_Users();
                            data.Users_ID = "2";
                            foreach (char c in uDesc)
                            {
                                if (c != '(')
                                {
                                    ea_code = ea_code + c;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            data.Users_Code = ea_code;
                            data.Users_Fname = name;
                            data.Users_Email = usrname;
                            data.Users_Position = _jbtitle;
                            data.Manager_ID = _manemail;
                            data.Username = usrname;
                            data.Department_ID = _dprt;
                            dbm.EA_Users.Add(data);
                            dbm.SaveChanges();

                            var bb = dbm.EA_Users.Where(z => z.Users_Email == _manemail).Any();

                            if (bb == false)
                            {
                                Microsoft.Exchange.WebServices.Data.Contact mngProfile = GetContactInfo(_man);
                                var _man2 = mngProfile.Manager;
                                var _manemail2 = GetManEmail(_man2);
                                var name2 = mngProfile.DisplayName;
                                var _jbtitle2 = mngProfile.JobTitle;
                                var _decrip2 = GetManDescip(_man);
                                var _email = _manemail;
                                var _dprt2 = mngProfile.Department;

                                var ea_code2 = "";
                                EA_Users data2 = new EA_Users();
                                data2.Users_ID = "3";
                                foreach (char c in _decrip2)
                                {
                                    if (c != '(')
                                    {
                                        ea_code2 = ea_code2 + c;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                data2.Users_Code = ea_code2;
                                data2.Users_Fname = name2;
                                data2.Users_Position = _jbtitle2;
                                data2.Users_Email = _email;
                                data2.Manager_ID = _manemail2;
                                data2.Username = _email;
                                data2.Department_ID = _dprt2;
                                dbm.EA_Users.Add(data2);
                                dbm.SaveChanges();
                            }
                            if (dbm.EA_Users.Any(u => u.Users_Email == usrname))
                            {
                                Session["data"] = dbm.EA_Users.Where(x => x.Users_Email == usrname).FirstOrDefault().Users_ID;
                                Session["Manager_Email"] = dbm.EA_Users.Where(x => x.Users_Email == usrname).FirstOrDefault().Users_Email;
                                return RedirectToAction("Manager_Dashboard", "Home");
                            }
                            else
                            {
                                TempData["msg"] = "<script>alert('There's problem. Please try again..');</script>";
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('Invalid Username or Password');</script>";
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception f)
                {
                    TempData["msg"] = "<script>alert('" + Server.HtmlEncode(f.Message) + "');</script>";
                    return RedirectToAction("Index", "Home");
                }
            }//eof service -- win user login checking
            else
            {
                TempData["msg"] = "<script>alert('Invalid Email/Password or Not Connected to Lan / Camfil, Please try again.. ');</script>";
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult RubySign(string rubyusrname, string rubypass)
        {
            try
            {
                rubypass = rubypass.Substring(3);
                rubypass = rubypass.ToUpper(); rubyusrname = rubyusrname.ToUpper();
                string code = dbi.Ruby_Emp.Where(x => x.Barkey == rubypass && x.Emp_Code == rubyusrname).FirstOrDefault().Emp_Code;
                string fname = dbi.Ruby_Emp.Where(x => x.Barkey == rubypass && x.Emp_Code == rubyusrname).FirstOrDefault().Emp_Name;
                int? dpart_id = dbi.Ruby_Emp.Where(x => x.Barkey == rubypass && x.Emp_Code == rubyusrname).FirstOrDefault().Department;
                string dpart = dbi.Ruby_Department_List.Where(x => x.Depart_id == dpart_id).FirstOrDefault().Depart_Name;
                if (dpart == "IT") { dpart = "Finance/IT"; }
                if (dbm.EA_Users.Where(x => x.Users_Code == code).Any() == false)
                {
                    EA_Users io = new EA_Users();
                    io.Users_ID = "1";
                    io.Users_Code = code;
                    io.Users_Fname = fname;
                    io.Users_Email = "NULL";
                    io.Username = rubyusrname;
                    io.Department_ID = dpart;
                    dbm.EA_Users.Add(io);
                    dbm.SaveChanges();
                    Session["ruby"] = code;
                    return RedirectToAction("Ruby_Manage");
                }
                else
                {
                    if (dbm.EA_Users.Where(x => x.Users_Code == code).FirstOrDefault().Manager_ID == null)
                    {
                        Session["ruby"] = code;
                        return RedirectToAction("Ruby_Manage");
                    }
                    else
                    {
                        var dd = dbm.EA_Users.Where(x => x.Users_Code == code).FirstOrDefault().Users_ID;
                        Session["Manager_Email"] = dbm.EA_Users.Where(x => x.Users_Code == code).FirstOrDefault().Users_Email; ;
                        Session["data"] = dd;
                        return RedirectToAction("Manager_Dashboard");
                    }
                }

            }
            catch (Exception f)
            {
                TempData["msg"] = "<script>alert('" + Server.HtmlEncode(f.Message) + "');</script>";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Ruby_Manage(string code)
        {
            if (!string.IsNullOrEmpty(Session["ruby"] as string))
            {
                code = Session["ruby"].ToString();
                Session["Current"] = dbm.EA_Users.Where(x => x.Users_Code == code).FirstOrDefault().Users_ID;
                List<string> data = new List<string>();
                List<string> data2 = new List<string>();
                data = dbm.EA_Users.Select(x => x.Users_Email).ToList();
                data.ForEach(x =>
                {
                    if (dbm.EA_Users.Where(y => y.Manager_ID == x).Any() == true)
                    {
                        var b = dbm.EA_Users.Where(z => z.Users_Email == x).FirstOrDefault().Users_Fname;
                        data2.Add(b.ToString());
                    }
                });
                ViewBag.Manager_data = data2;
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Ruby_Manage(string slct, string pstn)
        {
            if (!string.IsNullOrEmpty(slct as string) && !string.IsNullOrEmpty(pstn as string))
            {
                EA_Users data = new EA_Users();
                var code = Session["ruby"].ToString();
                data = dbm.EA_Users.Single(x => x.Users_Code == code);
                var bb = dbm.EA_Users.Where(x => x.Users_Fname == slct).FirstOrDefault().Users_Email;
                var dd = dbm.EA_Users.Where(x => x.Users_Code == code).FirstOrDefault().Users_ID;
                data.Manager_ID = bb;
                data.Users_Position = pstn;
                dbm.SaveChanges();
                Session["Manager_Email"] = dbm.EA_Users.Where(x => x.Users_Code == code).FirstOrDefault().Users_Email;
                Session["data"] = dd;
                return RedirectToAction("Manager_Dashboard");
            }
            else
            {
                TempData["rubysign"] = "<script> alert('Please insert these data before submit');</script>";
                return View();
            }
        }

        public ActionResult Manager_Dashboard(List<EA_Users> item1, EA_Users item2)
        {
            if (!string.IsNullOrEmpty(Session["Manager_Email"] as string))
            {
                var man = Session["Manager_Email"].ToString();
                var curr = Session["data"].ToString();

                item1 = (from a in dbm.EA_Users
                         where a.Manager_ID == man
                         select a).ToList();

                item2 = (from a in dbm.EA_Users
                         where a.Users_ID == curr
                         select a).FirstOrDefault();

                var t = new Tuple<List<EA_Users>, EA_Users>(item1, item2);

                return View("Manager_Dashboard", t);
            }
            else
            {
                TempData["msg"] = "<script>alert('Your Session has expired, Please Log In again');</script>";
                return RedirectToAction("Index");
            }

        }

        public ActionResult Route_Manager(string id)
        {
            Session["Current"] = id;
            Session["Manager"] = Session["Manager_Email"];
            return RedirectToAction("Instruction", "Home");
        }

        public ActionResult Instruction(Instruction data, string manid, string userid)
        {
            TempData["msg"] = null;
            if (manid != null)
            {
                Session["Current"] = manid;
                Session["Manager"] = "";
                Session["hehehe"] = manid;
                manid = null;
            }

            if (userid != null)
            {
                Session["Current"] = userid;
                Session["Manager"] = "";
            }

            if (!string.IsNullOrEmpty(Session["Current"] as string))
            {
                string current = Session["Current"].ToString();
                string man_id = dbm.EA_Users.Where(x => x.Users_ID == current).FirstOrDefault().Manager_ID;
                ViewBag.Depart = dbm.EA_Users.Where(x => x.Users_ID == current).FirstOrDefault().Department_ID;
                if (manid != null)
                {
                    Session["manager_name"] = dbm.EA_Users.Where(x => x.Users_Email == man_id).FirstOrDefault().Users_Fname;
                    if (!string.IsNullOrEmpty(Session["Current"] as string))
                    {
                        ViewBag.Man_Name = Session["manager_name"].ToString();
                    }
                }
                else
                {
                    Session["manager_name"] = dbm.EA_Users.Where(x => x.Users_Email == man_id).FirstOrDefault().Users_Fname;
                    if (!string.IsNullOrEmpty(Session["Current"] as string))
                    {
                        ViewBag.Man_Name = Session["manager_name"].ToString();
                    }
                }
                TempData["hehe"] = dbm.EA_Users.Where(x => x.Users_ID == current).FirstOrDefault().Users_Fname;
                var _id = dbm.EA_Users.Where(x => x.Users_ID == current).FirstOrDefault().Users_Code;

                data.EA_User = (from a in dbm.EA_Users
                                where a.Users_ID == current
                                select a).FirstOrDefault();
                data.EA_Ins = (from a in dbm.EA_Instruction
                               where a.Ins_ID == "Ins1"
                               select a).FirstOrDefault();
                data.EA_Ins2 = (from a in dbm.EA_Instruction
                                where a.Ins_ID == "Ins2"
                                select a).FirstOrDefault();
                data.EA_Leav = (from a in dbm.EA_Leave
                                where a.Emp_no == _id
                                select a).FirstOrDefault();

                return View("Instruction", data);
            }
            else
            {
                TempData["msg"] = "<script>alert('There's problem. Please try again..');</script>";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Section_2(string key, int man, int emp)
        {
            ViewBag.Key = key;
            ViewBag.Man = man;
            ViewBag.Emp = emp;

            return PartialView("Contact");

        }

        public ActionResult Contact()
        {
            if (!string.IsNullOrEmpty(Session["Current"] as string))
            {
                string btn_fwd = Session["Current"].ToString();
                string man_id = dbm.EA_Users.Where(x => x.Users_ID == btn_fwd).FirstOrDefault().Manager_ID;
                try
                {
                    ViewBag.Man_Name = dbm.EA_Users.Where(x => x.Users_Email == man_id).FirstOrDefault().Users_Fname;

                }
                catch (Exception a)
                {
                    TempData["temp"] = "<script>alert('Manager data not register yet'" + a + ");</script>";
                    return RedirectToAction("Manager_Dashboard");
                }
                All_Model model = new All_Model();
                string year = Convert.ToString(DateTime.Now.Year);
                string lyear = Convert.ToString(Convert.ToInt32(year) - 1);

                model.EA_User = (from a in dbm.EA_Users
                                 where a.Users_ID == btn_fwd
                                 select a).FirstOrDefault();
                model.EA_Sec1 = (from a in dbm.EA_Section_1
                                 where a.Emp_ID == btn_fwd && a.Years == year
                                 select a).ToList();
                model.EA_Sec2 = (from a in dbm.EA_Section_2
                                 where a.Emp_ID == btn_fwd && a.Years == year
                                 select a).ToList();
                model.EA_Sec3 = (from a in dbm.EA_Section_3
                                 where a.Emp_ID == btn_fwd && a.Years == year
                                 select a).ToList();
                model.EA_Sec4 = (from a in dbm.EA_Section_4
                                 where a.Emp_ID == btn_fwd && a.Years == year
                                 select a).ToList();
                model.EA_Sec5 = (from a in dbm.EA_Section_5
                                 where a.Emp_ID == btn_fwd && a.Years == year
                                 select a).ToList();
                model.EA_Sec6 = (from a in dbm.EA_Section_6
                                 where a.Emp_ID == btn_fwd && a.Years == year
                                 select a).ToList();
                model.EA_Sec3_ly = (from a in dbm.EA_Section_3
                                    where a.Emp_ID == btn_fwd && a.Years == lyear
                                    select a).ToList();
                model.EA_Sec4_ly = (from a in dbm.EA_Section_4
                                    where a.Emp_ID == btn_fwd && a.Years == lyear
                                    select a).ToList();

                return View(model);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        public JsonResult Section_1_Post(List<EA_Section_1> data)
        {
            using (EDIEntities entities = new EDIEntities())
            {
                var a = Session["Current"].ToString();
                var b = entities.EA_Users.Where(x => x.Users_ID == a).FirstOrDefault().Manager_ID;
                var c = entities.EA_Users.Where(x => x.Users_Email == b).FirstOrDefault().Users_ID;
                entities.Database.ExecuteSqlCommand("Delete from EA_Section_1 where Emp_ID = '" + a + "' and Years = '" + DateTime.Now.Year + "'");

                if (data == null)
                {
                    data = new List<EA_Section_1>();
                }
                int n = 100;
                foreach (EA_Section_1 dat in data)
                {
                    dat.Emp_ID = a;
                    dat.Man_ID = c;
                    n = n + 1;
                    dat.Sec1_ID = n;
                    dat.Years = Convert.ToString(DateTime.Now.Year);
                    entities.EA_Section_1.Add(dat);
                }
                int insertedRecords = entities.SaveChanges();
                return Json(insertedRecords);
            }
        }

        [HttpPost]
        public JsonResult Section_2_Post(List<EA_Section_2> data)
        {
            using (EDIEntities entities = new EDIEntities())
            {
                var a = Session["Current"].ToString();
                var b = entities.EA_Users.Where(x => x.Users_ID == a).FirstOrDefault().Manager_ID;
                var c = entities.EA_Users.Where(x => x.Users_Email == b).FirstOrDefault().Users_ID;
                entities.Database.ExecuteSqlCommand("Delete from EA_Section_2 where Emp_ID = '" + a + "' and Years = '" + DateTime.Now.Year + "'");

                if (data == null)
                {
                    data = new List<EA_Section_2>();
                }
                int n = 100;
                foreach (EA_Section_2 dat in data)
                {
                    dat.Emp_ID = a;
                    dat.Man_ID = c;
                    n = n + 1;
                    dat.Sec2_ID = n;
                    dat.Years = Convert.ToString(DateTime.Now.Year);
                    entities.EA_Section_2.Add(dat);
                }
                int insertedRecords = entities.SaveChanges();
                return Json(insertedRecords);
            }

        }

        [HttpPost]
        public ActionResult Section_3_Post(List<EA_Section_3> data)
        {
            using (EDIEntities entities = new EDIEntities())
            {
                var a = Session["Current"].ToString();
                var b = entities.EA_Users.Where(x => x.Users_ID == a).FirstOrDefault().Manager_ID;
                var c = entities.EA_Users.Where(x => x.Users_Email == b).FirstOrDefault().Users_ID;
                entities.Database.ExecuteSqlCommand("Delete from EA_Section_3 where Emp_ID = '" + a + "' and Years = '" + DateTime.Now.Year + "'");

                if (data == null)
                {
                    data = new List<EA_Section_3>();
                }
                int n = 100;
                foreach (EA_Section_3 dat in data)
                {
                    dat.Emp_ID = a;
                    dat.Man_ID = c;
                    n = n + 1;
                    dat.Sec3_ID = n;
                    dat.Years = Convert.ToString(DateTime.Now.Year);
                    entities.EA_Section_3.Add(dat);
                }
                int insertedRecords = entities.SaveChanges();
                return Json(insertedRecords);
            }
        }

        [HttpPost]
        public ActionResult Section_4_Post(List<EA_Section_4> data)
        {
            using (EDIEntities entities = new EDIEntities())
            {
                var a = Session["Current"].ToString();
                var b = entities.EA_Users.Where(x => x.Users_ID == a).FirstOrDefault().Manager_ID;
                var c = entities.EA_Users.Where(x => x.Users_Email == b).FirstOrDefault().Users_ID;
                entities.Database.ExecuteSqlCommand("Delete from EA_Section_4 where Emp_ID = '" + a + "' and Years = '" + DateTime.Now.Year + "'");

                if (data == null)
                {
                    data = new List<EA_Section_4>();
                }
                int n = 100;
                foreach (EA_Section_4 dat in data)
                {
                    dat.Emp_ID = a;
                    dat.Man_ID = c;
                    n = n + 1;
                    dat.Sec4_ID = n;
                    dat.Years = Convert.ToString(DateTime.Now.Year);
                    entities.EA_Section_4.Add(dat);
                }
                int insertedRecords = entities.SaveChanges();
                return Json(insertedRecords);
            }
        }

        [HttpPost]
        public ActionResult Section_5_Post(List<EA_Section_5> data)
        {
            using (EDIEntities entities = new EDIEntities())
            {
                var a = Session["Current"].ToString();
                var b = entities.EA_Users.Where(x => x.Users_ID == a).FirstOrDefault().Manager_ID;
                var c = entities.EA_Users.Where(x => x.Users_Email == b).FirstOrDefault().Users_ID;
                entities.Database.ExecuteSqlCommand("Delete from EA_Section_5 where Emp_ID = '" + a + "' and Years = '" + DateTime.Now.Year + "'");

                if (data == null)
                {
                    data = new List<EA_Section_5>();
                }
                int n = 100;
                foreach (EA_Section_5 dat in data)
                {
                    dat.Emp_ID = a;
                    dat.Man_ID = c;
                    n = n + 1;
                    dat.Sec5_ID = n;
                    dat.Years = Convert.ToString(DateTime.Now.Year);
                    entities.EA_Section_5.Add(dat);
                }
                int insertedRecords = entities.SaveChanges();
                return Json(insertedRecords);
            }
        }

        [HttpPost]
        public ActionResult Section_6_Post(List<EA_Section_6> data)
        {
            using (EDIEntities entities = new EDIEntities())
            {
                var a = Session["Current"].ToString();
                var b = entities.EA_Users.Where(x => x.Users_ID == a).FirstOrDefault().Manager_ID;
                var c = entities.EA_Users.Where(x => x.Users_Email == b).FirstOrDefault().Users_ID;
                entities.Database.ExecuteSqlCommand("Delete from EA_Section_6 where Emp_ID = '" + a + "' and Years = '" + DateTime.Now.Year + "'");

                if (data == null)
                {
                    data = new List<EA_Section_6>();
                }
                int n = 100;
                foreach (EA_Section_6 dat in data)
                {
                    dat.Emp_ID = a;
                    dat.Man_ID = c;
                    n = n + 1;
                    dat.Sec6_ID = n;
                    dat.Years = Convert.ToString(DateTime.Now.Year);
                    entities.EA_Section_6.Add(dat);
                }
                int insertedRecords = entities.SaveChanges();
                return Json(insertedRecords);
            }
        }

        public ActionResult HR_Page()
        {
            List<EA_Users> data = new List<EA_Users>();
            Session["HR"] = Session["data"].ToString();
            data = (from a in dbm.EA_Users
                    select a).ToList();
            List<string> data2 = new List<string>();
            data2 = dbm.EA_Users.Select(x => x.Department_ID).Distinct().ToList();
            ViewBag.depart_list = data2;
            return View(data);
        }

        public ActionResult Submitted()
        {
            List<EA_Users> data = new List<EA_Users>();

            data = (from a in dbm.EA_Users
                    join b in dbm.EA_Section_6 on a.Users_ID equals b.Emp_ID
                    where b.Emp_check == 1 && b.Man_check == 1
                    select a).ToList();

            List<string> data2 = new List<string>();
            data2 = dbm.EA_Users.Select(x => x.Department_ID).Distinct().ToList();
            ViewBag.depart_list = data2;

            return View("HR_Page", data);
        }

        public ActionResult Filter(string filters, string empname)
        {
            List<EA_Users> data = new List<EA_Users>();

            if (!string.IsNullOrWhiteSpace(empname))
            {
                if (filters != "All")
                {
                    data = (from a in dbm.EA_Users
                            where a.Department_ID == filters && a.Users_Fname.Contains(empname)
                            select a).ToList();

                    List<string> data2 = new List<string>();
                    data2 = dbm.EA_Users.Select(x => x.Department_ID).Distinct().ToList();
                    ViewBag.depart_list = data2;
                }
                else
                {
                    data = (from a in dbm.EA_Users
                            where a.Users_Fname.Contains(empname)
                            select a).ToList();
                    List<string> data2 = new List<string>();
                    data2 = dbm.EA_Users.Select(x => x.Department_ID).Distinct().ToList();
                    ViewBag.depart_list = data2;
                }
            }
            else if (string.IsNullOrWhiteSpace(empname))
            {
                if (filters != "All")
                {
                    data = (from a in dbm.EA_Users
                            where a.Department_ID == filters
                            select a).ToList();
                    List<string> data2 = new List<string>();
                    data2 = dbm.EA_Users.Select(x => x.Department_ID).Distinct().ToList();
                    ViewBag.depart_list = data2;
                }
                else
                {
                    data = (from a in dbm.EA_Users
                            select a).ToList();

                    List<string> data2 = new List<string>();
                    data2 = dbm.EA_Users.Select(x => x.Department_ID).Distinct().ToList();
                    ViewBag.depart_list = data2;
                }
            }
            return View("HR_Page", data);
        }

        public ActionResult HR_Details(string hrid)
        {
            Session["Current"] = hrid;
            Session["Manager"] = hrid;
            Session["HR"] = "HR";
            return RedirectToAction("Contact");
        }

        public ActionResult Edit_Instruction()
        {
            EA_Instruction item1 = new EA_Instruction();
            EA_Instruction item2 = new EA_Instruction();

            item1 = (from a in dbm.EA_Instruction
                     where a.Ins_ID == "Ins1"
                     select a).FirstOrDefault();
            item2 = (from b in dbm.EA_Instruction
                     where b.Ins_ID == "Ins2"
                     select b).FirstOrDefault();

            var t = new Tuple<EA_Instruction, EA_Instruction>(item1, item2);

            return View(t);
        }

        public ActionResult Language(string id)
        {
            if (id == "malay")
            {
                Session["Bahasa"] = "0";
                return View("Index");
            }
            else if (id == "english")
            {
                Session["Bahasa"] = "1";
                return View("Index");
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult Set_Admin(string id, string delete)
        {
            List<EA_Users> data = new List<EA_Users>();
            data = (from a in dbm.EA_Users
                    select a).ToList();
            List<string> data2 = new List<string>();
            data2 = dbm.EA_Users.Select(x => x.Department_ID).Distinct().ToList();
            ViewBag.depart_list = data2;
            if (!string.IsNullOrEmpty(id as string))
            {
                EA_Users d = new EA_Users();
                d = dbm.EA_Users.Single(x => x.Users_ID == id);
                d.Passkey = "HR Admin";
                TempData["HR_Set_Admin"] = "<script>alert('New Admin Has Been Set');</script>";
                dbm.SaveChanges();
            }
            if (!string.IsNullOrEmpty(delete as string))
            {
                EA_Users d = new EA_Users();
                d = dbm.EA_Users.Single(x => x.Users_ID == id);
                d.Passkey = null;
                TempData["HR_Set_Admin"] = "<script>alert('Admin Has Been Remove');</script>";
                dbm.SaveChanges();
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult Set_Admin_Filter(string filters, string empname)
        {
            List<EA_Users> data = new List<EA_Users>();

            if (!string.IsNullOrWhiteSpace(empname))
            {
                if (filters != "All")
                {
                    data = (from a in dbm.EA_Users
                            where a.Department_ID == filters && a.Users_Fname.Contains(empname)
                            select a).ToList();

                    List<string> data2 = new List<string>();
                    data2 = dbm.EA_Users.Select(x => x.Department_ID).Distinct().ToList();
                    ViewBag.depart_list = data2;
                }
                else
                {
                    data = (from a in dbm.EA_Users
                            where a.Users_Fname.Contains(empname)
                            select a).ToList();
                    List<string> data2 = new List<string>();
                    data2 = dbm.EA_Users.Select(x => x.Department_ID).Distinct().ToList();
                    ViewBag.depart_list = data2;
                }
            }
            else if (string.IsNullOrWhiteSpace(empname))
            {
                if (filters != "All")
                {
                    data = (from a in dbm.EA_Users
                            where a.Department_ID == filters
                            select a).ToList();
                    List<string> data2 = new List<string>();
                    data2 = dbm.EA_Users.Select(x => x.Department_ID).Distinct().ToList();
                    ViewBag.depart_list = data2;
                }
                else
                {
                    data = (from a in dbm.EA_Users
                            select a).ToList();

                    List<string> data2 = new List<string>();
                    data2 = dbm.EA_Users.Select(x => x.Department_ID).Distinct().ToList();
                    ViewBag.depart_list = data2;
                }
            }
            return View("Set_Admin", data);
        }

        public ActionResult Import_Excel()
        {
            ViewBag.EmployeeList = "";
            TempData["EmployeeList"] = "";
            return View();
        }

        public ActionResult Import(FormCollection formCollection)
        {
            if (Request != null)
            {
                DataTable dt = new DataTable();
                dbm.Database.ExecuteSqlCommand("Delete from EA_Leave where Years = '" + DateTime.Now.Year + "'");
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string path = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["XlsFilePath"] + fileName);
                    file.SaveAs(path);
                    if (!System.IO.Directory.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["XlsFilePath"])))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["XlsFilePath"]));
                    }
                    var excelData = new ExcelData(path);
                    var sData = excelData.getData("Sheet1");
                    List<EA_Leave> list = new List<EA_Leave>();
                    dt = sData.CopyToDataTable();
                    long a = 0;
                    foreach (DataRow item in dt.Rows)
                    {
                        a = a + 1;
                        EA_Leave emp = new EA_Leave();
                        emp.EA_Leav = a;
                        emp.Emp_no = item[0].ToString();
                        emp.ABSs = item[2].ToString();
                        emp.Casual = item[3].ToString();
                        emp.EL = item[4].ToString();
                        emp.EUPL = item[5].ToString();
                        emp.HL = item[6].ToString();
                        emp.ML = item[7].ToString();
                        emp.RL = item[8].ToString();
                        emp.UPL = item[9].ToString();
                        emp.Late_Min = item[10].ToString();
                        emp.Late_Time = item[11].ToString();
                        emp.Years = DateTime.Now.Year.ToString();
                        dbm.EA_Leave.Add(emp);
                        dbm.SaveChanges();
                        list.Add(emp);
                    }
                    ViewBag.EmployeeList = list;
                    TempData["EmployeeList"] = "<script>alert('Successfully load into database');</script>"; ;
                }
            }
            return View("Import_Excel");
        }

        [HttpPost]
        public ActionResult Edit_Instruction(string mt, string ytitle, string y1, string y2, string y3, string btitle, string b1, string gtitle, string g1, string g2, string g3, string g4, string g5, string g6, string g7)
        {
            dbm.Database.ExecuteSqlCommand("Delete from EA_Instruction where Ins_ID = 'Ins1'");
            EA_Instruction data = new EA_Instruction();

            data.Ins_ID = "Ins1";
            data.Main_Title = mt;
            data.Yellow_Title = ytitle;
            data.Y_1 = y1;
            data.Y_2 = y2;
            data.Y_3 = y3;
            data.Blue_Title = btitle;
            data.B_1 = b1;
            data.Green_Title = gtitle;
            data.G_1 = g1;
            data.G_2 = g2;
            data.G_3 = g3;
            data.G_4 = g4;
            data.G_5 = g5;
            data.G_6 = g6;
            data.G_7 = g7;
            dbm.EA_Instruction.Add(data);
            dbm.SaveChanges();

            return RedirectToAction("Edit_Instruction");
        }

        [HttpPost]
        public ActionResult Edit_Instruction2(string mt, string ytitle, string y1, string y2, string y3, string btitle, string b1, string gtitle, string g1, string g2, string g3, string g4, string g5, string g6, string g7)
        {
            dbm.Database.ExecuteSqlCommand("Delete from EA_Instruction where Ins_ID = 'Ins2'");
            EA_Instruction data = new EA_Instruction();

            data.Ins_ID = "Ins2";
            data.Main_Title = mt;
            data.Yellow_Title = ytitle;
            data.Y_1 = y1;
            data.Y_2 = y2;
            data.Y_3 = y3;
            data.Blue_Title = btitle;
            data.B_1 = b1;
            data.Green_Title = gtitle;
            data.G_1 = g1;
            data.G_2 = g2;
            data.G_3 = g3;
            data.G_4 = g4;
            data.G_5 = g5;
            data.G_6 = g6;
            data.G_7 = g7;
            dbm.EA_Instruction.Add(data);
            dbm.SaveChanges();

            return RedirectToAction("Edit_Instruction");
        }

    }
}