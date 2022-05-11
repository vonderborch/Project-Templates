using System.Collections.Generic;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using SmartFormat;

namespace GenericLibraryVSIX
{
    public class Wizard : IWizard
    {

        private Dictionary<string, string> FilesAndContent;

        public Wizard() : base()
        {
            FilesAndContent = new Dictionary<string, string>();

            FilesAndContent.Add("CODEOWNERS", "*       @{Username}");
            FilesAndContent.Add("LICENSE", @"MIT License

Copyright (c) {Year} {Displayname}

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE."
            );
            FilesAndContent.Add("README.md", @"# {ProjectName}

# Installation
[![NuGet version ({ProjectName})](https://img.shields.io/nuget/v/{ProjectName}.svg?style=flat-square)](https://www.nuget.org/packages/{ProjectName}/)

A nuget package is available: [{ProjectName}](https://www.nuget.org/packages/{ProjectName}/)

# Future Plans
See list of issues under the Milestones: https://github.com/vonderborch/{ProjectName}/milestones
            ");
        }

        private object[] _replacements;

        private string _solutionPath = string.Empty;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            _solutionPath = replacementsDictionary["$destinationdirectory$"];
            _replacements = new object[]
            {
                new {Year = replacementsDictionary["$year$"]},
                new {Time = replacementsDictionary["$time$"]},
                new {Username = replacementsDictionary["$username$"]},
                new {Displayname = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName},
                new {Domain = replacementsDictionary["$userdomain$"]},
                new {Machine = replacementsDictionary["$machinename$"]},
                new {Organization = replacementsDictionary["$registeredorganization$"]},
                new {SolutionDirectory = replacementsDictionary["$solutiondirectory$"]},
                new {ProjectName = replacementsDictionary["$projectname$"]},
                new {ProjectNameSafe = replacementsDictionary["$safeprojectname$"]},
                new {SolutionName = replacementsDictionary["$specifiedsolutionname$"]},
                new {DestinationName = replacementsDictionary["$destinationdirectory$"]}
            };
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
                File.WriteAllText(Path.Combine(_solutionPath, file.Key), Smart.Format(file.Value, _replacements));
            }
        }

    }
}
