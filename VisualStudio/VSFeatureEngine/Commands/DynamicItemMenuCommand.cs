using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;

namespace VSFeatureEngine
{
    public abstract class DynamicItemMenuCommand : OleMenuCommand
    {
        static private void InnerInvoked(object sender, EventArgs args)
        {
            DynamicItemMenuCommand invokedCommand = (DynamicItemMenuCommand)sender;
            // If the command is already checked, we don’t need to do anything
            if (invokedCommand.Checked)
                return;

            //// Find the project that corresponds to the command text and set it as the startup project
            //var projects = dte2.Solution.Projects;
            //foreach (Project proj in projects)
            //{
            //    if (invokedCommand.Text.Equals(proj.Name))
            //    {
            //        dte2.Solution.SolutionBuild.StartupProjects = proj.FullName;
            //        return;
            //    }
            //}
        }

        static private void InnerBeforeQueryStatus(object sender, EventArgs args)
        {
            DynamicItemMenuCommand matchedCommand = (DynamicItemMenuCommand)sender;
            matchedCommand.Enabled = true;
            matchedCommand.Visible = true;

            // Find out whether the command ID is 0, which is the ID of the root item.
            // If it is the root item, it matches the constructed DynamicItemMenuCommand and IsValidDynamicItem won't be called.
            bool isRootItem = (matchedCommand.MatchedCommandId == 0);

            //// The index is set to 1 rather than 0 because the Solution.Projects collection is 1-based.
            //int indexForDisplay = (isRootItem ? 1 : (matchedCommand.MatchedCommandId - (int)PkgCmdIDList.cmdidMyCommand) + 1);

            //matchedCommand.Text = dte2.Solution.Projects.Item(indexForDisplay).Name;

            //Array startupProjects = (Array)dte2.Solution.SolutionBuild.StartupProjects;
            //string startupProject = System.IO.Path.GetFileNameWithoutExtension((string)startupProjects.GetValue(0));

            //// Check the command if it isn't checked already selected
            //matchedCommand.Checked = (matchedCommand.Text == startupProject);

            //// Clear the ID because we are done with this item.
            //matchedCommand.MatchedCommandId = 0;
        }

        public DynamicItemMenuCommand(CommandID rootId) : base(InnerInvoked, null, InnerBeforeQueryStatus, rootId)
        {
        }


        protected abstract void OnCommandInvoked();

        protected abstract bool IsItemMatch(int cmdId);

        public override bool DynamicItemMatch(int cmdId)
        {
            return IsItemMatch(cmdId);
        }
    }
}
