using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CartoSys.Models;
using System.Drawing;
using SolidBrush = MindFusion.Drawing.SolidBrush;
using LinearGradientBrush = MindFusion.Drawing.LinearGradientBrush;
using MindFusion.Diagramming;
using MindFusion.Diagramming.Mvc;
using MindFusion.Diagramming.Layout;

namespace CartoSys.Controllers
{
    public class FlowController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        //
        // GET: /Flow/

        public ActionResult Index()
        {
            var flowList = from f in db.Flows
                           join sa in db.Applications on f.SourceId equals sa.ID
                           join ta in db.Applications on f.TargetId equals ta.ID
                           join ft in db.FlowTypes on f.FlowType equals ft.ID
                           select new FlowView() { ID = f.ID, SourceName = sa.Name, TargetName = ta.Name, Status = f.Status, FlowType = ft.Description, SendingMode = f.SendingMode, Code = f.Code, Description = f.Description };
            return View(flowList.ToList());
        }

        //
        // GET: /Flow/Details/5

        public ActionResult Details(int id = 0)
        {
            Flow flow = db.Flows.Find(id);
            if (flow == null)
            {
                return HttpNotFound();
            }
            return View(flow);
        }

        //
        // GET: /Flow/Create

        public ActionResult Create()
        {
            var AppLst = new List<string>();

            var appplications = from a in db.Applications
                           orderby a.Name
                           select a;
            var applicationList = new SelectList(appplications, "ID", "Name");
            //GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.applicationList = applicationList;
            //Get flow type
            var flowTypes = from ft in db.FlowTypes
                            select ft;
            ViewBag.flowTypeList = new SelectList(flowTypes, "ID", "Description");
            return View();
        }

        //
        // POST: /Flow/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Flow flow)
        {
            if (ModelState.IsValid)
            {
                db.Flows.Add(flow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(flow);
        }

        //
        // GET: /Flow/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Flow flow = db.Flows.Find(id);
            if (flow == null)
            {
                return HttpNotFound();
            }

            var AppLst = new List<string>();

            //Get application list
            var appplications = from a in db.Applications
                                orderby a.Name
                                select a;
            var applicationList = new SelectList(appplications, "ID", "Name");
            ViewBag.applicationList = applicationList;
            //Get flow type
            var flowTypes = from ft in db.FlowTypes
                            select ft;
            ViewBag.flowTypeList = new SelectList(flowTypes, "ID", "Description"); 
            
            return View(flow);
        }

        //
        // POST: /Flow/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Flow flow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flow);
        }

