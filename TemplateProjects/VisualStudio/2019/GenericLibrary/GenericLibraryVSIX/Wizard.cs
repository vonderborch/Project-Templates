using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace GenericLibraryVSIX
{
    public class Wizard : IWizard
    {

        private Dictionary<string, List<string>> FilesAndContent;

        public Wizard() : base()
        {
            FilesAndContent = new Dictionary<string, List<string>>();

            FilesAndContent.Add("CODEOWNERS", new List<string>() { "*       @vonderborch" } );
        }

        private string solutionPath = string.Empty;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            solutionPath = replacementsDictionary["$destinationdirectory$"];
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
            foreach (var file in FilesAndContent)
            {
                File.WriteAllLines(Path.Combine(solutionPath, file.Key), file.Value);
            }
        }

    }
}
