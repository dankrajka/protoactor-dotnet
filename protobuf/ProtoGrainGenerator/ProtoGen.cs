using System;
using System.IO;
using Microsoft.Build.Framework;
using Proto.GrainGenerator;
using MSBuildTask = Microsoft.Build.Utilities.Task;

namespace MSBuildTasks
{
    public class ProtoGen : MSBuildTask
    {

        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.High, "Running ProtoGen");

            var currentProject = BuildEngine.ProjectFileOfTaskNode;
            var dir = Path.GetDirectoryName(currentProject)!;
            var protoFiles = Directory.GetFiles(dir, "*.proto", new EnumerationOptions
                {
                    RecurseSubdirectories = true
                }
            )!;
            
            foreach (var protoFile in protoFiles)
            {
                Log.LogMessage(MessageImportance.High, $"Protofile! {protoFile}");
                var protoDir = Path.GetDirectoryName(protoFile);
                var outputFile = Path.Combine(protoDir!, protoFile + ".cs");
            
                var fiIn = new FileInfo(protoFile);
                var fiOut = new FileInfo(outputFile);
                Generator.GenerateOne(fiIn, fiOut, Array.Empty<DirectoryInfo>(), Log, dir);
            }

            return true;
        }
    }
}