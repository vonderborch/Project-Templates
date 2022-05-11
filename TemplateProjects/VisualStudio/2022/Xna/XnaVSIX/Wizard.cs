using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using Thread = System.Threading.Thread;

namespace XnaLibraryVSIX
{
    public class Wizard : IWizard
    {

        private Dictionary<string, List<string>> FilesAndContent;

        public Wizard() : base()
        {
            FilesAndContent = new Dictionary<string, List<string>>();

            FilesAndContent.Add("CODEOWNERS", new List<string>() { "*       @vonderborch" });
            FilesAndContent.Add(".gitmodules", new List<string>() { "[submodule \"FNA\"]", "	path = FNA", "	url = https://github.com/FNA-XNA/FNA" });
        }

        private string solutionPath = string.Empty;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            solutionPath = replacementsDictionary["$destinationdirectory$"];
            foreach (var file in FilesAndContent)
            {
                var path = Path.Combine(solutionPath, file.Key);
                File.WriteAllLines(path, file.Value);
            }
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
            if (Directory.Exists(Path.Combine(solutionPath, ".git")))
            {
                using (var process = new System.Diagnostics.Process())
                {
                    var startInfo = new ProcessStartInfo();
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    startInfo.WorkingDirectory = solutionPath;
                    startInfo.UseShellExecute = true;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C git submodule update --init --recursive";

                    process.StartInfo = startInfo;

                    process.Start();

                    while (!process.HasExited)
                    {
                        Thread.Sleep(500);
                    }
                }
            }
        }

    }
}