        //
        // GET: /Flow/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Flow flow = db.Flows.Find(id);
            if (flow == null)
            {
                return HttpNotFound();
            }
            return View(flow);
        }

        //
        // POST: /Flow/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Flow flow = db.Flows.Find(id);
            db.Flows.Remove(flow);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Flow/Flow1

        public ActionResult Flow1(int sourceId = 1)
        {
            int applicationId = sourceId;
            //Appplication list
            var AppLst = new List<string>();

            var appplications = from a in db.Applications
                                orderby a.Name
                                select a;
            var applicationList = new SelectList(appplications, "ID", "Name");
            //GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.applicationList = applicationList;

            DiagramView view = new DiagramView("diagramView1");
            view.Width = 5000;
            view.Height = 5000;

            Diagram diagram = view.Diagram;
            //diagram.Font = new Font("Verdana", 3);
            //diagram.ShapeBrush = new LinearGradientBrush(Color.LightBlue, Color.Blue, 90);
            //diagram.ShapeBrush = new LinearGradientBrush(Color.White, Color.Fuchsia, 90);

            //diagram.LinkBrush = new SolidBrush(Color.Black);
            diagram.LinkHeadShape = ArrowHeads.Triangle;
            diagram.LinkHeadShapeSize = 3;

            //Update the mouse hover distance - does not seem to work
            //diagram.MeasureUnit = GraphicsUnit.Pixel;
            diagram.LinkHitDistance = .5f; 

            //Create theme for the diagram
            Theme theme = new Theme();
            //Set style for shape
            ShapeNodeStyle shapeStyle = new ShapeNodeStyle();
            shapeStyle.FontFamily = "Tahoma";
            shapeStyle.FontSize = 14;
            theme.RegisterStyle(typeof(ShapeNode), shapeStyle);
            //Set style for links
            DiagramLinkStyle linkStyle = new DiagramLinkStyle();
            linkStyle.FontFamily = "MS Sans Serif";
            linkStyle.FontSize = 6;
            theme.RegisterStyle(typeof(DiagramLink), linkStyle);
            diagram.Theme = theme;

             //Get the name of the root application
            string rootAppName = db.Applications.Find(applicationId).Name;

            //Create the box for the root application
            ShapeNode rootApplication = diagram.Factory.CreateShapeNode(new RectangleF(10, 10, 50, 50));
            rootApplication.Id = applicationId;
            //appli.Shape = Shape.FromId(application.ID);
            rootApplication.Text = rootAppName;
            rootApplication.Brush = new LinearGradientBrush(Color.Yellow, Color.White, 90);

            //Get all applications with link to this one
            var toApplication = from f in db.Flows
                                join a in db.Applications on f.TargetId equals a.ID
                                where f.SourceId == applicationId
                                select new { a.ID, a.Name, f.Description, f.FlowType };
            var fromApplication = from f in db.Flows
                                  join a in db.Applications on f.SourceId equals a.ID
                                  where f.TargetId == applicationId
                                  select new { a.ID, a.Name, f.Description, f.FlowType };

            //List unique des applications
            var singleApplicationList = (from ta in toApplication.Union(fromApplication)
                                         join a in db.Applications on ta.ID equals a.ID
                                         select new { ta.ID, ta.Name, a.Documentation }).Distinct();

            //Create applications having flows for the selected application
            foreach (var application in singleApplicationList)
            {
                ShapeNode appli = diagram.Factory.CreateShapeNode(new RectangleF(10, 10, 50, 20));
                appli.Id = application.ID;
                appli.Text = application.Name;
                appli.ToolTip = "<html><div width='300' style='background-color: #FFFF99; font-size: small;'>" + application.Documentation + "</div></html>";
                appli.Brush = new LinearGradientBrush(Color.LightBlue, Color.White, 90);
            }

            //Create applications with flows to the selected application
            foreach (var application in toApplication)
            {
                DiagramLink link = diagram.Factory.CreateDiagramLink(rootApplication, diagram.FindNodeById(application.ID));
                link.Text = application.Description.Substring(0, Math.Min(application.Description.Length, 20));
                //Add suspension mark to show flow description is truncated
                if (application.Description.Length > 20) { link.Text = link.Text + "..."; }
                link.ToolTip = "<html><div width='300' style='background-color: #FFFF99; font-size: small;'>" + application.Description + "</div></html>";

                //Set link color
                link.Pen = new MindFusion.Drawing.Pen(ColorLink(application.FlowType));


                link.HyperLink = "http://www.google.com";

                //Do not allow link to be selected
                link.Locked = true;

                //Test for dotted line
                //link.Brush = new MindFusion.Drawing.HatchBrush(System.Drawing.Drawing2D.HatchStyle.ZigZag, Color.Yellow, Color.Red);
                //link.Brush = new MindFusion.Drawing.HatchBrush(MindFusion.Drawing.HatchStyle.ZigZag, Color.Yellow, Color.Red);
                Brush b =  new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.DashedHorizontal, Color.Black, Color.Red);

                //This works to set link with red color
                //link.Pen = new MindFusion.Drawing.Pen(Color.Red);

            }

            foreach (var application in fromApplication)
            {
                DiagramLink link = diagram.Factory.CreateDiagramLink(diagram.FindNodeById(application.ID), rootApplication);
                link.Text = application.Description.Substring(0, Math.Min(application.Description.Length, 20));
                //Add suspension mark to show flow description is truncated
                if (application.Description.Length > 20) { link.Text = link.Text + "..."; }
                link.ToolTip = "<html><div width='300' style='background-color: #FFFF99; font-size: small;'>" + application.Description + "</div></html>";
                //Set link color
                link.Pen = new MindFusion.Drawing.Pen(ColorLink(application.FlowType));
                link.HyperLink = "http://www.google.com";
                //Do not allow link to be selected
                //link.Locked = true;
            }


             //Layout the diagram
            //SpringLayout sl = new SpringLayout();
            //sl.IterationCount = 500;
            //sl.NodeDistance = 30;
            //sl.Arrange(diagram);
            //AnnealLayout cl = new AnnealLayout(); acceptable
            //cl.Arrange(diagram);
            GridLayout gl = new GridLayout();
            gl.GridSize = 75;
            gl.Arrange(diagram);

            //Layout: OrthogonalRouter to set links layout
            new OrthogonalRouter().Arrange(diagram);
            diagram.ResizeToFitItems(0); //Resize diagram to make sure everything is visible

            view.LinkClickedScript("onLinkClicked");

            ViewData["DiagramView"] = view;
            return View();
        }

        /// <summary>
        /// Assign a color for flow according to the type of flow
        /// </summary>
        /// <param name="flowType"></param>
        /// <returns>Color</returns>
        /// Fichier -> Yellow
        /// Service -> Red
        /// Base a base -> Red
        /// Autre ou inconnu -> Green
        protected Color ColorLink(int? flowType)
        {
            //if (flowType == "Base à base") return Color.Red;
            //if (flowType == "Service") return Color.Black;
            //if (flowType == "Fichier") return Color.Yellow;
            if (flowType == 1) return Color.Red;
            if (flowType == 2) return Color.Black;
            if (flowType == 3) return Color.Yellow;
            return Color.Green;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}